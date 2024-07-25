document.addEventListener("DOMContentLoaded",  () => {
    const form = document.getElementById('createMeetingForm');

    form.addEventListener('submit', (e) => {
        e.preventDefault()

        const formData = new FormData(form);

        const URL = "http://localhost:5064/api/Meetings";

        axios.post(URL, formData,{
            headers: {
                'Content-Type': 'multipart/form-data' 

            }
        })
            .then(response => {
                window.location.href = '/toplantı_listesi';
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Bir hata oluştu.');
            });
    });
});