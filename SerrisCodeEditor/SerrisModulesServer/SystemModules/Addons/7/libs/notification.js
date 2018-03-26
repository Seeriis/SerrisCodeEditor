function createWindowsNotification(text)
{
    var ToastNotifier = Windows.UI.Notifications.ToastNotificationManager.createToastNotifier();
    var toastXml = Windows.UI.Notifications.ToastNotificationManager.getTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
    var toastNodeList = toastXml.getElementsByTagName("text");
    toastNodeList.item(0).appendChild(toastXml.createTextNode(text));
    //toastNodeList.item(1).appendChild(toastXml.createTextNode(text));
    var toastNode = toastXml.selectSingleNode("/toast");
    var audio = toastXml.createElement("audio");
    audio.setAttribute("src", "ms-winsoundevent:Notification.SMS");

    var toast = new Windows.UI.Notifications.ToastNotification(toastXml);
    ToastNotifier.show(toast);
}