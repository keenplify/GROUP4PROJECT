async function handleAddProduct(event) {
    event.preventDefault();
    
    const imgFd = new FormData();
    const file = $("#product-image").prop('files');
    imgFd.append("file", file[0])
    const uploadedImage = await $.ajax({
        url: "/File/Upload",
        type: "POST",
        contentType: false,
        processData: false,
        data: imgFd,
        enctype: 'multipart/form-data',
    }).promise();

    const formData = {
        imageUrl: uploadedImage.url,
        CategoryId: $('[name="product-category"]:checked').val(),
        name: $("#product-name").val(),
        price: $("#product-price").val(),
        description: $("#product-description").val(),
    };

    $.post('/Products/', formData, (data) => {
        location.reload();
    }).fail((err) => {
        if (err.responseJSON?.errors) {
            err.responseJSON.errors.map((err) => {
                Toastify({
                    text: err.ErrorMessage,
                    className: "alert alert-danger",
                    style: {
                        background: "linear-gradient(to right, #c31432, #240b36)",
                    }
                }).showToast();
            })
        }
    })
}

const CONTAINER = $("#categories-container");
const CATEGORY = $("#category-btn");
const CURRENT_CATEGORY = $("#old-product-category");

$(document).ready(async () => {
    const categories = await $.ajax("/Category").promise();
    for (const category of categories) {
        if (category.Id == CURRENT_CATEGORY.val()) {
            $("#category-btn").text(`Category - ${category.Name}`);
        }
        $(`
        <div>
            <input
                type="radio"
                name="product-category"
                id="categories-${category.Id}" 
                value="${category.Id}" 
                required
                onclick="handleCategoryClick('${category.Name}')"
                ${CURRENT_CATEGORY.val() == category.Id ? "checked='true'" : ""}
            >
            <label for="categories-${category.Id}">${category.Name}</label>
        </div>
        `).appendTo(CONTAINER)
    }
})

function handleCategoryClick(name) {
    $("#category-btn").text(`Category - ${name}`);
}

// TODO - Add category form