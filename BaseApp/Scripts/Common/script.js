///#source 1 1 /Scripts/Common/QueryStorage.js
var BookmarkQueryDeleteState = "db_api.wrapper_api.DeleteState";
var EventEngineQueryGetEvents4App = "db_api.wrapper_api.GetEvent4Application";
var WindowQueryGetWindowName4Event = "db_api.wrapper_api.GetWindowName4Event";
var WindowEngineQueryGetUrl4WindowName = "db_api.wrapper_api.GetURL4WindowName";


var notificationQueryGetNotification = "db_api.WRAPPER_API.getNotification"; //возвращает все уведомления
var notificationQuerySetImportantStatus = "db_api.WRAPPER_API.setImportanceOnOff"; //устанавливаем уведомление - важное не важное true, false
var notificationQuerySetAsReading = "db_api.WRAPPER_API.NotificationDelivered";
///#source 1 1 /Scripts/Common/BookmarksEngine.js

function getStateForBookmark() {
    var ctl = GetCtl();
    if (ctl != undefined) {
        GetCtl().getCurrentState();
        var currentState = document.getElementById('hdnNewBookmark').value;
        if (currentState === '') {
            JLOG.error('Функция модуля getCurrentState() вернула пустую строку!');
            document.getElementById('hdnNewBookmark').value = '{}';
        }
    } else {
        window.getCurrentState();
    }
}

function getCurrentState() {
    setCurrentState('{}');
    return '{}';
}

function DeleteBookMark(idBookmark) {
    //пока нет этой ф-ции
    var url = GetBaseURL() + GetService() + "?inSQL=" + BookmarkQueryDeleteState + "(" + idBookmark + ")&inParams=&inType=2&nSeq=&isCompress=0&u=0.41930";
    //переделать строчку в скрытом поле
    ajaxThis(url, ParseDeleteBookMark, true);
}

//Может возвращать два состояния. Или ИмяМодуля, или ИмяМодуля_КлючСостояния, 
//если он существует
function GetWindowNameForBookmark() {
    var ctl = GetCtl();

    if (ctl != undefined) {
        if (typeof (ctl.getWindowNameWithKey) != "undefined") {
            return ctl.getWindowNameWithKey();
        }
    }

    return GetWindowName();
}

///#source 1 1 /Scripts/Common/ContextNavigationEngine.js
var toOpen = true;
var _isSynch = false;
var isOpenedCN = false;

var NAVIGATION_CONTEXT_CHANGED = {
    updateCN: function (e) {
        if (checkContext() && _isSynch) {
            var curCn = getCN();
            JLOG.info('Обновить контекст навигации во всех окнах: ' + curCn);
            sendEvent("NAVIGATION_CONTEXT_CHANGED", curCn, GetWindowName());
        }
    }
}

function getIsOpenedCN() {
    return isOpenedCN;
}

function setIsOpenedCN(value) {
    isOpenedCN = value;
}

function enableCNObserver() {
    observerable.addListener(NAVIGATION_CONTEXT_CHANGED, "NAVIGATION_CONTEXT_CHANGED", "updateCN");
}

function changeCNSync() {
    (!getIsOpenedCN()) ? enableCNSync() : disableCNSync();
}

//контекст навигации поменялся
function changedContext() {
    getCN();
}

function getCN() {
    var curCn = getCookieByName('ContextNavigation');
    if (curCn == null | curCn === "undefined") {
        curCn = '{}';
    } else {
        var obj = JSON.parse(curCn);

        var start = parseFloat(obj.KmStart);
        var end = parseFloat(obj.KmEnd);

        obj.KmStart = start.toFixed(2);
        obj.KmEnd = end.toFixed(2);

        curCn = JSON.stringify(obj);
    }

    return curCn;
}

function setCN(value) {
    setCookie("ContextNavigation", value);
}

//Устанавливает значение кнопки сонхронизации. Меняет стиль кнопки
function doSynch() {
    _isSynch = !_isSynch;
    if (_isSynch) {
        var currentCn = document.getElementById('hdnCurrentCN').text;
        if (currentCn != undefined) {
            setCN(currentCn);
        }
    }
    SetActiveSynch(_isSynch);
    changeCNSync();
}

//включить в текущем окне контекст навигации
function enableCNSync() {
    JLOG.debug("Включен контекст навигации");
    if (checkContext()) {
        observerable.triggerEvent("NAVIGATION_CONTEXT_CHANGED", {});
    }
}

//выключить в текущем окне контекст навигации
function disableCNSync() {
    JLOG.debug("Выключен контекст навигации");
    setIsOpenedCN(false);
}

