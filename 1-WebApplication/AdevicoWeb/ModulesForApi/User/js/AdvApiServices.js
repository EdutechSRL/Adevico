appUsersManagement.factory('FactoryLMSAPI', ['$http', '$rootScope', function ($http, $rootScope) {
    var urlRoot = LMSAPI_URL;
    var urlCommunityAPI = "api/Community";
    var urlCommunityUsersAPI = "api/Person";
    var FactoryLMSAPI = {};
    var DefaultHeader = function () {
        this.LanguageId = getCookie("LanguageId");
        this.Token = getCookie("Token");
        this.DeviceId = getCookie("DeviceId");
        this.CommunityId = getCookie("CommunityId");
        this.BehaviorCode = 0;
    }
    var option = {
        method: "",
        url: "",
        headers: new DefaultHeader()
    };

    // Community
    FactoryLMSAPI.getCommunities = function (behaviorCode) {
        option.url = urlRoot + urlCommunityAPI;
        option.method = "GET";
        option.headers = new DefaultHeader();
        if (behaviorCode)
            option.headers.BehaviorCode = behaviorCode;
        return $http(option);
    };
    // Community Users
    FactoryLMSAPI.getCommunityUsers = function (communityId, behaviorCode) {
        option.url = urlRoot + urlCommunityUsersAPI + "?communityId=" + communityId;
        option.method = "GET";
        option.headers = new DefaultHeader();
        if (behaviorCode)
            option.headers.BehaviorCode = behaviorCode;
        return $http(option);
    };
    FactoryLMSAPI.setRoles = function (userIds, newRoleId) {
        if (!angular.isArray(userIds))
            throw "userIds is not array";
        if (!newRoleId)
            throw "newRoleId is not valid";

        option.url = urlRoot + "api/SetRoles";
        option.method = "POST";
        option.headers = new DefaultHeader();
        option.data = {
            UsersId : userIds,
            NewRoleId : newRoleId,
            CommunityId : option.headers.CommunityId
        }
        return $http(option);
    };

    FactoryLMSAPI.modifySubscription = function (userIds, actionCode) {
        //var actionCode = { none = 0, Enable = 1, Disable = 2, Delete = 3 };
        if (!angular.isArray(userIds))
            throw "userIds is not array";

        option.url = urlRoot + "api/ModifySubscription";
        option.method = "POST";
        option.headers = new DefaultHeader();
        option.data = {
            UsersId: userIds,
            Action: actionCode,
            CommunityId: option.headers.CommunityId
        }
        return $http(option);
    };
    FactoryLMSAPI.setResponsible = function (userId) {
        if (!userId)
            throw "userIds is not valid";

        option.url = urlRoot + "api/SetResponsible";
        option.method = "POST";
        option.headers = new DefaultHeader();
        option.data = {
            UserId: userId,
            CommunityId: option.headers.CommunityId
        }
        return $http(option);
    };

    FactoryLMSAPI.getUserInfo = function (userId, serviceCode) {
        if (!userId)
            throw "userIds is not valid";

        option.url = urlRoot + "api/GetPersonInfo?UserId=" + userId + "&CommunityId=" + option.headers.CommunityId + ((serviceCode) ? "&serviceCode=" + serviceCode :"");
        option.method = "GET";
        option.headers = new DefaultHeader();
        return $http(option);
    };

    FactoryLMSAPI.setExpiryStart = function (userIds, setVoid, startDateTime) {
        if (!angular.isArray(userIds))
            throw "userIds is not array";

        option.url = urlRoot + "api/SetExpiryStart";
        option.method = "POST";
        option.headers = new DefaultHeader();
        option.data = {
            UsersId: userIds,
            SetVoid: setVoid,
            CommunityId: option.headers.CommunityId
        }
        if (!setVoid && startDateTime)
            if (angular.isDate(startDateTime))
                option.data.StartDateTime = startDateTime.toISOString();
            else
                option.data.StartDateTime = startDateTime;

        return $http(option);
    };

    FactoryLMSAPI.modifyExpiration = function (userIds, _validity, _extendValidity, _startBehaviour) {
        if (!angular.isArray(userIds))
            throw "userIds is not array";

        option.url = urlRoot + "api/ModifyExpiration";
        option.method = "POST";
        option.headers = new DefaultHeader();
        option.data = {
            UsersId: userIds,
            validity: _validity,
            extendValidity: _extendValidity,
            startBehaviour: _startBehaviour,
            communityId: option.headers.CommunityId
        }

        return $http(option);
    };
    return FactoryLMSAPI;
}]);

/*
{

    {
        "UsersId": [
            1,
            2
        ],
            "NewRoleId": 1,
                "CommunityId": 2
    }
}
 */



/* cookie */
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie() {
    var user = getCookie("username");
    if (user != "") {
        alert("Welcome again " + user);
    } else {
        user = prompt("Please enter your name:", "");
        if (user != "" && user != null) {
            setCookie("username", user, 365);
        }
    }
}
function getToken() {
    return getCookie("Token");
}
function getDeviceId() {
    return getCookie("DeviceId");
}

/* end cookie */