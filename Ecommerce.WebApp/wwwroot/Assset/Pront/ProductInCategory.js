productincategory = new Vue({
    el: "#productincategory",
    components: {
        product: {
            props: ['d', 'imagepath','linkhref', 'productuser', 'name', 'detail', 'price', 'province', 'district', 'state'],
            template: '<div class="col-lg-3 col-md-6 mb-4"><div class= "card h-100"><i class="far fa-heart"></i><a :href="linkhref"> <img v-if="imagepath!=null" style="width:255px; height:200px" class="card-img-top p-3" :src="imagepath" /></a><div class="card-body"><a class="card-title text-success" href="">{{ productuser }}</a><h6 class="card-title text-dark">{{ name }}</h6><p class="card-text">{{ detail }}</p><p class="card-text text-danger">{{ price }}</p></div><div class="row"><div class="col-md-6"><span class="card-text"><i class="fa fa-map-marker text-danger"></i> {{ province }}-{{ district }}</span></div><div class="col-md-6"><span class="card-text">{{ state }}</span></div></div></div></div>',
            filters: {
                showChatTime: function (createdat) {
                    var date = new Date(createdat);
                    date = ("0" + date.getDate()).slice(-2) + '/' + ("0" + date.getMonth()).slice(-2) + '/' + date.getFullYear() + ' ' +
                        ("0" + date.getHours()).slice(-2) + ':' + ("0" + date.getMinutes()).slice(-2);
                    return date;
                }
            }
        },
        collapenav: {
            template: '<div><!-- Single button triggers two "<b-collapse>" components --><b-button v-b-toggle.collapse-a.collapse-b>Toggle Both Collapse A and B</b-button><!-- Elements to collapse --><b-collapse id="collapse-a" class="mt-2"><b-card>I am collapsible content A!</b-card></b-collapse><b-collapse id="collapse-b" class="mt-2"><b-card>I am collapsible content B!</b-card></b-collapse></div>'
        }
    },
    data: {
        loading: true,
        productModel: Object,
        product: [],
        isLoaded: false,
        size: 8,
        pageNumber: 0,
        visible: true,
        minvalue: 0,
        maxvalue: 0,
        showSection: true,
        collapseopen: false,
        currentUserName: null
    },
    created: function () {
      
    },
    mounted: function () {
        
        window.addEventListener('load', () => {

        })
    },
    updated: function () {
       
    },
    methods: {
        priceslide: function () {
            window.addEventListener('load', () => {

            })
            
            var sliderLeft = document.getElementById("slider0to50");
            var sliderRight = document.getElementById("slider51to100");
            var inputMin = document.getElementById("min");
            var inputMax = document.getElementById("max");
            var inputMin1 = document.getElementById("min1");
            var inputMax1 = document.getElementById("max1");

            sliderRight.value = sliderLeft.value + 1;
            ///value updation from input to slider
            //function input update to slider
            function sliderLeftInput() {//input udate slider left
                
                sliderLeft.value = inputMin.value;
                sliderLeft.value = inputMin1.text;
            }
            function sliderRightInput() {//input update slider right
                sliderRight.value = (inputMax.value);//chnage in input max updated in slider right
                sliderRight.value = (inputMax1.text);
            }

            //calling function on change of inputs to update in slider
            inputMin.addEventListener("change", sliderLeftInput);
            inputMax.addEventListener("change", sliderRightInput);


            ///value updation from slider to input
            //functions to update from slider to inputs 
            function inputMinSliderLeft() {//slider update inputs
                inputMin.value = sliderLeft.value;
                this.minvalue = sliderLeft.value;
                if (sliderLeft.value == 0) {
                    inputMin1.innerHTML = "Từ: " +"<span class='price text-dark font-weight-bold'>" + sliderLeft.value + " Đồng </span>";
                } else {
                    inputMin1.innerHTML = "Từ: " + "<span class='price text-dark font-weight-bold'>" + sliderLeft.value + " Triệu </span>";
                }
                
            }
            function inputMaxSliderRight() {//slider update inputs
                inputMax.value = sliderRight.value;
                this.maxvalue = sliderRight.value;
                sliderRight.min = (parseInt(sliderLeft.value) + 1);
                sliderRight.max = (parseInt(sliderLeft.max) +1);
                if (sliderRight.value == 100) {
                    inputMax1.innerHTML = "Đến: " + " Không giới hạn";
                } else {
                    inputMax1.innerHTML = "Đến: " + "<span class='price text-dark font-weight-bold'>" + sliderRight.value + " Triệu </span>";
                }
                
                
            }
            sliderLeft.addEventListener("change", inputMinSliderLeft);
            sliderRight.addEventListener("change", inputMaxSliderRight);

        },
        getStringBeforeSubstring: function (parentString, substring) {
            return parentString.substring(0, parentString.indexOf(substring));
        },
        filtersubmit: function () {
            self = this;
            var inputMin = document.getElementById("min");
            var inputMax = document.getElementById("max");
            //self.product = self.productModel.products;
            var products = self.product.filter(function (value) {
                var temp = self.getStringBeforeSubstring(value.price, ",");
                var replace = parseInt(temp.replace(/[^0-9]+/g, ""),10);
                return replace > parseInt(inputMin.value,10) * 1000000;
            });
            self.product = products;
            $('#myModalfilter').modal('hide');
        },
        filterlocation: function (event) {
            self = this;
            var location = event.target.text;
            self.product = self.productModel.products;
            var products = self.product.filter(function (value) {
                return value.provinceName == location;
            });
            self.product = products;
            $('#filtermodel').modal('hide');
        },
        filterfirstcondition: function (event) {
            self = this;
            var filtervalue = event.target.value;
            if (filtervalue == "default") {
                //self.product = self.productModel.products;
                
            }
            else if (filtervalue == "newtoold") {
                self.product = self.product.sort(function (a, b) {
                    var nameA = a.createdDate, nameB = b.createdDate
                    if (nameA < nameB)
                        return 1
                    if (nameA > nameB)
                        return -1
                    return 0 //default return value (no sorting)
                });
            }else if (filtervalue == "oldtonew") {
                self.product = self.product.sort(function (a, b) {
                    var nameA = a.createdDate, nameB = b.createdDate
                    if (nameA < nameB)
                        return -1
                    if (nameA > nameB)
                        return 1
                    return 0 //default return value (no sorting)
                });
            } else if (filtervalue == "lowtohigh") {
                self.product = self.product.sort(function (a, b) {
                    var temp1 = self.getStringBeforeSubstring(a.price, ",");
                    var temp2 = self.getStringBeforeSubstring(b.price, ",");
                    var nameA = parseInt(temp1.replace(/[^0-9]+/g, ""), 10), nameB = parseInt(temp2.replace(/[^0-9]+/g, ""), 10)
                    if (nameA < nameB)
                        return -1
                    if (nameA > nameB)
                        return 1
                    return 0 //default return value (no sorting)
                });
            } else if (filtervalue == "hightolow") {
                self.product = self.product.sort(function (a, b) {
                    var temp1 = self.getStringBeforeSubstring(a.price, ",");
                    var temp2 = self.getStringBeforeSubstring(b.price, ",");
                    var nameA = parseInt(temp1.replace(/[^0-9]+/g, ""), 10), nameB = parseInt(temp2.replace(/[^0-9]+/g, ""), 10)
                    if (nameA < nameB)
                        return 1
                    if (nameA > nameB)
                        return -1
                    return 0 //default return value (no sorting)
                });
            }
            else {

            }
            
        },
       
        parentcatetoggle(parentid) {
            self = this;
            $('.collapse').on('shown.bs.collapse', function () {
               
                self.collapseopen = true;
                self.load(parentid);
            });

            $('.collapse').on('hidden.bs.collapse', function () {
                self.collapseopen = false;
            });
            //self.getproductbycateID(parentid);
        },
        toggle() {
            this.showSection = !this.showSection
        },
        getproductbycateID: function(cateid)
        {
            self = this;
            self.categoryid = cateid;
            //self.PrivateName = groupName;
            fetch(baseUrl + "getbycategoryid/" + self.categoryid)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    //if (self.collapseopen) {
                    //    for (var i = 0; i < result.productCategories.length; i++) {
                    //        if (result.productCategories[i].id == cateid) {
                    //            result.productCategories[i].show = "visible";
                    //        }
                    //        else {
                    //            result.productCategories[i].show = "invisible";
                    //        }
                    //    }
                    //}
                    //else {
                    //    for (var i = 0; i < result.productCategories.length; i++) {
                    //        result.productCategories[i].show = "visible";
                    //    }
                    //}
                
                    result.curentProductCategory = self.productModel.curentProductCategory;
                    result.productCategory = self.productModel.productCategory;
                    self.loading = false;
                    self.isLoaded = true;
                    self.bind(result);

                })
                .catch(function (error) { console.log("error:", error); });
        },
        LoadMoreItem: function () {
            self = this;
            self.pageNumber++;
            fetch(baseUrl + "home/" + self.pageNumber + "/" + self.size)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.productModel = result;
                    self.product = self.productModel;
                    for (var i = 0; i < self.productModel.items.length; i++) {
                        self.product.push(self.productModel.items[i]);
                    }
                    self.loading = false;
                    self.isLoaded = true;
                    //self.bind(result);

                })
                .catch(function (error) { console.log("error:", error); });
        },
        bind: function (result) {
            this.productModel = result;
            this.product = this.productModel.products;
            
        },
        load: function (categoryid) {
            var self = this;
            self.collapseopen = true;
            self.categoryid = categoryid;
            //self.PrivateName = groupName;
            fetch(baseUrl + "getbycategoryid/" + self.categoryid)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.loading = false;
                    self.isLoaded = true;
                    self.bind(result);
                   
                })
                .catch(function (error) { console.log("error:", error); });
        },
        
    }

});