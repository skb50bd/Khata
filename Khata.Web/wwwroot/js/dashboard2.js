$(document).ready(function () {


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
    const app = new Vue({
        el: "#app",
        data: {
            title: "Dashboard",
            perDayReportChart: new Chart(
                document.getElementById("perdayreport-chart"),
                pdrcc
            ),
            assetReportChart: new Chart(
                document.getElementById("assetChart"),
                {
                    type: 'pie',
                    data: {}
                }),
            liabilityReportChart: new Chart(
                document.getElementById("liabilityChart"),
                {
                    type: 'pie',
                    data: {}
                })
        },
        methods: {

            async getAssetData(assetReport) {
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
            },

            async getLiabilityData(liabilityReport) {
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
            },

            async getPerDayData(perDayData) {
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
            },

            async updateChart(chartData) {
                this.assetReportChart.data = await this.getAssetData(chartData.asset);
                this.assetReportChart.update();

                this.liabilityReportChart.data = await this.getLiabilityData(chartData.liability);
                this.liabilityReportChart.update();

                this.perDayReportChart.data = await this.getPerDayData(chartData.perDayReports);
                this.perDayReportChart.update();
            }
        }
    });

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/Reports")
        .build();
    connection.start().then(function () {
        connection.invoke("InitChartData");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("RefreshData", () => connection.invoke("RefreshData"));

    connection.on("UpdateChart", (chartData) => app.updateChart(chartData));
});