//Обновить шапку в БК под новый контекст
function changeLabelCN(str) {
    if (str != null | str != undefined) {
        var contextNavigationStr = JSON.parse(str);
        var threadName = contextNavigationStr.NameThreadShorten;
        if (threadName != undefined) {
            document.getElementById('hdnCurrentCN').text = str;
            if (threadName.length < 40) {
                document.getElementById('lblTest').innerText = threadName + "\n"
                    + contextNavigationStr.KmStart + " - " + contextNavigationStr.KmEnd + " м";
            } else {
                document.getElementById('lblTest').innerText = threadName.substr(0, 40) + '...' + "\n"
                    + contextNavigationStr.KmStart + " - " + contextNavigationStr.KmEnd + " м";
            }

            document.getElementById('lblTest').title = threadName + "\t " + contextNavigationStr.KmStart
                                                                  + " - " + contextNavigationStr.KmEnd + " м";

            SetKmToCnPanel(contextNavigationStr);
        }
    }
}

//Отобразить-скрыть панель ввода контекста навигации
function ShowContextNavigationPannel(toOpen) {
    var topPanel = 0;

    if (toOpen) {
        document.getElementById('divContextNavigation').style.top = topPanel;
        document.getElementById('divContextNavigation').style.visibility = 'visible';
        document.getElementById('divContent').style.top = topPanel;

        HidePannel('divBookMark');
        SetActiveContextNavigation(true);
    }
    else {
        HidePannel('divContextNavigation');
        document.getElementById('divContent').style.top = 0;
        SetActiveContextNavigation(false);
    }
    toggleDisabled(document.getElementById('divContent'), toOpen);
}

//установка css стиля кнопки синхронизации 
function SetActiveContextNavigation(isActive) {
    if (isActive) {
        document.getElementById('divCNChange').className = "imgCNChangeActive";
    }
    else {
        document.getElementById('divCNChange').className = "imgCNChange";
    }
}

//Уставновить КН для всех модулей с синхронизацией
function setContextNavigation() {
    ChangeIsOpenedCN();
    ShowContextNavigationPannel(false);
    observerable.triggerEvent("NAVIGATION_CONTEXT_CHANGED", {});
}

//задан ли контекст навигации
function checkContext() {
    var curCn = getCookieByName('ContextNavigation');
    return curCn != null;
}
//открыть контекст навигации
function openContext() {
    isOpenedCN = true;
    ShowContextNavigationPannel(getIsOpenedCN());
}

//закрыть контекст навигации
function closeContext() {
    isOpenedCN = false;
    ShowContextNavigationPannel(getIsOpenedCN());
}

function cancelContextMenu() {
    isOpenedCN = false;
    ShowContextNavigationPannel(false);
}

//Отправляет флешовому модулю выбраный КН и обновляет шапку в меню
function UpdateCNForModule(listerEvent, listenerValue) {
    var destination = '';
    var event = JSON.parse(listenerValue);
    if (event != null) {
        var destinations = event.Destination;
        if (destinations.length > 0) {
            destination = destinations[0];
        }
    }
    var lRes;
    if (_isSynch) {
        lRes = receiveAdapter(listerEvent, listenerValue);
        if (lRes === 0) {
            setTimeout(function () { receiveAdapter(listerEvent, listenerValue); }, 1000);
        }
        var curCn = getCN();
        changeLabelCN(curCn);
    }
    else if (_cWindowName === destination) {
        lRes = receiveAdapter(listerEvent, listenerValue);
        if (lRes === 0) {
            setTimeout(function () { receiveAdapter(listerEvent, listenerValue); }, 1000);
        }
        if (event.EventParam != null) {
            var start = parseFloat(event.EventParam.KmStart);
            var end = parseFloat(event.EventParam.KmEnd);
            event.EventParam.KmStart = start.toFixed(2);
            event.EventParam.KmEnd = end.toFixed(2);
            curCn = JSON.stringify(event.EventParam);
            changeLabelCN(curCn);
        }
    }
}

