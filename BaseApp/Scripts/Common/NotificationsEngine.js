
// Проверка времени последнего получения уведомлений. Если время истекло обновить
function checkTimeNotification(isFirstLoad) {

    alert("checkTimeNotification(isFirstLoad)_time");
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

   
   // alert("CheckNewNotification()");

    var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQueryGetNotification
                                          + "('" + document.getElementById('hdnUser').value + "','0')"
                                          + "&inParams=&inType=2&ContentType=application/json&isCompress=0&u=0.41930";

   // alert("CheckNewNotification()");

    ajaxThis(url, ParseCheckNewNotification, false);
}

//запись полученных уведомлений в хранилище
function ParseCheckNewNotification(req) {
    var jSonNotitications = req.responseText;
    //обновление контролов происходит при событии в localStorage
    setCookie("jSonNotitications", jSonNotitications);
}


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

    ChangeNotificationImportantStatus(itemkey, isImportant);
}

//Установить статус импорта для уведомления
function ChangeNotificationImportantStatus(notificationKey, isImportant) {
    var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQuerySetImportantStatus
                                          + "(" + notificationKey + "," + isImportant + ")"
                                          + "&inParams=&inType=2&ContentType=application/json&isCompress=0&u=0.41930";
    ajaxThis(url, ParseChangeNotificationImportantStatus, false);
}

// после обновления статуса в бд меняем цвет контрола
function ParseChangeNotificationImportantStatus() {
    var bindedDiv = $(this).parent().children("div#divCheckBox");

    bindedDiv.css("background", 'url(' + imageBindingDiv + ')');

    if (!this.checked) {
        bindedDiv.css("background-position", '-4px -55px');
    } else {
        bindedDiv.css("background-position", '-31px -55px');
    }
}

// обновление контролов уведомления
function UpdateNotificationControl() {
    //закомитила!!! return
   // return;

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
            if (notifications[i].isdelivered === 0) {
                var item1 = new Telerik.Web.UI.RadMenuItem();
                item1.set_text(notifications[i].cmessage);
                item1.set_value(notifications[i].nnotification);
                items1.getItem(0).get_items().add(item1);

                countNotDeliveringNotification += 1;
            }
        }
        menu.commitChanges();

        // Обновление лейбы количества уведомлений
        var lblNotificationCount = $('.notificationCount');
        if (countNotDeliveringNotification > 0) {
            lblNotificationCount.show();
            lblNotificationCount.text(countNotDeliveringNotification);
        } else {
            lblNotificationCount.hide();
        }

        // Обновление листБокса уведомлений на стартпейдже
        if (window.name == "STARTPAGE") {
            var list = $find("ctl00_Body_pnlNotifications_radLstBox");
            var items = list.get_items();
            list.trackChanges();
            items.clear();
            for (var i = 0; i < notifications.length; i++) {
                var item = new Telerik.Web.UI.RadListBoxItem();
                item.set_text(notifications[i].cmessage);
                item.set_value(notifications[i].nnotification);
                item.set_checked(true);
                items.add(item);
            }
            list.commitChanges();

            changeCheckBoxStyle();
        }
    }
}

// 
function NotificationItemClosed() {
    SetNotificationsAsReading();
}

//Установить уведомдения промотренными
function SetNotificationsAsReading() {
    var notificationKey = GetYoungerNotificationKey();

    if (notificationKey != null) {
        //var url = GetBaseURL() + GetService() + "?inSQL=" + notificationQuerySetAsReading
        //    + "(" + notificationKey + ")"
        //    + "&inParams=&inType=2&ContentType=application/json&isCompress=0&u=0.41930";
        //ajaxThis(url, ParseChangeNotificationImportantStatus, false);

        ParseSetNotificationsAsReading();
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
