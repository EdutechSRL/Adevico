using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.File
{
    public class Zip
    {
        
        public static FileMessage ZipFilesRename(List<dtoFileToZip> zipFiles, String OutputFile)
        {
            Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                 if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    return FileMessage.ImpersonationFailed;
                }
                else
                {
                    //ToDo: zip files...
                    return FileMessage.None;
                }
            }
            catch
            {
                if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                return FileMessage.Catch;
            }
            finally
            {
                if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
            }
        }
    }
}