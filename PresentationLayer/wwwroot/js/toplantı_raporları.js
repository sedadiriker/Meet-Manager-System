//toplantı_raporları.js
async function fetchReports() {
    try {
        const response = await fetch('http://localhost:5064/api/MeetingReports', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`, 
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Raporlar alınamadı.');
        }

        const reports = await response.json();
        renderReports(reports);
    } catch (error) {
        console.error('Raporları alma hatası:', error);
    }
}

function renderReports(reports) {
    const reportsList = document.getElementById('reportsList');
    reportsList.innerHTML = '';

    reports.forEach(report => {
        var listItem = document.createElement('li');
        var link = document.createElement('a');
        link.href = report.reportUrl;
        link.textContent = `Toplantı ${report.meeting.name} - ${new Date(report.createdAt).toLocaleDateString()}`;
        link.download = `${report.meeting.name}_meeting_report.pdf`;
        listItem.appendChild(link);
        reportsList.appendChild(listItem);
    });
}

document.addEventListener('DOMContentLoaded', fetchReports);

