const purchaseId        = gei("purchase-id");
const purchaseSelector  = gei("purchase-selector");
const purchaseSearchApi = purchaseSelector.getAttribute("data-path");
const purchaseBriefUrl  = "/Outgoing/Purchases/Details/Brief?id="; // Must concatenate Purchase Id
const lineItemId        = gei("lineitem-id");
const lineItemType      = gei("lineitem-type");
const lineItemSelector  = gei("lineitem-selector");
const prItemSearchApi   = "/api/PurchaseReturns/LineItems/"; // Must concatenate Purchase Id
const lineItemQuantity  = gei("lineitem-quantity");
const lineItemUnitPrice = gei("lineitem-unitprice");
const lineItemNetPrice  = gei("lineitem-netprice");
const lineItemAdd       = gei("lineitem-add-button");
const lineItemClear     = gei("lineitem-clear-button");
const lineItemAvailable = gei("lineitem-available");
const cart              = gei("cart");
const subtotal          = gei("subtotal");
const cashBack          = gei("cash-back");
const debtRollback      = gei("debt-rollback");
const debtBefore        = gei("debt-before");
const debtAfter         = gei("debt-after");
const description       = gei("Description");
const purchaseBriefElem = gei("purchase-briefing");
const currentDue        = gei("current-due");

var itemsAdded = 0;

function calculatePayment(event) {
    // Subtotal
    var subTotalValue = 0;
    const currentCartItemsNetPrices = gecn("cart-item-netprice");
    for (let i = 0; i < currentCartItemsNetPrices.length; i++)
        subTotalValue += currentCartItemsNetPrices[i].valueAsNumber;

    subtotal.value = toFixedIfNecessary(subTotalValue, 2);

    // Cash Back
    if (isNaN(cashBack.valueAsNumber))
        cashBack.value = 0;

    // Debt Rollback   
    if (isNaN(debtRollback.valueAsNumber))
        debtRollback.value = subTotalValue !== 0
            ? toFixedIfNecessary(
                Math.min(
                    debtBefore.valueAsNumber,
                    subTotalValue - cashBack.valueAsNumber), 2)
            : 0;

    // Previous Due
    if (isNaN(debtBefore.valueAsNumber))
        debtBefore.valueAsNumber = 0;

    // Debt After
    debtAfter.value = toFixedIfNecessary(
        debtBefore.valueAsNumber - debtRollback.valueAsNumber,
        2
    );
}

function calculateItemPrice(event) {
    if (isNaN(lineItemId.valueAsNumber)
        || lineItemId.valueAsNumber === 0) {
        clearLineItem(event);
        return;
    }

    const q = lineItemQuantity.valueAsNumber;
    if (isNaN(q) || q < 0)
        lineItemQuantity.value =
            lineItemQuantity.getAttribute("min");

    else if (q > Number(lineItemQuantity.getAttribute("max")))
        lineItemQuantity.value =
            lineItemQuantity.getAttribute("max");

    if (isNaN(lineItemUnitPrice.valueAsNumber))
        lineItemUnitPrice.value = minimumPrice;

    lineItemNetPrice.value =
        toFixedIfNecessary(lineItemQuantity.valueAsNumber
            * lineItemUnitPrice.valueAsNumber, 2);
}

function clearLineItem(event) {
    event.preventDefault();
    lineItemId.value = "";
    lineItemId.removeAttribute("min");
    lineItemType.value = "";
    lineItemSelector.value = "";
    lineItemQuantity.value = "";
    lineItemUnitPrice.value = "";
    lineItemNetPrice.value = "";
}

function getLineItem() {
    if (isNaN(lineItemId.valueAsNumber)
        || lineItemId.valueAsNumber === 0)
        return false;

    if (isNaN(lineItemQuantity.valueAsNumber)
        || lineItemQuantity.valueAsNumber === 0)
        return false;

    if (lineItemType.valueAsNumber !== 1
        && lineItemType.valueAsNumber !== 2)
        return false;

    if (isNaN(lineItemNetPrice.valueAsNumber))
        return false;

    return {
        itemId: lineItemId.valueAsNumber,
        type: lineItemType.valueAsNumber,
        name: lineItemSelector.value,
        quantity: lineItemQuantity.valueAsNumber,
        unitPrice: lineItemUnitPrice.valueAsNumber,
        netPrice: lineItemNetPrice.valueAsNumber
    };
}

