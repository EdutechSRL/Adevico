var appStatRep = angular.module('appStatRepository', []);
appStatRep.filter('unique', function () {
    return function (items, filterOn1, filterOn2) {
        if (!items || !filterOn1) {
            return items;
        }
        if (angular.isArray(items)) {
            var newItems = [];

            angular.forEach(items, function (item) {
                var bool1 = true;
                var bool2 = true;
                for (var i = 0; i < newItems.length; i++) {
                    var item2 = newItems[i];
                    bool1 = (item[filterOn1] == item2[filterOn1])
                    if (filterOn2)
                        bool2 = (item[filterOn2] == item2[filterOn2]);

                    if (bool1 && bool2)
                        break;
                }
                if (newItems.length == 0 || !bool1 || !bool2)
                    newItems.push(item);
            });
        }
        return newItems;
    };
});
appStatRep.filter('uniqueTags', function () {
    return function (items, filterOn) {
        if (filterOn === false) {
            return items;
        }
        if ((filterOn || angular.isUndefined(filterOn)) && angular.isArray(items)) {
            var newItems = [];

            angular.forEach(items, function (item) {
                var itemFilterOn = item[filterOn];
                if (itemFilterOn && itemFilterOn != "") {
                    itemFilterOn = (itemFilterOn + "").split(",");
                    angular.forEach(itemFilterOn, function (el) {
                        var valueToCheck, isDuplicate = false;
                        for (var i = 0; i < newItems.length; i++) {
                            if (angular.equals(newItems[i], el)) {
                                isDuplicate = true;
                                break;
                            }
                        }
                        if (!isDuplicate) {
                            newItems.push(el);
                        }
                    });
                }
            });
        }
        return newItems;
    };
});

appStatRep.filter('filterTags', function ($filter) {
    return function (items, compTags) {
        if (!compTags || compTags.length == 0)
            return items;

        var totalFiltered = [];
        for (var i = 0; i < compTags.length; i++) {
            var tag = compTags[i];
            fiteredTags = items.filter(function (el) {
                if (!el.Tags || el.Tags == "")
                    return false;
                var arrTemp = el.Tags.split(",");
                return arrTemp.indexOf(tag) >= 0
            });
            totalFiltered = totalFiltered.concat(fiteredTags);
        }
        items = $filter('unique')(totalFiltered, "Id", "IdVersion");
        return items;
    };
});

appStatRep.filter('filterExtensions', function () {
    return function (items, compEtensions) {
        if (!compEtensions || compEtensions.length == 0)
            return items;

        var filtered = [];
        for (var i = 0; i < compEtensions.length; i++) {
            var ext = compEtensions[i];
            filtered = filtered.concat(items.filter(function (el) {
                if (!el.Extension || el.Extension == "")
                    return false;
                return el.Extension == ext;
            }));
        }
        items = filtered;
        return items;
    };
});

