// gecmis-toplantılar.js
let currentPagePast = 1;
const pageSizePast = 10;


async function fetchPasthMeetings(type = 'past', page = 1) {
    currentPagePast = page;  
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
        console.error('Kullanıcı kimliği bulunamadı.');
        return;
    }
    try {
        const response = await fetch(`http://localhost:5064/api/meetings/past?page=${currentPagePast}&pageSize=${pageSizePast}`, {

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
        console.log("meet",data)

        const userMeetings = data.meetings?.filter(meeting => meeting.userId === user.id);
        userMeetings.sort((a, b) => new Date(a.startDate) - new Date(b.startDate));

        renderPastMeetings(userMeetings, user.id);
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
            fetchPasthMeetings(page);
        });

        paginationContainer.appendChild(pageItem);
    }
}

function renderPastMeetings(meetings, currentUserId) {
    const meetingsBody = document.getElementById('meetingsBody3');
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


function formatDate(date) {
    const options = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return new Date(date).toLocaleDateString('tr-TR', options);
}

function truncateDescription(description) {
    return description.length > 100 ? description.substring(0, 100) + '...' : description;
}

fetchPasthMeetings();
