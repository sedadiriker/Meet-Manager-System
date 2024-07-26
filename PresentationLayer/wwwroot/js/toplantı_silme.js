document.addEventListener('DOMContentLoaded', function () {
    var token = localStorage.getItem('token'); 
    var deleteMeetingId = null; 
    var commonModal = new bootstrap.Modal(document.getElementById('commonModal')); 
    var commonModalTitle = document.getElementById('commonModalTitle');
    var commonModalBody = document.getElementById('commonModalBody');
    var commonModalConfirmButton = document.getElementById('commonModalConfirmButton');

    // Toplantıyı silme fonksiyonu
    window.deleteMeeting = function (meetingId) {
        fetch(`http://localhost:5064/api/Meetings/${meetingId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        })
        .then(response => {
            if (response.ok) {
                alert('Toplantı başarıyla silindi.');
                location.reload(); 
            } else {
                throw new Error('Sunucudan hata yanıtı alındı: ' + response.statusText);
            }
        })
        .catch(error => {
            console.error('Silme işlemi sırasında bir hata oluştu:', error);
            alert('Silme işlemi sırasında bir hata oluştu.');
        });
    };

    commonModalConfirmButton.addEventListener('click', function () {
        if (deleteMeetingId !== null) {
            window.deleteMeeting(deleteMeetingId); 
            commonModal.hide(); 
        }
    });

    document.addEventListener('click', function (event) {
        if (event.target.classList.contains('btn-outline-danger')) {
            deleteMeetingId = event.target.getAttribute('data-id'); 
            commonModalTitle.textContent = 'Toplantı Silme';
            commonModalBody.textContent = 'Bu toplantıyı silmek istediğinizden emin misiniz?';
            commonModalConfirmButton.textContent = 'Sil';

            commonModal.show(); 
        }
    });
});
