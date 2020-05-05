userlist = new Vue({
    el: "#productlist",
    data: {
        loading: true,
        products: [],
        productcategories: [],
        size: 3,
        isActive: false,
        pageItem: true,

        pageNumber: 0,
        currentUserName: null
    },
    methods: {
        nextPage() {
            this.pageNumber++;

            var self = this;
            fetch(baseUrl + "user/product/list/" + this.pageNumber + "/" + this.size+"/"+0)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);

                    self.loading = false;
                })
                .catch(function (error) { console.log("error:", error); });

        },
        selectPage(page) {
            var i = page - 1;
            this.pageNumber = i;
            this.isActive = true;
            var self = this;
            fetch(baseUrl + "user/product/list/" + i +"/" + this.size + "/" + 0)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);

                    self.loading = false;

                })
                .catch(function (error) { console.log("error:", error); });

        },
        prevPage() {
            this.pageNumber--;
            var self = this;
            fetch(baseUrl + "user/product/list/" + this.pageNumber+ "/" + this.size + "/" + 0)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);

                    self.loading = false;
                })
                .catch(function (error) { console.log("error:", error); });

        },
        bind: function (result) {
            this.products = result;
            this.productcategories = result.productcategories;
        },
        load: function () {
            var self = this;
            
            fetch(baseUrl + "user/product/list/" + this.pageNumber * this.size + "/" + this.size + "/" + this.pageNumber)
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
        remove: function (product) {
            var self = this;

            var productInfo = "";
            if (product) {
                if (product.name && product.name.length > 0) {
                    productInfo += ' <br/>"' + product.name + '"';
                }
            }
            else {
                return;
            }

            alert.open({
                title: "Delete user",
                body: "delete user confirm" + productInfo,
                confirmCss: "btn-danger",
                confirmIcon: "fa fa-trash",
                confirmText: "delete",
                onConfirm: function () {
                    fetch(baseUrl + "user/product/delete/" + product.id)
                        .then(function (response) {
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