using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public enum FieldType
    {
        None = 0,
        SingleLine = 1,
        MultiLine =2,
        Disclaimer =3,
        Mail = 4,
        TelephoneNumber=5,
        Date = 6,
        DateTime=7,
        Time =8,
        TaxCode= 9, //codice fiscale persona 
        ZipCode = 10,
        RadioButtonList = 11,
        DropDownList = 12,
        CheckboxList = 13,
        CompanyCode = 14, //codice R.E.A. per aziende
        CompanyTaxCode = 15, //codice fiscale azienda NB.Dal 2001 la partita iva ed il codice fiscale coincidono per i soggetti diversi dalle persone fisiche (società)che al momento di iniziare l'attività rilevanti ai fini IVA non possiedevano già un codice fiscale.
        VatCode = 16, //partita iva sia per professionisti che imprese (da quanto dedotto via internet)
        Name = 17,
        Surname = 18,
        Note = 19,
        FileInput = 20
    }
}