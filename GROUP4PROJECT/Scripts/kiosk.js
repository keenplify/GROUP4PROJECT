function setup(type) {
    localStorage.setItem("orderType", type);
    localStorage.removeItem("kioskItems");
    localStorage.removeItem("kioskCustomerName");

    window.location.href = "/Kiosk/Menu";
}

function showProductModal(json) {
    const modal = document.querySelector("#product-modal");
    const product = JSON.parse(json);
    document.querySelector("#product-name").innerHTML = product.Name;
    document.querySelector("#product-price").innerHTML = `₱${product.Price}`;
    document.querySelector("#product-description").innerHTML = product.Description;
    document.querySelector("#product-image").src = product.ImageUrl;
    document.querySelector("#product-id").value = product.Id;

    new bootstrap.Modal(modal).show()
}

function addToCart() {
    const id = document.querySelector("#product-id")?.value;

    if (!id) {
        return Toastify({
            text: "Unable to add to cart",
        }).showToast();
    }

    let cartItems = tryParseLocalStorageObject("kioskItems");

    if (!cartItems) cartItems = [];

    const oldCart = cartItems.find(v => v.id == id);

    const remarks = document.querySelector("#remarks");
    console.log(remarks.value);

    if (oldCart) oldCart.quantity++;
    else cartItems.push({
        id,
        quantity: 1,
        remarks: remarks.value?.length > 0 ? remarks.value : undefined 
    })

    trySaveLocalStorageObject("kioskItems", cartItems);
    remarks.value = "";

    return Toastify({
        text: "Product added to cart.",
    }).showToast();
}