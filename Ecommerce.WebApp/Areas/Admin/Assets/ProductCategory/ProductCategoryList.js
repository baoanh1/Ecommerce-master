productcategorylist = new Vue({
    el: "#productcategorylist",
    data: {
        loading: true,
        productcategories: [],
        size: 4,
        isActive: false,
        pageItem: true,
        
        pageNumber: 0,
        currentUserName: null
    },
    components: {
        
    },
   
    computed:{
        
    },
    
    methods: {
        nextPage() {
            this.pageNumber++;
            
            var self = this;
            fetch(baseUrl + "admin/productcategory/list/" + this.pageNumber + "/" + this.size+"/"+0)
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
            fetch(baseUrl + "admin/productcategory/list/" + i +"/" + this.size + "/" + 0)
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
            fetch(baseUrl + "admin/productcategory/list/" + this.pageNumber+ "/" + this.size + "/" + 0)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);

                    self.loading = false;
                })
                .catch(function (error) { console.log("error:", error); });
         
        },
        onChangePage(pageOfItems) {
            // update page of items
            this.productcategories = pageOfItems;
        },
        bind: function (result) {
            this.productcategories = result;
        },
        load: function () {
            var self = this;
            fetch(baseUrl + "admin/productcategory/list/" + this.pageNumber * this.size + "/" + this.size + "/" + this.pageNumber)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);

                    self.loading = false;
                })
                .catch(function (error) { console.log("error:", error); });
        },
        remove: function (productCategory) {
            var self = this;

            var productCategoryInfo = "";
            if (productCategory) {
                if (productCategory.name && productCategory.name.length > 0) {
                    productCategoryInfo += ' <br/>"' + productCategory.name + '"';
                }
            }
            else {
                return;
            }

            alert.open({
                title: "Delete user",
                body: "delete user confirm" + productCategoryInfo,
                confirmCss: "btn-danger",
                confirmIcon: "fa fa-trash",
                confirmText: "delete",
                onConfirm: function () {
                    fetch(baseUrl + "admin/productcategory/delete/" + productCategory.id)
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