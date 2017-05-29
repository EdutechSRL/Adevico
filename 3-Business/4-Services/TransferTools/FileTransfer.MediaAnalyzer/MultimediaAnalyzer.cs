using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FileTransfer.DomainModel.Configuration;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.DomainModel.Repository;
using FileTransfer.Data;

namespace FileTransfer.MediaAnalyzer
{

    public class KeyValue
    {
        public string Key { get; set; }
        public Single Value { get; set; }

        public KeyValue(string k, Single v)
        {
            this.Key = k;
            this.Value = v;
        }
    }
    public class MultimediaAnalyzer
    {
        public const Single DefaultRatio = 0.55F;

        

        private Config Cfg = ConfigurationLoader.Configuration;

        private Platform Platform { get; set; }

        String path { get; set; }
        
        IEnumerable<String> temp_result;

        Manager manager;
        
        public MultimediaAnalyzer(Platform platform, String path)
        {
            this.Platform = platform;
            this.manager = new Manager(platform);
            this.path = path;
        }

        public IList<KeyValue> AnalyzeCandidates(String PackageName = "")
        {

            IList<String> result = new List<String>();

            temp_result = new List<String>();

            DirectoryInfo d = new DirectoryInfo(path);
            Single pfRatio = DefaultRatio; //pattern priority is more important than pattern frequency

            try
            {
                pfRatio = Platform.MultimediaAnalysis.RulePriorityFrequencyRatio;
            }
            catch (Exception ex)
            {
                
            }
           
            result = CheckFiles(PackageName, result, d);

            result = CheckDirectories(PackageName, result, d);

            var frequency = from r in temp_result group r by r into g select new { Count = g.Count(), ValueType = g.Key };

            List<KeyValue> List = new List<KeyValue>();

            int add = result.Count;
            foreach (var item in result)
             {
                List.Add(new KeyValue(item, add * pfRatio));
                add--;
            }

            foreach (var item in frequency)
            {
                List.Where(x => x.Key == item.ValueType).First().Value += (item.Count * (1 - pfRatio));
            }

            Single tot = List.Sum(x => x.Value);

            List.ForEach(x => x.Value = (x.Value / tot) * 100);

            var weighted = from r in List orderby r.Value descending select r;

            return weighted.ToList();
        }
        
        public IList<String> AnalyzeAll()
        {
            IList<String> result = new List<String>();

            DirectoryInfo d = new DirectoryInfo(path);

            FileInfo[] fi = d.GetFiles("*.*", SearchOption.AllDirectories);
            if (fi.Length > 0)
            {
                result = result.Concat(from fileinfo in fi select fileinfo.FullName).Distinct().ToList();
            }

            return result;
        }

        public static class TransferPolicyEx
        {
            public static Boolean Check(lm.Comol.Core.FileRepository.Domain.TransferPolicy actual, lm.Comol.Core.FileRepository.Domain.TransferPolicy expected)
            {
                return (actual & expected) == expected;
            }
        }

        public void CloneMultimediaIndexes(FileTransferMultimedia clone, FileTransferMultimedia original)
        {
            IList<MultimediaFileObject> previous = FindMultimediaFileIndexes(original);

            //MultimediaFileObject originaldefault = (from item in previous where item.IsDefaultDocument == true select item).FirstOrDefault();

            //String originalpath = originaldefault.Fullname.Replace(original.UniqueIdVersion.ToString(), clone.UniqueIdVersion.ToString());

            IList<MultimediaFileObject> cloned = (from item in previous
                                                  select new MultimediaFileObject()
                                                  {                                                      
                                                      IdItemTransfer = clone.IdItem,
                                                      IdVersion = clone.IdVersion,
                                                      UniqueIdItem = clone.UniqueIdItem,
                                                      UniqueIdVersion = clone.UniqueIdVersion,

                                                      IsDefaultDocument = item.IsDefaultDocument,
                                                      Probability = item.Probability,
                                                      Fullname = item.Fullname.Replace(original.UniqueIdVersion.ToString(), clone.UniqueIdVersion.ToString()),
                                                      Deleted = item.Deleted
                                                  }).ToList();

            
            MultimediaFileObject clonedefault = (from item in previous where item.IsDefaultDocument == true select item).FirstOrDefault();
            if (clonedefault != null)
            {
                String clonepath = clonedefault.Fullname;

                clone.DefaultDocument = clonedefault;
                clone.DefaultDocumentPath = clonepath;
            }

            try
            {
                manager.BeginTransaction();
                manager.SaveList(cloned);
                manager.SaveFile(clone);
                manager.Commit();
            }
            catch (Exception ex)
            {
                
                manager.Rollback();
                throw;
            }
        }

