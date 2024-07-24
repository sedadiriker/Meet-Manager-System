document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth', // Aylık takvim görünümü
        headerToolbar: false,        // Başlık ve araç çubuğu gizli
        events: [
            {
                title: 'Toplantı 1',
                start: '2024-07-23T10:00:00',
                end: '2024-07-23T12:00:00'
            },
            {
                title: 'Toplantı 2',
                start: '2024-07-25T14:00:00',
                end: '2024-07-25T16:00:00'
            }
        ]
    });
    calendar.render();
});
