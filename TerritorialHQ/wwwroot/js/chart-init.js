const ctx = document.getElementById('myChart');


var chartRequest = $.ajax({
    url: ("/Clans/Details/?handler=ChartData&clan=" + ctx.dataset.clan),
    type: 'GET',
    cache: true,
    success: function (cdata) {
        console.log(cdata);

        Chart.defaults.borderColor = '#4E431E';
        Chart.defaults.color = '#EACF69';

        new Chart(
            document.getElementById('myChart'),
            {
                type: 'line',
                data: {
                    labels: cdata.map(row => row.date),
                    datasets: [
                        {
                            label: 'Points',
                            data: cdata.map(row => row.value),
                            borderColor: '#A58728',
                            backgroundColor: '#EACF69',
                            pointRadius: 0,
                            hitRadius: 5
                        }
                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: false
                        }
                    },
                    plugins: {
                        legend: {
                            display: false
                        },
                        title: {
                            display: true,
                            text: '[' + ctx.dataset.clan + ']'
                        }
                    }
                }
            }
        );
    },
    error: function (a, b, c) {

    }
});



