let currentPage = 1;
const pageSize = 10;

async function fetchMeetings(page = 1) {
    currentPage = page;  // Mevcut sayfayı güncelle
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
        console.error('Kullanıcı kimliği bulunamadı.');
        return;
    }
    try {
        const response = await fetch(`http://localhost:5064/api/Meetings?page=${page}&pageSize=${pageSize}`, {
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

        const userMeetings = data.meetings?.filter(meeting => meeting.userId === user.id);
        // Toplantıları tarihe göre sıralama
        userMeetings.sort((a, b) => new Date(a.startDate) - new Date(b.startDate));
        renderMeetings(userMeetings, user.id);
        renderPagination(data.totalPages, data.currentPage);
    } catch (error) {
        console.error('Hata:', error);
    }
}

function renderPagination(totalPages, currentPage) {
    const paginationContainer = document.getElementById('paginationContainer');
    paginationContainer.innerHTML = '';

    for (let page = 1; page <= totalPages; page++) {
        const pageItem = document.createElement('button');
        pageItem.classList.add('btn', 'btn-primary', 'm-1');
        pageItem.textContent = page;
        pageItem.disabled = (page === currentPage);

        pageItem.addEventListener('click', () => {
            fetchMeetings(page);
        });

        paginationContainer.appendChild(pageItem);
    }
}

function renderMeetings(meetings, currentUserId) {
    const meetingsBody = document.getElementById('meetingsBody2');
    meetingsBody.innerHTML = '';

    meetings.forEach((meeting) => {
        var row = document.createElement("tr");

        var nameCell = document.createElement("td");
        nameCell.textContent = meeting.name;
        nameCell.classList.add("text-capitalize", "name");
        row.appendChild(nameCell);

        var startDateCell = document.createElement("td");
        startDateCell.textContent = formatDate(meeting.startDate);
        startDateCell.classList.add("start-date");
        row.appendChild(startDateCell);

        var endDateCell = document.createElement("td");
        endDateCell.textContent = formatDate(meeting.endDate);
        endDateCell.classList.add("end-date");
        row.appendChild(endDateCell);

        var descriptionCell = document.createElement("td");
        descriptionCell.className = "description";

        var summary = document.createElement("div");
        summary.className = "description-summary2";
        if (meeting.description && meeting.description.length > 100) {
            summary.textContent = truncateDescription(meeting.description);
            summary.addEventListener("click", function () {
                this.classList.toggle("open");
                var fullDescription = this.nextElementSibling;
                fullDescription.classList.toggle("open");
            });
        } else {
            summary.textContent = meeting.description ? meeting.description : "-";
        }

        var fullDescription = document.createElement("div");
        fullDescription.className = "description-full2";
        fullDescription.textContent = meeting.description ? meeting.description : "-";

        descriptionCell.appendChild(summary);
        descriptionCell.appendChild(fullDescription);
        row.appendChild(descriptionCell);

        var documentCell = document.createElement("td");
        documentCell.className = "document";

        if (meeting.documentPath) {
            var link = document.createElement("a");
            link.href = meeting.documentPath;
            link.textContent = "İndir";
            link.className = "document-link";
            link.setAttribute("data-user-id", meeting.userId);
            if (meeting.userId !== currentUserId) {
                link.style.pointerEvents = "none";
                link.style.color = "#ccc";
            }
            documentCell.appendChild(link);
        } else {
            documentCell.textContent = "-";
        }

        row.appendChild(documentCell);

        var actionsCell = document.createElement("td");
        actionsCell.className = "actions";
        actionsCell.style.display = "flex";
        actionsCell.style.justifyContent = "center";

        if (meeting.userId === currentUserId) {
            var deleteButton = document.createElement("button");
            deleteButton.innerHTML = '<i class="fas fa-trash delete"></i>';
            deleteButton.className = "btn btn-outline-danger btn-sm m-1 delete tooltip-button";
            deleteButton.style.fontSize = "0.5rem";
            deleteButton.setAttribute("data-id", meeting.id);
            deleteButton.setAttribute("data-tooltip", "Sil");
            deleteButton.onclick = () => deleteMeeting(meeting.id);
            actionsCell.appendChild(deleteButton);

            var editButton = document.createElement("button");
            editButton.innerHTML = '<i class="fas fa-edit edit"></i>';
            editButton.className = "btn btn-outline-primary btn-sm edit m-1 tooltip-button";
            editButton.style.fontSize = "0.5rem";
            editButton.setAttribute("data-id", meeting.id);
            editButton.setAttribute("data-tooltip", "Düzenle");
            editButton.onclick = () => editMeeting(meeting.id);
            actionsCell.appendChild(editButton);

            var emailButton = document.createElement("button");
            emailButton.innerHTML = '<i class="fas fa-envelope email"></i>';
            emailButton.className = "btn btn-outline-info btn-sm m-1 email tooltip-button";
            emailButton.style.fontSize = "0.5rem";
            emailButton.setAttribute("data-id", meeting.id);
            emailButton.setAttribute("data-tooltip", "Email Gönder");
            emailButton.onclick = () => openEmailModal(meeting);
            actionsCell.appendChild(emailButton);

            var raporButton = document.createElement("button");
            raporButton.innerHTML = '<i class="fas fa-file-alt"></i>';
            raporButton.className = "btn btn-outline-info btn-sm m-1 email tooltip-button";
            raporButton.style.fontSize = "0.5rem";
            raporButton.setAttribute("data-id", meeting.id);
            raporButton.setAttribute("data-tooltip", "Rapor Oluştur");
            raporButton.onclick = () => createMeetingReport(meeting.id);
            actionsCell.appendChild(raporButton);
        }

        row.appendChild(actionsCell);
        meetingsBody.appendChild(row);
    });

    var tooltipButtons = document.querySelectorAll(".tooltip-button");
    tooltipButtons.forEach((button) => {
        var tooltipText = button.getAttribute("data-tooltip");
        var tooltipSpan = document.createElement("span");
        tooltipSpan.className = "tooltip-text";
        tooltipSpan.textContent = tooltipText;
        button.appendChild(tooltipSpan);
    });
}

function openEmailModal(meeting) {
    selectedMeeting = meeting
    $('#emailModal').modal('show'); 
}

function closeEmailModal() {
    $('#emailModal').modal('hide'); 
}

async function sendEmailNotification() {
    const recipients = document.getElementById('emailRecipients').value.split(',').map(email => email.trim());
    if (recipients.length === 0) {
        alert('Lütfen en az bir e-posta adresi giriniz.');
        return;
    }

    const sendEmailButton = document.getElementById('sendEmailButton');
    sendEmailButton.disabled = true;
    sendEmailButton.textContent = 'Gönderiliyor...';

    const emailSubject = document.getElementById('emailSubject').value;
    const emailBody = document.getElementById('emailBody').value;

    try {
        const response = await fetch('http://localhost:5064/api/Meetings/SendEmailNotification', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                meetingId: selectedMeeting.id,
                recipients: recipients,
                subject: emailSubject,
                body: emailBody
            })
        });

        if (!response.ok) {
            throw new Error('E-posta gönderilemedi.');
        }

        alert('E-posta başarıyla gönderildi.');
        closeEmailModal();
    } catch (error) {
        console.error('Hata:', error);
        alert('E-posta gönderilirken bir hata oluştu.');
    } finally {
        sendEmailButton.disabled = false;
        sendEmailButton.textContent = 'Gönder';
    }
}

function formatDate(date) {
    const options = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return new Date(date).toLocaleDateString('tr-TR', options);
}

function truncateDescription(description) {
    return description.length > 100 ? description.substring(0, 100) + '...' : description;
}

fetchMeetings();
