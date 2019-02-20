// Assets
const assetChartElem = document.getElementById("assetChart");
window.assetData = null;
window.assetChart = new Chart(assetChartElem, {
    type: 'pie',
    data: {}
});
function getAssetChartData(assetReport) {
    return {
        datasets: [{
            data: [assetReport.totalDue, assetReport.cash, assetReport.inventoryWorth],
            backgroundColor: ['#ef5350', '#66BB6A', '#7E57C2']
        }],

        labels: [
            'Due',
            'Cash',
            'Inventory'
        ]
    };
}
function updateAssetData() {
    var data = window.assetData;
    document.getElementById('asset-total').innerHTML =
        formatter.format(data.cash + data.inventoryWorth + data.totalDue);

    document.getElementById('customers-with-due').innerHTML = data.dueCount;
    document.getElementById('due-amount').innerHTML = formatter.format(data.totalDue);
    document.getElementById('cash-in-hand').innerHTML = formatter.format(data.cash);
    document.getElementById('products-in-stock').innerHTML = data.inventoryCount;
    document.getElementById('products-total-cost').innerHTML = formatter.format(data.inventoryWorth);
}

// Liabilities
const liabilityChartElem = document.getElementById("liabilityChart");
window.liabilityChart = new Chart(liabilityChartElem, {
    type: 'pie',
    data: {}
});
window.liabilityData = null;
function getLiabilityChartData(liabilityReport) {
    return {
        datasets: [{
            data: [liabilityReport.totalDue, liabilityReport.unpaidAmount],
            backgroundColor: ['#42A5F5', '#FF7043']
        }],

        labels: [
            'Supplier Due',
            'Employee Salary'
        ]
    };
}
function updateLiabilityData() {
    var data = window.liabilityData;
    document.getElementById('liability-total').innerHTML =
        formatter.format(data.totalDue + data.unpaidAmount);
    document.getElementById('suppliers-with-due').innerHTML = data.dueCount;
    document.getElementById('total-supplier-due').innerHTML = formatter.format(data.totalDue);
    document.getElementById('unpaid-employees').innerHTML = data.unpaidEmployees;
    document.getElementById('unpaid-amount').innerHTML = formatter.format(data.unpaidAmount);
}