appStatRep.controller('MasterCtrl', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    var urlWebApiTemp = location.href.replace("//", "§§");
    urlWebApiTemp = ((urlWebApiTemp.indexOf("/") > -1) ? urlWebApiTemp.split("/")[0] : urlWebApiTemp).replace("§§", "//") + "/WebAPI/";
    var RootPathWebApi = "";
    if (urlWebApiTemp.indexOf("://localhost") > -1) {
        RootPathWebApi = (urlWebApiTemp.indexOf("s://localhost") > -1) ? "https://localhost/AdevicoWeb/LMSAPI/" : "http://localhost/AdevicoWeb/LMSAPI/";
    } else {
        RootPathWebApi = urlWebApiTemp;
    }

    var ListaFilePersons = []; //private
    var ListaFileDownloads = []; //private

    $scope.ListaFileTagsUnique = [];
    $scope.ListaFileExtensionUnique = [];
    $scope.ListaFileTagsSelected = [];
    $scope.ListaFileExtensionSelected = [];

    //ListaDocumenti
    $scope.ListaDocuRepos = [];//Adevico.Models.ListaDocumentiRepository;
    $scope.ListaDocuReposFiltered = [];
    $scope.ListaDocuReposShowed = [];
    $scope.MAX_ITERATION_DOC = 10;
    $scope.SelectedDoc = null;
    $scope.queryDocs = {};// { Name : "", ExtensionName : "" };

    //ListaUtenti 
    $scope.ListaUtentiRepos = [];//Adevico.Models.ListaPersonComunita;
    $scope.ListaUtentiReposFiltered = [];
    $scope.ListaUtentiReposShowed = [];
    $scope.MAX_ITERATION_USER = 20;
    $scope.queryUsers = {};//{ Name : "", Surname : "", Mail : "", HasReadDoc : "" };

    $scope.loadingFilesRepoResources = false;
    $scope.loadingFilePersonsResources = false;
    $scope.loadingFileDownloadsResources = false;

    $scope.Init = function () {
        $scope.loadingFilesRepoResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/Permission?ParServiceCode=SRVADMCMNT"
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            if (response && response.data && response.data.Admin) {
                $scope.CheckFilesRepoUpdates();
                return;
            }
            $scope.loadingFilesRepoResources = false;
            $scope.PermissionsError = true;
        }, function myError(response) {
            $scope.loadingFilesRepoResources = false;
            $scope.PermissionsError = true;
        });

    };
    $scope.CheckFilesRepoUpdates = function (forceUpdate) {
        $scope.loadingFilesRepoResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/FileStats"
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingFilesRepoResources = false;
            if (response && response.data && response.data.Files) {
                $scope.ListaDocuRepos = $scope.flatFileWithVersion(response.data.Files);
            }
        }, function myError(response) {
            $scope.errorManager(response);

            $scope.loadingFilesRepoResources = false;
        });
    };

    $scope.CheckFileDownloadsUpdates = function (itemId, versionId) {
        $scope.loadingFileDownloadsResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/FileStats/" + itemId + "/Downloads?VersionId=" + versionId
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingFileDownloadsResources = false;
            if (response && response.data && response.data.Downloads) {
                ListaFileDownloads = response.data.Downloads;

                //format Download for interface
                for (var i = 0; i < ListaFileDownloads.length; i++) {
                    if (ListaFileDownloads[i].CreatedOn)
                        ListaFileDownloads[i].CreatedOnFormated = new Date(ListaFileDownloads[i].CreatedOn);
                }

                var boolThereAreDownloads = (ListaFileDownloads.length > 0);
                for (var i = 0; i < ListaFilePersons.length; i++) {
                    var per = ListaFilePersons[i];
                    var arrDownloadEls = [];

                    if (boolThereAreDownloads)
                        arrDownloadEls = findInArray(ListaFileDownloads, "IdPerson", per.Id, true);
                    if (arrDownloadEls.length > 0) {
                        per.HasReadDoc = true;
                        arrDownloadEls = $filter('orderBy')(arrDownloadEls, 'CreatedOnFormated');
                        per.FirstDownload = new Date(arrDownloadEls[0].CreatedOn);  //$filter('date')(arrDownloadEls[0].CreatedOn, 'dd/MM/yyyy HH:mm:ss');
                        per.LastDownload = new Date(arrDownloadEls[arrDownloadEls.length - 1].CreatedOn); // $filter('date')(arrDownloadEls[arrDownloadEls.length - 1].CreatedOn, 'dd/MM/yyyy HH:mm:ss');
                    } else {
                        per.HasReadDoc = false;
                        per.FirstDownload = null;
                        per.LastDownload = null;
                    }
                    per.DownloadsCount = arrDownloadEls.length;
                    per.Downloads = arrDownloadEls;
                }

                $scope.ListaUtentiRepos = ListaFilePersons;
            }
        }, function myError(response) {
            $scope.errorManager(response);
            $scope.loadingFileDownloadsResources = false;
        });
    };
    $scope.CheckFilePersonsUpdates = function (itemId, versionId) {
        $scope.loadingFilePersonsResources = true;
        var option = {
            method: "GET",
            url: RootPathWebApi + "api/FileStats/" + itemId + "/FilePersons?VersionId=" + versionId
        }
        if (urlWebApiTemp.indexOf("://localhost") > -1) {
            option.headers = getHeadersApiForLocalHost();
        }
        $http(option).then(function mySucces(response) {
            $scope.loadingFilePersonsResources = false;
            if (response && response.data && response.data.Persons) {
                ListaFilePersons = response.data.Persons;

                $scope.CheckFileDownloadsUpdates(itemId, versionId);
            }

        }, function myError(response) {
            $scope.errorManager(response);

            $scope.loadingFilePersonsResources = false;
        });
    };
    //file Extensions
    $scope.ToogleFileExtensionFilter = function (value) {
        if (!value)
            return;

        var index = $scope.IndexOfExtensionInList(value);
        if (index < 0) {
            $scope.ListaFileExtensionSelected.push(value);
        } else {
            if (index > -1) {
                $scope.ListaFileExtensionSelected.splice(index, 1);
            }
        }
    };
    $scope.IndexOfExtensionInList = function (value) {
        if (value)
            return $scope.ListaFileExtensionSelected.indexOf(value);

        return -1;
    };
    //file tags
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
    };

    //ricerca	
    $scope.fnSelectedDoc = function (doc) {
        if (!doc)
            $scope.MAX_ITERATION_USER = 20;

        $scope.SelectedDoc = doc;

        $scope.ListaUtentiRepos = [];
        ListaFilePersons = [];
        ListaFileDownloads = [];

        if ($scope.SelectedDoc) {
            $scope.CheckFilePersonsUpdates(doc.Id, doc.IdVersion);
        }
    };
    $scope.getClassIcon = function (doc) {
        var className = "iconDoc";
        if (doc.Extension) {
            //doc.ExtensionName = doc.Name.substring(doc.Name.lastIndexOf(".") + 1).toLowerCase();
            switch (doc.Extension) {
                case ".doc":
                case "docx":
                    className = "iconDoc";
                    break;
                case ".pdf":
                    className = "iconPdf";
                    break;
                case ".xls":
                case ".xlsx":
                    className = "iconXlsx";
                    break;
                case ".zip":
                    className = "iconZip";
                    break;
                default:
                    break;
            }
        }
        return className;
    };
    $scope.showMoreDocItems = function () {
        if ($scope.MAX_ITERATION_DOC < $scope.ListaDocuRepos.length) {
            $scope.MAX_ITERATION_DOC = $scope.MAX_ITERATION_DOC + 5;
            if ($scope.MAX_ITERATION_DOC > $scope.ListaDocuRepos.length)
                $scope.MAX_ITERATION_DOC = $scope.ListaDocuRepos.length
        }
    };
    $scope.showMoreUserItems = function () {
        if ($scope.MAX_ITERATION_USER < $scope.ListaUtentiRepos.length) {
            $scope.MAX_ITERATION_USER = $scope.MAX_ITERATION_USER + 10;
            if ($scope.MAX_ITERATION_USER > $scope.ListaUtentiRepos.length)
                $scope.MAX_ITERATION_USER = $scope.ListaUtentiRepos.length
        }
    };
    $scope.laMiaLista = function (lista) {
        $scope.ListaUtentiReposFiltered = lista;
    };
    $scope.GetCSVListaUtenti = function (userItems, arrLabelHeader, arrLabelProperty) {
        if (!$scope.SelectedDoc || !userItems || userItems.length < 1) {
            alert("Errore: utenti non trovati.");
            return;
        }
        if (!arrLabelProperty) {	// di default esporta utenti
            arrLabelHeader = ["Id", "Name", "Surname", "Mail", "Scaricato", "Downloads Count", "First Download", "Last Download"];
            arrLabelProperty = ["Id", "Name", "Surname", "Mail", "HasReadDoc", "DownloadsCount", "FirstDownload", "LastDownload"];
        }
        var separetor = ";";
        var data = angular.copy(userItems);
        var csvContent = "data:text/csv;charset=utf-8,";
        var dataString = "";

        var lineArray = [];
        var line = "";
        for (var iHeadProp = 0; iHeadProp < arrLabelProperty.length; iHeadProp++) {
            line += (iHeadProp == 0 ? "" : separetor) + arrLabelHeader[iHeadProp];
        }

        // Header
        lineArray.push(csvContent + line);

        for (var index = 0; index < data.length; index++) {
            var line = "";
            for (var iProp = 0; iProp < arrLabelProperty.length; iProp++) {
                line += (iProp == 0 ? "" : separetor) + data[index][arrLabelProperty[iProp]];
            }

            lineArray.push(line);
        }
        var csvContent = lineArray.join("\n");

        var encodedUri = encodeURI(csvContent);
        var link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute("download", "Users Doc" + $scope.SelectedDoc.Id + "v" + $scope.SelectedDoc.IdVersion + " " + (new Date().getTime()) + ".csv");
        document.body.appendChild(link); // Required for FF

        link.click();
        setTimeout(function () {
            link.remove();
        }, 1000);
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
    $scope.flatFileWithVersion = function (listaWithVersion) {
        var listReturned = [];
        for (var i = 0; i < listaWithVersion.length; i++) {
            var el = listaWithVersion[i];
            if (el && el.FileVersions) {
                for (var j = 0; j < el.FileVersions.length; j++) {
                    elVer = el.FileVersions[j];
                    if (el.Id == elVer.IdItem) {
                        var newEl = angular.copy(el);
                        newEl.IdVersionActive = newEl.IdVersion;
                        newEl.IdVersion = elVer.Id;
                        newEl.Name = elVer.Name;
                        newEl.Url = elVer.Url;
                        newEl.LinkFile = encodeURI("/" + el.Id + "/" + elVer.Id + "/" + elVer.Name + elVer.Extension + ".download");
                        newEl.Extension = elVer.Extension;
                        newEl.Description = elVer.Description;
                        newEl.ContentType = elVer.ContentType;
                        newEl.Size = elVer.Size;
                        newEl.Number = elVer.Number;
                        newEl.VerDownloaded = elVer.Downloaded;
                        newEl.IdCommunity = elVer.IdCommunity;
                        newEl.ItemType = elVer.ItemType;
                        newEl.IsActive = elVer.IsActive;
                        newEl.DataVer = new Date(elVer.ModifiedOn || elVer.CreatedOn);
                        newEl.OrderVersion = (newEl.IdVersionActive + ((elVer.IsActive ? 0.1 : 0.11)));
                        newEl.Status = elVer.Status;
                        newEl.Deleted = elVer.Deleted;
                        listReturned.push(newEl);
                    }
                }
            }
        }

        return listReturned;
    }

    $scope.Init();
}]);

function findInArray(lista, attrId, value, findAll) {
    findAll = (findAll ? true : false);
    var arrReturn = [];
    for (var i = 0; i < lista.length; i++) {
        var el = lista[i];
        if (el[attrId] == value) {
            if (!findAll)
                return el;
            else
                arrReturn.push(el);
        }
    }
    return arrReturn;
}