//Обновление КН. Вызывается флеш модулем.
function changeKMContext(kmBeg, kmEnd) {
    if (_isSynch) {
        var curCn = getCN();
        var lR = JSON.parse(curCn);
        var lKmBegCur = lR.KmStart;
        var lKmEndCur = lR.KmEnd;
        if (lKmBegCur !== kmBeg || lKmEndCur !== kmEnd) {
            lR.KmStart = kmBeg;
            lR.KmEnd = kmEnd;
            var value = JSON.stringify(lR);
            setCN(value);
            observerable.triggerEvent("NAVIGATION_CONTEXT_CHANGED", {});
        }
    }
    else {
        //Обновить лейбу КН для одного модуля
        var cnStr = document.getElementById('hdnCurrentCN').text;
        if (cnStr != null) {
            var cnObj = JSON.parse(cnStr);
            var start = kmBeg;
            var end = kmEnd;
            cnObj.KmStart = start.toFixed(2);
            cnObj.KmEnd = end.toFixed(2);
            cnStr = JSON.stringify(cnObj);
            changeLabelCN(cnStr);
            document.getElementById('hdnCurrentCN').text = cnStr;
        }
    }
}

// Задает параметр открыта ли панель задания КН
function ChangeIsOpenedCN() {
    isOpenedCN = !isOpenedCN;
}

function HidePannel(namePanel) {
    document.getElementById(namePanel).style.top = 0;
    document.getElementById(namePanel).style.visibility = 'hidden';
}
///#source 1 1 /Scripts/Common/eventEngine.js
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
///#source 1 1 /Scripts/Common/localStorageWorker.js

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
///#source 1 1 /Scripts/Common/NotificationsEngine.js

// Проверка времени последнего получения уведомлений. Если время истекло обновить
function checkTimeNotification(isFirstLoad) {

   
    //отключаю уведомления. потому что ломают импорт. 
    //надо разобраться
    //return;

    // время задержки между обновлениями. 
    // 60000 перевод в минуты
    var updatingTimeMm = 0.2 * 60000;

    // При открытии страницы обновить состояние уведомлений для текущего окна
    if (isFirstLoad === undefined || isFirstLoad === null) {
        UpdateNotificationControl();
    }

    var now = new Date().getTime();
    var lastNotificationUptatingTime = getCookieByName('NotificationUpdatingTime');

    if (lastNotificationUptatingTime == undefined || lastNotificationUptatingTime == null) {
        setCookie('NotificationUpdatingTime', now);
        CheckNewNotification();
    } else {
        if (now - updatingTimeMm > lastNotificationUptatingTime) {
            setCookie('NotificationUpdatingTime', now);
            CheckNewNotification();
        }
    }

    setTimeout(function () { checkTimeNotification(1); }, 5000);
}

//функция для проверки новых уведомлений
function CheckNewNotification() {
    //var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQueryGetNotification
    //                                      + "('" + document.getElementById('hdnUser').value + "','0')"
    //                                      + "&inParams=&inType=2&ContentType=application/json&isCompress=0&u=0.41930";

    //задаем время в unixtime 1460630132 (04/19/2016)  db_api.WRAPPER_API.getNotification
    var dateToUnixTime7Day = setDataUnixTime7Day(); // "('1460630132')"
    var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQueryGetNotification
                                          + "(" + dateToUnixTime7Day + ")" 
                                          + "&inParams=&inType=2&ContentType=application/json&nSeq=&isCompress=0&u=0.41930";

    // если false - синхронный запрос true  - асинхронный
    ajaxThis(url, ParseCheckNewNotification, true);
}

//запись полученных уведомлений в хранилище
function ParseCheckNewNotification(req) {

    //var jSonNotitications = xmlToJson(req.responseText);

    var jSonNotitications = req.responseText;
    //обновление контролов происходит при событии в localStorage
    setCookie("jSonNotitications", jSonNotitications);
}

//парсер xml to json 
function xmlToJson(xml) {
 

     //Create the return object
    var obj = {};

    if (xml.nodeType == 1) { // element
        // do attributes
        if (xml.attributes.length > 0) {
            obj["@attributes"] = {};
            for (var j = 0; j < xml.attributes.length; j++) {
                var attribute = xml.attributes.item(j);
                obj["@attributes"][attribute.nodeName] = attribute.nodeValue;
            }
        }
    } else if (xml.nodeType == 3) { // text
        obj = xml.nodeValue;
    }

    // do children
    if (xml.hasChildNodes()) {
        for (var i = 0; i < xml.childNodes.length; i++) {
            var item = xml.childNodes.item(i);
            var nodeName = item.nodeName;
            if (typeof (obj[nodeName]) == "undefined") {
                obj[nodeName] = xmlToJson(item);
            } else {
                if (typeof (obj[nodeName].push) == "undefined") {
                    var old = obj[nodeName];
                    obj[nodeName] = [];
                    obj[nodeName].push(old);
                }
                obj[nodeName].push(xmlToJson(item));
            }
        }
    }
    return obj;
};