        public void SaveAnalyzeResult(IEnumerable<String> all, IEnumerable<KeyValue> candidates, FileTransferMultimedia FileTransfer)
        {
            if (TransferPolicyEx.Check(FileTransfer.Policy, lm.Comol.Core.FileRepository.Domain.TransferPolicy.deletePreviousAnalysis))
            {
                try
                {
                    manager.BeginTransaction();                    
                    IList<MultimediaFileObject> previous = FindMultimediaFileIndexes(FileTransfer);
                    manager.DeleteList(previous);
                    manager.Commit();                    
                }
                catch (Exception ex)
                {
                    manager.Rollback();                    
                    throw ex;
                }
            }
            IList<MultimediaFileObject> list = GenerateMultimediaFileIndexes(all, candidates, FileTransfer);

            try
            {
                manager.BeginTransaction();
                manager.SaveList(list);

                if (candidates.Count() == 0)
                {
                    
                }

                manager.Commit();
            }
            catch (Exception ex)
            {
                manager.Rollback();                
                throw ex;
            }
        }

        private IList<MultimediaFileObject> FindMultimediaFileIndexes(FileTransferMultimedia fileTransfer)
        {            
            //return (from item in DataManager.IQ<MultimediaFileIndex>() where item.MultimediaFile.FileUniqueId == fileTransfer.FileUniqueId select item).ToList();
            return (from item in manager.Query<MultimediaFileObject>() where item.UniqueIdVersion == fileTransfer.UniqueIdVersion select item).ToList();
        }

        private IList<MultimediaFileObject> GenerateMultimediaFileIndexes(IEnumerable<String> all, IEnumerable<KeyValue> candidates, FileTransferMultimedia FileTransfer)
        {
            KeyValue winner = candidates.FirstOrDefault();

            IList<MultimediaFileObject> list = new List<MultimediaFileObject>();

            foreach (var item in all)
            {
                MultimediaFileObject mfi = GenerateMultimediaFileIndex(candidates, FileTransfer, winner, item);
                if (mfi.IsDefaultDocument)
                {
                    FileTransfer.DefaultDocument = mfi;
                    FileTransfer.DefaultDocumentPath = mfi.Fullname;
                }
                list.Add(mfi);
            }
            return list;
        }

        private MultimediaFileObject GenerateMultimediaFileIndex(IEnumerable<KeyValue> candidates, FileTransferMultimedia FileTransfer, KeyValue winner, string item)
        {
            KeyValue current = candidates.Where(x => x.Key == item).FirstOrDefault();
            MultimediaFileObject mfi = new MultimediaFileObject()
            {
                Fullname = NormalizeFullname(item),                
                //MultimediaFile = FileTransfer,
                IdItemTransfer = FileTransfer.Id,
                IdVersion = FileTransfer.IdVersion,
                UniqueIdItem = FileTransfer.UniqueIdItem,
                UniqueIdVersion = FileTransfer.UniqueIdVersion,
                IdItem = FileTransfer.IdItem,
                IsDefaultDocument = (winner != null && winner.Key == item),
                Probability = (current != null) ? current.Value : 0
            };
            return mfi;
        }

        private String NormalizeFullname(String fullname)
        {
            if (this.Platform.MultimediaFilePath.EndsWith("\\"))
                return fullname.Replace(this.Platform.MultimediaFilePath, "");
            else
                return fullname.Replace(this.Platform.MultimediaFilePath + "\\", "");
            //return fullname.Replace(this.Platform.RemoteFilePath + "\\", "");
        }

        private IList<String> CheckDirectories(String PackageName, IList<String> result, DirectoryInfo d)
        {
            //only on the root...
            //foreach (String f_item in CurrentApp.DefaultDocumentCandidates)
            //{
            //    String candidate = f_item.Replace("{package-name}", PackageName);
            //    DirectoryInfo[] di = d.GetDirectories(candidate, SearchOption.TopDirectoryOnly);

            //    foreach (var d_item in di)
            //    {
            //        result = CheckFiles(PackageName, result, d_item);
            //    }
            //}

            foreach (String f_item in Platform.MultimediaAnalysis.DirectoryCandidates)
            {
                String candidate = f_item.Replace("{package-name}", PackageName);
                DirectoryInfo[] di = d.GetDirectories(candidate, SearchOption.AllDirectories);

                foreach (var d_item in di)
                {
                    result = CheckFiles(PackageName, result, d_item);
                }
            }
            return result;
        }

        private IList<String> CheckFiles(String PackageName, IList<String> result, DirectoryInfo d)
        {
            //only on the root...
            //foreach (String item in CurrentApp.DefaultDocumentCandidates)
            //{
            //    String candidate = item.Replace("{package-name}", PackageName);
            //    result = SearchFiles(result, d, candidate, SearchOption.TopDirectoryOnly);
            //}

            foreach (String item in Platform.MultimediaAnalysis.DocumentCandidates)
            {
                String candidate = item.Replace("{package-name}", PackageName);
                result = SearchFiles(result, d, candidate, SearchOption.AllDirectories);
            }

            return result;
        }

        private IList<String> SearchFiles(IList<String> result, DirectoryInfo d, String candidate, SearchOption option)
        {
            FileInfo[] fi = d.GetFiles(candidate, option);
            if (fi.Length > 0)
            {
                var q = from fileinfo in fi select fileinfo.FullName;
                temp_result = temp_result.Concat(q);
                result = result.Concat(q).Distinct().ToList();
            }
            return result;
        }
    }
}
