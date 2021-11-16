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
