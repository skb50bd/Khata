const purchaseDate        = gei("purchase-date");
const retail              = gei("retail");
const bulk                = gei("bulk");
const supplierId          = gei("supplier-id");
const supplierSelector    = gei("supplier-selector");
const registerNewSupplier = gei("register-new-supplier");
const supplierFirstName   = gei("first-name");
const supplierLastName    = gei("last-name");
const companyName         = gei("company-name");
const supplierAddress     = gei("address");
const supplierPhone       = gei("phone");
const supplierEmail       = gei("email");
const supplierBriefInfo   = gei("supplier-brief-info");
const lineItemId          = gei("lineitem-id");
const lineItemType        = gei("lineitem-type");
const lineItemSelector    = gei("lineitem-selector");
const lineItemQuantity    = gei("lineitem-quantity");
const lineItemUnitPrice   = gei("lineitem-unitprice");
const lineItemNetPrice    = gei("lineitem-netprice");
const lineItemAdd         = gei("lineitem-add-button");
const lineItemClear       = gei("lineitem-clear-button");
const lineItemAvailable   = gei("lineitem-available");
const cart                = gei("cart");
const subtotal            = gei("subtotal");
const discountCash        = gei("discount-cash");
const discountPercentage  = gei("discount-percentage");
const payableBefore       = gei("payable-before");
const payable             = gei("payable");
const paid                = gei("paid-amount");
const payableAfter        = gei("payable-after");


var itemsAdded = 0;

function supplierInputsReadonly(value) {
    const supplierInputs =
        document.getElementsByClassName("supplier-input");

    if (value === true) {
        for (let i = 0; i < supplierInputs.length; i++) {
            supplierInputs[i].setAttribute("readonly", true);
        }
    } else {
        for (let j = 0; j < supplierInputs.length; j++) {
            supplierInputs[j].removeAttribute("readonly");
        }
    }
}

function calculatePayment(event) {
    // Subtotal
    var subTotalValue = 0;
    const currentCartItemsNetPrices = document.getElementsByClassName("cart-item-netprice");
    for (let i = 0; i < currentCartItemsNetPrices.length; i++)
        subTotalValue += currentCartItemsNetPrices[i].valueAsNumber;

    subtotal.value = toFixedIfNecessary(subTotalValue, 2);

    // Discount Cash
    if (isNaN(discountCash.valueAsNumber))
        discountCash.value = 0;

    // Discount Percentage
    discountPercentage.value =
        subTotalValue !== 0
            ? toFixedIfNecessary(discountCash.valueAsNumber / subTotalValue * 100, 2)
            : 0;

    // Previous Due
    if (isNaN(payableBefore.valueAsNumber))
        payableBefore.valueAsNumber = 0;

    // Payable
    payable.value = toFixedIfNecessary(subtotal.valueAsNumber - discountCash.valueAsNumber + payableBefore.valueAsNumber, 2);

    // Paid
    if (isNaN(paid.valueAsNumber))
        paid.value = 0;

    // Payable After
    payableAfter.value = toFixedIfNecessary(payable.valueAsNumber - paid.valueAsNumber, 2);

    $('[data-toggle="tooltip"]').tooltip();
}

function calculateItemPrice(event) {
    if (isNaN(lineItemId.valueAsNumber) || lineItemId.valueAsNumber === 0) {
        clearLineItem(event);
        return;
    }

    if (isNaN(lineItemQuantity.valueAsNumber))
        lineItemQuantity.value = 1;

    const minimumPrice = Number(lineItemUnitPrice.getAttribute("min"));
    if (isNaN(lineItemUnitPrice.valueAsNumber) || lineItemUnitPrice.valueAsNumber < minimumPrice)
        lineItemUnitPrice.value = minimumPrice;

    lineItemNetPrice.value = lineItemQuantity.valueAsNumber * lineItemUnitPrice.valueAsNumber;
}

function setUnitPriceFromNetPrice(event) {
    if (isNaN(lineItemId.value) || lineItemId.valueAsNumber === 0) {
        clearLineItem(event);
        return;
    }

    if (isNaN(lineItemQuantity.valueAsNumber))
        lineItemQuantity.value = 1;

    const minimumPrice = Number(lineItemUnitPrice.getAttribute("min"));
    const minimumNetPrice = minimumPrice * lineItemQuantity.valueAsNumber;

    if (isNaN(lineItemNetPrice.valueAsNumber) || lineItemNetPrice.valueAsNumber < minimumNetPrice)
        lineItemNetPrice.value = minimumNetPrice;

    lineItemUnitPrice.value = lineItemNetPrice.valueAsNumber / lineItemQuantity.valueAsNumber;
}

