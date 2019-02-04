const sleep = (milliseconds) => {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
};

function toFixedIfNecessary(value, dp) {
    return parseFloat(value).toFixed(dp);
}

const customerId = document.getElementById('CustomerId');
const customerSelector = document.getElementById('customer-selector');
const saleId = document.getElementById('SaleId');
const saleSelector = document.getElementById('sale-selector');
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
const cashBack = document.getElementById('CashBack');
const debtRollback = document.getElementById('DebtRollback');
const debtBefore = document.getElementById('debt-before');
const debtAfter = document.getElementById('debt-after');
const description = document.getElementById('Description');

var itemsAdded = 0;

function calculatePayment(event) {
    // Subtotal
    var subTotalValue = 0;
    var currentCartItemsNetPrices = document.getElementsByClassName('cart-item-netprice');
    for (var i = 0; i < currentCartItemsNetPrices.length; i++)
        subTotalValue += currentCartItemsNetPrices[i].valueAsNumber;

    subtotal.value = toFixedIfNecessary(subTotalValue, 2);

    // Cash Back
    if (isNaN(cashBack.valueAsNumber))
        cashBack.value = 0;

    // Debt Rollback   
    debtRollback.value =
        subTotalValue !== 0
        ? toFixedIfNecessary(subTotalValue - cashBack.valueAsNumber, 2)
            : 0;

    // Previous Due
    if (isNaN(debtBefore.valueAsNumber))
        debtBefore.valueAsNumber = 0;
    
    // Debt After
    debtAfter.value = toFixedIfNecessary(debtBefore - debtRollback.valueAsNumber, 2);

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
    
    if (isNaN(lineItemUnitPrice.valueAsNumber))
        lineItemUnitPrice.value = minimumPrice;

    lineItemNetPrice.value = lineItemQuantity.valueAsNumber * lineItemUnitPrice.valueAsNumber;
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
        calculatePayment();
    });
}

$(document).ready(function () {
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

    $('#sale-selector').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: saleSelector.getAttribute("data-path"),
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
        minLength: 0,
        select: function (event, ui) {
            event.preventDefault();
            $.ajax({
                url: '/api/Sales/' + ui.item.value,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    // View Sale Info
                }
            });
            saleSelector.value = ui.item.label;
            saleId.value = ui.item.value;
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

            lineItemType.value = 1;
            lineItemId.value = lineitem.itemId;
            lineItemUnitPrice.value = lineitem.unitPrice;
            lineItemNetPrice.value = lineitem.netPrice;
            
            lineItemSelector.value = lineitem.name;
        }
    });
    
    subtotal.addEventListener('change', calculatePayment);
    cashBack.addEventListener('change', calculatePayment);
    debtRollback.addEventListener('focusout', function () {
        if (isNaN(debtRollback.valueAsNumber))
            debtRollback.value = 0;
        debtRollback.value = toFixedIfNecessary(debtRollback.valueAsNumber, 2);
        cashBack.value = toFixedIfNecessary(subtotal.valueAsNumber / 100 * debtRollback.valueAsNumber, 2);
        calculatePayment();
    });
    debtBefore.addEventListener('change', calculatePayment);
    paid.addEventListener('change', calculatePayment);
    lineItemQuantity.addEventListener('change', calculateItemPrice);
    lineItemQuantity.addEventListener('focusout', calculateItemPrice);
    lineItemAdd.addEventListener('click', addLineItem);
    lineItemClear.addEventListener('click', clearLineItem);
});
