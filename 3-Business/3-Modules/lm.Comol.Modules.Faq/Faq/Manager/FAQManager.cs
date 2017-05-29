using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Cfg;

namespace lm.Comol.Modules.Faq
{
    public class FAQManager
    {
        FAQDal FAQ_Dal;

#region Read
        public FAQManager(ISession ComolSession)
        {
            FAQ_Dal = new FAQDal(ComolSession);
        }

        public UserDataModel GetUserDataModel(Int32 CommunityId)
        {
            UserDataModel UDM = new UserDataModel();
            UDM.Faqs = FAQ_Dal.GetFaqs(CommunityId);
            UDM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            UDM.Category.Add(DefaultCategory());
            UDM.CurrentCategory = DefaultCategory();
            return UDM;
        }

        public UserDataModel GetUserDataModel(Int32 CommunityId, Int64 CategoryId)
        {
            
            UserDataModel UDM = new UserDataModel();
            if (CategoryId > 0)
            { 
                UDM.Faqs = FAQ_Dal.GetFaqs(CommunityId, CategoryId);
                UDM.CurrentCategory = FAQ_Dal.GetCategory(CommunityId, CategoryId);
            }
            else {
                UDM.Faqs = FAQ_Dal.GetFaqs(CommunityId);
                UDM.CurrentCategory = DefaultCategory();
            }
            
            UDM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            //UDM.Category.Add(DefaultCategory());


            
            return UDM;
        }

        public EditFaqModel GetEditFaqModel(Int64 FaqId, Int32 CommunityId)
        {
            EditFaqModel EFM = new EditFaqModel();
            EFM.Faq = FAQ_Dal.GetFaq(FaqId);
            EFM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            return EFM;
        }

        public EditFaqModel GetNewFaqModel(Int32 CommunityId)
        {
            EditFaqModel EFM = new EditFaqModel();
            Faq NewFaq = new Faq();
            NewFaq.Question = "";
            NewFaq.Answer = "";
            NewFaq.CommunityId = CommunityId;
            NewFaq.onCategories = new List<Category>();
            EFM.Faq = NewFaq;

            EFM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            //EFM.Faq = FAQ_Dal.GetFaq(FaqId);
            //EFM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            return EFM;
        }

        
        public EditCategoryModel GetEditCategoryModel(Int32 CommunityId)
        {
            EditCategoryModel ECM = new EditCategoryModel();
            ECM.Categories = FAQ_Dal.GetCategoriesList(CommunityId);

            foreach (Category cat in ECM.Categories)
            {
                cat.Elements = (from Faq faq in FAQ_Dal.GetFaqs(CommunityId, cat.Id) select faq).Count();
            }

            return ECM;
        }

        public EditCategoryModel GetEditCategoryModel(Int32 CommunityId, Int64 CategoryId)
        {
            EditCategoryModel ECM = new EditCategoryModel();
            ECM.Categories = FAQ_Dal.GetCategoriesList(CommunityId);

            foreach (Category cat in ECM.Categories)
            {
                cat.Elements = (from Faq faq in FAQ_Dal.GetFaqs(CommunityId, cat.Id) select faq).Count();
            }

            ECM.CategoryForModify = FAQ_Dal.GetCategory(CommunityId, CategoryId);

            return ECM;
        }

#endregion
#region Update/Save

        /// <summary>
        /// Utilizzato sia per modificare Domanda/risposta
        /// che per aggiornare la lista di categorie associate.
        /// </summary>
        /// <param name="Faq"></param>
        /// <remarks>
        /// Verificare la struttura in relazione ad nHibernate, Id ed affini...
        /// </remarks>
        public Enum.ErrorCode UpdateFaq(Faq Faq)
        {
            Enum.ErrorCode ErrorCode = Enum.ErrorCode.none;
            if (Faq.onCategories.Count > 0)
            {
                FAQ_Dal.UpdateFaq(Faq);
            } else
            {
                ErrorCode = Enum.ErrorCode.NoCategory;
            }

            return ErrorCode;
        }