// Обновление состояния уведомления в БД
// Передача ивента клику байдинговому чекбоксу
function changeCheckBox(divElement) {
    var checkBox = $(divElement).parent().children("input[type='checkbox']");
    // Изменение байдингового div по клику на чекбокс
   
    checkBox[0].onchange = onChangeEventFunc;

    checkBox[0].click();
  
}


// Событие изменения состояния чекБокса
// получение ключа уведомления и обновление его состояния в БД
function onChangeEventFunc() {
    var isImportant = this.checked;

    var divCheckBox = $(this).parent().children("div#divCheckBox");

    var notificationPosition = divCheckBox.attr('listBoxIndex');
    var lbNotification = $find("ctl00_Body_pnlNotifications_radLstBox");
    var item = lbNotification.get_items().getItem(notificationPosition);
    var itemkey = item.get_value();

    ChangeNotificationImportantStatus(itemkey);

}

//Установить статус импорта для уведомления (важные, не важные)
function ChangeNotificationImportantStatus(notificationKey) {
    var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQuerySetImportantStatus
                                          + "(" + notificationKey  + ")"
                                          + "&inParams=&inType=2&ContentType=application/json&isCompress=0&u=0.41930";
     ajaxThis(url, ParseChangeNotificationImportantStatus, true);
   // ajaxThis(url, UpdateNotificationControl, false);
}

// после обновления статуса в бд меняем цвет контрола
function ParseChangeNotificationImportantStatus() {

    CheckNewNotification();
    //var bindedDiv = $(this).parent().children("div#divCheckBox");
    //bindedDiv.css("background", 'url(' + imageBindingDiv + ')');

    //if (!this.checked) {
    //    bindedDiv.css("background-position", '-4px -55px');
    //} else {
    //    bindedDiv.css("background-position", '-31px -55px');
    //}

}

function myFunc (ev) 
{ 
    var e = window.event || ev, obj = e.target || e.srcElement; 

    //нужно выбрать элемент только текст (а не чекбокс)
    if (e.tagName === 'SPAN') {

        alert(e.item.get_attributes().getAttribute("cState"));
        alert(obj.id); //или (obj.href) или вообще всё то, что надо знать о теге obj, на который кликнули 

    }
}

// обновление контролов уведомления
function UpdateNotificationControl() {
    "use strict"; //был отключен - включила для тестировки
  //  return;

    var notificationJson = getCookieByName("jSonNotitications");


    if (notificationJson !== undefined && notificationJson !== null) {

        var notifications = JSON.parse(notificationJson);

        var menu = $find("rmNotification");
        
        // Если меню открыто клиентом подождать 2 секунты и попробовать обновить снова.
        if (menu.get_openedItem() !== null) {
            setTimeout(UpdateNotificationControl, 2000);
            return;
        }
          
        // Обновление меню в шапке тулбара
        var items1 = menu.get_items();
        menu.trackChanges();
        items1.getItem(0).get_items().clear();
        var countNotDeliveringNotification = 0;
        for (var i = 0; i < notifications.length; i++) {
           
                var item1 = new Telerik.Web.UI.RadMenuItem();
                var dateN = convertUnixtTimeToDate(notifications[i].dCreateDate);
                item1.set_text(dateN + '  ' + notifications[i].cMessage);
                item1.set_value(notifications[i].nNotification);

                var attributes = item1.get_attributes();
                attributes.setAttribute("cState", notifications[i].cState);
                attributes.setAttribute("cAppName", notifications[i].cAppName);

                items1.getItem(0).get_items().add(item1);

            //if (notifications[i].isDelivered === 0) {
            //    countNotDeliveringNotification += 1;
            //}
        }
        menu.commitChanges();

        //для отображения лейбы количества уведомлений(показываются все не прочитанные уведомления) 
        var countNotDeliveringNotification = 0;
        for (var i = 0; i < notifications.length; i++) {
            if (notifications[i].isDelivered === 0) {
                countNotDeliveringNotification += 1;
            } 
        }

        //временно пока
        countNotDeliveringNotification = notifications.length;

        // Обновление лейбы количества уведомлений
        var lblNotificationCount = $('.notificationCount');
        if (countNotDeliveringNotification > 0) {
            lblNotificationCount.show();
            lblNotificationCount.text(countNotDeliveringNotification);
        } else {
            lblNotificationCount.hide();
        };

        // Обновление листБокса уведомлений на стартпейдже
        if (window.name == "STARTPAGE") {
            var list = $find("ctl00_Body_pnlNotifications_radLstBox");
            var items = list.get_items();
            list.trackChanges();
            items.clear();
            for (var i = 0; i < notifications.length; i++) {
                var item = new Telerik.Web.UI.RadListBoxItem();
                var dateN = convertUnixtTimeToDate(notifications[i].dCreateDate);
                var attributes = item.get_attributes();

                if (notifications[i].isDelivered === true) {

                    item.set_text("<span style='font-weight:normal;'>" + (dateN + '  ' + notifications[i].cMessage));
                   

                } else {
                    
                    item.set_text("<span style='font-weight:bold;'>" + (dateN + '  ' + notifications[i].cMessage));
                    //item.get_element().style.backgroundColor = "red";
                    //item.set_text('<b>' + dateN + '  ' + notifications[i].cMessage + ':</b>&nbsp;');
                }
                
                item.set_value(notifications[i].nNotification);
               
                attributes.setAttribute("cState", notifications[i].cState);
                attributes.setAttribute("cAppName", notifications[i].cAppName);
  
               
                if (notifications[i].isImportant === true) {
                    item.set_checked(true);
                } else {
                    item.set_checked(false);
                }

             
                items.add(item);

            }

            list.commitChanges();
            changeCheckBoxStyle();

        }

       
    }
   
}

