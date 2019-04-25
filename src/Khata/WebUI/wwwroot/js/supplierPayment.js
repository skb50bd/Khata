const supplierId        = document.getElementById("supplier-id");
const supplierSelector  = document.getElementById("supplier-selector");
const supplierSearchUrl = supplierSelector.getAttribute("data-path");
const supplierInfoUrl   = "/api/Suppliers/"; // Must concatenate SUPPLIER Id to get the data
const payableBefore     = document.getElementById("payable-before");
const payableAfter      = document.getElementById("payable-after");
const paidAmount        = document.getElementById("paid-amount");

function updatePayable() {
    payableAfter.value = Number(payableBefore.value) - Number(paidAmount.value);
}

$(document).ready(function () {
    $(supplierSelector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: supplierSearchUrl,
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
            $.ajax({
                url: supplierInfoUrl + ui.item.value,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    payableBefore.value = data.payable;
                    updatePayable();
                }
            });

            supplierSelector.value = ui.item.label;
            supplierId.value       = ui.item.value;

            return false;
        }
    });

    payableBefore.onchange = updatePayable;
    payableBefore.onkeyup  = updatePayable;
    paidAmount.onchange    = updatePayable;
    paidAmount.onkeyup     = updatePayable;
});