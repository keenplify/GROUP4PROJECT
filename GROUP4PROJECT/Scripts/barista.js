async function markAsDone(id) {
    const request = await $.post("/Order/UpdateStatus", {
        id,
        Status: "Done"
    }).promise();
    poll();
}

async function poll() {
    const orders = await $.get("/Order/?filterDone=true").promise();

    let container = document.querySelector("#card-container");

    let html = "";
    if (orders?.length == 0) {
        return container.innerHTML = `
            <h3 class="d-flex justify-content-center align-items-center font-bold" style="height: 75vh">
                No orders to show.
            </h3>`;
    }

    for (const order of orders) {
        const date = dayjs(order.CreatedAt);

        let productsHtml = "";
        for (const orderProduct of order.OrderProducts) {
            productsHtml = productsHtml + `
                <div class="row py-1">
                    <div class="col-9 p-0 m-0">
                        <h4>${orderProduct.Product.Name}</h4>
                        ${orderProduct.Remarks ? `
                            <p class="text-muted">Remarks:</p>
                            <p>${orderProduct.Remarks}</p>
                        `: ''}
                    </div>
                    <div class="col-3 px-0 pb-1 text-center"> Qty: ${orderProduct.Quantity} </div>
                </div>
            `;
        }
        html = html + `
        <div class="col-md-6 col-lg-3 my-2">
                <!--card-->
                <div class="card ">
                    <div class="card-body ">
                        <div class="card-title">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-12 px-0 text-center">
                                        <h5>
                                            ${order.Type == 'TakeOut' ? '<span class="badge text-bg-success">Take Out</span>' : '<span class="badge text-bg-warning">Dine In</span>'}
                                        </h5>
                                    </div>
                                </div>
                                ${order.CustomerName?.length > 0 ? (`
                                    <div class="row text-muted">
                                        <div class="col-4 px-0">Customer</div>
                                        <div class="col-8 px-0 pb-1 text-end">${order.CustomerName}</div>
                                    </div>
                                `) : ''}
                                <div class="row text-muted">
                                    <div class="col-8 px-0"><p>${date.format("MMMM DD YYYY")}</p> </div>
                                    <div class="col-4 px-0 pb-1 text-end">
                                        <p>
                                            ${date.format("hh:mm A")}
                                        </p>
                                    </div>
                                    <hr>
                                </div>
                            </div>
                        </div>
                        <div class="container-fluid overflow-auto" style="height: 250px;">
                            ${productsHtml}
                        </div>

                        <div class="text-end">
                            <button type="button" class="btn btn-success mt-3 py-1 w-100" onclick="markAsDone('${order.Id}')">Done</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    container.innerHTML = html;
}

function init() {
    poll();
    setInterval(poll, 5000); //Poll every 5 seconds;
}

document.onload = init();