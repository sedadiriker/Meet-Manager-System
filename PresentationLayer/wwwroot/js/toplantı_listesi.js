//toplantı_listesi.js
document.addEventListener("DOMContentLoaded", function () {
  var token = localStorage.getItem("token");

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
        row.appendChild(nameCell);

        var startDateCell = document.createElement("td");
        startDateCell.textContent = formatDate(meeting.startDate);
        row.appendChild(startDateCell);

        var endDateCell = document.createElement("td");
        endDateCell.textContent = formatDate(meeting.endDate);
        row.appendChild(endDateCell);

        var descriptionCell = document.createElement("td");
        descriptionCell.classList.add("text-center");
        if (meeting.description) {
          descriptionCell.textContent = meeting.description;
        } else {
          descriptionCell.textContent = "-";
        }
        row.appendChild(descriptionCell);

        var documentCell = document.createElement("td");
        if (meeting.documentPath) {
          var link = document.createElement("a");
          link.href = meeting.documentPath;
          link.target = "_blank";
          link.textContent = "Döküman";
          documentCell.appendChild(link);
        } else {
          documentCell.textContent = "-";
        }
        row.appendChild(documentCell);

        var ıslemlercell = document.createElement("td");
        var deleteButton = document.createElement("button");
        deleteButton.innerHTML = '<i class="fas fa-trash delete"></i>'; 
        deleteButton.className = "btn btn-outline-danger btn-sm m-2 delete";
        deleteButton.setAttribute("data-id", meeting.id);
        ıslemlercell.appendChild(deleteButton);
        row.appendChild(ıslemlercell);

        var editButton = document.createElement("button");
        editButton.innerHTML = '<i class="fas fa-edit edit"></i>'; 
        editButton.className = "btn btn-outline-primary btn-sm edit m-2";
        editButton.setAttribute("data-id", meeting.id);
        ıslemlercell.appendChild(editButton);

        tbody.appendChild(row);
      });
    })
    .catch((error) => {
      console.error("Veriler alınırken bir hata oluştu:", error);
    });

  function formatDate(dateString) {
    var date = new Date(dateString);
    var hours = date.getHours().toString().padStart(2, "0");
    var minutes = date.getMinutes().toString().padStart(2, "0");
    return `${hours}:${minutes}`;
  }
});
