var appStatRep = angular.module('appMoocsStats', []);

appStatRep.filter('uniqueTags', function () {
    return function (items, filterOn) {
        if (filterOn === false) {
            return items;
        }
        if ((filterOn || angular.isUndefined(filterOn)) && angular.isArray(items)) {
            var hashCheck = {}, newItems = [];
            var extractValueToCompare = function (item) {
                if (angular.isObject(item) && angular.isString(filterOn)) {
                    return item[filterOn];
                } else {
                    return item;
                }
            };

            angular.forEach(items, function (item) {
                angular.forEach(item.Tags, function (tag) {
                    var valueToCheck, isDuplicate = false;
                    for (var i = 0; i < newItems.length; i++) {
                        if (angular.equals(extractValueToCompare(newItems[i]), extractValueToCompare(tag))) {
                            isDuplicate = true;
                            break;
                        }
                    }
                    if (!isDuplicate) {
                        newItems.push(tag);
                    }
                });
            });
            items = newItems;
        }
        return items;
    };
});
appStatRep.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});
appStatRep.filter('filterTags', function () {
    return function (items, compTags) {
        if (!compTags || compTags.length == 0)
            return items;

        var filtered = [];
        var filteredId = [];
        angular.forEach(items, function (item) {
            angular.forEach(compTags, function (tag) {
                if (item.Tags && item.Tags.indexOf(tag) >= 0) {
                    if (filteredId.indexOf(item.Id) < 0) {
                        filtered.push(item);
                        filteredId.push(item.Id);
                    }
                }
            });
        });
        filteredId = null;
        items = filtered;
        return items;
    };
});


