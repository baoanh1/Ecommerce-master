Vue.component('autocomplete', {
    template: `
  	<div class="auto-complete">
   		 <input class="form-control" type="text" v-model="input" placeholder="Tìm kiếm..." @keydown.tab.prevent="complete()" @focus="focus(true)" @blur="focus(false)" v-on:click.prevent="searchform($event)">
         <i class="iconsearch fa fa-search"></i>
   		 <table v-if="focused" class="table-hover result">
    	    <tbody>
            	<tr class="" v-for="(person, i) in data" v-if="filter(person)" @mousedown="complete(i)">
          	      <td class="itemhover"><img :src="person.images[0].imagePath" class="mr-4 p-1" style="width:30px;height:30px;"/>{{ person[field] }}
                  </td>
                    
        	    </tr>

      	  </tbody>
    	 </table>
		</div>
  `,

    props: {
        value: { type: String, required: false },
        data: { type: Array, required: true },
        field: { type: String, required: true }
    },

    methods: {
        complete(i) {
            if (i == undefined) {
                for (let row of this.data) {
                    if (this.filter(row)) {
                        this.select(row)
                        return
                    }
                }
            }


            this.select(this.data[i])
        },

        select(row) {
            this.input = row[this.field]
            this.productid = row.id
            window.location = window.location.origin + '/productdetail/' + this.productid
            this.selected = true
        },

        filter(row) {
            var r = row[this.field]
                .toLowerCase()
                .indexOf(this.input
                    .toLowerCase()) != -1
            return r;
        },

        focus(f) {
            this.focused = f
        },
        searchform(event) {

            fetch(baseUrl + 'searchform')
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.people = result.products;

                    self.loading = false;
                    self.isLoaded = true;
                    //self.bind(result);

                })
                .catch(function (error) { console.log("error:", error); });
            var inputLength = $('input').val();
            if (inputLength.length > 1) {

            }
        }
    },

    data() {
        return {
            input: '',
            focused: false
        }
    },
    mounted() {
        window.addEventListener('load', () => {
            
            });
       
    },
    created() {
        this.input = this.value || ''
    }
})

new Vue({
    el: "#searchform",

    data: {
        people: [],
        ships: [],
        planets: []
    },
    clickinput: function () {

    },
    created() {
        self = this;
        
    }
})