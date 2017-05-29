using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Faq;


namespace Adevico.Modules.Faq.Presentation
{
    /// <summary>
    /// Interfaccia implementata dalla view.
    /// Contiene tutti i metodi e le proprietà che sono manipolati
    /// e/o controllati dal presenter.
    /// 
    /// Di norma i parametri sono parametri come Id Oggetto, mentre le proprietà di un oggetto, come il nome,
    /// vengono passate via dto alle funzioni del presenter.
    /// 
    /// Le funzioni, invece, sono del tipo "Mostra questo", che puo' essere la lista di oggetti da visualizzare
    /// o messaggi di errore.
    /// 
    /// </summary>
    public interface IVIewFaqList : iDomainView
    {
        void LoadFaq(IList<DTO_Faq> Faqs, IList<DTO_Category> Cats, lm.Comol.Modules.Base.DomainModel.ModuleFaq oModule);  
        //object sarà un dto FAQ o simile. 
        //La View, ovvero la pagina si incaricherà di gestire la lista e farne il render correttamente.
        //i dto passati alla vista conterranno TUTTI i dati che servono alla vista stessa.
        //Ad esempio potranno contenere un flag per indicare SE mostrare o meno l'edit del singolo elemento, ma non il "deleted". (Solo un esempio. I permessi in questo caso sono a livello di pagina, mentre gli elementi "deleted" potrebbero NON essere visualizzati).


        Int64 CurrentCategoryId { get; set; }

        void ShowNoData();



        void ShowFaqModify(DTO_Faq faq);
        void ShowCatModify(DTO_Category cat);

        void ShowFaqModified(DTO_Faq faq, bool hasBeenModified);
        void ShowFaqDeleted(bool hasBeenDeleted);
        void ShowFaqInserted(DTO_Faq faq, bool hasBeenInserted);

        void ShowCatModified(DTO_Category cat, bool hasBeenModified);
        void ShowCatDeleted(bool hasBeenDeleted);
        void ShowCatInserted(DTO_Category cat, bool hasBeenInserted);
        void ShowNoPermission();


    }
}
