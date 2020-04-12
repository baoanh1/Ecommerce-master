new Vue({
    el: '#previewimage',
    template: `
        <div>
            <div class="file-upload-form">
                Upload an image file:
                <input multiple type="file" @change="previewImage" accept="image/*">
            </div>
            
            <div class="image-preview row col-md-8" v-if="imageData.length > 0">
                <div class="col-md-2" v-for="(img, key) in imageData">
                    <img class="preview" :src="img" style="width:80px">
                    <span class="fas fa-trash" v-show="image" v-on:click="removeImage(key)"style="display:unset !important" ></span>
                </div>
            </div>
        </div>
    `,
    data: {
        imageData: []  // we will store base64 format of image in this string
    },
    methods: {
        previewImage: function (event) {
            // Reference to the DOM input element
            var input = event.target;
            // Ensure that you have a file before attempting to read it
            if (input.files && input.files) {
                // create a new FileReader to read this image and convert to base64 format
                var reader = new FileReader();
                // Define a callback function to run, when FileReader finishes its job
                reader.onload = (e) => {
                    // Note: arrow function used here, so that "this.imageData" refers to the imageData of Vue component
                    // Read image as base64 and set to imageData
                    for (var i = 0; i < input.files.length; i++) {
                        this.imageData.push(e.target.result);
                    }
                    
                }
                // Start the reader job - read file as a data url (base64 format)
                var file = input.files;
                for (var i = 0; i < input.files.length; i++) {
                    reader.readAsDataURL(input.files[i]);
                }
                
            }
        }
    }
});