        public Enum.ErrorCode UpdateFaq(Int64 Id, String Question, String Answer)
        {
            Enum.ErrorCode ErrorCode = Enum.ErrorCode.none;

            if ((Question == null) || (Question == "") || (Answer == null) || (Answer == ""))
            {
                ErrorCode = Enum.ErrorCode.NoData;
            }
            else
            {
                Faq Faq = FAQ_Dal.GetFaq(Id);
                Faq.Question = Question;
                Faq.Answer = Answer;
                FAQ_Dal.UpdateFaq(Faq);
            }

            return ErrorCode;
        }

        public Enum.ErrorCode UpdateFaq(Int64 Id, String Question, String Answer, IList<Int64> CategoriesId)
        {
            Enum.ErrorCode ErrorCode = Enum.ErrorCode.none;

            if ((Question == null) || (Question == "") || (Answer == null) || (Answer == ""))
            {
                ErrorCode = Enum.ErrorCode.NoData;
            }
            else if (CategoriesId.Count < 1)
            {
                ErrorCode = Enum.ErrorCode.NoCategory;
            }
            else
            {
                Faq Faq = FAQ_Dal.GetFaq(Id);
                Faq.Question = Question;
                Faq.Answer = Answer;
                Faq.onCategories = FAQ_Dal.GetCategoriesList(CategoriesId);
                FAQ_Dal.UpdateFaq(Faq);
            }

            return ErrorCode;
        }


        public Faq CreateFaq(Int32 CommunityId, String Question, String Answer, IList<Int64> CategoriesId)
        {
            Faq newFaq = new Faq();

            newFaq.CommunityId = CommunityId;
            newFaq.Question = Question;
            newFaq.Answer = Answer;

            if (CategoriesId.Count > 0)
            {
                newFaq.onCategories = FAQ_Dal.GetCategoriesList(CategoriesId);
            }

            FAQ_Dal.AddFaq(newFaq);
            return newFaq;
        }

        public void UpdateCategory(Category Category)
        {
            FAQ_Dal.UpdateCategory(Category);
        }

        public void UpdateCategory(Int64 CategoryId, String CategoryName, Int32 CommunityId)
        {
            Category cat = FAQ_Dal.GetCategory(CommunityId, CategoryId);
            cat.Name = CategoryName;
            FAQ_Dal.UpdateCategory(cat);
        }

        public void CreateCategory(Category Category)
        {
            FAQ_Dal.AddCategory(Category);
        }

        public Enum.ErrorCode CreateCategory(String CategoryName, Int32 CommunityId)
        {
            if (CategoryName == "")
            {
                return Enum.ErrorCode.NoData;
            }
            else
            {
                Category Cat = new Category();
                Cat.Name = CategoryName;
                Cat.CommunityId = CommunityId;
                FAQ_Dal.AddCategory(Cat);
                return Enum.ErrorCode.none;
            }
        }

#endregion
#region Delete
        public void DeleteFaq(Int64 FaqId)
        {
            FAQ_Dal.DeleteFaq(FaqId);
        }

        public void DeleteFaq(Faq faq)
        {
            FAQ_Dal.DeleteFaq(faq);
        }

        public Enum.ErrorCode DeleteCategory(Int32 CommunityId, Int64 CategoryId)
        {
            Category Cat = FAQ_Dal.GetCategory(CommunityId, CategoryId);

            if (Cat != null)
            {
                //FAQ_Dal.DeleteFaqOnCategory(CategoryId);
                //Cat.Faqs = FAQ_Dal.GetFaqs(CommunityId, CategoryId);

                //if (Cat.Faqs.Count > 0)
                //{
                //    return Enum.ErrorCode.CategoryWithFaq;
                //}
                //else
                //{
                    FAQ_Dal.DeleteCategory(Cat);
                //    FAQ_Dal.DeleteOrphanFaq(CategoryId);
                //}
            }
            else 
            {
                return Enum.ErrorCode.NoData;
            }

            return Enum.ErrorCode.none;
            
        }

#endregion

        private Category DefaultCategory()
        {
            Category cat = new Category();
            cat.Name = "All";
            cat.Id = -1;
            return cat;
        }
    }
}
