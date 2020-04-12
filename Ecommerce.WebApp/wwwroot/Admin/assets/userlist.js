userlist = new Vue({
    el: "#userlist",
    data: {
        loading: true,
        users: [],
        roles: [],
        currentUserName: null
    },
    methods: {
        bind: function (result) {
            this.users = result.users;
            this.roles = result.roles;
        },
        load: function () {
            var self = this;
            fetch(baseUrl + "manager/users/list")
                .then(function (response) { return response.json(); })
                .then(function (result) {
                    self.bind(result);

                    self.loading = false;
                })
                .catch(function (error) { console.log("error:", error); });
        },
        remove: function (user) {
            var self = this;

            var userInfo = "";
            if (user) {
                if (user.userName && user.userName.length > 0) {
                    userInfo += ' <br/>"' + user.userName + '"';
                }
            }
            else {
                return;
            }

            alert.open({
                title: "Delete user",
                body: "delete user confirm" + userInfo,
                confirmCss: "btn-danger",
                confirmIcon: "fa fa-trash",
                confirmText: "delete",
                onConfirm: function () {
                    fetch(baseUrl + "manager/user/delete/" + user.id)
                        .then(function (response)
                        {
                            ok = response.ok;
                            return response.json();
                        })
                        .then(function (result) {
                            notifications.push(result);

                            self.load();
                        })
                        .catch(function (error) {
                            console.log("error:", error);

                            notifications.push({
                                body: error,
                                type: "danger",
                                hide: true
                            });
                        });
                }
            });
        }
    }

});