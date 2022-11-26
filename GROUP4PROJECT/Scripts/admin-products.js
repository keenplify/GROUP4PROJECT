const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());

let CONTAINER;
let CATEGORY;
let CURRENT_CATEGORY;
let CURRENT_IMAGEURL;

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
        errors.responseJSON.errors.map((err) => Toastify({
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
    CURRENT_IMAGEURL = $("#old-imageUrl");

    console.log({ uploadedImageUrl, val: CURRENT_IMAGEURL.val() });

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

async function resetCategories() {
    const categories = await $.ajax("/Category").promise();
    CONTAINER.html("");

    for (const category of categories) {
        if (category.Id == CURRENT_CATEGORY.val()) {
            $("#category-btn").text(`Category - ${category.Name}`);
        }
        $(`
        <div style="display: flex; gap: .5em; margin-bottom: .2em; align-items: center; ">
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
            <button
                type="button"
                style="margin-left: auto" class="btn btn-danger btn-sm"
                onclick="handleCategoryDelete('${category.Id}')"
            >
                Delete
            </button>
        </div>
        `).appendTo(CONTAINER)
    }
}

$(document).ready(resetCategories)

function handleCategoryClick(name) {
    $("#category-btn").text(`Category - ${name}`);
}

function handleCategoryDelete(id) {
    const formData = {
        id,
    };

    $.post('/Category/Delete', formData, (data) => {
        resetCategories();
        Toastify({
            text: "Category successfully deleted."
        }).showToast();
    }).fail((err) => handleError(err))
}

// TODO - Add category form

function init() {
    CONTAINER = $("#categories-container");
    CATEGORY = $("#category-btn");
    CURRENT_CATEGORY = $("#old-product-category");
    CURRENT_IMAGEURL = $("#old-imageUrl");
}

document.onload = init()