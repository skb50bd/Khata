﻿const sleep = (milliseconds) => {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
};

function toFixedIfNecessary(value, dp) {
    return parseFloat(value).toFixed(dp);
}

function getDate() {
    var date = new Date(),
        year = date.getFullYear(),
        month = (date.getMonth() + 1).toString(),
        formatedMonth = month.length === 1 ? "0" + month : month,
        day = date.getDate().toString(),
        formatedDay = day.length === 1 ? "0" + day : day;
    return formatedDay + "/" + formatedMonth + "/" + year;
}

const saleDate = document.getElementById('SaleDate');
const saleType = document.getElementById('Type');
const retail = document.getElementById('retail');
const bulk = document.getElementById('bulk');
const customerId = document.getElementById('CustomerId');
const customerSelector = document.getElementById('customer-selector');
const registerNewCustomer = document.getElementById('RegisterNewCustomer');
const customerFirstName = document.getElementById('Customer_FirstName');
const customerLastName = document.getElementById('Customer_LastName');
const customerCompanyName = document.getElementById('Customer_CompanyName');
const customerAddress = document.getElementById('Customer_Address');
const customerPhone = document.getElementById('Customer_Phone');
const customerEmail = document.getElementById('Customer_Email');
const lineItemId = document.getElementById('lineitem-id');
const lineItemType = document.getElementById('lineitem-type');
const lineItemSelector = document.getElementById('lineitem-selector');
const lineItemQuantity = document.getElementById('lineitem-quantity');
const lineItemUnitPrice = document.getElementById('lineitem-unitprice');
const lineItemNetPrice = document.getElementById('lineitem-netprice');
const lineItemAdd = document.getElementById('lineitem-add-button');
const lineItemClear = document.getElementById('lineitem-clear-button');
const lineItemAvailable = document.getElementById('lineitem-available');
const cart = document.getElementById('cart');
const subtotal = document.getElementById('subtotal');
const discountCash = document.getElementById('Payment_DiscountCash');
const discountPercentage = document.getElementById('Payment_DiscountPercentage');
const debtBefore = document.getElementById('debt-before');
const payable = document.getElementById('payable');
const paid = document.getElementById('Payment_Paid');
const debtAfter = document.getElementById('debt-after');
const description = document.getElementById('Description');


var itemsAdded = 0;

function customerInputsReadonly(value) {
    var customerInputs = document.getElementsByClassName('customer-input');
    if (value === true) {
        for (var i = 0; i < customerInputs.length; i++) {
            customerInputs[i].setAttribute('readonly', true);
        }
    }
    else {
        for (var j = 0; j < customerInputs.length; j++) {
            customerInputs[j].removeAttribute('readonly');
        }
    }
}

function calculatePayment(event) {
    // Subtotal
    var subTotalValue = 0;
    var currentCartItemsNetPrices = document.getElementsByClassName('cart-item-netprice');
    for (var i = 0; i < currentCartItemsNetPrices.length; i++)
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
    if (isNaN(debtBefore.valueAsNumber))
        debtBefore.valueAsNumber = 0;

    // Payable
    payable.value = toFixedIfNecessary(subtotal.valueAsNumber - discountCash.valueAsNumber + debtBefore.valueAsNumber, 2);

    // Paid
    if (isNaN(paid.valueAsNumber))
        paid.value = 0;

    // Debt After
    debtAfter.value = toFixedIfNecessary(payable.valueAsNumber - paid.valueAsNumber, 2);

    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
}

function calculateItemPrice(event) {
    if (isNaN(lineItemId.valueAsNumber) || lineItemId.valueAsNumber === 0) {
        clearLineItem(event);
        return;
    }

    if (isNaN(lineItemQuantity.valueAsNumber))
        lineItemQuantity.value = 1;

    const minimumPrice = Number(lineItemUnitPrice.getAttribute('min'));
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

    const minimumPrice = Number(lineItemUnitPrice.getAttribute('min'));
    var minimumNetPrice = minimumPrice * lineItemQuantity.valueAsNumber;

    if (isNaN(lineItemNetPrice.valueAsNumber) || lineItemNetPrice.valueAsNumber < minimumNetPrice)
        lineItemNetPrice.value = minimumNetPrice;

    lineItemUnitPrice.value = lineItemNetPrice.valueAsNumber / lineItemQuantity.valueAsNumber;
}

function clearLineItem(event) {
    event.preventDefault();
    lineItemId.value = '';
    lineItemId.removeAttribute('min');
    lineItemType.value = '';
    lineItemSelector.value = '';
    lineItemQuantity.value = '';
    lineItemUnitPrice.value = '';
    lineItemNetPrice.value = '';
}

