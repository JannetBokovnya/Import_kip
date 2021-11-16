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