// Per Day Report
const perDayReportChartElem = document.getElementById("perdayreport-chart");
var pdrcc = {
    type: 'line',
    data: {},
    options: {
        tooltips: {
            mode: 'index'
        },
        fill: false,
        responsive: true,
        title: {
            display: false,
            text: 'Overview'
        },
        hover: {
            mode: 'index'
        },
        scales: {
            xAxes: [{
                type: 'time',
                time: {
                    unit: 'week',
                    tooltipFormat: 'DD MMM YYYY'
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Date'
                }
            }],
            yAxes: [{
                stacked: false,
                scaleLabel: {
                    display: true,
                    labelString: 'Amount'
                }
            }]
        }
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
                backgroundColor: "rgba(33, 150, 243 , 0.8)",
                borderColor: "rgba(33, 150, 243 , 0.8)",
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


const dailyIncomeChartElem = document.getElementById("daily-income-chart");
const dailyExpenseChartElem = document.getElementById("daily-expense-chart");
const weeklyIncomeChartElem = document.getElementById("weekly-income-chart");
const weeklyExpenseChartElem = document.getElementById("weekly-expense-chart");
const monthlyIncomeChartElem = document.getElementById("monthly-income-chart");
const monthlyExpenseChartElem = document.getElementById("monthly-expense-chart");

window.dailyIncomeData = null;
window.dailyExpenseData = null;
window.weeklyIncomeData = null;
window.weeklyExpenseData = null;
window.monthlyIncomeData = null;
window.monthlyExpenseData = null;

window.dailyIncomeChart = new Chart(dailyIncomeChartElem, {
    type: 'pie',
    data: {}
});
window.dailyExpenseChart = new Chart(dailyExpenseChartElem, {
    type: 'pie',
    data: {}
});
window.weeklyIncomeChart = new Chart(weeklyIncomeChartElem, {
    type: 'pie',
    data: {}
});
window.weeklyExpenseChart = new Chart(weeklyExpenseChartElem, {
    type: 'pie',
    data: {}
});
window.monthlyIncomeChart = new Chart(monthlyIncomeChartElem, {
    type: 'pie',
    data: {}
});
window.monthlyExpenseChart = new Chart(monthlyExpenseChartElem, {
    type: 'pie',
    data: {}
});

function getIncomeChartData(incomeReport) {
    return {
        datasets: [{
            data: [incomeReport.debtReceived, incomeReport.saleReceived, incomeReport.depositAmount],
            backgroundColor: ['#0093fd', '#fd6900', 'green']
        }],

        labels: [
            'Debt',
            'Sale',
            'Deposit'
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
            backgroundColor: ['#f44336', '#2196F3', '#8BC34A', '#FF9800', '#607D8B', '#9C27B0']
        }],

        labels: [
            'Expense',
            'Purchase',
            'Supplier Payment',
            'Employee Payment',
            'Refund',
            'Withdrawal'
        ]
    };
}

function setPeriodicalData(p, incomeData, expenseData) {
    document.getElementById(p + '-income-total').innerHTML
        = formatter.format(incomeData.saleReceived
            + incomeData.debtReceived
            + incomeData.depositAmount);
    document.getElementById(p + '-sales').innerHTML = incomeData.saleCount;
    document.getElementById(p + '-sale-receives').innerHTML = formatter.format(incomeData.saleReceived);
    document.getElementById(p + '-debt-receive-count').innerHTML = incomeData.debtPaymentCount;
    document.getElementById(p + '-debt-receive-amount').innerHTML = formatter.format(incomeData.debtReceived);
    document.getElementById(p + '-deposits-count').innerHTML = incomeData.depositsCount;
    document.getElementById(p + '-deposits-amount').innerHTML = formatter.format(incomeData.depositAmount);
    
    document.getElementById(p + 'expense-total').innerHTML
        = formatter.format(expenseData.supplierPaymentAmount
            + expenseData.purchasePaid
            + expenseData.employeePaymentAmount
            + expenseData.refundAmount
            + expenseData.withdrawalAmount);
    document.getElementById(p + '-purchases').innerHTML = expenseData.purchaseCount;
    document.getElementById(p + '-purchase-paid').innerHTML = formatter.format(expenseData.purchasePaid);
    document.getElementById(p + '-supplier-payments').innerHTML = expenseData.supplierPaymentCount;
    document.getElementById(p + '-supplier-paid').innerHTML = formatter.format(expenseData.supplierPaymentAmount);
    document.getElementById(p + '-employee-payments').innerHTML = expenseData.employeePaymentCount;
    document.getElementById(p + '-employee-paid').innerHTML = formatter.format(expenseData.employeePaymentAmount);
}
function updatePeriodicalReport() {
    var p = ['daily', 'weekly', 'monthly'];
    var incomes = [window.dailyIncomeData, window.weeklyIncomeData, window.monthlyIncomeData];
    var expenses = [window.dailyExpenseData, window.weeklyExpenseData, window.monthlyExpenseData];

    for (var i = 0; i < p.length; i++) {
        setPeriodicalData(p[i], incomes[i], expenses[i]);
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
    window.weeklyIncomeData = data.income.weekly;
    window.weeklyExpenseData = data.expense.weekly;
    window.monthlyIncomeData = data.income.monthly;
    window.monthlyExpenseData = data.expense.monthly;
    window.dailyIncomeChart.data = getIncomeChartData(data.income.daily);
    window.dailyIncomeChart.update();
    window.dailyExpenseChart.data = getExpenseChartData(data.expense.daily);
    window.dailyExpenseChart.update();
    window.weeklyIncomeChart.data = getIncomeChartData(data.income.weekly);
    window.weeklyIncomeChart.update();
    window.weeklyExpenseChart.data = getExpenseChartData(data.expense.weekly);
    window.weeklyExpenseChart.update();
    window.monthlyIncomeChart.data = getIncomeChartData(data.income.monthly);
    window.monthlyIncomeChart.update();
    window.monthlyExpenseChart.data = getExpenseChartData(data.expense.monthly);
    window.monthlyExpenseChart.update();
    updatePeriodicalReport();
}

var connection = new signalR.HubConnectionBuilder().withUrl("/Reports").build();
$(document).ready(function () {
    connection.start().then(function () {
        connection.invoke("InitChartData");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("RefreshData", () => connection.invoke("RefreshData"));

    connection.on("UpdateChart", (chartData) => updateChart(chartData));
});