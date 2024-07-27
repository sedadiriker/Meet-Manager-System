document.addEventListener("DOMContentLoaded", () => {

  document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("FirstName", document.getElementById("firstName").value);
    formData.append("LastName", document.getElementById("lastName").value);
    formData.append("Email", document.getElementById("email").value);
    formData.append("Phone", document.getElementById("phone").value);
    formData.append("Password", document.getElementById("password").value);
    formData.append("ProfilePicture", document.getElementById("profilePicture").files[0]);

    const URL = "http://localhost:5064/api/User";

    try {
      const response = await axios.post(URL, formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });

      if (response.status === 200) {
        setTimeout(() => {
          window.location.href = "/";
        }, 1000);
    
        toastr["success"]("Kayıt İşlemi Başarılı!");
      } else {
        throw new Error("Kayıt İşlemi Başarısız");
      }
    } catch (error) {
      toastr["error"]("Kayıt İşlemi Başarısız");
      console.error("Error:", error);
    }
  });
});
