document.addEventListener("DOMContentLoaded", function () {
    var token = localStorage.getItem("token");
    var currentUserId = JSON.parse(localStorage.getItem("user"))?.id;
    var currentPage = 1;
    var pageSize = 10; 
    var totalPages = 1; 

    function fetchMeetings(page = 1) {
        fetch(`http://localhost:5064/api/Meetings?page=${page}&pageSize=${pageSize}`, {
            method: "GET",
            headers: {
                Authorization: "Bearer " + token,
                'Content-Type': 'application/json'
            },
        })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Sunucudan hata yanıtı alındı: " + response.statusText);
            }
            return response.json();
        })
        .then((data) => {
            var tbody = document.getElementById("meetingsBody");
            tbody.innerHTML = "";

            data.meetings.forEach((meeting) => {
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
                summary.className = "description-summary";
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
                fullDescription.className = "description-full";
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
                }

                row.appendChild(actionsCell);

                tbody.appendChild(row);
            });

            totalPages = data.totalPages;
            document.getElementById("pageInfo").textContent = `Sayfa ${data.currentPage} / ${totalPages}`;
            document.getElementById("prevPage").disabled = data.currentPage <= 1;
            document.getElementById("nextPage").disabled = data.currentPage >= totalPages;

        })
        .catch((error) => {
            console.error("Veriler alınırken bir hata oluştu:", error);
        });
    }

    function formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(dateString).toLocaleDateString(undefined, options);
    }

    function truncateDescription(description) {
        return description.length > 100 ? description.substring(0, 100) + "..." : description;
    }

    function editMeeting(meetingId) {
        console.log('Düzenle: ', meetingId);
    }

    function deleteMeeting(meetingId) {
        console.log('Sil: ', meetingId);
    }

    document.getElementById("prevPage").addEventListener("click", () => {
        if (currentPage > 1) {
            currentPage--;
            fetchMeetings(currentPage);
        }
    });

    document.getElementById("nextPage").addEventListener("click", () => {
        if (currentPage < totalPages) {
            currentPage++;
            fetchMeetings(currentPage);
        }
    });

    fetchMeetings();
});
