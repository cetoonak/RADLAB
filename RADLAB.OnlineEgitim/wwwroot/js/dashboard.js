function ViewChartGrafik(Rotate, DonemValues, BasvurulanValues, TeslimEdilenValues) {
    var options = {
        series: [{
            name: 'Başvurulan',
            data: BasvurulanValues,
        },
        {
            name: 'TeslimEdilen',
            data: TeslimEdilenValues
        }],
        chart: {
            height: 250,
            type: 'area',
            toolbar: {
                show: false
            },
        },
        markers: {
            size: 4
        },
        colors: ['#4154f1', '#2eca6a'],
        fill: {
            type: "gradient",
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.3,
                opacityTo: 0.4,
                stops: [0, 90, 100]
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        xaxis: {
            type: 'number',
            categories: DonemValues,
            labels: {
                rotate: -90,
                rotateAlways: Rotate,
            }
        },
        tooltip: {
            x: {
                format: '####-#'
            },
        }
    };

    var chart = new ApexCharts(document.querySelector("#ChartGrafik"), options);
    chart.render();
    chart.resetSeries();
}

/*function ViewChartGrafik(DonemValues, BasvurulanValues, OlumluValues, OlumsuzValues, DevamEdenValues) {
    var options = {
        series: [{
            name: 'Başvurulan',
            data: BasvurulanValues,
        },
        {
            name: 'Olumlu Sonuçlanan',
            data: OlumluValues
        }, {
            name: 'Olumsuz Sonuçlanan',
            data: OlumsuzValues
        }, {
            name: 'Devam Eden',
            data: DevamEdenValues
        }],
        chart: {
            height: 250,
            type: 'area',
            toolbar: {
                show: false
            },
        },
        markers: {
            size: 4
        },
        colors: ['#4154f1', '#2eca6a', 'red', 'coral'],
        fill: {
            type: "gradient",
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.3,
                opacityTo: 0.4,
                stops: [0, 90, 100]
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        xaxis: {
            type: 'number',
            categories: DonemValues
        },
        tooltip: {
            x: {
                format: '####-#'
            },
        }
    };

    var chart = new ApexCharts(document.querySelector("#ChartGrafik"), options);
    chart.render();
    chart.resetSeries();
}*/