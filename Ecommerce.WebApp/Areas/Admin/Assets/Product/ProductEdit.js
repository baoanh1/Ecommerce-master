/*global
    piranha userlist
*/
//Vue.use(CKEditor);
productedit = new Vue({
    el: "#productedit",
    
    data: {
        loading: true,
        isNew: false,
        isNewProduct: false,
        productModel: null,
        htmlstring:'',
        selectedCateggory: 0,
        productCategories: [],
        currentProductName: null,
        Index: 0,
        districts: Object,
        IndexupdateImage: 0,
        imageData: [],
        htmlstring: "",
        /* Your data and models here */
        myModel: '<p><span style="color: black;"></span></p>',

        /* Config can be declare here */
        myPlugins: [
            'advlist autolink lists link image charmap print preview anchor textcolor',
            'searchreplace visualblocks code fullscreen',
            'insertdatetime media table contextmenu paste code directionality template colorpicker textpattern'
        ],
        myToolbar1: 'undo redo | bold italic strikethrough | forecolor backcolor | template link | bullist numlist | ltr rtl | removeformat | image',
        myToolbar2: '',
        myOtherOptions: {
            height: 300,
            templates: [
                { title: 'Test template 1', content: 'Test 1' },
                { title: 'Test template 2', content: 'Test 2' }
            ],
            content_css: 'css/tinymce-content.css',
            //,width:600,
            //directionality: 'rtl',
            //theme: 'modern',
            //menubar: false
            //, etc...
            file_browser_callback: function RoxyFileBrowser(field_name, url, type, win) {
                var cmsURL = '/lib/fileman/index.html?integration=tinymce4';  // script URL - use an absolute path!
                if (cmsURL.indexOf("?") < 0) {
                    cmsURL = cmsURL + "?type=" + type;
                }
                else {
                    cmsURL = cmsURL + "&type=" + type;
                }
                cmsURL += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
                tinyMCE.activeEditor.windowManager.open({
                    file: cmsURL,
                    title: 'Roxy File Browser',
                    width: 850, // Your dimensions may differ - toy around with them!
                    height: 650,
                    resizable: "yes",
                    plugins: "media",
                    inline: "yes", // This parameter only has an effect if you use the inlinepopups plugin!
                    close_previous: "no"
                }, {
                    window: win,
                    input: field_name
                });
                return false;
            },
            file_browser_callback_types: 'file image media'
        },
        
        
    },
   
    components: {
        'tinymce': VueEasyTinyMCE
    },
    updated: function () {
        
        productedit.loadcategory();
        
    },
    
    mounted() {
      
        window.addEventListener('load', () => {
           
        })
        window.addEventListener('load', () => {
            if(!this.isNewProduct) {
                this.loadDistrict(this.productModel.product.provinceID);
            }

        })
    },

    methods: {
        loadcategory: function () {
            this.htmlstring = '';
            this.htmlstring += '<option value="0">Root</option>';
            this.cate_parent(this.productCategories, 0, "__", this.selectedCateggory);
            var htmlstring1 = this.htmlstring;
            var myselect = document.getElementById("myselect");
            $('#myselect').text(htmlstring1);

            $('#myselect').html($('#myselect').text());
        },
        loadDistrict: function (id) {
            var district = this.productModel.districts.filter(function (index) {
                return index.provinceID === id;
            });
            this.districts = district;
        },
        onChange(provinceid) {
            var self = this;
            self.districts = self.productModel.districts.filter(function (index) {
                return index.provinceID === parseInt(provinceid);
            });
        },
        getNestedChildren: function (arr, parent) {
            var out = [];
            for (var i in arr) {
                if (arr[i].parentID == parent) {
                    var children = this.getNestedChildren(arr, arr[i].id);

                    if (children.length) {
                        arr[i].children = children;
                    }
                    out.push(arr[i]);
                }
            }
            return out;
        },
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
        bindOptionSelect: function (arr, parent, select) {
            var htmlstring = '';
            var myselect = document.getElementById("myselect");
            htmlstring += '<option value="0">Select category</option>';
            for (var i in arr) {
                if (arr[i].children != null) {
                    if (select != 0 && arr[i].id == select) {
                        htmlstring += '<option value="' + arr[i].id + '" selected="selected">' + "__" + arr[i].name + '</option>';
                    }
                    else {
                         htmlstring += '<option value="' + arr[i].id + '">' + "__" + arr[i].name + '</option>';
                    }
                   
                    for (var item in arr[i].children) {
                        if (select != 0 && arr[i].children[item].id == select) {
                            htmlstring += '<option value="' + arr[i].children[item].id + '" selected="selected">' + "____" + arr[i].children[item].name + '</option>';
                        }
                        else {
                            htmlstring += '<option value="' + arr[i].children[item].id + '">' + "____" + arr[i].children[item].name + '</option>';
                        }
                        
                    }
                }
                else {
                    if (select != 0 && arr[i].id == select) {
                        htmlstring += '<option value="' + arr[i].id + '" selected="selected">' + "__" + arr[i].name + '</option>';
                    }
                    else {
                        htmlstring += '<option value="' + arr[i].id + ' ">' + "__" + arr[i].name + '</option>';
                    }
                    
                }
            }

            $('#myselect').text(htmlstring);

            $('#myselect').html($('#myselect').text());
        },
        
        loadimage: function () {
            var indx = this.IndexupdateImage;
            if (indx == 1) {
                $.each(this.productModel.productImages, function (i, value) {
                    var divid = document.getElementById("divcontain" + i);
                    divid.remove();
                    IndexupdateImage = 0;
                });
                $.each(this.productModel.productImages, function (i, value) {

                    //Create an input type dynamically.
                    var contain = document.createElement("div");
                    contain.setAttribute('id', 'divcontain' + i);

                    var element = document.createElement("input");

                    ////Assign different attributes to the element.
                    element.setAttribute("type", "text");
                    //element.setAttribute("v-model", "productModel.productImage.imagePath");

                    element.setAttribute("style", "border:1px solid #ccc;cursor:pointer;padding:4px;width:98%;");
                    element.setAttribute("id", "txtSelectedFile_" + i);
                    element.setAttribute("value", value.imagePath);
                    element.setAttribute('class', 'divmodel');
                    element.setAttribute(':readonly', 'true');
                    element.setAttribute("onclick", "openCustomRoxy2(" + i + ")");

                    var icon = document.createElement("i");
                    icon.setAttribute('class', 'remove-image fa fa-trash');
                    icon.setAttribute("onclick", "removeaddimage(" + i + ")");
                    icon.style = "padding-left:4px";
                    var div = document.createElement('div');
                    div.setAttribute('id', 'roxyCustomPanel2' + i);

                    div.setAttribute('style', 'display: none;');

                    var iframe = document.createElement('iframe');
                    iframe.src = '/lib/fileman/index.html?integration=custom&type=image&txtFieldId=txtSelectedFile_' + i + '';
                    iframe.style = "width:100%;height:100%; border:1px solid #ccc";
                    div.appendChild(iframe);
                    var hr = document.createElement('hr');
                    hr.style = "border:1px color:red";
                    contain.append(element, icon, div, hr);
                    var foo = document.getElementById("fooBar");

                    foo.append(contain);

                });
            }
            else {
                $.each(this.productModel.productImages, function (i, value) {

                    //Create an input type dynamically.
                    var contain = document.createElement("div");
                    contain.setAttribute('id', 'divcontain' + i);

                    var element = document.createElement("input");

                    ////Assign different attributes to the element.
                    element.setAttribute("type", "text");
                    //element.setAttribute("v-model", "productModel.productImage.imagePath");

                    element.setAttribute("style", "border:1px solid #ccc;cursor:pointer;padding:4px;width:98%;");
                    element.setAttribute("id", "txtSelectedFile_" + i);
                    element.setAttribute("value", value.imagePath);
                    element.setAttribute(':readonly', 'true');
                    element.setAttribute('class', 'divmodel');
                    element.setAttribute("onclick", "openCustomRoxy2(" + i + ")");

                    var icon = document.createElement("i");
                    icon.setAttribute('class', 'remove-image fa fa-trash');
                    icon.setAttribute("onclick", "removeaddimage(" + i + ")");
                    icon.style = "padding-left:4px";
                    var div = document.createElement('div');
                    div.setAttribute('id', 'roxyCustomPanel2' + i);

                    div.setAttribute('style', 'display: none;');

                    var iframe = document.createElement('iframe');
                    iframe.src = '/lib/fileman/index.html?integration=custom&type=image&txtFieldId=txtSelectedFile_' + i + '';
                    iframe.style = "width:100%;height:100%; border:1px solid #ccc";
                    div.appendChild(iframe);
                    var hr = document.createElement('hr');
                    hr.style = "border:1px color:red";
                    contain.append(element, icon, div, hr);
                    var foo = document.getElementById("fooBar");

                    foo.append(contain);

                });
                this.IndexupdateImage++;
            }
           
        },
        add: function (length) {
            var i = this.Index + length;
            //Create an input type dynamically.
            var contain = document.createElement("div");
            contain.setAttribute('id', 'divcontain' + i);

            var element = document.createElement("input");

            ////Assign different attributes to the element.
            element.setAttribute("type", "text");
            //element.setAttribute("v-model", "productModel.productImage.imagePath");

            element.setAttribute("style", "border:1px solid #ccc;cursor:pointer;padding:4px;width:98%;");
            element.setAttribute("id", "txtSelectedFile_" + i);
            element.setAttribute("value", "click and select one image");
            element.setAttribute('class', 'divmodel');
            element.setAttribute(':readonly', 'true');
            element.setAttribute("onclick", "openCustomRoxy2(" + i + ")");

            var icon = document.createElement("i");
            icon.setAttribute('class', 'remove-image fa fa-trash');
            icon.setAttribute("onclick", "removeaddimage(" + i + ")");
            icon.style = "padding-left:4px";
            var div = document.createElement('div');
            div.setAttribute('id', 'roxyCustomPanel2' + i);

            div.setAttribute('style', 'display: none;');

            var iframe = document.createElement('iframe');
            iframe.src = '/lib/fileman/index.html?integration=custom&type=image&txtFieldId=txtSelectedFile_' + i + '';
            iframe.style = "width:100%;height:100%; border:1px solid #ccc";
            div.appendChild(iframe);
            var hr = document.createElement('hr');
            hr.style = "border:1px color:red";
            contain.append(element, icon, div, hr);
            var foo = document.getElementById("fooBar");

            foo.append(contain);
            this.Index++;
            
        },
        
        bind: function (result) {
            this.productModel = result;
            this.districts = result.districts;
            this.isNew = result.product.id === "00000000-0000-0000-0000-000000000000";
            
        },
        load: function (id, isNew) {
            var self = this;
            self.isNewProduct = isNew;
            var url = isNew ? baseUrl + "admin/product/add" : baseUrl + "admin/product/edit/" + id;

            fetch(url)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);
                    self.loading = false;
                    self.productCategories = result.productCategorys;
                    self.selectedCateggory = result.product.categoryID;
                    onChange(result.product.categoryID);
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
            var cateid = document.getElementById("myselect").value;
            if (parseInt(cateid) == 0) {
                notifications.push({
                    body: "<strong>" + "select category different root" + "</strong>",
                    type: "danger",
                    hide: true
                });
            } else {
                // Validate form
                var form = document.getElementById("usereditForm");
                if (form.checkValidity() === false) {
                    form.classList.add("was-validated");
                    return;
                }

            var currentUserID = document.getElementById("CurrentUserID").value;
                var inputs = $(".divmodel");
                for (var i = 0; i < inputs.length; i++) {
                    var item = $(inputs[i]).val();
                    this.productModel.productImagePath.push(item);
                }

                var ok = false;
                var self = this;
            self.productModel.product.userID = currentUserID;
                console.log(JSON.stringify(self.productModel));
                fetch(baseUrl + "admin/product/save", {
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
            }
            
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
                    fetch(baseUrl + "admin/product/delete/" + productId)
                        .then(function (response) {
                            ok = response.ok;
                            return response.json();
                        })
                        .then(function (result) {
                            notifications.push(result);
                            if (ok) {
                                window.location = baseUrl + "admin/products/?d=1";
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