function getLineItem() {
    if (isNaN(lineItemId.valueAsNumber) || lineItemId.valueAsNumber === 0)
        return false;

    if (isNaN(lineItemQuantity.valueAsNumber) || lineItemQuantity.valueAsNumber === 0)
        return false;

    if (lineItemType.valueAsNumber !== 1 && lineItemType.valueAsNumber !== 2)
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
    var row = document.createElement('div');
    row.className = "row";
    row.innerHTML = `
        <div class="col-12">
        <div class="col" hidden>
            <input type="number"
                name="Cart.Index"
                value="`+ itemsAdded + `" />
            <input type="number"
                name="Cart[`+ itemsAdded + `].ItemId" 
                class="cart-item-itemid"
                value="` + newItem.itemId + `" />
            <input type="number"
                name="Cart[`+ itemsAdded + `].Type" 
                    class="cart-item-type"
                    value="` + newItem.type + `"/>
        </div>        

        <div class="input-group input-group-sm mb-0">
            <input type="text"
                class="form-control cart-item-name cart-item"
                data-toggle="tooltip" title="Name"
                value="` + newItem.name + `" 
                aria-label="Name" readonly/>

            <div class="input-group-append">
                <button class="btn btn-outline-danger cart-item-removeitem"
                    id="remove-item-button`+ itemsAdded + `"
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
                value="`+ newItem.unitPrice + `"/>

            <div class="input-group-prepend">
                <span class="input-group-text">X</span>
            </div>

            <input type="number" readonly
                name="Cart[`+ itemsAdded + `].Quantity" 
                class="text-right cart-item-quantity cart-item form-control"
                data-toggle="tooltip" title="Quantity"
                value="` + newItem.quantity + `"/>            

            <div class="input-group-prepend">
                <span class="input-group-text">=</span>
            </div>

            <input type="number" readonly
                name="Cart[`+ itemsAdded + `].NetPrice" 
                class="text-right cart-item-netprice cart-item form-control"
                data-toggle="tooltip" title="Net Price"
                value="` + newItem.netPrice + `"/>

        </div>
     </div>
    `;

    return row;
}

function addLineItem(event) {
    event.preventDefault();

    var newItem = getLineItem();
    if (newItem === false)
        return;
    var it = createCartItem(newItem);
    //it.parentElement = cart;
    //it.fadein();
    cart.appendChild(it);
    document.getElementById('remove-item-button' + itemsAdded).addEventListener('click', removeCartItem);

    itemsAdded++;

    clearLineItem(event);
    calculatePayment();
}

function removeCartItem(event) {
    event.preventDefault();
    var row = this.parentElement.parentElement.parentElement.parentElement;
    $(row).fadeOut();
    sleep(500).then(function () {
        row.parentElement.removeChild(row);
    });
}

$(document).ready(function () {
    if (saleDate.value === '')
        saleDate.value = getDate();

    registerNewCustomer.removeAttribute('checked');

    $('#customer-selector').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: customerSelector.getAttribute("data-path"),
                type: 'GET',
                cache: true,
                data: request,
                dataType: 'json',
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
                url: '/api/Customers/' + ui.item.value,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    debtBefore.value = data.debt;
                    customerFirstName.value = data.firstName;
                    customerLastName.value = data.lastName;
                }
            });
            customerSelector.value = ui.item.label;
            customerId.value = ui.item.value;
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
                    ul.append("<li class='ui-autocomplete-category'>" + item.value.category + "</li>");
                    currentCategory = item.value.category;
                }
                li = that._renderItemData(ul, item);
                if (item.value.category) {
                    li.attr("aria-label", item.value.category + " : " + item.label);
                }
            });
        }
    });

    $('#lineitem-selector').catcomplete({
        source: function (request, response) {
            $.ajax({
                url: lineItemSelector.getAttribute("data-path"),
                type: 'GET',
                cache: true,
                data: request,
                dataType: 'json',
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
            var lineitem = ui.item.value;
            if (lineitem.category === 'Product') {
                lineItemType.value = 1;
                lineItemAvailable.innerHTML = lineitem.available;
                lineItemAvailable.parentElement.removeAttribute('hidden');
            }
            else {
                lineItemType.value = 2;
                lineItemAvailable.parentElement.setAttribute('hidden');
            }
            lineItemId.value = lineitem.itemId;

            if (retail.checked) {
                lineItemUnitPrice.value = lineitem.unitPriceRetail;
            }
            else {
                lineItemUnitPrice.value = lineitem.unitPriceBulk;
            }

            if (lineitem.category === 'Service') {
                lineItemQuantity.value = 1;
                //calculateItemPrice(event);
            }
            lineItemUnitPrice.setAttribute('min', lineitem.minimumPrice);

            lineItemSelector.value = lineitem.name;
        }
    });

    registerNewCustomer.addEventListener('change', function () {
        if (this.checked === true) {
            customerId.value = '0';
            customerSelector.value = '';
            customerInputsReadonly(false);
        }
        else {
            customerInputsReadonly(true);
        }
    });
    subtotal.addEventListener('change', calculatePayment);
    discountCash.addEventListener('change', calculatePayment);
    discountPercentage.addEventListener('focusout', function () {
        if (isNaN(discountPercentage.valueAsNumber))
            discountPercentage.value = 0;
        discountPercentage.value = toFixedIfNecessary(discountPercentage.valueAsNumber, 2);
        discountCash.value = toFixedIfNecessary(subtotal.valueAsNumber / 100 * discountPercentage.valueAsNumber, 2);
        calculatePayment();
    });
    debtBefore.addEventListener('change', calculatePayment);
    paid.addEventListener('change', calculatePayment);
    lineItemQuantity.addEventListener('change', calculateItemPrice);
    lineItemQuantity.addEventListener('focusout', calculateItemPrice);
    lineItemUnitPrice.addEventListener('focusout', calculateItemPrice);
    lineItemNetPrice.addEventListener('focusout', setUnitPriceFromNetPrice);
    lineItemAdd.addEventListener('click', addLineItem);
    lineItemClear.addEventListener('click', clearLineItem);
});