function OnClientItemClickNotification(sender, args) {

    //if (args.get_domEvent().target.className === 'rlbText') {
    if (args.get_domEvent().target.nodeName === 'SPAN') {
       var cState = args.get_item().get_attributes().getAttribute('cState');
       var cAppName = args.get_item().get_attributes().getAttribute('cAppName');
       var nk = args.get_item().get_value();

       //переход на модуль при клике на уведомлении
       sendEvent('LOAD_STATE', cState, cAppName);

       //устанавливаем уведомление просмотренным
       SetNotificationsAsReading(nk);
   }

}

function OnClientItemSelectedMenu(sender, args) {

    if (args.get_domEvent().target.className === 'rmText') {
        var cState = args.get_item().get_attributes().getAttribute('cState');
        var cAppName = args.get_item().get_attributes().getAttribute('cAppName');

        //переход на модуль при клике на уведомлении
        sendEvent('LOAD_STATE', cState, cAppName);
    }


}   



var bindingDivWidth = "16px";
var bindingDivHeight = "16px";
var bindingDivMargin = "1px";
var bindingDivBorder = "0px solid black";
var imageBindingDiv = "../../Images/head.png";

function changeCheckBoxStyle() {
    var listBoxIndex = 0;
    $("#upnlNotifications input[type='checkbox']").each(function () {
        $(this).css({ opacity: 0.0 });

        var bindingDivPosition = "absolute";
        var divLeft = $(this).position().left + "px";
        var divTop = $(this).position().top + "px";

        var divStyle = "width:" + bindingDivWidth
            + ";height:" + bindingDivHeight
            + ";border:" + bindingDivBorder
            + ";position:" + bindingDivPosition
            + ";margin:" + bindingDivMargin
            + ";left:" + divLeft
            + ";top:" + divTop
            + ";background:" + 'url(' + imageBindingDiv + ')';

        if (!this.checked) {
            divStyle += ";background-position:-4px -55px";//красный
           // divStyle += ";background-position:-31px -55px";//синий
        } else {
           // divStyle += ";background-position:-4px -55px";//красный
            divStyle += ";background-position:-31px -55px"; //синий
        }

        var div = "<div id='divCheckBox' onclick='changeCheckBox(this)' style='" + divStyle + "' listBoxIndex=" + listBoxIndex++ + "></div>";
       

        $(this).after(div);
    });
}


function convertUnixtTimeToDate(timestamp) {
    var d = new Date(timestamp * 1000),	// Convert the passed timestamp to milliseconds
          yyyy = d.getFullYear(),
          mm = ('0' + (d.getMonth() + 1)).slice(-2),	
          dd = ('0' + d.getDate()).slice(-2),			
          hh = d.getHours(),
          h = hh,
          min = ('0' + d.getMinutes()).slice(-2),		
          time;
   
    time = dd +'.' + mm +'.'+ yyyy +  ' ' + h + ':' + min ;

    return time;
}

function setDataUnixTime7Day() {
    var today = new Date().getTime() - (7 * 24 * 60 * 60 * 1000);
    return parseInt(today / 1000);

}


//Telerik.Web.UI.RadListBox.prototype._onFocus = function (e) {
//    if (this._activeItem) {
       
//        return;
//    }

//    var item = this.get_selectedItem();
//    if (item) {
//        this._activateItem(item, true);
//        return;
//    }

//    item = this._getFirstVisibleItem()
//    if (item) {
//        this._activateItem(item);
//        return;
//    }
//}


// 
//function NotificationItemClosed() {
//    SetNotificationsAsReading();
//}

