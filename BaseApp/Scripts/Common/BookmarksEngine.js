
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
