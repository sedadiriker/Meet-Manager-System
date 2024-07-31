document.addEventListener("DOMContentLoaded", function () {
  var now = new Date();
  var today = now.toISOString().split("T")[0] + 'T' + now.toTimeString().split(" ")[0];

  var minDate = today.substring(0, 16); 

  document.getElementById("StartDate").setAttribute("min", minDate);
  document.getElementById("EndDate").setAttribute("min", minDate);

  document.getElementById("StartDate").addEventListener("change", function () {
    var startDate = document.getElementById("StartDate").value;
    document.getElementById("EndDate").setAttribute("min", startDate);
  });
});

const form = document.getElementById("createMeetingForm");

form.addEventListener("submit", (e) => {
  e.preventDefault();

  const name = document.getElementById("Name").value;
  const startDate = document.getElementById("StartDate").value;
  const endDate = document.getElementById("EndDate").value;
  const description = document.getElementById("Description").value;
  const documentPath = document.getElementById("DocumentPath").files[0]; 

  const token = localStorage.getItem("token");
  const userString = localStorage.getItem("user");


  const user = JSON.parse(userString);

  const formData = new FormData();
  formData.append("Name", name);
  formData.append("StartDate", startDate);
  formData.append("EndDate", endDate);
  formData.append("Description", description);
  formData.append("DocumentPath", documentPath); // Dosyayı ekle
  formData.append("UserId", user.id);

  // console.log("FormData içeriği:");
  // for (let [key, value] of formData.entries()) {
  //   console.log(`${key}: ${value}`);
  // }

  const URL = "http://localhost:5064/api/Meetings";

  axios
    .post(URL, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
        "Authorization": `Bearer ${token}`
      },
    })
    .then((response) => {
      console.log("Yanıt:", response);
      setTimeout(() => {
        window.location.href = "/toplantı_listesi";
      }, 2000);
  
      toastr["success"]("Toplantı Eklendi!");
    })
    .catch((error) => {
      console.error("Error:", error.response ? error.response.data : error);
      toastr["error"]('Bir hata oluştu');
    });
});
