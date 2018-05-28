ADV = {};
ADV.Auth = {};
ADV.Erros = {
    FatalMessage: function (msg) {
        jQuery("body").html("");
        jQuery("body").append("<h1>" + msg + "</h1>")
    }
};

ADV.Models = {
    dtoMessaggioBacheca: function (IdComunita) {
        this.Id = null;
        this.Text = "";
        this.OwnerName = "";
        this.OwnerId = null;
        this.CreateDate = null;

        this.ParentMessageId = 0;
        this.Messages = [];
        this.Latitude = null;
        this.Longitude = null;
        this.Zoom = 12;
        this.CommunityId = IdComunita;
        this.ImageUri = null;

        // client side
        this.isReadIt = true;
        this.isMine = true;
    }
}
ADVBS = {
    MessageSelected: null,
    comunityMessages: [],
    comunityMessagesTree: []
};
ADV.GetUrlWebApi = function () {
    var urlWebApiTemp = location.href.replace("//", "§§");
    urlWebApiTemp = ((urlWebApiTemp.indexOf("/") > -1) ? urlWebApiTemp.split("/")[0] : urlWebApiTemp).replace("§§", "//") + "/WebAPI/";
    var RootPathWebApi = "";
    if (urlWebApiTemp.indexOf("://localhost") > -1) {
        RootPathWebApi = (urlWebApiTemp.indexOf("s://localhost") > -1) ? "https://localhost/AdevicoWeb/LMSAPI/" : "http://localhost/AdevicoWeb/LMSAPI/";
    } else {
        RootPathWebApi = urlWebApiTemp;
    }

    return RootPathWebApi;
};
ADVBS.Params = {
    PersonId: getCookie("PersonId"),
    WebApiBaseUrl: ADV.GetUrlWebApi(),
    DateTimeNow: new Date(),
    getDateTimeRef: function () { return new Date(ADVBS.Params.DateTimeNow.setDate(ADVBS.Params.DateTimeNow.getDate() - 300)) }
};
function escapeHtml(unsafe, withoutLink) {
    if (!unsafe)
        return;
    var safeHMTL = unsafe.replace(/</g, "&lt;").replace(/>/g, "&gt;");//.replace(/"/g, "&quot;").replace(/'/g, "&#039;");

    if (!withoutLink)
        safeHMTL = safeHMTL.replace(/\[link\]/g, '<a target="_blank" href="').replace(/\[\/link\]/g, '">link</a>');

    return safeHMTL;
}
ADVBS.Services = {
    doCheckMessagesUpdates: function () {
        jQuery.ajax({
            type: "GET",
            url: ADVBS.Params.WebApiBaseUrl + "api/NoticeBoard?ReferenceDateTime=" + ADVBS.Params.getDateTimeRef().toISOString(),
            success: function (response) {
                if (response && response.Messages)
                    ADVBS.comunityMessages = response.Messages;

                ADVBS.UI.RenderMessages();
            }, error: function (e) {
                //alert("Errore: " + JSON.stringify(e));
            }
        });
    },
    doSendMessage: function () {
        if (!ADVBS.MessageSelected)
            return;
        var safeHTML = escapeHtml(jQuery("#input-newMessage").val(), true);
        ADVBS.MessageSelected.Text = safeHTML;
        if (!ADVBS.MessageSelected.Text) {
            alert("Il messaggio non contiente testo");
            return;
        }
        $.ajax({
            url: ADVBS.Params.WebApiBaseUrl + "api/NoticeBoard",
            type: 'POST',
            dataType: "json",
            data: ADVBS.MessageSelected,
            success: function (data) {
                alert('Messaggio inserito.');
                ADVBS.Actions.doCancelNewMessage();
                ADVBS.Services.doCheckMessagesUpdates();
            }
        });
    },
    doRemoveMessage: function (messId) {
        if (confirm("Delete?")) {
            $.ajax({
                url: ADVBS.Params.WebApiBaseUrl + "api/NoticeBoard/" + messId,
                type: 'DELETE',
                dataType: "json",
                success: function (data) {
                    alert('Messaggio cancellato.');
                    ADVBS.Services.doCheckMessagesUpdates();
                }
            });
        }
    }
};
jQuery.ajax({
    type: "GET",
    url: ADVBS.Params.WebApiBaseUrl + "api/Permission?ParServiceCode=SRVBKSOCIAL",
    success: function (response) {
        ADV.Auth.Permissions = response;
        if (!ADV.Auth.Permissions || !ADV.Auth.Permissions.View) {
            ADV.Erros.FatalMessage("permissions error");
            return;
        }

        ADVBS.Init(document.getElementById("boxBachecaSocial"));
    }, error: function (e) {
        ADV.Erros.FatalMessage("permissions error");
    }
});
ADVBS.Actions = {
    incrementaZoomAnteprima: function () {
        if (ADVBS.MessageSelected && ADVBS.MessageSelected.Zoom) {
            ADVBS.MessageSelected.Zoom++;
            ADVBS.Actions.doGetCoordinateAnteprima()
        }
    },
    decrementaZoomAnteprima: function () {
        if (ADVBS.MessageSelected && ADVBS.MessageSelected.Zoom) {
            ADVBS.MessageSelected.Zoom--;
            ADVBS.Actions.doGetCoordinateAnteprima()
        }
    },
    doNewMessage: function (messaggioId) {
        if (jQuery("#box-input-newMessage").hasClass("active"))
            return;

        jQuery("#btn-newMessage").parent().addClass("ADVhideMe");
        ADVBS.MessageSelected = new ADV.Models.dtoMessaggioBacheca();
        try {
            messaggioId = parseInt(messaggioId);
        } catch (e) {
            return;
        }
        if (messaggioId) {
            ADVBS.MessageSelected.ParentMessageId = messaggioId;
            var y = jQuery(window).scrollTop();
            jQuery("#box-input-newMessage").css("padding-top", ((y < 5) ? "4" : y + "") + "px");
        }
        if (!jQuery("#box-input-newMessage").hasClass("active"))
            jQuery("#box-input-newMessage").addClass("active");

        ADVBS.Actions.doGetCoordinateAnteprima();
        jQuery("#input-newMessage").val(ADVBS.MessageSelected.Text);
        jQuery("#input-newMessage").focus();
    },
    doRemoveMessage: function (messId) {
        ADVBS.Services.doRemoveMessage(messId);
    },
    doCancelNewMessage: function () {
        ADVBS.MessageSelected = null;
        jQuery("#box-input-newMessage").css("padding-top", "");
        jQuery("#box-input-newMessage").removeClass("active");
        jQuery("#btn-newMessage").parent().removeClass("ADVhideMe");
        ADVBS.Actions.doGetCoordinateAnteprima();
        jQuery("#input-newMessage").val('');
        jQuery("#input-newMessage").focusout();
    },
    doGetCoordinateAnteprima: function (takeCoords) {
        jQuery("#box-newMessageGeolocalization").addClass("ADVhideMe");

        if (!ADVBS.MessageSelected || ADV.isExplorerLTE9)
            return;

        if (!ADVBS.MessageSelected.Zoom)
            ADVBS.MessageSelected.Zoom = 12;

        if (ADVBS.MessageSelected.Latitude && ADVBS.MessageSelected.Longitude) {
            jQuery("#box-newMessageGeolocalization").removeClass("ADVhideMe");
            jQuery("#imgGoogleMapsAnteprima").attr("src", "https://maps.googleapis.com/maps/api/staticmap?maptype=TERRAIN&zoom=" + ADVBS.MessageSelected.Zoom + "&center=" + ADVBS.MessageSelected.Latitude + "," + ADVBS.MessageSelected.Longitude + "&markers=color:red%7Clabel:o%7C" + ADVBS.MessageSelected.Latitude + "," + ADVBS.MessageSelected.Longitude + "&size=320x200&key=AIzaSyCASRJqlZwddRJ04Etc076bMBGiVcmU4Ao");
        } else {
            if (takeCoords) {
                ADVBS.Utils.ADVlocation.getLocation(function (position) {
                    ADVBS.MessageSelected.Latitude = position.coords.latitude;
                    ADVBS.MessageSelected.Longitude = position.coords.longitude;

                    jQuery("#box-newMessageGeolocalization").removeClass("ADVhideMe");
                    jQuery("#imgGoogleMapsAnteprima").attr("src", "https://maps.googleapis.com/maps/api/staticmap?maptype=TERRAIN&zoom=" + ADVBS.MessageSelected.Zoom + "&center=" + ADVBS.MessageSelected.Latitude + "," + ADVBS.MessageSelected.Longitude + "&markers=color:red%7Clabel:o%7C" + ADVBS.MessageSelected.Latitude + "," + ADVBS.MessageSelected.Longitude + "&size=320x200&key=AIzaSyCASRJqlZwddRJ04Etc076bMBGiVcmU4Ao");
                });
            }
        }
    }
};
ADVBS.Utils = {
    customSortFromDate_DESC: function (a, b) {
        return new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime();
    },
    customSortFromDate_CRESC: function (a, b) {
        return new Date(a.CreateDate).getTime() + new Date(b.CreateDate).getTime();
    },
    ADVlocation: {
        getLocation: function (callBackWithPosition) {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    callBackWithPosition(position);
                });
            } else {
                alert("Geolocation is not supported by this browser.");
                return false;
            }
        }
    },
    getMessaggiofromList: function (MessId) {
        for (var i = 0; i < ADVBS.comunityMessages.length; i++) {
            if (ADVBS.comunityMessages[i].Id == MessId)
                return ADVBS.comunityMessages[i];
        }
    },
    getPermissions: function (name) {
        if (!ADV.Auth.Permissions)
            return false;

        if (ADV.Auth.Permissions["Admin"] || ADV.Auth.Permissions[name])
            return true;

        return false;
    }
};
ADVBS.Init = function (elParent) {
    _elParent = elParent;
    if (!_elParent)
        _elParent = document.body;

    _elParent = jQuery(_elParent);

    setTimeout(function () {
        ADVBS.UI.InitTemplateView();
    });
};
ADVBS.UI = {
    InitTemplateView: function () {
        if (_elParent.find("#AdvBachecaSocial").size() > 0) {
            return;
        }
        _elParent.html(
        '<div id="ADVBachecaSocial">' +
            '<div id="box-input-newMessage" style="' + (ADVBS.Utils.getPermissions("NewPost") ? '' : 'display:none;') + '">' +
                '<div id="box-scroll-newMessage">' +
                '</div>' +
            '</div>' +
            //this.HTMLHelper.getInputNewMessage(new ADV.Models.dtoMessaggioBacheca()) + 
            '<div id="box-listaMessaggi" class="clearfix">' +
            '</div>' +
        '</div>');
        this.RenderInputNewMessage(new ADV.Models.dtoMessaggioBacheca());
        ADVBS.Services.doCheckMessagesUpdates();
    },
    RenderMessages: function () {
        if (!ADVBS.comunityMessages)
            return;

        var strRender = "";
        jQuery("#box-listaMessaggi").html("");
        ADVBS.comunityMessages.sort(ADVBS.Utils.customSortFromDate_DESC);
        for (var i = 0; i < ADVBS.comunityMessages.length; i++) {
            var thisMess = ADVBS.comunityMessages[i];
            if (thisMess && (!thisMess.ParentMessageId || thisMess.ParentMessageId == 0) && jQuery("#messaggio-" + thisMess.Id).size() < 1)
                strRender += this.HTMLHelper.getOneMessage(thisMess);
        }
        jQuery("#box-listaMessaggi").prepend(strRender);

        for (var i = 0; i < ADVBS.comunityMessages.length; i++) {
            var thisMess = ADVBS.comunityMessages[i];
            if (thisMess && (thisMess.ParentMessageId && thisMess.ParentMessageId != 0) && jQuery("#messaggio-" + thisMess.Id).size() < 1)
                jQuery("#messaggio-" + thisMess.ParentMessageId + " .list-group-comments:first").append(this.HTMLHelper.getOneMessageComment(thisMess));
        }
    },
    RenderInputNewMessage: function (newMessage) {
        if (!ADVBS.comunityMessages)
            return;

        var strRender = this.HTMLHelper.getInputNewMessage(newMessage);

        jQuery("#box-scroll-newMessage").html(strRender);
    },
    HTMLHelper: {
        getButtonsNewMessage: function (newMessage) {
            strReturn = '';
            if (newMessage && !newMessage.ParentMessageId)
                //strReturn += '<!--i onmousedown="doGetPhoto($event)" class="btn btn-default btnChatAction glyphicon glyphicon-camera">&nbsp;</i-->';
                if (newMessage && !newMessage.ParentMessageId)
                    strReturn += '<i onmousedown="ADVBS.Actions.doGetCoordinateAnteprima(true)" class="btn btn-default btnChatAction glyphicon glyphicon-map-marker">&nbsp;</i>';
            //strReturn += '<!-- <i show="false && newMessage && !newMessage.ParentMessageId" onmousedown="doGetAttachment($event)" class="btn btn-default btnChatAction glyphicon glyphicon-paperclip"></i> + -->';
            if (newMessage)
                strReturn += '<i onclick="ADVBS.Services.doSendMessage()" class="btn btn-primary btnChatAction glyphicon glyphicon-send pull-right">&nbsp;</i>';
            strReturn += '<i onclick="ADVBS.Actions.doCancelNewMessage()" class="btn btn-danger btnChatAction glyphicon glyphicon-remove pull-right">&nbsp;</i>';
            strReturn += '</div>';

            strReturn += '<div id="box-newMessageGeolocalization" class="' + ((newMessage && newMessage.Latitude && newMessage.Longitude) ? ' ADVshowMe' : ' ADVhideMe') + '">' +
                            '<div class="positionPlusMinusgoogleMaps">' +
                                '<i class="btn glyphicon glyphicon-plus btn btn-default" onclick="ADVBS.Actions.incrementaZoomAnteprima();">&nbsp;</i><br />' +
                                '<i class="btn glyphicon glyphicon-minus btn btn-default" onclick="ADVBS.Actions.decrementaZoomAnteprima();">&nbsp;</i>' +
                            '</div>' +
                            '<img id="imgGoogleMapsAnteprima" style="width:100%;height:auto;" border="0" alt="geolocalizzazione messaggio" />' +
                        '</div>';

            strReturn += '<div class="box-fotoMessage' + ((newMessage && newMessage.appImageUri) ? ' ADVshowMe' : ' ADVhideMe') + '" style="padding: 12px 0;">' +
                            '<div class="divImgLoader" style="height:' + newMessage.percImgUpload + '%;"><span> ( ' + (newMessage.percImgUpload ? newMessage.percImgUpload : 0) + '%)</span></div>' +
                            '<img class="fotoMessage" src="' + (newMessage.appImageUri ? newMessage.appImageUri : '') + '" />' +
                        '</div>';
            return strReturn;
        },
        getInputNewMessage: function (newMessage) {
            strReturn = '';
            strReturn += '<div class="input-group" style="width:100%;">' +
                            '<textarea id="input-newMessage" onfocus="ADVBS.Actions.doNewMessage()" class="form-control" placeholder="New message..." style="width:100%;"></textarea>' +
                            '<span class="input-group-btn" style="vertical-align: bottom;">' +
                                '<span id="btn-newMessage" onclick="ADVBS.Actions.doNewMessage()" class="btn btn-success glyphicon glyphicon-newMessage btnChatAction pull-right">&nbsp;</span>' +
                            '</span>' +
                        '</div>';
            strReturn += '<div id="boxNewMessageActions" class="clearfix" style="padding: 4px 10px;">' +
                            this.getButtonsNewMessage(newMessage) +
                        '</div>';
            return strReturn;
        },
        getOneMessage: function (messaggio) {
            var strReturn = '<div id="messaggio-' + messaggio.Id + '" class="boxMessaggioAM">' +

            '<div class="popover ' + ((messaggio.OwnerId + "" !== ADVBS.Params.PersonId + "") ? 'right' : 'left') + ' messaggioAM ' + ((messaggio.Id) ? '' : 'newMessage progress-bar-striped') + '">' +
            '<div class="arrow" style="top: 14px;"></div>' +
            '<div class="popover-content">' +
            '<div style="font-size: 18px;color:#555;"><span>' + this.getTextRows(escapeHtml(messaggio.Text)) + '<br /></span></div>' +
            '<div class="popover-header"><span class="ownerName">' + messaggio.OwnerName + '</span><span class="dataCreazioneMessaggio pull-right">' + this.dateToString(messaggio.CreateDate, true) + '</span></div>';

            strReturn += '<div class="boxImgGeo clearfix">';
            strReturn += '<div class="' + ((messaggio && messaggio.Latitude && messaggio.Longitude) ? ' ADVshowMe' : ' ADVhideMe') + '">' +
            '<a href="https://www.google.com/maps?q=@' + messaggio.Latitude + ',' + messaggio.Longitude + '" target="_blank">' +
            '<img style="width:100%;height:auto;" border="0" src="https://maps.googleapis.com/maps/api/staticmap?maptype=TERRAIN&zoom=' + (messaggio.Zoom ? messaggio.Zoom : 12) + '&center=' + messaggio.Latitude + ',' + messaggio.Longitude + '&markers=color:red%7Clabel:o%7C' + messaggio.Latitude + ',' + messaggio.Longitude + '&size=320x200&key=AIzaSyCASRJqlZwddRJ04Etc076bMBGiVcmU4Ao" alt="geolocalizzazione messaggio" />' +
            '</a>' +
            '</div>';

            strReturn += '<div class="box-fotoMessage">';
            if (messaggio.ImgFileName)
                strReturn += '<img class="fotoMessage horizontal imagefullscreen" src="' + ADVBS.Params.WebApiBaseUrl + 'api/Repository?fileName=' + messaggio.ImgFileName + '" />';
            strReturn += '</div>';
            strReturn += '</div>';
            strReturn += '<div class="popover-footer clearfix">&nbsp;' +
			'<span onclick="ADVBS.Actions.doRemoveMessage(\'' + messaggio.Id + '\');" style="' + (ADVBS.Utils.getPermissions("Admin") ? '' : 'display:none;') + '" class="btn btn-danger btnAM-remove">X</span>' +
			'<span onclick="ADVBS.Actions.doNewMessage(\'' + messaggio.Id + '\');" style="' + (ADVBS.Utils.getPermissions("Comment") ? '' : 'display:none;') + '" class="btn btn-default btnAM-replay">Add comment</span>' +
			'</div>' +
            '</div>' +
            '</div>' +
            '<div class="list-group list-group-comments"></div>' +
            '</div>';

            return strReturn;
        },
        getOneMessageComment: function (messReplay) {
            var htmlReturn = '<div id="messaggio-' + messReplay.Id + '" class="list-group-item' + ((messReplay.OwnerId + "" !== ADVBS.Params.PersonId + "") ? '' : ' myMessage') + '">' +
                '<h4 class="list-group-item-heading">' + messReplay.OwnerName + '<span class="dataCreazioneMessaggio pull-right">' + this.dateToString(messReplay.CreateDate, true) + '</span></h4>' +
                '<div class="list-group-item-text">' + escapeHtml(messReplay.Text) + '</div>' +
				'<div class="clearfix"><span onclick="ADVBS.Actions.doRemoveMessage(\'' + messReplay.Id + '\');" style="' + (ADVBS.Utils.getPermissions("Delete") ? '' : 'display:none;') + '" class="btn btn-danger btn-xs btnAM-remove">X</span></div>' +
                '</div>';

            return htmlReturn;
        },
        getTextRows: function (strText) {
            if (strText) {
                return strText.replace(/(?:\r\n|\r|\n)/g, '<br />'); // togleire HTML dal testo
            }
            return '';
        },
        dateToString: function (date, withTime) {
            if (!date)
                return;
            var d = date;
            if (!d.getDate && !d.getFullYear) {
                d = new Date(date);
                d.setHours(d.getHours() + (d.getTimezoneOffset() / 60));
            }

            var day = d.getDate();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            var hours = d.getHours();
            var minutes = d.getMinutes();
            return '' + ((day < 10) ? "0" + day : day) + '/' + ((month < 10) ? "0" + month : month) + '/' + d.getFullYear() + (withTime ? " " + ((hours < 10) ? "0" + hours : hours) + ':' + ((minutes < 10) ? "0" + minutes : minutes) + '' : "");
        }
    }
};

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