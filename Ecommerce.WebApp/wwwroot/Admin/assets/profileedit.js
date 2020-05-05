/*global
    piranha userlist
*/

profileedit = new Vue({
    el: "#profileedit",
    data: {
        loading: true,
        isNew: false,
        profileModel: null,
        currentUserName: null
    },
    methods: {
        bind: function (result) {
            this.profileModel = result;
            this.isNew = result.user.id === "00000000-0000-0000-0000-000000000000";
        },
        load: function () {
            var self = this;

            var url = baseUrl + "getprofile";

            fetch(url)
                .then(function (response)
                {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);
                    self.loading = false;
                })
                .catch(function (error) { console.log("error:", error); });
        },
       
       
        save: function () {
            // Validate form
            var form = document.getElementById("usereditForm");
            if (form.checkValidity() === false) {
                form.classList.add("was-validated");
                return;
            }

            var ok = false;
            var self = this;
            console.log(JSON.stringify(self.userModel));
            fetch(baseUrl + "profile/save", {
                method: "post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(self.profileModel)
            })
                .then(function (response) {
                    ok = response.ok;
                    return response.json();
                })
                .then(function (result) {
                    if (ok) {
                        self.bind(result);

                        notifications.push({
                            body: "luu thanh cong",
                            type: "success",
                            hide: true
                        });
                    }
                    else if (result.status) {
                        notifications.push(result.status);
                    }
                    else {
                        notifications.push({
                            body: "<strong>" + "co loi xay ra" + "</strong>",
                            type: "danger",
                            hide: true
                        });
                    }

                })
                .catch(function (error) {
                    notifications.push({
                        body: error,
                        type: "danger",
                        hide: true
                    });

                    console.log("error:", error);
                });
        },
        remove: function (userId) {
            var self = this;

            alert.open({
                title: "Delete product",
                body: "delete user confirm" + userId,
                confirmCss: "btn-danger",
                confirmIcon: "fa fa-trash",
                confirmText: "delete",
                onConfirm: function () {
                    fetch(baseUrl + "profile/delete/" + userId)
                        .then(function (response) {
                            ok = response.ok;
                            return response.json();
                        })
                        .then(function (result) {
                            notifications.push(result);
                            if (ok) {
                                window.location = baseUrl + "profile/?d=1";
                            }
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