function clearLineItem(event) {
    event.preventDefault();
    lineItemId.value = "";
    lineItemId.removeAttribute("min");
    lineItemType.value      = "";
    lineItemSelector.value  = "";
    lineItemQuantity.value  = "";
    lineItemUnitPrice.value = "";
    lineItemNetPrice.value  = "";
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
                name="Cart.Index"
                value="${itemsAdded}" />
            <input type="number"
                name="Cart[${itemsAdded}].ItemId" 
                class="cart-item-itemid"
                value="${newItem.itemId}" />
            <input type="number"
                name="Cart[${itemsAdded}].Type" 
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
                name="Cart[${itemsAdded}].Quantity" 
                class="text-right cart-item-quantity cart-item form-control"
                data-toggle="tooltip" title="Quantity"m
                value="${newItem.quantity}"/>            

            <div class="input-group-prepend">
                <span class="input-group-text">=</span>
            </div>

            <input type="number" readonly
                name="Cart[${itemsAdded}].NetPrice" 
                class="text-right cart-item-netprice cart-item form-control"
                data-toggle="tooltip" title="Net Price"
                value="${newItem.netPrice}"/>

        </div>
     </div>`;

    return row;
}

function addLineItem(event) {
    event.preventDefault();

    const newItem = getLineItem();
    if (newItem === false)
        return;

    const it = createCartItem(newItem);
    removeCartItemIfExists(newItem.itemId);
    cart.appendChild(it);

    const removeButton =
        gei(`remove-item-button${itemsAdded}`);

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
            console.log(`Found ${itemId}`);
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

$(document).ready(function () {
    $("#purchase-date").datetimepicker({
        format: "DD/MM/YYYY",
        useCurrent: true
    });
    if (purchaseDate.value === "")
        purchaseDate.value = getDate();

    registerNewSupplier.removeAttribute("checked");

    $("#supplier-selector").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: supplierSelector.getAttribute("data-path"),
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
        minLength: 1,
        select: function (event, ui) {
            event.preventDefault();
            $.ajax({
                url: `/api/Suppliers/${ui.item.value}`,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    payableBefore.value = data.payable;
                    supplierFirstName.value = data.firstName;
                    supplierLastName.value = data.lastName;
                }
            }).then(() => {
                $.ajax({
                    url: `/Suppliers/Details/Brief?supplierId=${ui.item.value}`,
                    type: "GET",
                    dataType: "html",
                    success: function (response) {
                        supplierBriefInfo.innerHTML = response;
                    }
                });
            });
            supplierSelector.value = ui.item.label;
            supplierId.value = ui.item.value;
        }
    });

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
        },
        _renderMenu: function (ul, items) {
            var that = this,
                currentCategory = "";
            $.each(items, function (index, item) {
                var li;
                if (item.value.category !== currentCategory) {
                    ul.append(`<li class='ui-autocomplete-category'>${item.value.category}</li>`);
                    currentCategory = item.value.category;
                }
                li = that._renderItemData(ul, item);
                if (item.value.category) {
                    li.attr("aria-label", item.value.category + " : " + item.label);
                }
            });
        }
    });

    $("#lineitem-selector").catcomplete({
        source: function (request, response) {
            $.ajax({
                url: lineItemSelector.getAttribute("data-path"),
                type: "GET",
                cache: true,
                data: request,
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.name,
                            value: item
                        };
                    }));
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {
            event.preventDefault();
            const lineitem = ui.item.value;
            if (lineitem.category === "Product") {
                lineItemType.value = 1;
                lineItemAvailable.innerHTML = lineitem.available;
                lineItemAvailable.parentElement.removeAttribute("hidden");
            }
            else {
                lineItemType.value = 2;
                lineItemAvailable.parentElement.setAttribute("hidden");
            }
            lineItemId.value = lineitem.itemId;

            lineItemUnitPrice.value = lineitem.unitPurchasePrice;

            if (lineitem.category === "Service") {
                lineItemQuantity.value = 1;
                //calculateItemPrice(event);
            }
            lineItemUnitPrice.setAttribute("min", lineitem.minimumPrice);

            lineItemSelector.value = lineitem.name;
        }
    });

    registerNewSupplier.addEventListener("change", function () {
        if (this.checked === true) {
            supplierId.value = "0";
            supplierSelector.value = "";
            supplierInputsReadonly(false);
            supplierBriefInfo.innerHTML = "";
        }
        else {
            supplierInputsReadonly(true);
        }
    });
    subtotal.addEventListener("change", calculatePayment);
    discountCash.addEventListener("change", calculatePayment);
    discountPercentage.addEventListener("focusout", function () {
        if (isNaN(discountPercentage.valueAsNumber))
            discountPercentage.value = 0;
        discountPercentage.value = toFixedIfNecessary(discountPercentage.valueAsNumber, 2);
        discountCash.value = toFixedIfNecessary(subtotal.valueAsNumber / 100 * discountPercentage.valueAsNumber, 2);
        calculatePayment();
    });
    payableBefore.addEventListener("change", calculatePayment);
    paid.addEventListener("change", calculatePayment);
    lineItemQuantity.addEventListener("change", calculateItemPrice);
    lineItemQuantity.addEventListener("focusout", calculateItemPrice);
    lineItemUnitPrice.addEventListener("focusout", calculateItemPrice);
    lineItemNetPrice.addEventListener("focusout", setUnitPriceFromNetPrice);
    lineItemAdd.addEventListener("click", addLineItem);
    lineItemClear.addEventListener("click", clearLineItem);
});
