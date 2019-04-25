const employeeSelector  = document.getElementById("employee-selector");
const employeeSearchUrl = employeeSelector.getAttribute("data-path");
const employeeInfoUrl   = "/api/Employees/"; // must concatenate the employee Id with it
const balanceBefore     = document.getElementById("balance-before");
const balanceAfter      = document.getElementById("balance-after");
const issueAmount       = document.getElementById("issue-amount");
const paidAmount        = document.getElementById("paid-amount");
const employeeId        = document.getElementById("employee-id");

function updateBalance() {
    const balBefore = Number(balanceBefore.value);

    var issue = 0;
    if (issueAmount)
        issue = Number(issueAmount.value);

    var paid = 0;
    if (paidAmount)
        paid = Number(paidAmount.value);

    const result = balBefore + issue - paid;
    balanceAfter.value = result;
}

$(document).ready(function () {
    $(employeeSelector).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: employeeSearchUrl,
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
                url: employeeInfoUrl + ui.item.value,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    balanceBefore.value = data.balance;
                    updateBalance();
                }
            });
            employeeSelector.value = ui.item.label;
            employeeId.value       = ui.item.value;
            return false;
        }
    });

    balanceBefore.onchange   = updateBalance;
    issueAmount.onchange     = updateBalance;
    issueAmount.onkeyup      = updateBalance;
    paidAmount.onchange      = updateBalance;
    paidAmount.onkeyup       = updateBalance;

    //$(document).on("change, keyup", "balance-before", updateBalance);
    //$(document).on("change, keyup", "issue-amount", updateBalance);
});