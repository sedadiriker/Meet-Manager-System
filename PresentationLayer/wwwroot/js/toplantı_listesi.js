document.addEventListener('DOMContentLoaded', function () {
    var token = localStorage.getItem('token'); // veya başka bir yerden token'ı alın

    fetch('http://localhost:5064/api/Meetings', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Sunucudan hata yanıtı alındı: ' + response.statusText);
        }
        return response.json();
    })
    .then(data => {
        var tbody = document.getElementById('meetingsBody');
        tbody.innerHTML = ''; // Önceden var olan içeriği temizle

        data.forEach(meeting => {
            var row = document.createElement('tr');

            var nameCell = document.createElement('td');
            nameCell.textContent = meeting.name;
            row.appendChild(nameCell);

            var startDateCell = document.createElement('td');
            startDateCell.textContent = formatDate(meeting.startDate);
            row.appendChild(startDateCell);

            var endDateCell = document.createElement('td');
            endDateCell.textContent = formatDate(meeting.endDate);
            row.appendChild(endDateCell);

            var descriptionCell = document.createElement('td');
            descriptionCell.textContent = meeting.description;
            row.appendChild(descriptionCell);

            var documentCell = document.createElement('td');
            if (meeting.documentPath) {
                var link = document.createElement('a');
                link.href = meeting.documentPath;
                link.target = '_blank';
                link.textContent = 'Döküman';
                documentCell.appendChild(link);
            } else {
                documentCell.textContent = 'Yok';
            }
            row.appendChild(documentCell);

            tbody.appendChild(row);
        });
    })
    .catch(error => {
        console.error('Veriler alınırken bir hata oluştu:', error);
    });

    function formatDate(dateString) {
        var date = new Date(dateString);
        var hours = date.getHours().toString().padStart(2, '0');
        var minutes = date.getMinutes().toString().padStart(2, '0');
        return `${hours}:${minutes}`;
    }
});
