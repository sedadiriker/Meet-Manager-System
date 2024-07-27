document.addEventListener("DOMContentLoaded", function () {
  var token = localStorage.getItem("token");
  var currentUserId = JSON.parse(localStorage.getItem("user"))?.id; 

  fetch("http://localhost:5064/api/Meetings", {
    method: "GET",
    headers: {
      Authorization: "Bearer " + token,
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

    data.forEach((meeting) => {
      var row = document.createElement("tr");

      var nameCell = document.createElement("td");
      nameCell.textContent = meeting.name;
      nameCell.classList.add("text-capitalize", "name-cell");
      row.appendChild(nameCell);

      var startDateCell = document.createElement("td");
      startDateCell.textContent = formatDate(meeting.startDate);
      startDateCell.classList.add("start-date-cell");
      row.appendChild(startDateCell);

      var endDateCell = document.createElement("td");
      endDateCell.textContent = formatDate(meeting.endDate);
      endDateCell.classList.add("end-date-cell");
      row.appendChild(endDateCell);

      var descriptionCell = document.createElement("td");
      descriptionCell.className = "description-cell";

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
      documentCell.className = "document-cell";

      if (meeting.documentPath) {
        var link = document.createElement("a");
        link.href = meeting.documentPath;
        link.textContent = "Döküman";
        link.className = "document-link";
        link.setAttribute("data-user-id", meeting.userId); 
        if (meeting.userId !== currentUserId) {
          link.style.pointerEvents = 'none'; 
          link.style.color = '#ccc'; 
        }
        documentCell.appendChild(link);
      } else {
        documentCell.textContent = "-";
      }

      row.appendChild(documentCell);

      var actionsCell = document.createElement("td");
      actionsCell.className = "actions-cell";
      
      // Sadece toplantı sahibi ise silme ve düzenleme butonlarını göster
      if (meeting.userId === currentUserId) {
        var deleteButton = document.createElement("button");
        deleteButton.innerHTML = '<i class="fas fa-trash delete"></i>';
        deleteButton.className = "btn btn-outline-danger btn-sm m-2 delete";
        deleteButton.setAttribute("data-id", meeting.id);
        actionsCell.appendChild(deleteButton);

        var editButton = document.createElement("button");
        editButton.innerHTML = '<i class="fas fa-edit edit"></i>';
        editButton.className = "btn btn-outline-primary btn-sm edit m-2";
        editButton.setAttribute("data-id", meeting.id);
        actionsCell.appendChild(editButton);
      }

      row.appendChild(actionsCell);

      tbody.appendChild(row);
    });
  })
  .catch((error) => {
    console.error("Veriler alınırken bir hata oluştu:", error);
  });

  function formatDate(dateString) {
    var date = new Date(dateString);
    var day = date.getDate().toString().padStart(2, "0");
    var month = (date.getMonth() + 1).toString().padStart(2, "0");
    var year = date.getFullYear();
    var hours = date.getHours().toString().padStart(2, "0");
    var minutes = date.getMinutes().toString().padStart(2, "0");
    return `${day}/${month}/${year} ${hours}:${minutes}`;
  }

  function truncateDescription(description, maxLength = 30) {
    if (description.length > maxLength) {
      return description.slice(0, maxLength) + '...';
    }
    return description;
  }
});
