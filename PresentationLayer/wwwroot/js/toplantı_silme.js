document.addEventListener('DOMContentLoaded', function () {
    var token = localStorage.getItem('token'); 
    var deleteMeetingId = null; 
    var commonModal = new bootstrap.Modal(document.getElementById('commonModal')); 
    var commonModalTitle = document.getElementById('commonModalTitle');
    var commonModalBody = document.getElementById('commonModalBody');
    var commonModalConfirmButton = document.getElementById('commonModalConfirmButton');

    document.addEventListener('click', function (event) {
        if (event.target.classList.contains('delete')) {
            var button = event.target.closest('button');
            var meetingId = button.getAttribute('data-id'); // 'target' kullanımı yanlış
            console.log(meetingId);
            deleteMeetingId = meetingId; // Burada deleteMeetingId'yi ayarlıyoruz

            // Modal içeriğini ayarla
            commonModalTitle.textContent = 'Toplantı Silme';
            commonModalBody.textContent = 'Bu toplantıyı silmek istediğinizden emin misiniz?';
            commonModalConfirmButton.textContent = 'Sil';

            commonModal.show(); // Modal'ı göster
        }
    });

    commonModalConfirmButton.addEventListener('click', function () {
        if (deleteMeetingId !== null) {
            deleteMeeting(deleteMeetingId); // deleteMeetingId'yi kullanarak toplantıyı sil
            commonModal.hide(); // Modal'ı gizle
        }
    });

    function deleteMeeting(meetingId) {
        fetch(`http://localhost:5064/api/Meetings/${meetingId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        })
        .then(response => {
            if (response.ok) {
                setTimeout(() => {
                    location.reload(); 
                }, 2000);
                toastr["success"]('Toplantı başarıyla silindi.');
            } else {
                throw new Error('Sunucudan hata yanıtı alındı: ' + response.statusText);
            }
        })
        .catch(error => {
            console.error('Silme işlemi sırasında bir hata oluştu:', error);
            toastr["error"]('Silme işlemi sırasında bir hata oluştu.');
        });
    }
});
