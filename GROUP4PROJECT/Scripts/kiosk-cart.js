let container;
let totalItemsElement;
let totalPriceElement;

function init() {
    container = document.querySelector("#cart-container");
    totalItemsElement = document.querySelector("#cart-total-items");
    totalPriceElement = document.querySelector("#cart-total-price");

    updateCartView();
}

function removeFromCart(id) {
    let cartItems = tryParseLocalStorageObject("kioskItems");

    if (!cartItems) cartItems = [];

    const cartIndex = cartItems.findIndex(v => v.id == id);

    if (cartIndex != -1) cartItems.splice(cartIndex, 1);

    trySaveLocalStorageObject("kioskItems", cartItems);
    updateCartView();
}

async function updateCartView() {
    const kioskItems = tryParseLocalStorageObject("kioskItems");

    if (!kioskItems) return;

    let html = "";
    totalPrice = 0;
    let totalItems = 0;
    for (const cart of kioskItems) {
        const product = await $.ajax(`/Products/Single?id=${cart.id}`, {
            cache: true
        }).promise();
        const price = (product.Price * cart.quantity)
        totalPrice += price;
        totalItems += cart.quantity;

        html += `
            <tr>
                <td><img class="d-block mx-auto" src="${product.ImageUrl}" width="125"></td>
                <td>${product.Name}</td>
                <td>${cart.remarks ?? 'N/A'}</td>
                <td>x${cart.quantity}</td>
                <td>${product.Price}</td>
                <td>${price}</td>
                <td>
                    <button class="btn btn-ghost" onclick="removeFromCart('${product.Id}')">
                        <img
                            src="/Kiosk/icon/Kiosk_Cart-Remove.svg"
                            width="40"
                            ondragstart="return false;"
                        >
                    </button>
                </td>
            </tr>
        `;
    }

    container.innerHTML = html;
    totalPriceElement.innerHTML = `₱${totalPrice}`;
    totalItemsElement.innerHTML = totalItems;
}

async function generateTicket() {
    const customerName = document.querySelector("#customer-name");

    if (customerName.value && customerName.value.length > 0) {
        console.log('customerName', customerName);
        localStorage.setItem("kioskCustomerName", customerName.value)
    }

    window.location.href = "/Kiosk/Receipt"
}

document.onload = init();