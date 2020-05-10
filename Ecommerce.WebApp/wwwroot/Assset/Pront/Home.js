myHome = new Vue({
    el: "#myHome",
    components: {
        product: {
            props: ['d', 'imagepath','linkhref', 'productuser', 'name', 'detail', 'price', 'province', 'district', 'state'],
            template: '<div class="col-md-3 col-sm-6 col-xs-6 mb-4"><div class= "card h-100"><i class="far fa-heart"></i><a :href="linkhref"> <img v-if="imagepath!=null" style="width:255px; height:255px" class="card-img-top p-3" :src="imagepath" /></a><div class="card-body"><a class="card-title text-success" href="">{{ productuser }}</a><h6 class="card-title text-dark">{{ name }}</h6><p class="card-text">{{ detail }}</p><p class="card-text text-danger">{{ price }}</p></div><div class="row"><div class="col-md-6"><span class="card-text"><i class="fa fa-map-marker text-danger"></i> {{ province }}-{{ district }}</span></div><div class="col-md-6"><span class="card-text">{{ state }}</span></div></div></div></div>',
            filters: {
                showChatTime: function (createdat) {
                    var date = new Date(createdat);
                    date = ("0" + date.getDate()).slice(-2) + '/' + ("0" + date.getMonth()).slice(-2) + '/' + date.getFullYear() + ' ' +
                        ("0" + date.getHours()).slice(-2) + ':' + ("0" + date.getMinutes()).slice(-2);
                    return date;
                }
            }
        },
    },
    data: {
        loading: true,
        productModel: Object,
        product: [],
        isLoaded: false,
        size: 8,
        pageNumber:0,
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
        filterlocation: function (event) {
            self = this;
            var location = event.target.text;
            self.product = self.productModel.items;
            var products = self.product.filter(function (value) {
                return value.provinceName == location;
            });
            self.product = products;
            $('#seachModal').modal('hide');
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
            for (var i = 0; i < this.productModel.items.length; i++) {
                this.product.push(this.productModel.items[i]);
            }
            
        },
        load: function (userids,productid) {
            var self = this;
            self.userids = userids;
            self.productid = productid;
            //self.PrivateName = groupName;
            fetch(baseUrl + "home/" + this.pageNumber + "/" + this.size)
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