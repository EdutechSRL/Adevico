﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public enum ProfileAttributeType
    {
        skip = -1,
        unknown = 0,
        externalId = 1,
        name = 3,
        surname = 4,
        mail = 5,
        telephoneNumber = 6,
        fax = 7,
        mobile = 9,
        taxCode = 10,
        login = 11,
        password = 12,
        companyName = 16,
        companyAddress = 17,
        companyCity = 18,
        companyRegion = 19,
        companyTaxCode = 20,
        externalUserInfo = 27,
        address = 29,
        zipCode = 30,
        city = 31,
        autoGeneratedLogin = 32,
        companyReaNumber = 33,
        companyAssociations = 34,
        webSite = 35,
        agencyExternalCode = 36,
        agencyTaxCode = 37,
        agencyNationalCode = 38,
        agencyName = 39,
        agencyInternalCode = 40,
        language = 41,

        job = 42,
        sector = 43,
        birthDate = 44,
        birthPlace = 45
    }
}