appStatRep.controller('MasterCtrl', ['$scope', '$http', '$timeout', '$filter', function ($scope, $http, $timeout, $filter) {
    var urlWebApiTemp = location.href.replace("//", "§§");
    urlWebApiTemp = ((urlWebApiTemp.indexOf("/") > -1) ? urlWebApiTemp.split("/")[0] : urlWebApiTemp).replace("§§", "//") + "/WebAPI/";
    var RootPathWebApi = "";
    if (urlWebApiTemp.indexOf("://localhost") > -1) {
        RootPathWebApi = (urlWebApiTemp.indexOf("s://localhost") > -1) ? "https://localhost/AdevicoWeb/LMSAPI/" : "http://localhost/AdevicoWeb/LMSAPI/";
    } else {
        RootPathWebApi = urlWebApiTemp;
    }
    $scope.myFormatDate = function (d) {
        return new Date(d);
    };

    $scope.loadingMoocsInfoResources = false;
    $scope.loadingAttivitaComunitaResources = false;
    $scope.loadingModuleActionResources = false;
    $scope.moocsInfo = null;
    $scope.moocsAttivitaInfo = null;
    $scope.moocsModuleAction = null;

    $scope.last30days = [];
    for (var i = 0; i < 30; i++) {
        var datenow = new Date();
        datenow.setDate(datenow.getDate() - i);
        $scope.last30days.push(datenow);
    };
    $scope.Init = function () {
        $scope.CheckMoocsInfo();
        $scope.CheckAttivitaComunitaInfo();
        $scope.CheckModuleActionInfo();
    };
    $scope.formatInDate = function (d) {
        if (!angular.isDate(d))
            return new Date(d);

        return d;
    };
    $scope.CompareDates = function (d) {
        if (!$scope.moocsAttivitaInfo)
            return;
        if (!$scope.moocsAttivitaInfo.DatesList || !$scope.moocsAttivitaInfo.DatesList.length)
            return;


        if (!angular.isDate(d))
            d = new Date(d);

        d = new Date(d.setHours(0, 0, 0, 0));

        for (var i = 0; i < $scope.moocsAttivitaInfo.DatesList.length; i++) {
            var d1 = new Date(new Date($scope.moocsAttivitaInfo.DatesList[i]).setHours(0, 0, 0, 0));
            if (angular.equals(d1, d))
                return "active";
        }

        return;
    };
    $scope.calcolareStriciaCorrente = function (d) {
        if (!$scope.moocsAttivitaInfo || !$scope.moocsAttivitaInfo.DatesList)
            return "";

        if ($scope.moocsAttivitaInfo.DatesList.length > 1) {
            var numberIteration = 1;
            var record = 1;
            for (var i = $scope.moocsAttivitaInfo.DatesList.length - 1; i > 0; i--) { // concludo a 1 per controllare quello precedente(0)
                var previusDate = new Date(new Date($scope.moocsAttivitaInfo.DatesList[i - 1]).setHours(0, 0, 0, 0));
                var currentDate = new Date(new Date($scope.moocsAttivitaInfo.DatesList[i]).setHours(0, 0, 0, 0));
                var currentDateLessOneDay = new Date(new Date($scope.moocsAttivitaInfo.DatesList[i]).setHours(0, 0, 0, 0));
                currentDateLessOneDay.setDate(currentDate.getDate() - 1);

                if (angular.equals(currentDateLessOneDay, previusDate)) {
                    numberIteration++;
                    record = (numberIteration > record) ? numberIteration : record;
                }
                else {
                    numberIteration = 1;
                }
            }
            return record;
        }

        if ($scope.moocsAttivitaInfo.DatesList.length > 0)
            return 1

        return "-";
    };
    $scope.CheckAttivitaComunitaInfo = function () {
        $scope.loadingAttivitaComunitaResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/UserComAccessStat",
            headers: {}
        };
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingAttivitaComunitaResources = false;
            $scope.moocsAttivitaInfo = response.data;
        }, function myError(response) {
            $scope.errorManager(response);

            $scope.loadingAttivitaComunitaResources = false;
        });
    };
    $scope.CheckModuleActionInfo = function () {
        $scope.loadingModuleActionResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/ModuleAction" + "?take=4",
            headers: {}
        };
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingModuleActionResources = false;
            $scope.moocsModuleAction = response.data;
        }, function myError(response) {
            $scope.errorManager(response);
            $scope.loadingModuleActionResources = false;
        });
    };
    $scope.CheckMoocsInfo = function () {
        $scope.loadingMoocsInfoResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/MoocStat",
            headers: {}
        };
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingMoocsInfoResources = false;
            $scope.moocsInfo = response.data;
        }, function myError(response) {
            $scope.errorManager(response);

            $scope.loadingMoocsInfoResources = false;
        });
    };
    $scope.GetMoocsStatus = function (moocsInfo) {
        if (!moocsInfo)
            return;

        if (moocsInfo.mType == 3)
            if (moocsInfo.mookCompleted)
                return "Certification CompletedPassed"
            else
                return "Certification"

        if (moocsInfo.mookCompleted) {
            if (moocsInfo.mType == 2 && moocsInfo.Completion == 100)
                return "CompletedPassed gold";
            else
                return "CompletedPassed";
        }

        if (moocsInfo.Completion > 0)
            return "BrowsedStarted";

    };
    $scope.errorManager = function (response) {
        if (response.status + "" === "401" || response.status + "" === "403" || response.status + "" === "405") {
            $timeout(function () {
                $scope.PermissionsError = true;
            });
            $timeout(function () {
                jQuery('.modal.fade.in').trigger({ type: "click" });
            }, 1000);
        }
    };
    $scope.dateDiffarence = function (date1, date2) {
        var d1 = null;
        var d2 = null;
        if (!date1)
            return;
        else
            d1 = date1;
        if (!date2)
            d2 = new Date();
        else
            d2 = date2;

        if (!angular.isDate(d1)) {
            d1 = new Date(d1);
            d1.setHours(d1.getHours() + (d1.getTimezoneOffset() / 60))
        }
        if (!angular.isDate(d2)) {
            d2 = new Date(d2);
            d2.setHours(d2.getHours() + (d2.getTimezoneOffset() / 60))
        }

        var miliseconds = Math.abs(d2 - d1);
        var days = (miliseconds / (1000 * 3600 * 24));
        var daysFloor = Math.floor(days);
        if (daysFloor > 60) {
            return "mesi fa";
        }
        if (daysFloor > 30) {
            return "il mese scorso";
        }

        if (daysFloor > 7) {
            return "più di una settimana";
        }
        if (daysFloor > 1) {
            return "alcuni giorni fa";
        }

        if (daysFloor > 0) {
            return "ieri";
        }

        var daysRemainder = days - daysFloor;
        var hours = daysRemainder * 24;
        var hoursFloor = Math.floor(hours);

        var hoursRemainder = hours - hoursFloor;
        var minutes = hoursRemainder * 60;
        var minutesFloor = Math.floor(minutes);

        var minutesRemainder = minutes - minutesFloor;
        var seconds = minutesRemainder * 60;
        var secondsFloor = Math.floor(seconds);


        var returnStr = "";
        if (hoursFloor > 0) {
            returnStr += hoursFloor + "h ";
        }
        if (minutesFloor > 0) {
            returnStr += minutesFloor + "m ";
        }
        if (secondsFloor > 0) {
            returnStr += secondsFloor + "s ";
        }
        return returnStr;
    };
    $scope.Init();
}]);

function downloadClientFile(content, fileName, mimeType) {
    var a = document.createElement('a');
    mimeType = mimeType || 'application/octet-stream';

    if (navigator.msSaveBlob) { // IE10
        return navigator.msSaveBlob(new Blob([content], { type: mimeType }), fileName);
    } else if ('download' in a) { //html5 A[download]
        a.href = 'data:' + mimeType + ',' + encodeURIComponent(content);
        a.setAttribute('download', fileName);
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        return true;
    } else { //do iframe dataURL download (old ch+FF):
        var f = document.createElement('iframe');
        document.body.appendChild(f);
        f.src = 'data:' + mimeType + ',' + encodeURIComponent(content);

        setTimeout(function () {
            document.body.removeChild(f);
        }, 100);
        return true;
    }
}