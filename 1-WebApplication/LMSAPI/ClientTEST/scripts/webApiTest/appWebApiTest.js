var appStatRep = angular.module('appWebApiTest', []);

appStatRep.controller('MasterCtrl', ['$scope', '$http', function ($scope, $http) {

    var urlWebApiTemp = location.href.replace("//", "§§");
    urlWebApiTemp = ((urlWebApiTemp.indexOf("/") > -1) ? urlWebApiTemp.split("/")[0] : urlWebApiTemp).replace("§§", "//") + "/WebAPI/";
    var RootPathWebApi = "";
    if (urlWebApiTemp.indexOf("://localhost") > -1) {
        RootPathWebApi = (urlWebApiTemp.indexOf("s://localhost") > -1) ? "https://localhost/AdevicoWeb/LMSAPI/" : "http://localhost/AdevicoWeb/LMSAPI/";
    } else {
        RootPathWebApi = urlWebApiTemp;
    }

    $scope.loadingEntities = false;

    $scope.ListaUrlWebApi = [RootPathWebApi];
    $scope.RootPathWebApiTemp = $scope.ListaUrlWebApi[0];
    $scope.RootPathWebApi = $scope.ListaUrlWebApi[0];
    $scope.ListaWebApiName = ["api/Role", "api/Repository", "api/UserComAccessStat", "api/Community", "api/MoocStat", "api/Person", "api/Notification", "api/NoticeBoard", "api/NoticeBoardPlain", "api/Permission", "api/ScormStat", "api/Document", "api/ModuleAction"]; //Sarebbe carino fare come per ListaUrlWebAPI...
    $scope.WebApiNameTemp = $scope.ListaWebApiName[0];
    $scope.WebApiName = $scope.ListaWebApiName[0];

    $scope.Token = "240e67f4-bdeb-4c00-87f0-c77a36d35de2";
    $scope.DeviceId = "ce4bad76-39ae-4b2e-8d45-af47cb033cb4";
    $scope.CommunityId = 2;
    $scope.Response = "Click GO!";
    $scope.Headers = '{"ex": "string", "ex2": 10}';
    $scope.Data = "";
    $scope.Method = "GET";
    $scope.Params = "";

    $scope.ListaEntities = [];
    $scope.Init = function () {
        //$scope.CheckUserRolesUpdates();
    };

    $scope.CheckEntityUpdates = function () {
        $scope.loadingEntities = true;

        var options = {
            method: $scope.Method,
            url: $scope.RootPathWebApi + $scope.WebApiName + $scope.Params
        };
        if ($scope.Headers == "") {
            $scope.Headers = "{}";
        }
        options.headers = JSON.parse($scope.Headers);
        options.headers.Token = $scope.Token;
        options.headers.DeviceId = $scope.DeviceId;
        options.headers.CommunityId = $scope.CommunityId;

        if ($scope.Data != "" && $scope.Data != "{}")
            options.data = JSON.parse($scope.Data);
        
        $http(options).then(function mySucces(response) {
            $scope.loadingEntities = false;
            $scope.Response = formatResponse(response);
        }, function myError(response) {
            $scope.loadingEntities = false;
            $scope.Response = formatResponse(response);
        });
    };

    $scope.Init();
    $scope.changeRootUrl = function () {
        $scope.RootPathWebApi = $scope.RootPathWebApiTemp;
    };

    $scope.changeApiName = function () {
        $scope.WebApiName = $scope.WebApiNameTemp;
    };
    
}]);

var formatResponse = function (response) {
    response = JSON.stringify(response, null, 4);
    return response;
};