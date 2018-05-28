var appUsersManagement = angular.module('appUsersManagement', []);

appUsersManagement.filter('uniqueTags', function () {
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

appUsersManagement.filter('isInArray', function () {
    return function (items, arr, filterOn, nullValuePass) {
        nullValuePass = nullValuePass ? true : false;
        if (!items || !arr || !angular.isArray(arr) || arr.length < 1 || !filterOn || !angular.isArray(items)) {
            return items;
        }

        var newItems = [];

        angular.forEach(items, function (item) {
            var val = item[filterOn];
            if ((nullValuePass && !val) || (val && arr.indexOf(val) >= 0))
                newItems.push(item);
        });
        return newItems;
    };
});
appUsersManagement.filter('isInArrayScadTypes', function () {
    return function (items, arr) {
        if (!items || !arr || !angular.isArray(arr) || arr.length < 1 || !angular.isArray(items)) {
            return items;
        }

        var newItems = [];
        angular.forEach(items, function (item) {
            var getIt = false;

            if (!getIt && arr.indexOf("Illimitati") > -1 && item["DurationDay"] < 0)
                getIt = true;
            if (!getIt && arr.indexOf("NonIniziato") > -1 && !item["StartDate"])
                getIt = true;
            if (!getIt && arr.indexOf("InCorso") > -1 && (item["StartDate"] && !item["Expired"]))
                getIt = true;
            if (!getIt && arr.indexOf("Scaduti") > -1 && item["Expired"])
                getIt = true;

            if (getIt)
                newItems.push(item);
        });
        return newItems;
    };
});
appUsersManagement.filter('filterTags', function () {
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

LMSAPI_URL = "";
var urlWebApiTemp = location.href.replace("//", "§§");
urlWebApiTemp = ((urlWebApiTemp.indexOf("/") > -1) ? urlWebApiTemp.split("/")[0] : urlWebApiTemp).replace("§§", "//") + "/WebAPI/";
if (urlWebApiTemp.indexOf("://localhost") > -1) {
    LMSAPI_URL = (urlWebApiTemp.indexOf("s://localhost") > -1) ? "https://localhost/AdevicoWeb/LMSAPI/" : "http://localhost/AdevicoWeb/LMSAPI/";
} else {
    LMSAPI_URL = urlWebApiTemp;
}

appUsersManagement.controller('MasterCtrl', ['$scope', '$http', '$timeout', '$filter', 'FactoryLMSAPI', function ($scope, $http, $timeout, $filter, FactoryLMSAPI) {
    $scope.PermissionsError = false;
    $scope.CountTypeScad = {
        Illimitati: 0,
        NonIniziato: 0,
        InCorso: 0,
        Scaduti: 0,
    }
    $scope.DelaySubconfLocal = DelaySubconfLocal;
    $scope.userInfosel = null;

    $scope.CommunityUsersLimitTo = 10;
    $scope.showMoreCommunityUsersLimitTo = function () {
        var num = $scope.CommunityUsersLimitTo + 50;
        if (num > $scope.CommunityUsersLimitTo.length - 1)
            num = $scope.CommunityUsersLimitTo.length - 1;
        $scope.CommunityUsersLimitTo = num;
    };
    $scope.TipiScadenze = [
        {
            code: "NonIniziato",
            label: "Non iniziato",
            customClasses: "label-primary"
        },
        {
            code: "Illimitati",
            label: "Illimitati",
            customClasses: "label-success"
        },
        {
            code: "InCorso",
            label: "In corso",
            customClasses: "label-warning"
        },
        {
            code: "Scaduti",
            label: "Scaduti",
            customClasses: "label-danger"
        }
    ];
    $scope.resetCommunityUsersLimitTo = function () {
        $scope.CommunityUsersLimitTo = 10;
    };
    $scope.CommunityRolesSel = [];
    $scope.toggleFilterRole = function (roleId) {
        var indexEl = $scope.CommunityRolesSel.indexOf(roleId);
        if (indexEl > -1)
            $scope.CommunityRolesSel.splice(indexEl, 1);
        else
            $scope.CommunityRolesSel.push(roleId);
    };
    $scope.ScadenzaStatesSel = [];
    $scope.toggleScadenzaStatesSel = function (stateCode) {
        if (!stateCode) {
            $scope.ScadenzaStatesSel = [];
            return;
        }

        var indexEl = $scope.ScadenzaStatesSel.indexOf(stateCode);
        if (indexEl > -1)
            $scope.ScadenzaStatesSel.splice(indexEl, 1);
        else
            $scope.ScadenzaStatesSel.push(stateCode);
    };
    $scope.CommunityUsersSel = [];
    $scope.toggleUser = function (usrId) {
        var indexEl = $scope.CommunityUsersSel.indexOf(usrId);
        if (indexEl > -1)
            $scope.CommunityUsersSel.splice(indexEl, 1);
        else
            $scope.CommunityUsersSel.push(usrId);
    };

    $scope.selectAll = function (num) {
        if (num && num > 0)
            $scope.CommunityUsersSel = []; // deselect all
        else {
            $scope.CommunityUsersSel = flatArray(angular.copy($scope.CommunityUsersViewed), "Id"); // select all
        }
    };
    $scope.CommunityUsers = []; // all users
    $scope.CommunityUsersFiltered = []; // filtrated in memory
    $scope.CommunityUsersViewed = []; // trasformed in table HTML
    $scope.CommunityUserRoles = [];
    $scope.PersonId = getCookie("PersonId");
    $scope.Init = function () {
        $scope.RefreshCommunityUsers(function (responseUsers) {
            $scope.RefreshUserRoles(function (responseRoles) {
                for (var i = 0; i < responseUsers.data.Persons.length; i++) {
                    var pers = responseUsers.data.Persons[i];
                    for (var j = 0; j < responseRoles.data.Roles.length; j++) {
                        var role = responseRoles.data.Roles[j];
                        if (pers.RoleId == role.Id)
                            pers.RoleName = role.Name;
                    }
                }

                $scope.CommunityUsers = responseUsers.data.Persons;
                $scope.CommunityUserRoles = responseRoles.data.Roles;

                $scope.CountTypeScad["Illimitati"] = $filter('isInArrayScadTypes')(responseUsers.data.Persons, ["Illimitati"]).length;
                $scope.CountTypeScad["NonIniziato"] = $filter('isInArrayScadTypes')(responseUsers.data.Persons, ["NonIniziato"]).length;
                $scope.CountTypeScad["InCorso"] = $filter('isInArrayScadTypes')(responseUsers.data.Persons, ["InCorso"]).length;
                $scope.CountTypeScad["Scaduti"] = $filter('isInArrayScadTypes')(responseUsers.data.Persons, ["Scaduti"]).length;
            });
        });
    };

    $scope.RefreshUserRoles = function (callBack) {
        $scope.loadingUserRolesResources = true;
        var option = {
            method: "GET",
            url: LMSAPI_URL + "api/Role?behaviourCode=7"
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingUserRolesResources = false;
            if (response && response.data && response.data.Roles) {
                if (typeof (callBack) === "function")
                    callBack(response);
                else
                    $scope.CommunityUserRoles = response.data.Roles;
            }
        }, function myError(response) {
            $scope.errorManager(response);

            $scope.loadingUserRolesResources = false;
        });
    };
    $scope.RefreshCommunityUsers = function (callBack) {
        $scope.RefreshUserRoles();
        $scope.loadingUserResources = true;
        var option = {
            method: "GET",
            url: LMSAPI_URL + "api/Person?behaviorCode=-1",
            headers: {}
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }

        $http(option).then(function mySucces(response) {
            $scope.loadingUserResources = false;
            if (response && response.data && response.data.Persons) {
                if (typeof (callBack) === "function")
                    callBack(response);
                else {
                    for (var i = 0; i < response.data.Persons.length; i++) {
                        var pers = response.data.Persons[i];
                        for (var j = 0; j < $scope.CommunityUserRoles.length; j++) {
                            var role = $scope.CommunityUserRoles[j];
                            if (pers.RoleId == role.Id)
                                pers.RoleName = role.Name;
                        }
                    }

                    $scope.CommunityUsers = response.data.Persons;

                    $scope.CountTypeScad["Illimitati"] = $filter('isInArrayScadTypes')(response.data.Persons, ["Illimitati"]).length;
                    $scope.CountTypeScad["NonIniziato"] = $filter('isInArrayScadTypes')(response.data.Persons, ["NonIniziato"]).length;
                    $scope.CountTypeScad["InCorso"] = $filter('isInArrayScadTypes')(response.data.Persons, ["InCorso"]).length;
                    $scope.CountTypeScad["Scaduti"] = $filter('isInArrayScadTypes')(response.data.Persons, ["Scaduti"]).length;
                }
            }
        }, function myError(response) {
            $scope.loadingUserResources = false;
            $scope.errorManager(response);
            /*alert("Errore: Aggiornamento non riuscito.");*/
        });
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

    $scope.GetCSVListaUtentiComunita = function () {
        var users = angular.copy($scope.CommunityUsersFiltered);
        GetCSVListaGenerica(
            users,
            ["Name", "Surname", "Mail", "RoleName", "IsActiveCommunity", "IsEnabledPortal", "DurationDay", "StartDate", "EndDate", "MissingDays", "Expired"],
            ["Name", "Surname", "Mail", "RoleName", "IsActiveCommunity", "IsEnabledPortal", "DurationDay", "StartDate", "EndDate", "MissingDays", "Expired"]
        );
    };

    $scope.LMSAPIservices = {
        setRolesInSelUsers: function (newRoleid) {
            if (confirm("Sei sicuro di voler sovrascivere questi ruoli?")) {
                $scope.loadingUserResources = true;
                FactoryLMSAPI.setRoles(angular.copy($scope.CommunityUsersSel), newRoleid).then(function (response) {
                    showInfo("Ruoli cambiati");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.errorMessage = 'Error: ' + error.data;
                    $scope.loadingUserResources = false;
                });
            }
        },
        disableSelUsers: function () {
            if (confirm("Sei sicuro di voler diabilitare gli utenti selezionati?")) {
                $scope.loadingUserResources = true;
                FactoryLMSAPI.modifySubscription(angular.copy($scope.CommunityUsersSel), 2).then(function (response) {
                    showInfo("utenti disabilitati");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        },
        enableSelUsers: function () {
            if (confirm("Sei sicuro di voler abilitare gli utenti selezionati?")) {
                $scope.loadingUserResources = true;
                FactoryLMSAPI.modifySubscription(angular.copy($scope.CommunityUsersSel), 1).then(function (response) {
                    showInfo("Utenti abilitati");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        },
        deleteSelUsers: function () {
            var textCanc = prompt("Scrivi 'Cancella' per rendre");
            if (textCanc){
                if ((textCanc + "").toLowerCase() == 'cancella') {
                    $scope.loadingUserResources = true;
                    FactoryLMSAPI.modifySubscription(angular.copy($scope.CommunityUsersSel), 3).then(function (response) {
                        showInfo("Utenti eliminati");
                        $scope.loadingUserResources = false;
                        $scope.RefreshCommunityUsers();
                    }, function (error) {
                        showError(error.data.Message);
                        $scope.loadingUserResources = false;
                    });
                } else {
                    showError("stringa non corretta, utenti <strong>non</strong> eliminati");
                }
            }
        },
        setResponsible: function (usr) {
            if (usr.IsResponsabile) {
                showInfo("L'utente selezionato è già responsabile.");
                return;
            }
            if (confirm("Sei sicuro di rendere l'utente responsabile?")) {
                $scope.loadingUserResources = true;
                FactoryLMSAPI.setResponsible(usr.Id).then(function (response) {
                    showInfo("L'utente selezionato è diventato responsabile");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        },        
        resetSelUsersExpiryStart : function () {
            if (confirm("Sei sicuro di resettare la data di inizio scadenza?")) {
                $scope.loadingUserResources = true;
                FactoryLMSAPI.setExpiryStart(angular.copy($scope.CommunityUsersSel), true, null).then(function (response) {
                    showInfo("La data di inizio scadenza degli utenti selezionati è stata resettata");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        },
        setSelUsersExpiryStartNow: function () {
            if (confirm("Sei sicuro di cambiare la data di inizio scadenza?")) {
                $scope.loadingUserResources = true;
                FactoryLMSAPI.setExpiryStart(angular.copy($scope.CommunityUsersSel), false, new Date()).then(function (response) {
                    showInfo("La data di inizio scadenza degli utenti selezionati è stata cambiata");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        },
        setSelUsersExpiration: function (_validity, _extendValidity, _startBehaviour) {
            if (confirm("Sei sicuro di cambiare le data di scadenza?")) {
                FactoryLMSAPI.modifyExpiration(angular.copy($scope.CommunityUsersSel), _validity, _extendValidity, _startBehaviour).then(function (response) {
                    showInfo("La data scadenza degli utenti selezionati è stata cambiata");
                    $scope.loadingUserResources = false;
                    $scope.RefreshCommunityUsers();
                }, function (error) {
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        },
        selezUserInfo : function (usr) {
            $scope.userInfosel = null;
            if (usr) {
                jQuery('#modalUserInfo').modal('show');
                FactoryLMSAPI.getUserInfo(usr.Id).then(function (response) {
                    $scope.userInfosel = response.data.Person;
                    if (!$scope.userInfosel)
                        jQuery('#modalUserInfo').modal('hide');

                    $scope.loadingUserResources = false;
                }, function (error) {
                    jQuery('#modalUserInfo').modal('hide');
                    showError(error.data.Message);
                    $scope.loadingUserResources = false;
                });
            }
        }
    };

    $scope.Init();
}]);

var generalMsgsBox = function () {
    if (jQuery("#generalMsgsBox").length > 0)
        return;
    jQuery("body").append('<div id="generalMsgsBox" style="position:fixed;top:20px;left:20px;right:20px;z-index:1040;"><b class=\"btn btn-sm btn-default pull-right\" onclick="jQuery(this).parent().remove()">X</b></div>')
}
var showError = function (errMsg) {
    generalMsgsBox();
    jQuery("#generalMsgsBox").append('<div class="alert alert-danger">' +
        'Error: ' + errMsg + '</div>'
    );
}
var showInfo = function (infoMsg) {
    generalMsgsBox();
    jQuery("#generalMsgsBox").append('<div class="alert alert-info">' +
        'Info: ' + infoMsg + '</div>'
    );
}

var GetCSVListaGenerica = function (generalItems, arrLabelHeader, arrLabelProperty, fileName) {
    if (!generalItems || generalItems.length < 1) {
        alert("Errore: utenti non trovati.");
        return;
    }
    if (!arrLabelProperty) {	// di default esporta utenti
        arrLabelHeader = ["Id", "Name", "Surname", "Mail", "Role", "Active"];
        arrLabelProperty = ["Id", "Name", "Surname", "Mail", "RoleName", "IsActiveCommunity"];
    }
    if (!fileName)
        fileName = "Community Persons";
    var separetor = ";";
    var data = angular.copy(generalItems);
    var csvContent = "data:text/csv;charset=utf-8,";
    var dataString = "";

    var lineArray = [];
    var line = "";
    for (var iHeadProp = 0; iHeadProp < arrLabelProperty.length; iHeadProp++) {
        line += (iHeadProp == 0 ? "" : separetor) + arrLabelHeader[iHeadProp];
    }

    // Header
    lineArray.push(line);

    for (var index = 0; index < data.length; index++) {
        var line = "";
        for (var iProp = 0; iProp < arrLabelProperty.length; iProp++) {
            line += (iProp == 0 ? "" : separetor) + data[index][arrLabelProperty[iProp]];
        }

        lineArray.push(line);
    }
    var csvContent = lineArray.join("\n");

    downloadClientFile(csvContent, fileName + " " + (new Date().getTime()) + ".csv", 'text/csv');
};
function flatArray(arr, propertyName) {
    try {
        if (arr && arr.length > 0 && propertyName) {
            var flatArr = [];
            for (var i = 0; i < arr.length; i++) {
                flatArr.push(arr[i][propertyName]);
            }
            return flatArr;
        }
    } catch (e) {
        return [];
    }

}

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