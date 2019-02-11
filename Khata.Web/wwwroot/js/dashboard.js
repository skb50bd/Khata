"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Reports").build();

function getAssetData(assetReport) {
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

function getLiabilityData(liabilityReport) {
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

function getIncomeData(incomeReport) {
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

function getExpenseData(expenseReport) {
    return {
        datasets: [{
            data: [incomeReport.debtReceived, incomeReport.saleReceived, incomeReport.depositAmount],
            backgroundColor: ['#0093fd', '#fd6900', 'green', 'yello']
        }],

        labels: [
            'Purchase',
            'Supplier Payment',
            'Employee Payment',
            'Expense'
        ]
    };
}

function getPerDayData(perDayData) {
    return {
        datasets: [
            {
                label: "Cash In",
                backgroundColor: "#66BB6A",
                borderColor: "#388E3C",
                data: perDayData.map(d => d.cashIn)
            },
            {
                label: "Cash Out",
                backgroundColor: "#ef5350",
                borderColor: "#d32f2f",
                data: perDayData.map(d => d.cashOut)
            }, {
                label: "New Due",
                backgroundColor: "#42A5F5",
                borderColor: "#1976D2",
                data: perDayData.map(d => d.newReceivable)
            }, {
                label: "New Payable",
                backgroundColor: "#FFA726",
                borderColor: "#F57C00",
                data: perDayData.map(d => d.newPayable)
            }],

        labels: perDayData.map((d) => d.date)
    };
}

const assetChartElem = document.getElementById("assetChart");
window.assetChart = new Chart(assetChartElem, {
    type: 'pie',
    data: {}
});

const liabilityChartElem = document.getElementById("liabilityChart");
window.liabilityChart =  new Chart(liabilityChartElem, {
    type: 'pie',
    data: {}
});

var dailyIncomeChart;
const dailyIncomeChartElem = document.getElementById("dailyIncomeChart");

var weeklyIncomeChart;
const weeklyIncomeChartElem = document.getElementById("weeklyIncomeChart");

var monthlyIncomeChart;
const monthlyIncomeChartElem = document.getElementById("monthlyIncomeChart");

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


function updateChart(chartData) {
    window.assetChart.data = getAssetData(chartData.asset);
    window.assetChart.update();

    window.liabilityChart.data = getLiabilityData(chartData.liability);
    window.liabilityChart.update();

    window.perDayReportChart.data = getPerDayData(chartData.perDayReports);
    window.perDayReportChart.update();
}

$(document).ready(function () {
    connection.start().then(function () {
        connection.invoke("InitChartData");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("RefreshData", () => connection.invoke("RefreshData"));

    connection.on("UpdateChart", (chartData) => updateChart(chartData));
});