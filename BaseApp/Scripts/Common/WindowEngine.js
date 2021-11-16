var _paramsDelim = "|";

var _cWindowName = null;
var _WindowRegisterName = "WindowLoaded";
var _WindowStatus = "WindowStatus";
var _WindowIsCleanStorage = "IsCleanStorage";

function OpenNewWindow(windowName, instance) {
    JLOG.info('Получить URL для приложения');
    var url = GetBaseURL() + GetService() + "?inSQL=" + WindowEngineQueryGetUrl4WindowName + "('" + windowName + "','" + instance + "')"
                                          + "&inParams=&inType=2&nSeq=&isCompress=0&u=0.41930";
    ajaxThis(url, ParseOpenNewWindow, true);
}

function isWindowOpen(window2Send) {
    window2Send = window2Send + _paramsDelim;
    var loadWindow = getCookieByName(_WindowRegisterName);
    var isExists = loadWindow.indexOf(window2Send);

    var result = (isExists >= 0) ? 1 : 0;
    return result;
}

function ParseOpenNewWindow(req) {
    var response = req.responseText;
    JLOG.debug('Получено URL для прилоежния: ' + response);
    var url = JSON.parse(response).WindowURL;
    var instance = JSON.parse(response).Instance;
    var lurlInstance = (instance !== "") ? "?instanceName=" + instance : "";
    var urlFull = GetBaseURL() + url + lurlInstance;
    JLOG.debug('window.open(' + urlFull + ',' + instance + ')');
    window.open(urlFull, instance).focus();
}

function GetWindowName() {
    return _cWindowName;
}

function windowClosed(windowName) {
    JLOG.info('Модуль закрыт:' + windowName);
    windowRegister(windowName, 'windowClosed');
}

function closeAllWindows() {
    var curWinStatus = getCookieByName(_WindowStatus);
    if (curWinStatus != undefined) {
        var curWinStatusObj = JSON.parse(curWinStatus);

        for (var i = 0; i < curWinStatusObj.WindowStatus.length; i++) {
            var windowName = curWinStatusObj.WindowStatus[i].WindowName;

            if (window.GetWindowName() !== windowName) {
                CloseWindow(windowName);
            }
        }

        setCookie(_WindowIsCleanStorage, "true");
    }
}

function CloseWindow(windowName) {
    var curWinStatus = getCookieByName(_WindowStatus);
    var l_r = JSON.parse(curWinStatus);
    for (var i = 0; i < l_r.WindowStatus.length; i++) {
        if (l_r.WindowStatus[i].WindowName === windowName) {
            l_r.WindowStatus[i].Status = "Close";
        }
    }
    var a = JSON.stringify(l_r);
    setCookie(_WindowStatus, a);
}

function DeleteWindow(windowName) {
    var curWinStatus = getCookieByName(_WindowStatus);
    var curWinStatusObj = JSON.parse(curWinStatus);

    for (var i = 0; i < curWinStatusObj.WindowStatus.length; i++) {
        if (curWinStatusObj.WindowStatus[i].WindowName === windowName) {
            curWinStatusObj.WindowStatus.splice([i], 1);
        }
    }

    var a = JSON.stringify(curWinStatusObj);
    setCookie(_WindowStatus, a);
}

function addWindow() {
    var curWinStatus = getCookieByName(_WindowStatus);

    var l_r = (curWinStatus == null) ? { "WindowStatus": [] } : JSON.parse(curWinStatus);

    l_cPush = '{ "WindowName": "' + GetWindowName() + '" ,"WindowTitle":"' + GetWindowTitle() + '","Status":"Up", "CNSync":"false", "EventListen":[]}';

    l_cPushJson = JSON.parse(l_cPush);
    l_r.WindowStatus.push(l_cPushJson);
    var a = JSON.stringify(l_r);
    setCookie(_WindowStatus, a);
}


//register new window
function windowRegister(windowName, eventName) {
    setWindowName(windowName);
    var windowNameWithDelim = windowName + _paramsDelim;
    var loadWindow = getCookieByName(_WindowRegisterName);
    if (eventName === "windowLoad") {
        if (loadWindow == null) {

            setCookie(_WindowRegisterName, windowNameWithDelim);
        }
        else {
            var l_r = (loadWindow == null) ? windowNameWithDelim : loadWindow + windowNameWithDelim;

            setCookie(_WindowRegisterName, l_r);
        }
        addWindow();
        ChekWindowStatus();
    }
    else if (eventName === "windowClosed") {
        DeleteWindow(GetWindowName());
        var l_r = loadWindow.replace(windowNameWithDelim, '');
        setCookie(_WindowRegisterName, l_r);
    }
}

function ChekWindowStatus() {
    var currentWindowName = GetWindowName();
    var windowStatus = getCookieByName(_WindowStatus);
    var jsonStr = JSON.parse(windowStatus);
    var l_status = jsonStr.WindowStatus;

    SetHdnWindowOpen(windowStatus);

    for (var i = 0; i < l_status.length; i++) {
        if (l_status[i].WindowName === currentWindowName) {
            if (l_status[i].Status === "Close") {
                window.close();
            }
        }
    }

    setTimeout(ChekWindowStatus, 500);
}

function countOpenedWindows() {

    var curWinStatus = getCookieByName(_WindowStatus);
    if (curWinStatus != undefined) {
        var json = JSON.parse(curWinStatus);
        return json.WindowStatus.length;
    }
    else {
        return 0;
    }
}

function setWindowName(windowName) {
    window.name = windowName;
}

function GetWindowTitle() {
    return window.document.title;
}