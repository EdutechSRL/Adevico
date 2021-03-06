﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public interface IViewInternalLogin : IViewLogin
    {
        Boolean AllowRetrievePassword { set; }
        Boolean AllowExternalWebAuthentication { set; }
        Boolean AllowSubscription { set; }
        
        Boolean HasUrlValues { get; }
        Boolean SubscriptionActive { get; }

        dtoUrlToken GetUrlToken(List<String> identifiers);
        void GoToProfile(long idProvider, dtoUrlToken urlToken, string wizardUrl);
        void GotoRemoteUrl(String url);

        void DisplayAccountDisabled(String url);
        void DisplayLogonInput();
        void DisplayInvalidCredentials();
        void SetExternalWebLogonUrl(String url);
    }
}