//Установить уведомдения промотренными
function SetNotificationsAsReading(notificationKey) {
    //var notificationKey = GetYoungerNotificationKey();

    if (notificationKey != null) {
        var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQuerySetAsReading
            + "(" + notificationKey + ")"
            + "&inParams=&inType=2&ContentType=application/json&isCompress=0&u=0.41930";
        ajaxThis(url, ParseSetNotificationsAsReading, true);
        //ajaxThis(url, ParseChangeNotificationImportantStatus, false);

        //ParseSetNotificationsAsReading();
    } else {
        JLOG.info('NotificationEngine: не получен ключ уведомления. Устанавливать в "Прочитано" нечего.');
    }
}

// после обновления статуса в бд меняем цвет контрола
function ParseSetNotificationsAsReading() {
    var notificationJson = getCookieByName("jSonNotitications");
    if (notificationJson !== undefined && notificationJson !== null) {
        var notifications = JSON.parse(notificationJson);
        for (var i = 0; i < notifications.length; i++) {
            notifications[i].isdelivered = 1;
        }

        notificationJson = JSON.stringify(notifications);
        setCookie("jSonNotitications", notificationJson);

        UpdateNotificationControl();
    }

}

//Получить ключ верхнего уведомления (ключ самого нового уведомления)
function GetYoungerNotificationKey() {
    var youngerNotification = null;
    var notificationMenu = $find("rmNotification");
    var rootItem = notificationMenu.get_items().getItem(0);
    if (rootItem != undefined) {
        var notificationItems = rootItem.get_items();
        var notificationCount = notificationItems.get_count();
        if (notificationCount > 0) {
            youngerNotification = notificationItems.getItem(0).get_value();
        }
    }

    return youngerNotification;
}

///#source 1 1 /Scripts/Common/scriptCommon.js
var JLOG = new Log(Log.NONE, Log.popupLogger);
JLOG.info('Модуль запускается');

if (!Array.prototype.indexOf)
{
  Array.prototype.indexOf = function(elt /*, from*/)
  {
    var len = this.length >>> 0;

    var from = Number(arguments[1]) || 0;
    from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
    if (from < 0)
      from += len;

    for (; from < len; from++)
    {
      if (from in this &&
          this[from] === elt)
        return from;
    }
    return -1;
  };
}

function removeFromArray(arr) {
    var what, a = arguments, l = a.length, ax;
    while (l > 1 && arr.length) {
        what = a[--l];
        while ((ax = arr.indexOf(what)) !== -1) {
            arr.splice(ax, 1);
        }
    }
    return arr;
}

function GetBaseURL() {
    return "http://" + window.location.host + getAppPath();
}

function GetClassWindowName() {
    var classWindowName;
    var windowName = GetWindowName();
    var afterClassWindowName = windowName.indexOf("_");
    if (afterClassWindowName !== -1) {
        classWindowName = windowName.substring(0, afterClassWindowName);
    }
    if (classWindowName == undefined) {
        classWindowName = windowName;
    }
    return classWindowName;
}

function GetService() {
    return "/DataProvider/OraWCI.aspx";
}

//функция для обработки открытия справки
function ParseGetHelpUrl(req) {
    var response = req.responseText;
    var helpUrl = JSON.parse(response).HELPURL;
    //главная страница не находится в documents
    if (helpUrl !== "index.html") {
        helpUrl = "Documents/" + helpUrl;
    }

    var params = "menubar=no,location=yes,status=no";
    var win = window.open("/../User_Help/ru/" + helpUrl, "", params);
    win.focus();
}

//функция открытия справки
function GetHelpUrl() {
    var url = GetBaseURL() + GetService() + "?inSQL=gis_meta.help_api.gethelpurl('" + GetClassWindowName() + "')&inParams=&inType=2&nSeq=&isCompress=0&u=0.41930";

    ajaxThis(url, ParseGetHelpUrl, false);
}

function ParseWindowByEventKey(req) {
    JLOG.info('Получаем данные (название окна)');
    
    var response = req.responseText;
    JLOG.debug('Название окна, для события' + response);
    try {
        var windowName = JSON.parse(response).WindowName;
        var instance = JSON.parse(response).Instance;
        var window2Send = (instance == null || instance === "") ? windowName : instance;

        if (window2Send != null && window2Send !== "") {
            var isOpen = isWindowOpen(window2Send);
            if (isOpen === 0) {
                OpenNewWindow(windowName, instance);
            }
            else if (isOpen === 1) {
                if (window2Send !== _cWindowName) {
                    var lOpen = window.open("", window2Send);
                    lOpen.window.focus();
                }
            }
        }
    }
    catch (e) {
        JLOG.error('Во время вычитывания названия приложения, произошла ошибка: ' + e.ToString);
    }
}

