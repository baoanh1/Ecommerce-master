/*global
    piranha userlist
*/
//Vue.use(CKEditor);
productcategorylist = new Vue({
    el: "#productcategorylist",
    
    data: {
        loading: true,
        isNew: false,
        isNewProduct:false,
        productModel: null,
        productCategories: [],
        htmlstring: '',
        selectedParentCateggory: 0,
        isData: false,
        currentProductName: null,
        Index: 0,
        IndexupdateImage: 0,
        imageData: [],
        htmlstring: ""
    },
    created: function () {
    },
    mounted: function () {
    },
    updated: function () {
        this.htmlstring = '';
        this.htmlstring += '<option value="0">Root</option>';

        this.cate_parent(this.productCategories, 0, "__", this.selectedParentCateggory);
        var htmlstring1 = this.htmlstring;
        var myselect = document.getElementById("myselect");
        $('#myselect').text(htmlstring1);

        $('#myselect').html($('#myselect').text());
        //this.bindOptionSelect(this.getNestedChildren(this.getProductRows(), 0), 0, this.selectedCateggory);
    },
    methods: {
        cate_parent: function (data, parent, str, select) {
            //var htmlstring = '';
            parent = (typeof parent !== 'undefined') ? parent : 0;
            str = (typeof str !== 'undefined') ? str : "__";
            select = (typeof select !== 'undefined') ? select : 0;
            
            for (var i in data) {
                var id = data[i].id;
                var name = data[i].name;

                if (data[i].parentID == parent) {
                    if (select != 0 && id == select) {
                        this.htmlstring += '<option value="' + data[i].id + '" selected="selected">' + str + data[i].name + '</option>';
                    }
                    else {
                        this.htmlstring += '<option value="' + data[i].id + '">' + str + data[i].name + '</option>';
                    }

                    this.cate_parent(data, id, str + "__", select);
                }

            }
            //return this.htmlstring;
        },
        loadimage: function () {
            var image = this.productModel.productCategory.image;
            //Create an input type dynamically.
            var contain = document.createElement("div");
            contain.setAttribute('id', 'divcontain');

            var element = document.createElement("input");

            ////Assign different attributes to the element.
            element.setAttribute("type", "text");
            //element.setAttribute("v-model", "productModel.productImage.imagePath");

            element.setAttribute("style", "border:1px solid #ccc;cursor:pointer;padding:4px;width:98%;");
            element.setAttribute("id", "txtSelectedFile");
            element.setAttribute("value", image);
            element.setAttribute(':readonly', 'true');
            element.setAttribute('class', 'divmodel');
            element.setAttribute("onclick", "openCustomRoxy2()");

            var icon = document.createElement("i");
            icon.setAttribute('class', 'remove-image fa fa-trash');
            icon.setAttribute("onclick", "removeaddimage()");
            icon.style = "padding-left:4px";
            var div = document.createElement('div');
            div.setAttribute('id', 'roxyCustomPanel2');

            div.setAttribute('style', 'display: none;');

            var iframe = document.createElement('iframe');
            iframe.src = '/lib/fileman/index.html?integration=custom&type=image&txtFieldId=txtSelectedFile';
            iframe.style = "width:100%;height:100%; border:1px solid #ccc";
            div.appendChild(iframe);
            var hr = document.createElement('hr');
            hr.style = "border:1px color:red";
            contain.append(element, icon, div, hr);
            var foo = document.getElementById("fooBar");

            foo.append(contain);


            var previewIamge = document.createElement("div");
            previewIamge.setAttribute('id', 'image-contain');
            previewIamge.setAttribute('class', 'col-md-2');

            var img = document.createElement("img");
            img.setAttribute('class', 'img-responsive');
            img.setAttribute('src', image);
            img.style = "height: 150px;width: 150px;padding-top:10px";
            var span = document.createElement("span");
            span.setAttribute('class', "delete fa fa-trash");
            span.setAttribute('onClick', "deletePreview(this)");
            span.textContent = "remove";
            previewIamge.append(img, span);
            var image_preview = document.getElementById("image_preview");
            image_preview.append(previewIamge);
        },
        add: function () {
            var i = this.Index;
            //Create an input type dynamically.
            var contain = document.createElement("div");
            contain.setAttribute('id', 'divcontain');

            var element = document.createElement("input");

            ////Assign different attributes to the element.
            element.setAttribute("type", "text");
            //element.setAttribute("v-model", "productModel.productImage.imagePath");

            element.setAttribute("style", "border:1px solid #ccc;cursor:pointer;padding:4px;width:98%;");
            element.setAttribute("id", "txtSelectedFile");
            element.setAttribute("value", "click and select one image");
            element.setAttribute('class', 'divmodel');
            element.setAttribute(':readonly', 'true');
            element.setAttribute("onclick", "openCustomRoxy2()");

            var icon = document.createElement("i");
            icon.setAttribute('class', 'remove-image fa fa-trash');
            icon.setAttribute("onclick", "removeaddimage()");
            icon.style = "padding-left:4px";
            var div = document.createElement('div');
            div.setAttribute('id', 'roxyCustomPanel2');

            div.setAttribute('style', 'display: none;');

            var iframe = document.createElement('iframe');
            iframe.src = '/lib/fileman/index.html?integration=custom&type=image&txtFieldId=txtSelectedFile';
            iframe.style = "width:100%;height:100%; border:1px solid #ccc";
            div.appendChild(iframe);
            var hr = document.createElement('hr');
            hr.style = "border:1px color:red";
            contain.append(element, icon, div, hr);
            var foo = document.getElementById("fooBar");

            foo.append(contain);

        },
        bind: function (result) {
            this.productModel = result;
            this.isNew = result.productCategory.id === "00000000-0000-0000-0000-000000000000";
            if (this.productModel.productCategory.image != null) {
                this.isData = true;
            }
           
        },
        load: function (id, isNew) {
            var self = this;
            self.isNewProduct = isNew;
            var url = isNew ? baseUrl + "admin/productcategory/add" : baseUrl + "admin/productcategory/edit/" + id;

            fetch(url)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);
                    self.loading = false;
                    self.productCategories = result.productCategorys;
                    self.selectedParentCateggory = result.productCategory.parentID;
                })
                .catch(function (error) { console.log("error:", error); });
        },
        getProductRows: function () {
            var productRows = Array();
            $.each(this.productModel.productCategorys, function (key, value) {
                productRows.push(value);
            });
            return productRows;
        },
        save: function () {
            // Validate form
            var form = document.getElementById("usereditForm");
            if (form.checkValidity() === false) {
                form.classList.add("was-validated");
                return;
            }
            var imagepath = null;
            if (document.getElementById("txtSelectedFile") != null) {
                imagepath = document.getElementById("txtSelectedFile").value;
            }
            
            var ok = false;
            var self = this;
            self.productModel.productCategory.image = imagepath;
            console.log(JSON.stringify(self.productModel));
            fetch(baseUrl + "admin/productcategory/save", {
                method: "post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(self.productModel)
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
        remove: function (productId) {
            var self = this;

            alert.open({
                title: "Delete product",
                body: "delete user confirm" + productId,
                confirmCss: "btn-danger",
                confirmIcon: "fa fa-trash",
                confirmText: "delete",
                onConfirm: function () {
                    fetch(baseUrl + "admin/productcategory/delete/" + productId)
                        .then(function (response) {
                            ok = response.ok;
                            return response.json();
                        })
                        .then(function (result) {
                            notifications.push(result);
                            if (ok) {
                                window.location = baseUrl + "admin/productcategories/?d=1";
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
