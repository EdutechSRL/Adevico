var appStatRep = angular.module('appStatScorm', []);

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


appStatRep.controller('MasterCtrl', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
    var urlWebApiTemp = location.href.replace("//", "§§");
    urlWebApiTemp = ((urlWebApiTemp.indexOf("/") > -1) ? urlWebApiTemp.split("/")[0] : urlWebApiTemp).replace("§§", "//") + "/WebAPI/";
    var RootPathWebApi = "";
    if (urlWebApiTemp.indexOf("://localhost") > -1) {
        RootPathWebApi = (urlWebApiTemp.indexOf("s://localhost") > -1) ? "https://localhost/AdevicoWeb/LMSAPI/" : "http://localhost/AdevicoWeb/LMSAPI/";
    } else {
        RootPathWebApi = urlWebApiTemp;
    }

    $scope.PermissionsError = false;
    $scope.PersonId = getCookie("PersonId");
    $scope.ArrParamDocsId = getRequestParam("idItem") ? getRequestParam("idItem") : [];
    $scope.ArrParamPersId = getRequestParam("RUid") ? getRequestParam("RUid") : [];
    var varIdLinkAppoggio = getRequestParam("idLink");
    $scope.ArrIdLink = (varIdLinkAppoggio && varIdLinkAppoggio.length > 0) ? varIdLinkAppoggio[0] : null;
    $scope.ViewMode = '';
    if ($scope.ArrParamDocsId.length > 0 && $scope.ArrParamPersId.length > 0)
        $scope.ViewMode = 'FromParams';
    else if ($scope.ArrParamDocsId.length > 0 && $scope.ArrParamPersId.length < 1)
        $scope.ViewMode = 'FileParams';
    else if ($scope.ArrParamDocsId.length < 1 && $scope.ArrParamPersId.length > 0)
        $scope.ViewMode = 'UserParams';

    if ($scope.ViewMode === "FileParams")
        $scope.GroupByPerson = false;
    else
        $scope.GroupByPerson = true;

    $scope.ListaPlayDocRepos = [];
    $scope.ListaTreePlays = [];
    $scope.DetailPlayScorm = {
        PersonEntity: null,
        FileEntity: null,
        ListaPlaysOneByOne: []
    };

    $scope.loadingUserResources = false;
    $scope.loadingDocResources = false;
    $scope.loadingUserRolesResources = false;
    $scope.OrderReverse = false;
    $scope.OrderPropertyName = 'Surname';
    $scope.OrderUserReverse = false;
    $scope.OrderUserPropertyName = 'Surname';
    $scope.OrderDocReverse = false;
    $scope.OrderDocPropertyName = 'Surname';

    //ListaUserRoles
    $scope.ListaUserRoles = [];

    //ListaDocmenti
    $scope.ListaIdDocScormSelected = []; // 1,34,76,98,222, ecc..
    $scope.ListaDocScormChecked = [];
    $scope.ListaDocScorm = [];//Adevico.Models.ListaDocmentiRepository;
    $scope.ListaDocScormFiltered = [];
    $scope.ListaDocScormShowed = [];
    $scope.MAX_ITERATION_DOC = 10;
    $scope.queryDocs = {}; // { Name : "", Path : "" };

    //ListaUtenti 
    $scope.ListaIdUtentiScormSelected = []; // 1,34,76,98,222, ecc..
    $scope.ListaUtentiScormChecked = [];
    $scope.ListaUtentiScorm = [];//Adevico.Models.ListaPersonComunita;
    $scope.ListaUtentiScormFiltered = [];
    $scope.ListaUtentiScormShowed = [];
    $scope.MAX_ITERATION_Person = 20;
    $scope.queryPersons = {}; //{ Name : "", Surname : "", Mail : "" };
    $scope.queryPersonsStrict = {};


    $scope.myParseInt = function (str) {
        if (str == "")
            return {};

        return { 'RoleId': parseInt(str) };
    };
    $scope.Init = function () {
        if ($scope.ViewMode != "") {
            $scope.CheckPersonUpdates();
            $scope.CheckFileScormUpdates();
            if ($scope.ViewMode === 'FileParams')
                jQuery("#btnModalComunityPersons").click();

            if ($scope.ViewMode === 'UserParams')
                jQuery("#btnModalComunityScormFile").click();
        }

    };
    $scope.ToggleGroupByPerson = function (mybool) {
        if ($scope.GroupByPerson == mybool)
            return;

        if (typeof mybool !== "undefined")
            $scope.GroupByPerson = mybool;
        else
            $scope.GroupByPerson = !$scope.GroupByPerson;

        if ($scope.ListaTreePlays && $scope.ListaTreePlays.length > 0) {
            $scope.ListaTreePlays = $scope.BuildTreePlaysDoc(angular.copy($scope.ListaPlayDocRepos));
        }
    }
    $scope.SetRoleName = function (person) {
        for (var i = 0; i < $scope.ListaUserRoles.length; i++) {
            var role = $scope.ListaUserRoles[i];
            if (role.Id + "" === person.RoleId + "")
                person.RoleName = role.Name;
        }
    };
    $scope.CheckUserRolesUpdates = function (forceUpdate) {
        $scope.loadingUserRolesResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/Role"
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingUserRolesResources = false;
            if (response && response.data && response.data.Roles) {

                $scope.ListaUserRoles = response.data.Roles;
            }
        }, function myError(response) {
            $scope.errorManager(response);

            $scope.loadingUserRolesResources = false;
        });
    };
    $scope.CheckPersonUpdates = function (forceUpdate) {
        $scope.CheckUserRolesUpdates();
        $scope.loadingUserResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/Person?behaviorCode=-1",
            headers: {}
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }

        option.headers.LinkId = $scope.ArrIdLink;

        $http(option).then(function mySucces(response) {
            $scope.loadingUserResources = false;
            if (response && response.data && response.data.Persons) {
                for (var i = 0; i < response.data.Persons.length; i++) {
                    var index = $scope.findEl($scope.ListaUtentiScormChecked, "Id", response.data.Persons[i].Id);
                    if (index >= 0)
                        response.data.Persons[i].select = true;
                    else {
                        var index = $scope.ArrParamPersId.indexOf(response.data.Persons[i].Id + "");
                        if (index >= 0)
                            response.data.Persons[i].select = true;
                        else
                            response.data.Persons[i].select = false;
                    }
                }
                $scope.ListaUtentiScorm = response.data.Persons;
                if ($scope.ViewMode == "FromParams" || $scope.ViewMode == "UserParams") {
                    $scope.applicaCheckedPersonById($scope.ArrParamPersId);

                    if ($scope.ViewMode == "FromParams") {
                        $scope.CheckPlayFileScormUpdates();
                    }
                }
            }
        }, function myError(response) {
            $scope.loadingUserResources = false;
            $scope.errorManager(response);
            /*alert("Errore: Aggiornamento non riuscito.");*/
        });
    };
    $scope.CheckFileScormUpdates = function (forceUpdate) {
        $scope.loadingDocResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/ScormFile",
            headers: {}
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        option.headers.LinkId = $scope.ArrIdLink;
        $http(option).then(function mySucces(response) {
            $scope.loadingDocResources = false;
            if (response && response.data && response.data.Files) {
                for (var i = 0; i < response.data.Files.length; i++) {
                    var index = $scope.findEl($scope.ListaDocScormChecked, "Id", response.data.Files[i].Id);
                    if (index >= 0)
                        response.data.Files[i].select = true;
                    else {
                        var index = $scope.ArrParamDocsId.indexOf(response.data.Files[i].Id + "");
                        if (index >= 0)
                            response.data.Files[i].select = true;
                        else
                            response.data.Files[i].select = false;
                    }
                }
                $scope.ListaDocScorm = response.data.Files;


                if ($scope.ViewMode == "FromParams" || $scope.ViewMode == "FileParams") {
                    $scope.applicaCheckedFilesById($scope.ArrParamDocsId);

                    if ($scope.ViewMode == "FromParams") {
                        $scope.CheckPlayFileScormUpdates();
                    }
                }
            }
        }, function myError(response) {
            $scope.loadingDocResources = false;
            $scope.errorManager(response);

            /*alert("Errore: Aggiornamento non riuscito.");*/
        });
    };
    $scope.CheckPlayFileScormUpdates = function () {
        if ($scope.ListaUtentiScormChecked.length < 1 || $scope.ListaDocScormChecked.length < 1)
            return;
        var data = {
            personsId: flatArray($scope.ListaUtentiScormChecked, "Id"), // angular.copy($scope.ListaIdUtentiScormSelected),
            filesId: flatArray($scope.ListaDocScormChecked, "Id") // angular.copy($scope.ListaIdDocScormSelected)
        };
        var config = {
            headers: { "LinkId": $scope.ArrIdLink }
        };
        $http.post(
            RootPathWebApi + "api/ScormStat",
            data,
            config
        ).then(function mySucces(response) {
            if (response && response.data && response.data.Plays) {
                $scope.ListaPlayDocRepos = response.data.Plays;
                $scope.ListaTreePlays = $scope.BuildTreePlaysDoc(response.data.Plays);
            }
        }, function myError(response) {
            $scope.errorManager(response);

            /*alert("Errore: Aggiornamento non riuscito.");*/
        });
    };
    $scope.CheckDettailPlayFileScorm = function (play) {
        $scope.DetailPlayScorm = {
            PersonEntity: null,
            FileEntity: null,
            ListaPlaysOneByOne: []
        };
        var data = {
            personsId: [play.PersonId], // angular.copy($scope.ListaIdUtentiScormSelected),
            filesId: [play.FileId],
            BehaviorCode: -1// angular.copy($scope.ListaIdDocScormSelected)
        };
        var config = {
            /* headers: { DeviceId: "4a70666a-0a5f-ef37-5163-e9aba4a15d4e", Token: "3b29f2d4-5a6c-44ec-a8ea-34cce1a58fbc", CommunityId: 2 }*/
        };
        $http.post(
            RootPathWebApi + "api/ScormStat",
            data,
            config
        ).then(function mySucces(response) {
            if (response && response.data && response.data.Plays) {
                if (response.data.Plays.length > 0) {
                    var onePlay = response.data.Plays[0];
                    var indexPerson = $scope.findEl($scope.ListaUtentiScormChecked, "Id", onePlay.PersonId);
                    var indexFile = $scope.findEl($scope.ListaDocScormChecked, "Id", onePlay.FileId);
                    if (indexPerson >= 0 && indexFile >= 0) {
                        $scope.DetailPlayScorm = {
                            PersonEntity: $scope.ListaUtentiScormChecked[indexPerson],
                            FileEntity: $scope.ListaDocScormChecked[indexFile],
                            ListaPlaysOneByOne: response.data.Plays
                        };
                    }
                }
            }
        }, function myError(response) {
            $scope.errorManager(response);

            alert("Errore: Aggiornamento non riuscito.");
        });
    };
    $scope.sortBy = function (propertyName) {
        $scope.OrderReverse = ($scope.OrderPropertyName === propertyName) ? !$scope.OrderReverse : false;
        $scope.OrderPropertyName = propertyName;
    };
    $scope.userSortBy = function (propertyName) {
        $scope.OrderUserReverse = ($scope.OrderUserPropertyName === propertyName) ? !$scope.OrderUserReverse : false;
        $scope.OrderUserPropertyName = propertyName;
    };
    $scope.docSortBy = function (propertyName) {
        $scope.OrderDocReverse = ($scope.OrderDocPropertyName === propertyName) ? !$scope.OrderDocReverse : false;
        $scope.OrderDocPropertyName = propertyName;
    };
    /* //file tags
     $scope.ToogleFileTagFilter = function (value) {
         if (!value)
             return;
 
         if ($scope.IndexOfTagInList(value) < 0) {
             $scope.ListaFileTagsSelected.push(value);
         } else {
             $scope.RemoveFileTagFilter(value);
         }
     };
     $scope.RemoveFileTagFilter = function (value) {
         var index = $scope.IndexOfTagInList(value);
         if (index > -1) {
             $scope.ListaFileTagsSelected.splice(index, 1);
         }
     };
     $scope.IndexOfTagInList = function (value) {
         if (value)
             return $scope.ListaFileTagsSelected.indexOf(value);
 
         return -1;
     };*/

    //ricerca
    $scope.showMoreDocItems = function () {
        if ($scope.MAX_ITERATION_DOC < $scope.ListaDocScorm.length) {
            $scope.MAX_ITERATION_DOC = $scope.MAX_ITERATION_DOC + 5;
            if ($scope.MAX_ITERATION_DOC > $scope.ListaDocScorm.length)
                $scope.MAX_ITERATION_DOC = $scope.ListaDocScorm.length
        }
    };
    $scope.showMorePersonItems = function () {
        if ($scope.MAX_ITERATION_Person < $scope.ListaUtentiScorm.length) {
            $scope.MAX_ITERATION_Person = $scope.MAX_ITERATION_Person + 10;
            if ($scope.MAX_ITERATION_Person > $scope.ListaUtentiScorm.length)
                $scope.MAX_ITERATION_Person = $scope.ListaUtentiScorm.length
        }
    };
    $scope.applicaCheckedPerson = function (ListaTemp) {
        $scope.ListaUtentiScormChecked = ListaTemp;
    };
    $scope.applicaCheckedFiles = function (ListaTemp) {
        $scope.ListaDocScormChecked = ListaTemp;
    };
    $scope.applicaCheckedPersonById = function (ListaId) {
        var ListaTemp = [];
        if (ListaId && ListaId.length > 0) {
            for (var i = 0; i < ListaId.length; i++) {
                var indexPerson = $scope.findEl($scope.ListaUtentiScorm, "Id", ListaId[i]);
                if (indexPerson >= 0) {
                    $scope.ListaUtentiScorm[indexPerson].select = true;
                    ListaTemp.push($scope.ListaUtentiScorm[indexPerson]);
                }

            }
        }


        //$timeout(function () {
        $scope.ListaUtentiScormChecked = ListaTemp;
        //}, 300);
    };
    $scope.applicaCheckedFilesById = function (ListaId) {
        var ListaTemp = [];
        if (ListaId && ListaId.length > 0) {
            for (var i = 0; i < ListaId.length; i++) {
                var indexFile = $scope.findEl($scope.ListaDocScorm, "Id", ListaId[i]);
                if (indexFile >= 0) {
                    $scope.ListaDocScorm[indexFile].select = true;
                    ListaTemp.push($scope.ListaDocScorm[indexFile]);
                }

            }
        }

        //$timeout(function () {
        $scope.ListaDocScormChecked = ListaTemp;
        //}, 300);
    };
    $scope.toggleCheckTuttiDocs = function (isAllSelected) {
        if (isAllSelected) {
            for (var i = 0; i < $scope.ListaDocScormFiltered.length; i++) {
                $scope.ListaDocScormFiltered[i].select = false;
            }
        } else {
            for (var i = 0; i < $scope.ListaDocScormFiltered.length; i++) {
                $scope.ListaDocScormFiltered[i].select = true;
            }
        }
    };
    $scope.toggleCheckTuttiUsers = function (isAllSelected) {
        if (isAllSelected) {
            for (var i = 0; i < $scope.ListaUtentiScorm.length; i++) {
                $scope.ListaUtentiScorm[i].select = false;
            }
        } else {
            for (var i = 0; i < $scope.ListaUtentiScorm.length; i++) {
                $scope.ListaUtentiScorm[i].select = true;
            }
        }
    };
    $scope.GetCSVListaPlayScorm = function (PlayTreeItems) {
        if (!PlayTreeItems || PlayTreeItems.length < 1)
            return;

        var treeCopy = angular.copy(PlayTreeItems);
        var PlayItems = [];
        var isGroupedByFile = false;
        if (treeCopy[0].Extension)
            isGroupedByFile = true;
        for (var i = 0; i < treeCopy.length; i++) {
            var rootItem = treeCopy[i];
            for (var j = 0; j < rootItem.Plays.length; j++) {
                var objPlay = rootItem.Plays[j];

                if (isGroupedByFile) {
                    //file
                    objPlay.NameFile = rootItem.Name;
                    objPlay.ExtensionFile = rootItem.Extension;
                    objPlay.PathFile = rootItem.Path;
                    //non serve per scorm   objPlay.TotalPlay = (objPlay.FileEntity) ? objPlay.FileEntity.TotalPlay : 'error data FileEntity not found';
                    objPlay.TotalDownloadFile = rootItem.TotalDownload;
                    objPlay.LastUpdateFile = rootItem.LastUpdate;
                    objPlay.LastVersionIdFile = rootItem.LastVersionId;

                    //Person
                    objPlay.NamePerson = (objPlay.PersonEntity) ? objPlay.PersonEntity.Name : 'error data PersonEntity not found';
                    objPlay.Surname = (objPlay.PersonEntity) ? objPlay.PersonEntity.Surname : 'error data PersonEntity not found';
                    objPlay.Mail = (objPlay.PersonEntity) ? objPlay.PersonEntity.Mail : 'error data PersonEntity not found';
                } else {
                    //Person
                    objPlay.NamePerson = rootItem.Name;
                    objPlay.Surname = rootItem.Surname;
                    objPlay.Mail = rootItem.Mail;
                    //file
                    objPlay.NameFile = objPlay.FileEntity.Name;
                    objPlay.ExtensionFile = objPlay.FileEntity.Extension;
                    objPlay.PathFile = objPlay.FileEntity.Path;
                    //non serve per scorm   objPlay.TotalPlay = objPlay.FileEntity.TotalPlay;
                    objPlay.TotalDownloadFile = objPlay.FileEntity.TotalDownload;
                    objPlay.LastUpdateFile = objPlay.FileEntity.LastUpdate;
                    objPlay.LastVersionIdFile = objPlay.FileEntity.LastVersionId;
                }

                PlayItems.push(objPlay);
            }
        }
        if (isGroupedByFile) {
            $scope.GetCSVListaGenerica(PlayItems,
                ["NameFile", "VersionNumber", "ExtensionFile", "PathFile", "TotalDownloadFile", "LastUpdateFile", "LastVersionIdFile",
                    "NamePerson", "Surname", "Mail",
                    "Status", 'AlreadyCompleted', "PercCompletion", "ScormCompletion", "EndPlayOn", "PlayScore", "MinScore", "PlayTime", "MinTime", "ActivitiesDone", "ActivitiesTotal"],
                ["NameFile", "VersionNumber", "ExtensionFile", "PathFile", "TotalDownloadFile", "LastUpdateFile", "LastVersionIdFile",
                    "NamePerson", "Surname", "Mail",
                    "Status", 'AlreadyCompleted', "PercCompletion", "ScormCompletion", "EndPlayOn", "PlayScore", "MinScore", "PlayTime", "MinTime", "ActivitiesDone", "ActivitiesTotal"],
                "Scorm play by file");
        } else {
            $scope.GetCSVListaGenerica(PlayItems,
                ["NamePerson", "Surname", "Mail",
                    "NameFile", "VersionNumber", "ExtensionFile", "PathFile", "TotalDownloadFile", "LastUpdateFile", "LastVersionIdFile",
                    "Status", 'AlreadyCompleted', "PercCompletion", "ScormCompletion", "EndPlayOn", "PlayScore", "MinScore", "PlayTime", "MinTime", "ActivitiesDone", "ActivitiesTotal"],
                ["NamePerson", "Surname", "Mail",
                    "NameFile", "VersionNumber", "ExtensionFile", "PathFile", "TotalDownloadFile", "LastUpdateFile", "LastVersionIdFile",
                    "Status", 'AlreadyCompleted', "PercCompletion", "ScormCompletion", "EndPlayOn", "PlayScore", "MinScore", "PlayTime", "MinTime", "ActivitiesDone", "ActivitiesTotal"],
                "Scorm play by user");
        }
    };
    $scope.GetCSVListaGenerica = function (generalItems, arrLabelHeader, arrLabelProperty, fileName) {
        if (!generalItems || generalItems.length < 1) {
            alert("Errore: utenti non trovati.");
            return;
        }
        if (!arrLabelProperty) {	// di default esporta utenti
            arrLabelHeader = ["Id", "Name", "Surname", "Mail", "Role", "Active"];
            arrLabelProperty = ["Id", "Name", "Surname", "Mail", "RoleName", "IsActiveCommunity"];
        }
        if (!fileName)
            fileName = "Persons Doc";
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
    $scope.getScormStatus = function (play) {
        if (!play)
            return;

        var idStatus = play.Status;
        var strReturn = "";
        switch (idStatus + "") {
            case "0":
                strReturn = "notstarted";
                break;
            case "1":
                if (play.AlreadyCompleted)
                    strReturn = "started"; // "restarted"
                else
                    strReturn = "started";
                break;
            case "2":
                strReturn = "completed";
                break;
            case "3":
                strReturn = "passed";
                break;
            case "4":
                strReturn = "completedpassed";
                break;
            case "5":
                strReturn = "failed";
                break;
            case "6":
                strReturn = "completed";
                break;
            default:
                strReturn = "";
        }
        return strReturn
    };

    $scope.findEl = function (arr, propertyName, propertyValue) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i][propertyName] == propertyValue) {
                return i;
            }
        }
        return -1;
    };
    $scope.isFileChecked = function (doc) {
        var index = $scope.findEl($scope.ListaDocScormChecked, "Id", doc.Id);
        if (index > -1) {
            $scope.ListaDocScormChecked[index].select = true;
        }
    };
    $scope.isPersonChecked = function (person) {
        var index = $scope.findEl($scope.ListaUtentiScormChecked, "Id", person.Id);
        if (index > -1) {
            $scope.ListaUtentiScormChecked[index].select = true;
        }
    };

    $scope.BuildTreePlaysDoc = function (ListaTreePlaysRif) {
        var DocPlay = 0;
        if ($scope.ListaUtentiScormChecked.length < 1 && $scope.ListaDocScormChecked.length < 1) {
            return [];
        }

        var arrPersonChecked = angular.copy($scope.ListaUtentiScormChecked);
        var arrDocChecked = angular.copy($scope.ListaDocScormChecked);
        if ($scope.GroupByPerson) {
            for (var i = 0; i < ListaTreePlaysRif.length; i++) {
                var playScorm = ListaTreePlaysRif[i];
                var indexPerson = $scope.findEl(arrPersonChecked, "Id", playScorm.PersonId);
                if (indexPerson > -1) { // se trovo persona
                    var persona = arrPersonChecked[indexPerson];
                    if (!persona.Plays)
                        persona.Plays = [];

                    var indexDoc = $scope.findEl(arrDocChecked, "Id", playScorm.FileId);

                    if (indexDoc > -1) {
                        var currFile = arrDocChecked[indexDoc];
                        persona.Plays.push(playScorm);
                        playScorm.LabelName = currFile.Name;
                        playScorm.FileEntity = currFile;
                    }
                }
            }

            for (var i = 0; i < arrPersonChecked.length; i++) {
                var persona = arrPersonChecked[i];
                if (!persona.Plays)
                    persona.Plays = [];
                for (var y = 0; y < arrDocChecked.length; y++) {
                    var currFile = arrDocChecked[y];
                    var indexDoc = $scope.findEl(persona.Plays, "FileId", currFile.Id);
                    if (indexDoc < 0) {
                        var copyPlay = {
                            "Id": --DocPlay,
                            "PersonId": "",
                            "FileId": "",
                            "VersionNumber": "",
                            "VersionId": "",
                            "Status": "0",
                            "PercCompletion": "0",
                            "EndPlayOn": "",
                            "PlayScore": "0",
                            "MinScore": "0",
                            "PlayTime": "00:00:00",
                            "MinTime": "",
                            "ActivitiesDone": "0",
                            "ActivitiesTotal": "0",
                            "ScormCompletion": "",
                            "PlayNumber": "0"
                        };
                        copyPlay.LabelName = currFile.Name;
                        copyPlay.FileEntity = currFile;
                        persona.Plays.push(copyPlay);
                    }
                }
            }
            return arrPersonChecked;
        } else {
            for (var i = 0; i < ListaTreePlaysRif.length; i++) {
                var playScorm = ListaTreePlaysRif[i];
                var indexDoc = $scope.findEl(arrDocChecked, "Id", playScorm.FileId);
                if (indexDoc > -1) { // se trovo file
                    var currFile = arrDocChecked[indexDoc];
                    if (!currFile.Plays)
                        currFile.Plays = [];

                    var indexPerson = $scope.findEl(arrPersonChecked, "Id", playScorm.PersonId);

                    if (indexPerson > -1) {
                        var persona = arrPersonChecked[indexPerson];
                        currFile.Plays.push(playScorm);
                        playScorm.LabelName = persona.Name + " " + persona.Surname;
                        playScorm.PersonEntity = persona;
                    }
                }
            }

            for (var i = 0; i < arrDocChecked.length; i++) {
                var currFile = arrDocChecked[i];
                if (!currFile.Plays)
                    currFile.Plays = [];
                for (var y = 0; y < arrPersonChecked.length; y++) {
                    var persona = arrPersonChecked[y];
                    var indexPersona = $scope.findEl(currFile.Plays, "PersonId", persona.Id);
                    if (indexPersona < 0) {
                        var copyPlay = {
                            "Id": --DocPlay,
                            "PersonId": "",
                            "FileId": "",
                            "VersionNumber": "",
                            "VersionId": "",
                            "Status": "0",
                            "PercCompletion": "0",
                            "EndPlayOn": "",
                            "PlayScore": "0",
                            "MinScore": "0",
                            "PlayTime": "00:00:00",
                            "MinTime": "",
                            "ActivitiesDone": "0",
                            "ActivitiesTotal": "0",
                            "ScormCompletion": "",
                            "PlayNumber": "0"
                        };
                        copyPlay.LabelName = persona.Name + " " + persona.Surname;
                        copyPlay.PersonEntity = persona;
                        currFile.Plays.push(copyPlay);
                    }
                }
            }
            return arrDocChecked;
        }

        return [];
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
    $scope.Init();
}]);


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