function getUserInfo() {
    var userInfo = new Object();
    userInfo.FIO = document.getElementById('hdnUserFIO').value;
    userInfo.Key = document.getElementById('hdnUser').value;
    return userInfo;
}

function Send2Window(eventName, instance) {
    JLOG.info('Получить по ключу события, название окна');
    var url = GetBaseURL() + GetService() + "?inSQL=" + WindowQueryGetWindowName4Event + "('" + eventName + "','" + instance + "')"
                                          + "&inParams=&inType=2&nSeq=&isCompress=0&u=0.41930";
    ajaxThis(url, ParseWindowByEventKey, true);
}

function isEmpty(obj) {
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop))
            return false;
    }
    return true;
}

function ChangeFlashToDoubleFlash(srt) {
	return srt.replace(/\\/g, '\\\\');
}


function GetOpenWindowAPI() {
    var curWinStatusJson = getCookieByName(_WindowStatus);
    var curWinStatus = JSON.parse(curWinStatusJson);
    var windowNames = "";
    for (var i = 0; i < curWinStatus.WindowStatus.length; i++) {
        if (windowNames !== "") {
            windowNames = windowNames + ",";
        }
        var windowsNameObj = '{"Name":"' + curWinStatus.WindowStatus[i].WindowName + '", "Title":"'
                                  + curWinStatus.WindowStatus[i].WindowTitle + '"}';
        windowNames = windowNames + windowsNameObj;
    }

    var result = '{"WindowName": [' + windowNames + "]}";
    return result;
}


//Use this function to do all your ajax related stuff.  
function ajaxThis(url, callBackFunction, async, method, params) {
    //Set default parameters
    method = typeof method !== 'undefined' ? method : 'GET';
    params = typeof params !== 'undefined' ? params : null;

    var urlNew = url + "&randomnumber=" + Math.floor(Math.random() * 10000);  //prevent browser caching by sending always sending a unique url.  
    JLOG.debug('AJAX запрос по URL:' + urlNew);
    var req = newXMLHttpRequest();    // Obtain an XMLHttpRequest instance  
    var handlerFunction = getReadyStateHandler(req, callBackFunction);    // Set the handler function to receive callback notifications from the request object  
    req.onreadystatechange = handlerFunction;
    req.open(method, urlNew, async);  // Third parameter specifies request is asynchronous.  
     req.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    req.send(params);
}

// Returns a new XMLHttpRequest object, or false if this browser doesn't support it  
function newXMLHttpRequest() {
    // debugger;
    JLOG.debug('Начало создание объекта XMLHttpRequest');
    var xmlreq = false;
    if (window.XMLHttpRequest) {
        // Create XMLHttpRequest object in non-Microsoft browsers  
        xmlreq = new XMLHttpRequest();
        JLOG.debug('Объект XMLHttpRequest - создан');
    } else if (window.ActiveXObject) {
        // Create XMLHttpRequest via MS ActiveX  
        try {
            // Try to create XMLHttpRequest in later versions  
            // of Internet Explorer  
            xmlreq = new ActiveXObject("Msxml2.XMLHTTP");
            JLOG.debug('Объект XMLHttpRequest - создан');
        } catch (e1) {
            try {
                JLOG.error(e1.ToString);
                // Try version supported by older versions  
                // of Internet Explorer  
                xmlreq = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e2) {
                JLOG.fatal(e2.ToString);
            }
        }
    }
    return xmlreq;
}

/* Returns a function that waits for the specified XMLHttpRequest 
* to complete, then passes its XML response 
* to the given handler function. 
* req - The XMLHttpRequest whose state is changing 
* responseXmlHandler - Function to pass the XML response to */
function getReadyStateHandler(req, responseXmlHandler) {
    // Return an anonymous function that listens to the   
    // XMLHttpRequest instance
    return function () {
        // If the request's status is "complete"  
        if (req.readyState === 0) {
            JLOG.debug("ReadyState: request not initialized ");
        }
        else if (req.readyState === 1) {
            JLOG.debug("ReadyState: server connection established");
        }
        else if (req.readyState === 2) {
            JLOG.debug("ReadyState: request received ");
        }
        else if (req.readyState === 3) {
            JLOG.debug("ReadyState: processing request");
        }
        else if (req.readyState === 4) {
            // Check that a successful server response was received  
            JLOG.debug("ReadyState: request finished and response is ready");
            if (req.status === 200) {              
                if (IsAuthPage(req)) {
                    window.location.reload();
                }

                // Pass the XML payload of the response to the   
                // handler function  
                responseXmlHandler(req);
            } else {
                JLOG.fatal('HTTP error: ' + req.status);
                // An HTTP problem has occurred  
                //alert("HTTP error: "+req.status);  
                //  $('lblError').innerHTML += "HTTP error: " + req.status;
            }
        }
    }
}

