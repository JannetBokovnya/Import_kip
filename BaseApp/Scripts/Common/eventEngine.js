var _Ctl = null;
var _ListenerEvents = null;
var _isLoad = false;

function GetCtl() {
    return _Ctl;
}

function SetCtl(objectElement) {
    _Ctl = objectElement;
    _isLoad = true;
    JLOG.info('Контекст приложения задан' + objectElement);
}

function receiveAdapter(eventName, params) {
    var result = 0;
    JLOG.info('Параметры готовы к передачи в приложение. in_EventName:' + eventName + '; in_Params:' + params);
    if (_isLoad) {
        var getCtl = GetCtl();
        if (getCtl != null) {
            GetCtl().receiveEvent(eventName, params);
            result = 1;
            JLOG.info('Параметры переданы');
        }
    }

    return result;
}

function receiveEvent() {
    var listenerEventArray = ListenerEvents();
    if (listenerEventArray != null && listenerEventArray !== "") {
        var listerEvent;
        var listenerValue;
        var events = JSON.parse(listenerEventArray).EventName;

        for (var i = 0; i < events.length; i++) {
            listerEvent = events[i];
            listenerValue = getCookieByName(listerEvent);

            if (listenerValue != null && listenerValue !== "") {
                var json = JSON.parse(listenerValue);
                var destination = json.Destination;

                if (isEmpty(destination) === true) {
                    ///check function exists, if not delete event
                    if (window.receiveAdapter) {
                        var isSend = receiveAdapter(listerEvent, listenerValue);
                    } else {

                        isSend = 1;
                    }
                    //if event= windowOpen delete event
                    if (listerEvent === "OPEN_WINDOW") {
                        isSend = 1;
                    }

                    if (isSend === 1) {
                        delCookie(listerEvent);
                    }

                } else {
                    for (var k = destination.length - 1; k >= 0; k--) {
                        if (destination[k] === GetWindowName()) {
                            if (listerEvent === "NAVIGATION_CONTEXT_CHANGED") {
                                SendEventFromModuleWindows(listerEvent, listenerValue);

                                k = -1;
                                delCookie(listerEvent);
                            }
                            else {
                                ///check function exists, if not delete event
                                if (window.receiveAdapter) {
                                    isSend = receiveAdapter(listerEvent, listenerValue);

                                } else {
                                    isSend = 1;
                                }
                                //if event= windowOpen delete event
                                if (listerEvent === "OPEN_WINDOW") {
                                    isSend = 1;
                                }
                                if (isSend === 1) {
                                    if (destination.length === 1) {
                                        delCookie(listerEvent);
                                    }
                                    else {
                                        removeFromArray(destination, destination[k]);

                                        json.Destination = destination;
                                        a = JSON.stringify(json);
                                        setCookie(listerEvent, a);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    setTimeout(receiveEvent, 500);
}

//Отправление в флеш модуля события на прямую, задав имя и значение события
function SendEventToFlashModules(eventName, eventValue) {
    if (_isLoad) {
        var getCtl = GetCtl();
        if (getCtl != null) {
            GetCtl().receiveEvent(eventName, eventValue);
        }
    }
}

//Обновление модуля из соседней вкладки с помощью window.open
function SendEventFromModuleWindows(listerEvent, listenerValue) {
    //Сделал эту функцию для гарантиии что все модули получат по событию. 
    //Так как раньше один модуль мог получить два раза событие, а второй ни разу.
    //Так за отправку события всем модулям отвечает один модуль
    var curWinStatus = getCookieByName(_WindowStatus);
    if (curWinStatus != undefined) {
        l_r = JSON.parse(curWinStatus);
        var count = l_r.WindowStatus.length;
        var cmdStr = "javascript:UpdateCNForModule('" + listerEvent + "', '" + ChangeFlashToDoubleFlash(listenerValue) + "')";
        var openedWindows = [];
        for (var y = 0; y < count; y++) {
            openedWindows[y] = l_r.WindowStatus[y].WindowName;
        }
        for (y = 0; y < count; y++) {
            window.open(cmdStr, openedWindows[y]);
        }
    }
}

function sendEvent(eventName, params, inDestination) {
    //переопределение метода. объект in_Params в строку
    if (typeof (params) == 'object') {
        params = JSON.stringify(params);
    }
    var destination = "";
    if (inDestination === "broadcast") {
        destination = GetListenWindow(eventName);
    }
    else if (inDestination !== "") {
        Send2Window(eventName, inDestination);
        destination = '"' + inDestination + '"';
    }
    else {
        Send2Window(eventName, inDestination);
    }

    var jsonStr = '{"SenderWindow":"' + GetWindowName() + '", "Destination":[' + destination + '], "EventParam": ' + params + ' }';
    setCookie(eventName, jsonStr);
}


function GetListenerEvent() {

    JLOG.info('Получить список возможных событий для приложения');
    var url = GetBaseURL() + GetService() + "?inSQL=" + EventEngineQueryGetEvents4App + "('" + GetWindowName() + "')"
                                          + "&inParams=&inType=2&nSeq=&isCompress=0&u=0.41930";
    ajaxThis(url, ParseListenerEvent, true);
}

function ParseListenerEvent(req) {
    _ListenerEvents = req.responseText;

    var curWinStatus = getCookieByName(_WindowStatus);
    JLOG.info('Список событий получен :' + _ListenerEvents);
    var l_r = JSON.parse(curWinStatus);
    var events = JSON.parse(_ListenerEvents);
    for (var i = 0; i < l_r.WindowStatus.length; i++) {
        var winName = GetWindowName();
        if (l_r.WindowStatus[i].WindowName === winName) {
            l_r.WindowStatus[i].EventListen.push(events);
        }
    }
    var a = JSON.stringify(l_r);
    setCookie(_WindowStatus, a);
}

function ListenerEvents() {
    return _ListenerEvents;
}


function GetListenWindow(inEventName) {
    var l_c = "";
    var windowName;

    var curWinStatus = getCookieByName(_WindowStatus);
    var l_r = JSON.parse(curWinStatus);
    for (var i = 0; i < l_r.WindowStatus.length; i++) {
        var eventListen = l_r.WindowStatus[i].EventListen;
        windowName = l_r.WindowStatus[i].WindowName;
        for (var j = 0; j < eventListen.length; j++) {
            var eventName = eventListen[j].EventName;
            for (var k = 0; k < eventName.length; k++) {
                if (eventName[k] === inEventName) {
                    if (l_c !== "") {
                        l_c = l_c + ",";
                    }
                    l_c = l_c + '"' + windowName + '"';
                }
            }
        }
    }

    return l_c;
}