using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;


/*Usando le vecchie calssi*/
using lm.Comol.Modules.Standard.Faq;
using lm.Comol.Modules.Base.DomainModel;
using COL_BusinessLogic_v2.UCServices;

namespace Adevico.Modules.Faq.Presentation
{
    public class FaqListPresenter : DomainPresenter
    {
        private ModuleFaq _oModule = null;
        private ModuleFaq oModule
        {
            get
            {
                if (_oModule == null)
                {
                    if (UserContext.CurrentCommunityID == 0)
                    {
                        _oModule = ModuleFaq.CreatePortalmodule(UserContext.CurrentUser.TypeID);
                    }
                    else
                    {
                        _oModule = (from sb in COL_BusinessLogic_v2.Comol.Manager.ManagerPersona.GetPermessiServizio(
                             UserContext.CurrentUserID, Services_Faq.Codex)
                                    where (sb.CommunityID == UserContext.CurrentCommunityID)
                                    select new ModuleFaq(new Services_Faq(sb.PermissionString)
                                        )
                            ).FirstOrDefault();
                    }
                }
                return _oModule;
            }
        }

        #region Initialize

        public FaqListPresenter(iApplicationContext oContext) : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public FaqListPresenter(iApplicationContext oContext, iDomainView view) : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #region FAQ Service - accesso ai dati - EVENTUALMENTE RIFARE!
        private lm.Comol.Modules.Standard.Faq.FAQService _service;
        
        private lm.Comol.Modules.Standard.Faq.FAQService service
        {
            get
            {
                if(_service == null)
                    _service = new FAQService(AppContext, AppContext.DataContext.GetCurrentSession());
                return _service;
            }
        }
        #endregion



        protected virtual IVIewFaqList View
        {
            get { return (IVIewFaqList)base.View; }
        }

        #endregion Initialize


        public void InitView()
        {
            //Carico dati 
            int PersonId = UserContext.CurrentUserID;
            int CommunityId = UserContext.CurrentCommunityID;


            //Check Permission: ToDo

            Int32 UserTypeId = -1;
            
            //Se <0 carica le FAQ di TUTTE le categorie
            Int64 CategoryId = View.CurrentCategoryId;

            IList<DTO_Faq> Faqs = service.GetFaqList(CategoryId);   //La comunità è automaticamente utilizzata quella corrente
            IList<DTO_Category> Cats = service.GetCategories();
            if (!oModule.ViewFaq)
                View.ShowNoPermission();
            else
                View.LoadFaq(Faqs, Cats, oModule);
                
        }

        private List<DTO_Category> CovnertCategoryInDTO(IList<Category> listcat)
        {
            return listcat.Select(c => new DTO_Category() { ID = c.Id, Name = c.Name }).ToList();
        }
        private List<Int64> CovnertDTO_CategoryInListInt64(IList<DTO_Category> listcat)
        {
            return listcat.Select(c => c.ID).ToList();
        }
        public void UpdateOrderFaqs(List<DTO_Faq> faqs)
        {
            foreach (DTO_Faq faq in faqs)
            {
                service.UpdateFaq(faq.ID, faq.Question, faq.Answer, this.CovnertDTO_CategoryInListInt64(faq.Categories), faq.Order);
            }

            this.InitView();
        }
        public void InsertFaq(DTO_Faq faq)
        {
            try
            {
                if (!oModule.Admin && !oModule.CreateFaq)
                    throw new Exception("No permission");

                service.CreateFaq(UserContext.CurrentCommunityID, faq.Question, faq.Answer, this.CovnertDTO_CategoryInListInt64(faq.Categories));

                View.ShowFaqInserted(faq, true);
            }
            catch { View.ShowFaqInserted(faq, false); }

        }       

        public void InsertFaqCategory(DTO_Category cat)
        {
            try
            {
                if (!oModule.Admin && !oModule.ManageCategory)
                    throw new Exception("No permission");

                service.CreateCategory(cat.Name, UserContext.CurrentCommunityID);
                View.ShowCatInserted(cat, true);
            }
            catch { View.ShowCatInserted(cat,false); }
        }

        public void DeleteFaq(Int64 FaqId)
        {
            //gestione errori...
            try
            {
                if (!oModule.Admin && !oModule.DeleteFaq)
                    throw new Exception("No permission");

                service.DeleteFaq(FaqId);
                View.ShowFaqDeleted(true);
            }
            catch { View.ShowFaqDeleted(false); }
        }
        
        public void ModifyFaq(DTO_Faq faq)
        {
            //gestione errori...            
            try
            {
                if (!oModule.Admin && !oModule.ModifyFaq)
                    throw new Exception("No permission");

                service.UpdateFaq(faq.ID, faq.Question, faq.Answer, this.CovnertDTO_CategoryInListInt64(faq.Categories), faq.Order);
                View.ShowFaqModified(faq, true);
            }
            catch { View.ShowFaqModified(faq, false); }
        }

        public void DeleteCat(Int64 CatId)
        {
            //gestione errori...
            try
            {
                if (!oModule.Admin && !oModule.ManageCategory)
                    throw new Exception("No permission");

                service.DeleteCategory(UserContext.CurrentCommunityID, CatId);
                View.ShowCatDeleted(true);
            }
            catch { View.ShowCatDeleted(false); }
        }

        public void ModifyCat(DTO_Category cat)
        {
            //gestione errori...
            try
            {
                if (!oModule.Admin && !oModule.ManageCategory)
                    throw new Exception("No permission");

                service.UpdateCategory(cat.ID, cat.Name, UserContext.CurrentCommunityID);
                View.ShowCatModified(cat, true);
            }
            catch { View.ShowCatModified(cat, false); }


        }
        public List<DTO_Faq> GetAllFaqs()
        {
            return service.GetFaqList(-1);
        }
        public void SetFaqModify(Int64 faqId){
            try
            {
                View.ShowFaqModify(service.GetFaq(faqId));
            }
            catch { View.ShowFaqModify(null); }
        }


        public void SetCatModify(Int64 catId)
        {
            try
            {
                View.ShowCatModify(service.GetCategories().Where(c => c.ID == catId).FirstOrDefault());
            }
            catch { View.ShowCatModify(null); }
        }
    }
}
