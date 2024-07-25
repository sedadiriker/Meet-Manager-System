document.addEventListener('DOMContentLoaded', function() {
    var ctx = document.getElementById('meetingTrendChart').getContext('2d');
    var meetingTrendChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['1 Gün', '2 Gün', '3 Gün', '4 Gün', '5 Gün', '6 Gün', '7 Gün'],
            datasets: [{
                label: 'Toplantı Sayısı',
                data: [5, 6, 7, 8, 9, 10, 11],
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true
                }
            }
        }
    });
});