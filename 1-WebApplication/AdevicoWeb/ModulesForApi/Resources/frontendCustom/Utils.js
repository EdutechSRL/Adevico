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
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getRequestParam(param) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == param) {
            var retParms = pair[1];
            if (retParms.indexOf(",") >= 0)
                return retParms.split(",");

            return [retParms];
        }
    }
    return null;
}

function getHeadersApiForLocalHost() {
    return {
        Token: getCookie("Token"),
        DeviceId: getCookie("DeviceId"),
        CommunityId: getCookie("CommunityId"),
        LangId: getCookie("LangId"),
        LangCode: getCookie("LangCode"),
        PersonId: getCookie("PersonId")
    };
}