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
        fullDescription.textContent = meeting.description
          ? meeting.description
          : "-";

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
            link.style.pointerEvents = "none";
            link.style.color = "#ccc";
          }
          documentCell.appendChild(link);
        } else {
          documentCell.textContent = "-";
        }

        row.appendChild(documentCell);

        var actionsCell = document.createElement("td");
        actionsCell.className = "actions-cell";
        actionsCell.style.display = "flex";

        if (meeting.userId === currentUserId) {
          var deleteButton = document.createElement("button");
          deleteButton.innerHTML = '<i class="fas fa-trash delete"></i>';
          deleteButton.className =
            "btn btn-outline-danger btn-sm m-1 delete tooltip-button";
          deleteButton.style.fontSize = "0.5rem";
          deleteButton.setAttribute("data-id", meeting.id);
          deleteButton.setAttribute("data-tooltip", "Sil");
          actionsCell.appendChild(deleteButton);

          var editButton = document.createElement("button");
          editButton.innerHTML = '<i class="fas fa-edit edit"></i>';
          editButton.className =
            "btn btn-outline-primary btn-sm edit m-1 tooltip-button";
          editButton.style.fontSize = "0.5rem";
          editButton.setAttribute("data-id", meeting.id);
          editButton.setAttribute("data-tooltip", "Düzenle");
          actionsCell.appendChild(editButton);
        }

        row.appendChild(actionsCell);

        tbody.appendChild(row);
      });

      var tooltipButtons = document.querySelectorAll(".tooltip-button");
      tooltipButtons.forEach((button) => {
        var tooltipText = button.getAttribute("data-tooltip");
        var tooltipSpan = document.createElement("span");
        tooltipSpan.className = "tooltip-text";
        tooltipSpan.textContent = tooltipText;
        button.appendChild(tooltipSpan);
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

  function truncateDescription(description) {
    return description.length > 100
      ? description.substring(0, 100) + "..."
      : description;
  }
});
