const form = document.getElementById("createMeetingForm");

form.addEventListener("submit", (e) => {
  e.preventDefault();

  const formData = new FormData(form);

  // for (let [key, value] of formData.entries()) {
  //   console.log(`${key}: ${value}`);
  // }

  const URL = "http://localhost:5064/api/Meetings";


  const token = localStorage.getItem("token");

  axios
    .post(URL, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
        Authorization: `Bearer ${token}`,
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
      console.error("Error:", error);
      alert("Bir hata oluştu.");
    });
});