function toggleDisabled(el, isDisabled) {
    try {
        el.disabled = isDisabled;
    }
    catch (e) {
    }
    if (el.childNodes && el.childNodes.length > 0) {
        for (var x = 0; x < el.childNodes.length; x++) {
            toggleDisabled(el.childNodes[x], isDisabled);
        }
    }
}

function IsAuthPage(req) {
    var val = req.responseText.search('<meta name="PageName" content="Authorization"');
    if (val !== -1) {
        return true;
    }
    else {
        return false;
    }
}


/*For TelerikMenu*/
var RadMenuItemIDs = new Array();

function AddMenuToArray(sender) {
    if (arrayContains(RadMenuItemIDs, sender.get_id()))
        return;
    var index = RadMenuItemIDs.length;
    RadMenuItemIDs[index] = sender.get_id();

}
function CloseAllOpenMenus(sender) {
    for (var i = 0; i < RadMenuItemIDs.length; i++) {
        if (RadMenuItemIDs[i] !== sender.get_id()) {
            var menu = $find(RadMenuItemIDs[i]);
            var openedItem = menu.get_openedItem();
            if (openedItem != null) {
                openedItem.close();
                menu.set_clicked(false);
            }
        }
    }
    DeselectFilterMenu();
}

function arrayContains(array, item) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] === item)
            return true;
    }
    return false;
}

function OnClientMouseOverHandler(sender, eventArgs) {
    if (eventArgs.get_item().get_parent() === sender) {
        sender.set_clicked(false);
    }
}

function OnClientItemClicking(sender, args) {
    var level = args.get_item().get_level();
    if (level === 0) {
        CloseAllOpenMenus(sender, args);
    }
}

function CloseRadMenu(sender, args) {
    var childrenCount = args.get_item().get_items().get_count();
    if (childrenCount === 0) {
        sender.close(true);
    }
}

///#source 1 1 /Scripts/Common/WindowEngine.js
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
///#source 1 1 /Scripts/Common/VideoPlayerEngine.js
function VideoPlayerMenuShow(videoName, videoFilePath) {
    var width = 550;
    var height = 300;
    $("#playerVideoName").val(videoName);

    $("#playerVideoFon").css('display', 'block');
    $("#playerMenu").css('width', width + 'px');
    $("#playerMenu").css('height', height + 'px');
    $("#playerMenu").css('margin-top', -height / 2 + 'px');
    $("#playerMenu").css('margin-left', -width / 2 + 'px');

    jwplayer("playerVideoSwf").setup({
        file: videoFilePath,
        flashplayer: "../../UserControls/VideoPlayer/player.swf",
        width: width,
        height: height
    });
}

function VideoPlayerMenuHidden() {
    jwplayer("playerVideoSwf").stop();
    $("#playerVideoFon").css('display', 'none');
}




//var listBox = $find("RadListBox1");
//var $ = $telerik.$;

//$(".rlbItem", listBox._get_element())
//    .die("click")
//    .live("click", function (e) {


//        var item = listBox._extractItemFromDomElement(e.target);

//    });


//$telerik.$(item.get_element()).click(function (event)
//$telerik.$(item.get_element()).click(function (event) {

//    event = event || window.event;
//    event = event.target || e.srcElement;


// //нужно выбрать элемент только текст (а не чекбокс)
//   if (event.tagName === 'SPAN') {
//       var t = $telerik.$(item.get_element().get_attributes().getAttribute("cState"));
//       alert(t);
//       // alert($telerik.$(item.get_attributes().getAttribute("cState")));
//       //alert($telerik.$(item.get_attributes()[0].getAttribute("cState")[0]));
//       // alert(e.item.get_attributes().getAttribute("cState"));
//       //alert(item.get_attributes().getAttribute("cState"));
//       //items.get_items().getItem()

//       //var result = [];
//       //var allItems = list._getAllItems();
//       //for (var i = 0; i < allItems.length; i++) {
//       //    if (allItems[i].get_attributes().getAttribute(attributeName) == value)
//       //        result.push(allItems[i]);
//       //}
//       //return result;
//       //alert($(this).attr('cState'));
//   }

//});

