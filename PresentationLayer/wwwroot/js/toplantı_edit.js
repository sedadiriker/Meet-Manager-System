document.addEventListener("DOMContentLoaded", function () {
    var token = localStorage.getItem("token");

    // Toplantıyı düzenleme butonlarına tıklama
    document.addEventListener('click', function (event) {
        if (event.target.classList.contains('edit')) {
            var meetingId = event.target.getAttribute('data-id');
            fetchMeetingDetails(meetingId);
        }
    });

    // Toplantı detaylarını al ve formu doldur
    function fetchMeetingDetails(meetingId) {
        fetch(`http://localhost:5064/api/Meetings/${meetingId}`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        })
        .then(response => response.json())
        .then(meeting => {
            console.log(meeting); // Debug: meeting objesinin içeriğini kontrol edin

            // Element ID'lerinin doğru olduğundan emin olun
            document.getElementById('editMeetingId').value = meeting.id || '';
            document.getElementById('editName').value = meeting.name || '';
            document.getElementById('editStartDate').value = meeting.startDate || '';
            document.getElementById('editEndDate').value = meeting.endDate || '';
            document.getElementById('editDescription').value = meeting.description || '';
            document.getElementById('currentDocument').textContent = meeting.documentPath ? `Mevcut döküman: ${meeting.documentPath}` : "Mevcut döküman yok";

            var editMeetingModal = new bootstrap.Modal(document.getElementById('editMeetingModal'));
            editMeetingModal.show();
        })
        .catch(error => {
            console.error('Toplantı detayları alınırken bir hata oluştu:', error);
            alert('Toplantı detayları alınırken bir hata oluştu.');
        });
    }

    // Formu gönderme işlemi
    document.getElementById('editMeetingForm').addEventListener('submit', function (event) {
        event.preventDefault();

        var formData = new FormData(this);
        var meetingId = document.getElementById('editMeetingId').value;

        fetch(`http://localhost:5064/api/Meetings/${meetingId}`, {
            method: 'PUT',
            headers: {
                'Authorization': 'Bearer ' + token
            },
            body: formData
        })
        .then(response => {
            if (response.ok) {
                alert('Toplantı başarıyla güncellendi.');
                location.reload();
            } else {
                throw new Error('Sunucudan hata yanıtı alındı: ' + response.statusText);
            }
        })
        .catch(error => {
            console.error('Toplantı güncellenirken bir hata oluştu:', error);
            alert('Toplantı güncellenirken bir hata oluştu.');
        });
    });
});
