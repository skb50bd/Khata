const sleep = (milliseconds) => {
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

const purchaseDate = document.getElementById('PurchaseDate');
const purchaseType = document.getElementById('Type');
const retail = document.getElementById('retail');
const bulk = document.getElementById('bulk');
const supplierId = document.getElementById('SupplierId');
const supplierSelector = document.getElementById('supplier-selector');
const registerNewSupplier = document.getElementById('RegisterNewSupplier');
const supplierFirstName = document.getElementById('Supplier_FirstName');
const supplierLastName = document.getElementById('Supplier_LastName');
const supplierCompanyName = document.getElementById('Supplier_CompanyName');
const supplierAddress = document.getElementById('Supplier_Address');
const supplierPhone = document.getElementById('Supplier_Phone');
const supplierEmail = document.getElementById('Supplier_Email');
const lineItemId = document.getElementById('lineitem-id');
const lineItemType = document.getElementById('lineitem-type');
const lineItemSelector = document.getElementById('lineitem-selector');
const lineItemQuantity = document.getElementById('lineitem-quantity');
const lineItemUnitPrice = document.getElementById('lineitem-unitprice');
const lineItemNetPrice = document.getElementById('lineitem-netprice');
const lineItemAdd = document.getElementById('lineitem-add-button');
const lineItemClear = document.getElementById('lineitem-clear-button');
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

function supplierInputsReadonly(value) {
    var supplierInputs = document.getElementsByClassName('supplier-input');
    if (value === true) {
        for (var i = 0; i < supplierInputs.length; i++) {
            supplierInputs[i].setAttribute('readonly');
        }
    }
    else {
        for (var j = 0; j < supplierInputs.length; j++) {
            supplierInputs[j].removeAttribute('readonly');
        }
    }
}

function calculatePayment(event) {
    // Subtotal
    // Todo - Calculate Subtotal
    var subTotalValue = 0;
    var currentCartItemsNetPrices = document.getElementsByClassName('cart-item-netprice');
    for (var i = 0; i < currentCartItemsNetPrices.length; i++)
        subTotalValue += currentCartItemsNetPrices[i].valueAsNumber;

    subtotal.value = toFixedIfNecessary(subTotalValue, 2);
    
    // Previous Due
    if (isNaN(debtBefore.valueAsNumber))
        debtBefore.valueAsNumber = 0;

    // Payable
    payable.value = toFixedIfNecessary(subtotal.valueAsNumber + debtBefore.valueAsNumber, 2);

    // Paid
    if (isNaN(paid.valueAsNumber))
        paid.value = 0;

    // Debt After
    debtAfter.value = toFixedIfNecessary(payable.valueAsNumber - paid.valueAsNumber, 2);
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
    var row = document.createElement('tr');

    row.innerHTML = `
        <td hidden>
            <input type="number"
                name="Cart.Index"
                value="`+ itemsAdded + `" />
            <input type="number"
                name="Cart[`+ itemsAdded + `].ItemId" 
                class="cart-item-itemid"
                value="` + newItem.itemId + `" />
        </td>
        <td hidden>
            <input type="number"
                name="Cart[`+ itemsAdded + `].Type" 
                    class="cart-item-type"
                    value="` + newItem.type + `"/>
        </td>
        <td colspan="3">
            <input type="text" readonly
                    class="cart-item-name"
                    value="` + newItem.name + `"/>
        </td>
        <td class="text-right">
            <input type="number" readonly
                name="Cart[`+ itemsAdded + `].Quantity" 
                class="text-right cart-item-quantity"
                value="` + newItem.quantity + `"/></td>
        <td class="text-right">
            <input type="number" readonly class="text-right"
                class="cart-item-unirprice"
                value="`+ newItem.unitPrice + `"/>
        </td>
        <td class="text-right">
            <input type="number" readonly
                name="Cart[`+ itemsAdded + `].NetPrice" 
                class="text-right cart-item-netprice"
                value="` + newItem.netPrice + `"/>
        </td>
        <td>
            <button class="btn btn-sm btn-danger"
                id="remove-item-button`+ itemsAdded + `"
                class="text-right cart-item-removeitem">
                Remove
            </button>
        </td>
    `;

    return row;
}

function addLineItem(event) {
    event.preventDefault();

    var newItem = getLineItem();
    if (newItem === false)
        return;

    cart.appendChild(createCartItem(newItem));
    document.getElementById('remove-item-button' + itemsAdded).addEventListener('click', removeCartItem);

    itemsAdded++;

    clearLineItem(event);
    calculatePayment();
}

function removeCartItem(event) {
    event.preventDefault();
    var row = this.parentElement.parentElement;
    row.parentElement.removeChild(row);
}

$(document).ready(function () {
    if (purchaseDate.value === '')
        purchaseDate.value = getDate();

    registerNewSupplier.removeAttribute('checked');

    $('#supplier-selector').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: supplierSelector.getAttribute("data-path"),
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
                url: '/api/Suppliers/' + ui.item.value,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    debtBefore.value = data.payable;
                    supplierFirstName.value = data.firstName;
                    supplierLastName.value = data.lastName;
                }
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
            if (lineitem.category === 'Product')
                lineItemType.value = 1;
            else
                lineItemType.value = 2;

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

    registerNewSupplier.addEventListener('change', function () {
        if (this.checked === true) {
            supplierId.value = '';
            supplierSelector.value = '';
            supplierInputsReadonly(false);
        }
        else {
            supplierInputsReadonly(true);
        }
    });
    subtotal.addEventListener('change', calculatePayment);
    debtBefore.addEventListener('change', calculatePayment);
    paid.addEventListener('change', calculatePayment);
    lineItemQuantity.addEventListener('change', calculateItemPrice);
    lineItemQuantity.addEventListener('focusout', calculateItemPrice);
    lineItemUnitPrice.addEventListener('focusout', calculateItemPrice);
    lineItemNetPrice.addEventListener('focusout', setUnitPriceFromNetPrice);
    lineItemAdd.addEventListener('click', addLineItem);
    lineItemClear.addEventListener('click', clearLineItem);
});
