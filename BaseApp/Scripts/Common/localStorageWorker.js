
function delCookie(name) {
    setCookie(name, '');
    JLOG.debug('Кука: ' + name + ' - очищена');
}

function getCookieByName(name) {
    CheckCookiesId();
    var item = localStorage.getItem(name);
    if (item != null)
        item = unescape(item);
    return item;
}

function setCookie(name, value) {
    CheckCookiesId();
    var localStoreSupport = isLocalStorageAvailable();
    if (localStoreSupport) {
        try {
            var storageParam = new Object();
            storageParam.key = name;
            storageParam.oldValue = getCookieByName(name);
            storageParam.newValue = value;

            localStorage.setItem(name, value);

        } catch (e) {
            JLOG.fatal('LocalStorage: ошибка при записи.' + e.message);
        }
    }

}

function CheckCookiesId() {
    var cookiesId = GetCookiesId();
    if (cookiesId == null || cookiesId == 'null' ||
        cookiesId == undefined || cookiesId == 'undefined') {
        localStorage.clear();
        SetCookiesId();
    }
}

function SetCookiesId() {
    document.cookie = "CookiesId" + "="
            + Math.floor((1 + Math.random()) * 0x10000).
                                toString(16).substring(1) + "; path=/";;
}

function GetCookiesId() {
    var i, x, y, cookiesArr = document.cookie.split(";");
    for (i = 0; i < cookiesArr.length; i++) {
        x = cookiesArr[i].substr(0, cookiesArr[i].indexOf("="));
        y = cookiesArr[i].substr(cookiesArr[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x === "CookiesId") {
            return unescape(y);
        }
    }

    return null;
}

function isLocalStorageAvailable() {
    try {
        return 'localStorage' in window && window['localStorage'] !== null;
    } catch (e) {
        JLOG.fatal('Не поддерживается LocalStorage!');
        return false;
    }
}

function setSessionStart() {
    var sessionStart = "sessionStart";
    var isSessionStart = getCookieByName(sessionStart);
    if (isSessionStart == null) {
        var timeOnPageLoad = new Date();
        setCookie(sessionStart, timeOnPageLoad);
    }
}

function SetLocalStorageEvent() {
    if (window.addEventListener) {
        window.addEventListener("storage", handle_storage, false);
    } else {
        handle_storageIE8();
    };
}

function handle_storage(e) {
    if (!e) { e = window.event; }

    if (e.key === "jSonNotitications") {
        UpdateNotificationControl();
    }
}

//Таймер замены событий localStorage в IE8
var prevNotificationJson;
function handle_storageIE8() {
    var jSonNotifications = getCookieByName('jSonNotitications');
    if (prevNotificationJson !== jSonNotifications) {
        UpdateNotificationControl();
        prevNotificationJson = jSonNotifications;
    }

    setTimeout(handle_storageIE8, 1000);
}