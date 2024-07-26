document.addEventListener('DOMContentLoaded', function() {
    var commonModalTitle = document.getElementById('commonModalTitle');
    var commonModalBody = document.getElementById('commonModalBody');
    var commonModalConfirmButton = document.getElementById('commonModalConfirmButton');
    var commonModal = new bootstrap.Modal(document.getElementById('commonModal'));

    document.getElementById('logoutButton').addEventListener('click', function() {
        commonModalTitle.textContent = 'Çıkış Yap';
        commonModalBody.textContent = 'Oturumunuzu sonlandırmak istediğinizden emin misiniz?';
        commonModalConfirmButton.textContent = 'Çıkış Yap';

        commonModalConfirmButton.onclick = function() {
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            window.location.href = '/'; 
        };

        commonModal.show();
    });
});
