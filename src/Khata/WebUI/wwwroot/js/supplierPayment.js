const supplierId           = gei("supplier-id");
const supplierSelector     = gei("supplier-selector");
const supplierSearchUrl    = supplierSelector.getAttribute("data-path");
const supplierInfoUrl      = "/api/Suppliers/"; // Must concatenate SUPPLIER Id to get the data
const payableBefore        = gei("payable-before");
const payableAfter         = gei("payable-after");
const paidAmount           = gei("paid-amount");
const supplierBriefInfo    = gei("supplier-brief-info");
const supplierBriefInfoUrl = "/People/Suppliers/Details/Brief?supplierId="; // Must concatenate SUPPLIER Id to get the data

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

            $.ajax({
                url: supplierBriefInfoUrl + ui.item.value,
                type: "GET",
                dataType: "html",
                success: function (response) {
                    supplierBriefInfo.innerHTML = response;
                }
            });

            return false;
        }
    });

    payableBefore.onchange = updatePayable;
    payableBefore.onkeyup  = updatePayable;
    paidAmount.onchange    = updatePayable;
    paidAmount.onkeyup     = updatePayable;
});