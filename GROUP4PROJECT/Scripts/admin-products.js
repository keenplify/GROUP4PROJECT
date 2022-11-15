const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());

const CONTAINER = $("#categories-container");
const CATEGORY = $("#category-btn");
const CURRENT_CATEGORY = $("#old-product-category");
const CURRENT_IMAGEURL = $("#old-imageUrl");

async function uploadFile(selector) {
    const imgFd = new FormData();
    const file = $(selector).prop('files');

    if (!file[0]) return;
    console.log(file);
    imgFd.append("file", file[0])
    const resp = await $.ajax({
        url: "/File/Upload",
        type: "POST",
        contentType: false,
        processData: false,
        data: imgFd,
        enctype: 'multipart/form-data',
    }).promise();
    return resp.url;
}

function handleError(errors) {
    if (errors.responseJSON?.errors) {
        err.responseJSON.errors.map((err) => Toastify({
            text: err.ErrorMessage,
            className: "alert alert-danger",
            style: {
                background: "linear-gradient(to right, #c31432, #240b36)",
            }
        }).showToast())
    }
}

async function handleEditProduct(event) {
    event.preventDefault();

    const uploadedImageUrl = await uploadFile("#product-image");

    const formData = {
        id: params["id"],
        imageUrl: uploadedImageUrl ? uploadedImageUrl : CURRENT_IMAGEURL.val(),
        CategoryId: $('[name="product-category"]:checked').val(),
        name: $("#product-name").val(),
        price: $("#product-price").val(),
        description: $("#product-description").val(),
    };

    $.post('/Products/Update', formData, (data) => {
        location.reload();
    }).fail((err) => handleError(err))
}

async function handleAddProduct(event) {
    event.preventDefault();

    const uploadedImageUrl = await uploadFile("#product-image");

    const formData = {
        imageUrl: uploadedImageUrl,
        CategoryId: $('[name="product-category"]:checked').val(),
        name: $("#product-name").val(),
        price: $("#product-price").val(),
        description: $("#product-description").val(),
    };

    $.post('/Products/', formData, (data) => {
        location.reload();
    }).fail((err) => handleError(err))
}


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