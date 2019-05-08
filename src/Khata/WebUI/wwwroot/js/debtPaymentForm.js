const amount               = gei("amount");
const debtBefore           = gei("debt-before");
const debtAfter            = gei("debt-after");
const customerId           = gei("customer-id");
const customerSelector     = gei("customer-selector");
const customerSearchApi    = customerSelector.getAttribute("data-path");
const customerInfoApi      = "/api/Customers/"; // Must Concatenate CustomerId
const customerBriefInfo    = gei("customer-brief-info");
const customerBriefInfoUrl = "/People/Customers/Brief?id=";

function updateDebt() {
    const dbVal = Number(debtBefore.value);
    const aVal = Number(amount.value);
    const result = dbVal - aVal;
    debtAfter.value = result;
}


$(document).ready(function () {
    $(customerSelector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: customerSearchApi,
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
                url: customerInfoApi + ui.item.value,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    debtBefore.value = data.debt;
                    updateDebt();
                }
            });

            customerSelector.value = ui.item.label;
            customerId.value = ui.item.value;
            $.ajax({
                url: customerBriefInfoUrl + ui.item.value,
                type: "GET",
                dataType: "html",
                success: function (response) {
                    customerBriefInfo.innerHTML = response;
                }
            });

            return false;
        }
    });

    debtBefore.onchange = updateDebt;
    debtBefore.onkeyup = updateDebt;
    amount.onchange = updateDebt;
    amount.onkeyup = updateDebt;
});