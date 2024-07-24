document.addEventListener("DOMContentLoaded", () => {
    const registerForm = document.getElementById("registerForm");
  
    document.getElementById("registerForm").addEventListener("submit", async (e) => {
      e.preventDefault();
  
      const formData = new FormData();
      formData.append("FirstName", document.getElementById("firstName").value);
      formData.append("LastName", document.getElementById("lastName").value);
      formData.append("Email", document.getElementById("email").value);
      formData.append("Phone", document.getElementById("phone").value);
      formData.append("Password", document.getElementById("password").value);
      formData.append("ProfilePicture", document.getElementById("profilePicture").files[0]);
  
      const URL = "http://localhost:5284/api";
  
      try {
        const response = await axios.post(`${URL}/Auth/register`, formData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        });
  
        if (response.status === 201) {
          alert("Registration successful");
          window.location.href = "/";
        } else {
          throw new Error("Registration failed");
        }
      } catch (error) {
        alert("Registration failed");
        console.error("Error:", error);
      }
    });
  });
  