// Assets
const assetChartElem = gei("assetChart");
window.assetData = null;
window.assetChart = new Chart(assetChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    }, data: {}
});
function getAssetChartData(assetReport) {
    return {
        datasets: [{
            data: [
                toFixedIfNecessary(assetReport.totalDue),
                toFixedIfNecessary(assetReport.cash),
                toFixedIfNecessary(assetReport.inventoryWorth)
            ],
            backgroundColor: ["#ef5350", "#66BB6A", "#7E57C2"]
        }],

        labels: [
            "Due",
            "Cash",
            "Inventory"
        ]
    };
}
function updateAssetData() {
    var data = window.assetData;
    gei("asset-total").innerHTML =
        formatter.format(data.cash + data.inventoryWorth + data.totalDue);

    gei("customers-with-due").innerHTML = data.dueCount;
    gei("due-amount").innerHTML = formatter.format(data.totalDue);
    gei("cash-in-hand").innerHTML = formatter.format(data.cash);
    gei("products-in-stock").innerHTML = data.inventoryCount;
    gei("products-total-cost").innerHTML = formatter.format(data.inventoryWorth);
}

// Liabilities
const liabilityChartElem = gei("liabilityChart");
window.liabilityChart = new Chart(liabilityChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.liabilityData = null;
function getLiabilityChartData(liabilityReport) {
    return {
        datasets: [{
            data: [liabilityReport.totalDue, liabilityReport.unpaidAmount],
            backgroundColor: ["#42A5F5", "#FF7043"]
        }],

        labels: [
            "Supplier Due",
            "Employee Salary"
        ]
    };
}
function updateLiabilityData() {
    var data = window.liabilityData;
    gei("liability-total").innerHTML =
        formatter.format(data.totalDue + data.unpaidAmount);
    gei("suppliers-with-due").innerHTML = data.dueCount;
    gei("total-supplier-due").innerHTML = formatter.format(data.totalDue);
    gei("unpaid-employees").innerHTML = data.unpaidEmployees;
    gei("unpaid-amount").innerHTML = formatter.format(data.unpaidAmount);
}

// Per Day Report
const perDayReportChartElem =
    gei("perdayreport-chart");

var pdrcc = {
    type: "line",
    data: {},
    options: {
        tooltips: {
            mode: "index",
            callbacks: {
                label: function (tooltipItem, data) {
                    var label = data.datasets[tooltipItem.datasetIndex].label || "";

                    if (label) {
                        label += ": ";
                    }
                    return label + formatter.format(tooltipItem.yLabel);
                }
            }
        },
        fill: true,
        responsive: true,
        title: {
            display: false,
            text: "Overview"
        },
        hover: {
            mode: "index"
        },
        scales: {
            xAxes: [{
                type: "time",
                time: {
                    displayFormats: {
                        'millisecond': "MMM DD",
                        'second': "MMM DD",
                        'minute': "MMM DD",
                        'hour': "MMM DD",
                        'day': "MMM DD",
                        'week': "MMM DD",
                        'month': "MMM DD",
                        'quarter': "MMM DD",
                        'year': "MMM DD"
                    },
                    unit: "day",
                    tooltipFormat: "DD MMM YYYY"
                },
                scaleLabel: {
                    display: false
                }
            }],
            yAxes: [{
                stacked: false,
                scaleLabel: {
                    display: true
                },
                ticks: {
                    beginAtZero: true,
                    callback: function (value) {
                        return formatter.format(value);
                    }
                }
            }]
        },
        legend: {
            display: true,
            position: "bottom"
        }
    },
    onResize: function (perDayReportChart, size) {
        const showTicks = size.height < 140 ? false : true;
        perDayReportChart.options = {
            scales: {
                xAxes: [
                    {
                        ticks: {
                            display: showTicks
                        }
                    }
                ]
            }
        };
    }
};
window.perDayReportChart = new Chart(perDayReportChartElem, pdrcc);
window.perDayData = null;
function getPerDayChartData(perDayData) {
    return {
        datasets: [
            {
                label: "New Due",
                backgroundColor: "rgba(255,152,0 ,0.8)",
                borderColor: "rgba(255,152,0 ,0.8)",
                data: perDayData.map(d => d.newReceivable)
            }, {
                label: "Cash In",
                backgroundColor: "rgba(76,175,80 ,0.8)",
                borderColor: "rgba(76,175,80 ,0.8)",
                data: perDayData.map(d => d.cashIn)
            },
            {
                label: "Cash Out",
                backgroundColor: "rgba(255,87,34 ,0.8)",
                borderColor: "rgba(255,87,34 ,0.8)",
                data: perDayData.map(d => d.cashOut)
            }, {
                label: "New Payable",
                backgroundColor: "rgba(156,39,176 ,0.8)",
                borderColor: "rgba(156,39,176 ,0.8)",
                data: perDayData.map(d => d.newPayable)
            }],

        labels: perDayData.map((d) => d.date)
    };
}


const dailyIncomeChartElem =
    gei("daily-income-chart");

const dailyExpenseChartElem =
    gei("daily-expense-chart");

const dailyPayableChartElem =
    gei("daily-payable-chart");

const dailyReceivableChartElem =
    gei("daily-receivable-chart");

const weeklyIncomeChartElem =
    gei("weekly-income-chart");

const weeklyExpenseChartElem =
    gei("weekly-expense-chart");

const weeklyPayableChartElem =
    gei("weekly-payable-chart");

const weeklyReceivableChartElem =
    gei("weekly-receivable-chart");

const monthlyIncomeChartElem =
    gei("monthly-income-chart");

const monthlyExpenseChartElem =
    gei("monthly-expense-chart");

const monthlyPayableChartElem =
    gei("monthly-payable-chart");

const monthlyReceivableChartElem =
    gei("monthly-receivable-chart");


window.dailyIncomeData = null;
window.dailyExpenseData = null;
window.dailyPayableData = null;
window.dailyReceivableData = null;
window.weeklyIncomeData = null;
window.weeklyExpenseData = null;
window.weeklyPayableData = null;
window.weeklyReceivableData = null;
window.monthlyIncomeData = null;
window.monthlyExpenseData = null;
window.monthlyPayableData = null;
window.monthlyReceivableData = null;

window.dailyIncomeChart = new Chart(dailyIncomeChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.dailyExpenseChart = new Chart(dailyExpenseChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.dailyPayableChart = new Chart(dailyPayableChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.dailyReceivableChart = new Chart(dailyReceivableChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.weeklyIncomeChart = new Chart(weeklyIncomeChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.weeklyExpenseChart = new Chart(weeklyExpenseChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.weeklyPayableChart = new Chart(weeklyPayableChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.weeklyReceivableChart = new Chart(weeklyReceivableChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.monthlyIncomeChart = new Chart(monthlyIncomeChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.monthlyExpenseChart = new Chart(monthlyExpenseChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.monthlyPayableChart = new Chart(monthlyPayableChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});
window.monthlyReceivableChart = new Chart(monthlyReceivableChartElem, {
    type: "pie",
    options: {
        legend: {
            display: false,
            position: "bottom"
        }
    },
    data: {}
});

function getIncomeChartData(incomeReport) {
    return {
        datasets: [{
            data: [
                toFixedIfNecessary(incomeReport.debtReceived),
                toFixedIfNecessary(incomeReport.saleReceived),
                toFixedIfNecessary(incomeReport.purchaseReturnsReceived),
                toFixedIfNecessary(incomeReport.depositAmount)
            ],
            backgroundColor: ["#0093fd", "#4CAF50", "#9C27B0", "#FF5722"]
        }],

        labels: [
            "Debt",
            "Sale",
            "Purchase-Return",
            "Deposit"
        ]
    };
}
function getExpenseChartData(expenseReport) {
    return {
        datasets: [{
            data: [
                expenseReport.expenseAmount,
                expenseReport.purchasePaid,
                expenseReport.supplierPaymentAmount,
                expenseReport.employeePaymentAmount,
                expenseReport.refundAmount,
                expenseReport.withdrawalAmount],
            backgroundColor: ["#f44336", "#2196F3", "#8BC34A", "#FF9800", "#607D8B", "#9C27B0"]
        }],

        labels: [
            "Expense",
            "Purchase",
            "Supplier-Payment",
            "Employee Payment",
            "Refund",
            "Withdrawal"
        ]
    };
}
function getPayableChartData(payableReport) {
    return {
        datasets: [{
            data: [payableReport.purchaseDueAmount, payableReport.salaryIssueAmount, payableReport.debtOverPaymentAmount],
            backgroundColor: ["#0093fd", "#4CAF50", "#9C27B0"]
        }],

        labels: [
            "Purchases Due",
            "Salary-Issues",
            "Customer Advance"
        ]
    };
}
function getReceivableChartData(receivableReport) {
    return {
        datasets: [{
            data: [receivableReport.salesDueAmount, receivableReport.supplierOverPaymentAmount, receivableReport.salaryOverPaymentAmount],
            backgroundColor: ["#0093fd", "#4CAF50", "#9C27B0"]
        }],

        labels: [
            "Sales Due",
            "Supplier Advance",
            "Salary Advance"
        ]
    };
}

function setPeriodicalData(p, incomeData, expenseData, payableData, receivableData) {
    gei(p + "-income-total").innerHTML
        = formatter.format(incomeData.saleReceived
            + incomeData.debtReceived
            + incomeData.purchaseReturnsReceived
            + incomeData.depositAmount);
    gei(p + "-sales").innerHTML = incomeData.saleCount;
    gei(p + "-sale-receives").innerHTML = formatter.format(incomeData.saleReceived);
    gei(p + "-sale-profit").innerHTML = formatter.format(incomeData.saleProfit);
    gei(p + "-debt-receive-count").innerHTML = incomeData.debtPaymentCount;
    gei(p + "-debt-receive-amount").innerHTML = formatter.format(incomeData.debtReceived);
    gei(p + "-purchase-returns-count").innerHTML = incomeData.purchaseReturnsCount;
    gei(p + "-purchase-returns-amount").innerHTML = formatter.format(incomeData.purchaseReturnsReceived);
    gei(p + "-deposits-count").innerHTML = incomeData.depositsCount;
    gei(p + "-deposits-amount").innerHTML = formatter.format(incomeData.depositAmount);

    gei(p + "-expense-total").innerHTML
        = formatter.format(expenseData.supplierPaymentAmount
            + expenseData.expenseAmount
            + expenseData.purchasePaid
            + expenseData.employeePaymentAmount
            + expenseData.refundAmount
            + expenseData.withdrawalAmount);
    gei(p + "-expenses").innerHTML = expenseData.expenseCount;
    gei(p + "-expense-amount").innerHTML = formatter.format(expenseData.expenseAmount);
    gei(p + "-purchases").innerHTML = expenseData.purchaseCount;
    gei(p + "-purchase-paid").innerHTML = formatter.format(expenseData.purchasePaid);
    gei(p + "-supplier-payments").innerHTML = expenseData.supplierPaymentCount;
    gei(p + "-supplier-paid").innerHTML = formatter.format(expenseData.supplierPaymentAmount);
    gei(p + "-employee-payments").innerHTML = expenseData.employeePaymentCount;
    gei(p + "-employee-paid").innerHTML = formatter.format(expenseData.employeePaymentAmount);
    gei(p + "-refund-count").innerHTML = expenseData.refundCount;
    gei(p + "-refund-amount").innerHTML = formatter.format(expenseData.refundAmount);
    gei(p + "-withdrawals-count").innerHTML = expenseData.withdrawalCount;
    gei(p + "-withdrawals-amount").innerHTML = formatter.format(expenseData.withdrawalAmount);

    gei(p + "-payable-total").innerHTML
        = formatter.format(
            + payableData.purchaseDueAmount
            + payableData.salaryIssueAmount
            + payableData.debtOverPaymentAmount);
    gei(p + "-purchases-due-count").innerHTML = payableData.purchaseDueCount;
    gei(p + "-purchases-due-amount").innerHTML = formatter.format(payableData.purchaseDueAmount);
    gei(p + "-salary-issue-count").innerHTML = payableData.salaryIssueCount;
    gei(p + "-salary-issue-amount").innerHTML = formatter.format(payableData.salaryIssueAmount);
    gei(p + "-debt-over-payment-count").innerHTML = payableData.debtOverPaymentCount;
    gei(p + "-debt-over-payment-amount").innerHTML = formatter.format(payableData.debtOverPaymentAmount);

    gei(p + "-receivable-total").innerHTML
        = formatter.format(expenseData.supplierPaymentAmount
            + receivableData.salesDueAmount
            + receivableData.supplierOverPaymentAmount
            + receivableData.salaryOverPaymentAmount);
    gei(p + "-sales-due-count").innerHTML = receivableData.salesDueCount;
    gei(p + "-sales-due-amount").innerHTML = formatter.format(receivableData.salesDueAmount);
    gei(p + "-supplier-over-payment-count").innerHTML = receivableData.supplierOverPaymentCount;
    gei(p + "-supplier-over-payment-amount").innerHTML = formatter.format(receivableData.supplierOverPaymentAmount);
    gei(p + "-salary-over-payment-count").innerHTML = receivableData.salaryOverPaymentCount;
    gei(p + "-salary-over-payment-amount").innerHTML = formatter.format(receivableData.salaryOverPaymentAmount);
}
function updatePeriodicalReport() {
    var p = ["daily", "weekly", "monthly"];
    var incomes = [window.dailyIncomeData, window.weeklyIncomeData, window.monthlyIncomeData];
    var expenses = [window.dailyExpenseData, window.weeklyExpenseData, window.monthlyExpenseData];
    var payables = [window.dailyPayableData, window.weeklyPayableData, window.monthlyPayableData];
    var receivables = [window.dailyReceivableData, window.weeklyReceivableData, window.monthlyReceivableData];

    for (var i = 0; i < p.length; i++) {
        setPeriodicalData(p[i], incomes[i], expenses[i], payables[i], receivables[i]);
    }
}


function updateChart(data) {
    window.assetData = data.asset;
    updateAssetData();
    window.assetChart.data = getAssetChartData(data.asset);
    window.assetChart.update();

    window.liabilityData = data.liability;
    updateLiabilityData();
    window.liabilityChart.data = getLiabilityChartData(data.liability);
    window.liabilityChart.update();

    window.perDayData = data.perDayReports;
    window.perDayReportChart.data = getPerDayChartData(data.perDayReports);
    window.perDayReportChart.update();

    window.dailyIncomeData = data.income.daily;
    window.dailyExpenseData = data.expense.daily;
    window.dailyPayableData = data.payable.daily;
    window.dailyReceivableData = data.receivable.daily;
    window.weeklyIncomeData = data.income.weekly;
    window.weeklyExpenseData = data.expense.weekly;
    window.weeklyPayableData = data.payable.weekly;
    window.weeklyReceivableData = data.receivable.weekly;
    window.monthlyIncomeData = data.income.monthly;
    window.monthlyExpenseData = data.expense.monthly;
    window.monthlyPayableData = data.payable.monthly;
    window.monthlyReceivableData = data.receivable.monthly;
    window.dailyIncomeChart.data = getIncomeChartData(data.income.daily);
    window.dailyIncomeChart.update();
    window.dailyExpenseChart.data = getExpenseChartData(data.expense.daily);
    window.dailyExpenseChart.update();
    window.dailyPayableChart.data = getPayableChartData(data.payable.daily);
    window.dailyPayableChart.update();
    window.dailyReceivableChart.data = getReceivableChartData(data.receivable.daily);
    window.dailyReceivableChart.update();
    window.weeklyIncomeChart.data = getIncomeChartData(data.income.weekly);
    window.weeklyIncomeChart.update();
    window.weeklyExpenseChart.data = getExpenseChartData(data.expense.weekly);
    window.weeklyExpenseChart.update();
    window.weeklyPayableChart.data = getPayableChartData(data.payable.weekly);
    window.weeklyPayableChart.update();
    window.weeklyReceivableChart.data = getReceivableChartData(data.receivable.weekly);
    window.weeklyReceivableChart.update();
    window.monthlyIncomeChart.data = getIncomeChartData(data.income.monthly);
    window.monthlyIncomeChart.update();
    window.monthlyExpenseChart.data = getExpenseChartData(data.expense.monthly);
    window.monthlyExpenseChart.update();
    window.monthlyPayableChart.data = getPayableChartData(data.payable.monthly);
    window.monthlyPayableChart.update();
    window.monthlyReceivableChart.data = getReceivableChartData(data.receivable.monthly);
    window.monthlyReceivableChart.update();
    updatePeriodicalReport();
}

var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/Reports")
        .build();

$(document).ready(function () {
    connection.start().then(function () {
        connection.invoke("InitChartData");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("RefreshData", () => connection.invoke("RefreshData"));

    connection.on("UpdateChart", (chartData) => updateChart(chartData));
});