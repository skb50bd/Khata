"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Reports").build();

function getAssetData(assetReport) {
    return {
        datasets: [{
            data: [assetReport.totalDue, assetReport.cash, assetReport.inventoryWorth],
            backgroundColor: ['#6200EE', '#FF1744', '#0091EA']
        }],

        labels: [
            'Cash',
            'Due',
            'Inventory'
        ]
    };
}

function getLiabilityData(liabilityReport) {
    return {
        datasets: [{
            data: [liabilityReport.totalDue, liabilityReport.unpaidAmount],
            backgroundColor: ['#0093fd', '#fd6900']
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

var assetChart;
var assetChartElem = document.getElementById("assetChart");

var liabilityChart;
var liabilityChartElem = document.getElementById("liabilityChart");

var dailyIncomeChart;
var dailyIncomeChartElem = document.getElementById("dailyIncomeChart");

var weeklyIncomeChart;
var weeklyIncomeChartElem = document.getElementById("weeklyIncomeChart");

var monthlyIncomeChart;
var monthlyIncomeChartElem = document.getElementById("monthlyIncomeChart");

function updateChart(chartData) {
    assetChart = new Chart(assetChartElem, {
        type: 'pie',
        data: getAssetData(chartData.asset)
    });
    
    liabilityChart = new Chart(liabilityChartElem, {
        type: 'pie',
        data: getLiabilityData(chartData.liability)
    });

    dailyIncomeChart = new Chart(dailyIncomeChartElem, {
        type: 'pie',
        data: getIncomeData(chartData.income.daily)
    });

    weeklyIncomeChart = new Chart(weeklyIncomeChartElem, {
        type: 'pie',
        data: getIncomeData(chartData.income.weekly)
    });

    monthlyIncomeChart = new Chart(monthlyIncomeChartElem, {
        type: 'pie',
        data: getIncomeData(chartData.income.monthly)
    });
}

$(document).ready(function () {
    connection.start().then(function () {
            connection.invoke("InitChartData");
        }).catch(function (err) {
            return console.error(err.toString());
        });

    connection.on("UpdateChart", (assetData) => updateChart(assetData));
});