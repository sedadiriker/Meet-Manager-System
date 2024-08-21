document.addEventListener('DOMContentLoaded', function () {
    var scrollToTopBtn = document.querySelector('.scroll-to-top');

    window.addEventListener('scroll', function () {
        if (window.scrollY > 100) { 
            scrollToTopBtn.style.display = 'block';
        } else {
            scrollToTopBtn.style.display = 'none';
        }
    });

    scrollToTopBtn.addEventListener('click', function (e) {
        e.preventDefault();
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
});

// Yaklaşan Toplantılar 
async function fetchUpcomingMeetings() {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
        console.error('Kullanıcı kimliği bulunamadı.');
        return;
    }

    try {
        const response = await fetch(`http://localhost:5064/api/meetings/`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Toplantılar alınamadı.');
        }

        const data = await response.json();
        console.log("data",data); 

        const userMeetings = data.meetings?.filter(meeting => meeting.userId === user.id);

        const now = new Date();
        const fiveDaysLater = new Date();
        fiveDaysLater.setDate(now.getDate() + 5);

        console.log("Şu An:", now);
        console.log("5 Gün Sonra:", fiveDaysLater);

        const filteredMeetings = userMeetings.filter(meeting => {
            const meetingStartDate = new Date(meeting.startDate);
            return meetingStartDate >= now && meetingStartDate <= fiveDaysLater;
        });

        filteredMeetings.sort((a, b) => new Date(a.startDate) - new Date(b.startDate));

        renderUpcomingMeetings(filteredMeetings);
    } catch (error) {
        console.error('Hata:', error);
    }
}

function renderUpcomingMeetings(meetings) {
    const listGroup = document.querySelector('.yaklasan-toplantilar');
    const noMeetingsMessage = document.querySelector('#noMeetingsMessage');
    
    listGroup.innerHTML = ''; 
    
    if (meetings.length === 0) {
        noMeetingsMessage.style.display = 'block'; 
    } else {
        noMeetingsMessage.style.display = 'none'; 
        meetings.slice(0, 5).forEach(meeting => {
            const listItem = document.createElement('li');
            listItem.className = 'custom-list-item';
            const name = meeting.name;
            const firstLetter = name.charAt(0).toUpperCase();
            const restOfName = name.slice(1);

            listItem.innerHTML = `<span class="first-letter">${firstLetter}</span>${restOfName} - ${formatDate(meeting.startDate)}`;
            listGroup.appendChild(listItem);
        });
    }
}

function formatDate(date) {
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    return new Date(date).toLocaleDateString('tr-TR', options);
}

fetchUpcomingMeetings();

// Son Toplantılar