function createCartItem(newItem) {
    const row = document.createElement("div");
    row.className = "row";
    row.innerHTML =
        `<div class="col-12">
        <div class="col" hidden>
            <input type="number"
                name="PurchaseReturnVm.Cart.Index"
                value="${itemsAdded}" />
            <input type="number"
                name="PurchaseReturnVm.Cart[${itemsAdded}].ItemId" 
                class="cart-item-itemid"
                value="${newItem.itemId}" />
            <input type="number"
                name="PurchaseReturnVm.Cart[${itemsAdded}].Type" 
                    class="cart-item-type"
                    value="${newItem.type}"/>
        </div>        

        <div class="input-group input-group-sm mb-0">
            <input type="text"
                class="form-control cart-item-name cart-item"
                data-toggle="tooltip" title="Name"
                value="${newItem.name}" 
                aria-label="Name" readonly/>

            <div class="input-group-append">
                <button class="btn btn-outline-danger cart-item-removeitem"
                    id="remove-item-button${itemsAdded}"
                    type="button">
                    Remove
                </button>
           </div>
        </div>
        <div class="input-group input-group-sm mt-0 mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text">Net Price :</span>
            </div>   

            <input type="number" readonly
                class="text-right cart-item-unirprice cart-item form-control"
                data-toggle="tooltip" title="Unit Price"
                value="${newItem.unitPrice}"/>

            <div class="input-group-prepend">
                <span class="input-group-text">X</span>
            </div>

            <input type="number" readonly
                name="PurchaseReturnVm.Cart[${itemsAdded}].Quantity" 
                class="text-right cart-item-quantity cart-item form-control"
                data-toggle="tooltip" title="Quantity"
                value="${newItem.quantity}"/>            

            <div class="input-group-prepend">
                <span class="input-group-text">=</span>
            </div>

            <input type="number" readonly
                name="PurchaseReturnVm.Cart[${itemsAdded}].NetPrice" 
                class="text-right cart-item-netprice cart-item form-control"
                data-toggle="tooltip" title="Net Price"
                value="${newItem.netPrice}"/>

        </div>
     </div>`;

    return row;
}

function addLineItem(event) {
    event.preventDefault();
    calculateItemPrice();

    const newItem = getLineItem();
    if (newItem === false)
        return;
    const it = createCartItem(newItem);
    removeCartItemIfExists(newItem.itemId);
    cart.appendChild(it);

    const removeButton = gei("remove-item-button" + itemsAdded);
    removeButton.onclick = removeCartItem;

    itemsAdded++;

    clearLineItem(event);
    calculatePayment();
}

function removeCartItemIfExists(itemId) {
    const items = document.getElementsByClassName("cart-item-itemid");
    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        if (item.valueAsNumber === itemId) {
            console.log("Found " + itemId);
            const row = item.parentElement.parentElement.parentElement;
            row.parentElement.removeChild(row);
        }
    }
}

function removeCartItem(event) {
    event.preventDefault();
    var row = this.parentElement.parentElement.parentElement.parentElement;
    $(row).fadeOut();
    sleep(500).then(function () {
        row.parentElement.removeChild(row);
        calculatePayment();
    });
}

function setDebtRollbackOnFocusout(event) {
    if (isNaN(debtRollback.valueAsNumber))
        debtRollback.value = 0;

    debtRollback.value =
        Math.min(
            subtotal.valueAsNumber,
            toFixedIfNecessary(debtRollback.valueAsNumber, 2)
        );

    calculatePayment();
}

$(document).ready(function () {
    $(purchaseSelector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: purchaseSearchApi,
                type: "GET",
                cache: true,
                data: request,
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.label,
                            value: item.id
                        };
                    }));
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {
            event.preventDefault();
            $.ajax({
                url: purchaseBriefUrl + ui.item.value,
                type: "GET",
                dataType: "html",
                success: function (response) {
                    purchaseBriefElem.innerHTML = response;
                }
            }).then(function () {
                const due = gei("current-due").value;
                purchaseSelector.value = ui.item.label;
                purchaseId.value = ui.item.value;
                debtBefore.value = toFixedIfNecessary(due, 2);
            });
        }
    });

    $(lineItemSelector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: prItemSearchApi + purchaseId.value,
                type: "GET",
                cache: true,
                data: request,
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.label,
                            value: item.value
                        };
                    }));
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {
            event.preventDefault();
            const lineitem          = ui.item.value;
            lineItemType.value      = 1;
            lineItemId.value        = lineitem.productId;
            lineItemUnitPrice.value = lineitem.unitPurchasePrice;
            lineItemNetPrice.value  = lineitem.netPurchasePrice;
            lineItemSelector.value  = lineitem.name;
            lineItemQuantity.setAttribute("max", lineitem.quantity);
        }
    });

    subtotal.onchange           = calculatePayment;
    cashBack.onchange           = calculatePayment;
    debtRollback.onfocusout     = setDebtRollbackOnFocusout;
    debtRollback.onchange       = setDebtRollbackOnFocusout;
    debtBefore.onchange         = calculatePayment;
    lineItemQuantity.onchange   = calculateItemPrice;
    lineItemQuantity.onfocusout = calculateItemPrice;
    lineItemAdd.onclick         = addLineItem;
    lineItemClear.onclick       = clearLineItem;
});
