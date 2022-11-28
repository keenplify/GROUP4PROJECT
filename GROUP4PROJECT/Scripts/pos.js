let container;
let totalItemsElement;
let totalPriceElement;
let checkoutContainer;
let checkoutTotalItemsElement;
let checkoutReceivedAmountElement;
let checkoutChangeElement;
let checkoutCustomerNameElement;
let checkoutTypeElement;
let cartReceivedAmountElement;
let totalPrice = 0;
let html5QrcodeScanner;

function addToCart(id) {
    let cartItems = tryParseLocalStorageObject("cartItems");

    if (!cartItems) cartItems = [];

    const oldCart = cartItems.find(v => v.id == id);

    if (oldCart) oldCart.quantity++;
    else cartItems.push({
        id,
        quantity: 1
    })

    trySaveLocalStorageObject("cartItems", cartItems);
    updateCartView();
}

function init() {
    checkoutContainer = document.querySelector("#checkout-container");
    container = document.querySelector("#cart-container");
    totalItemsElement = document.querySelector("#cart-total-items");
    totalPriceElement = document.querySelector("#cart-total-price");

    checkoutTotalItemsElement = document.querySelector("#checkout-total-items");
    checkoutReceivedAmountElement = document.querySelector("#checkout-received-amount");
    checkoutChangeElement = document.querySelector("#checkout-change");
    cartReceivedAmountElement = document.querySelector("#cart-received-amount");
    checkoutCustomerNameElement = document.querySelector("#checkout-customer-name");
    checkoutTypeElement = document.querySelector("#checkout-type");

    updateCartView();
}

async function openScanner() {
    await $('#QRScannerModal').modal("show").promise();
    html5QrcodeScanner = new Html5QrcodeScanner("reader", { fps: 10, qrbox: { width: 250, height: 250 } });
    html5QrcodeScanner.render(onScanSuccess);
}

function handleDeleteModalButton(id) {
    trySaveLocalStorageObject("toDeleteItem", id);
}

function handleDelete() {
    const item = tryParseLocalStorageObject("toDeleteItem");

    let cartItems = tryParseLocalStorageObject("cartItems");

    if (!cartItems) cartItems = [];

    const cartIndex = cartItems.findIndex(v => v.id == item);

    if (cartIndex != -1) cartItems.splice(cartIndex, 1);

    trySaveLocalStorageObject("cartItems", cartItems);
    updateCartView();
}

async function updateCartView() {
    const cartItems = tryParseLocalStorageObject("cartItems");
    
    if (!cartItems) return;

    let html = "";
    let checkoutHtml = "";
    totalPrice = 0;
    let totalItems = 0;
    for (const cart of cartItems) {
        const product = await $.ajax(`/Products/Single?id=${cart.id}`, {
            cache: true
        }).promise();
        const price = (product.Price * cart.quantity)
        totalPrice += price;
        totalItems += cart.quantity;

        html += `
             <div class="list-group-item" style="position:relative;">
                <div class="d-flex w-100 justify-content-between align-items-start">
                    <span class="fw-bold">${product.Name}</span>
                    <span class="badge bg-transparent text-black">Qty: ${cart.quantity}</span>
                    <span>
                        <a data-bs-toggle="modal" data-bs-target="#DeleteModal" onclick="handleDeleteModalButton('${cart.id}')">
                            <i class="fa-solid fa-trash-can" style="color:red"></i>
                        </a>
                        </span>
                </div>
                ${cart.remarks ? `
                    <small class="fw-light">Remarks:</small><br />
                ` : ''}
                <div class="d-flex justify-content-between py-1">
                    ${cart.remarks ? `
                        <small class="fw-lighter">${cart.remarks}</small>
                    ` : ''}
                    <p class="fw-bold">₱${price}</p>
                </div>
            </div>
        `;

        checkoutHtml += `
            <tr>
                <th scope="row">${product.Name}</th>
                <td>${cart.quantity}</td>
                <td>₱${product.Price}</td>
                <td>₱${price}</td>
            </tr>
        `;
    }

    const receivedAmount = Number.parseInt(cartReceivedAmountElement.value);

    container.innerHTML = html;
    checkoutContainer.innerHTML = checkoutHtml;
    totalPriceElement.innerHTML = `₱${totalPrice}`;
    totalItemsElement.innerHTML = totalItems;
    checkoutTotalItemsElement.innerHTML = totalItems;
    checkoutReceivedAmountElement.innerHTML = `₱${receivedAmount}`;
    checkoutChangeElement.innerHTML = `₱${receivedAmount - totalPrice}`;
}

function handleCheckout() {
    const receivedAmount = Number.parseInt(cartReceivedAmountElement.value);

    if (totalPrice == 0 || receivedAmount == 0) return Toastify({
        text: "Invalid transaction",
    }).showToast();

    if (totalPrice > receivedAmount) return Toastify({
        text: "Unable to checkout, insufficient received amount",
    }).showToast();

    updateCartView();

    $('#CheckoutModal').modal("show");
}

async function handleFinalizeCheckout() {
    const name = checkoutCustomerNameElement?.value;

    let cartItems = tryParseLocalStorageObject("cartItems");

    const formData = {
        CustomerName: name,
        Status: 'Queued',
        OrderProducts: cartItems.map(cart => ({
            ProductId: cart.id,
            Quantity: cart.quantity,
            Remarks: cart.remarks,
        })),
        Type: checkoutTypeElement.value
    };

    await $.post("/Order", formData).promise();

    let cIs = [];

    for (const cI of cartItems) {
        const product = await $.ajax(`/Products/Single?id=${cI.id}`, {
            cache: true
        }).promise();
        const price = (product.Price * cI.quantity)

        cIs.push({ ...cI, ...product, price });
    }

    let receiptData = {
        name,
        type: checkoutTypeElement.value,
        cartItems: cIs
    }

    console.log(receiptData);

    localStorage.removeItem("cartItems");
    window.open("/POS/PrintableReceipt?data=" + encodeURIComponent(JSON.stringify(receiptData)), '_blank').focus();
    location.reload();
}

function onScanSuccess(decodedText) {
    const data = JSON.parse(decodedText);
    console.log({ data });

    if (data.items) {
        trySaveLocalStorageObject("cartItems", data.items);
    }

    if (data.customerName) {
        checkoutCustomerNameElement.value = data.customerName;
    }

    if (data.orderType) {
        checkoutTypeElement.value = data.orderType;
    }

    updateCartView();

    $('#QRScannerModal').modal("hide").promise();

    return Toastify({
        text: "Scan Successful",
    }).showToast();

}

document.onload = init();