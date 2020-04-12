/*global
    piranha
*/
notifications = new Vue({
    el: "#notification-hub",
    data: {
        items: [],
    },
    methods: {
        push: function (notification) {

            notification.style = {
                visible: false,
                'notification-info': notification.type === "info",
                'notification-danger': notification.type === "danger",
                'notification-success': notification.type === "success",
                'notification-warning': notification.type === "warning"
            };

            notifications.items.push(notification);

            setTimeout(function () {
                notification.style.visible = true;

                if (notification.hide) {
                    setTimeout(function () {
                        notification.style.visible = false;

                        setTimeout(function () {
                            notifications.items.shift();
                        }, 200);
                    }, 5000);
                }
            }, 200);
        }
    }
});