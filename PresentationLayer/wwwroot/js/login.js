const login = async (email, password) => {
  try {
    const res = await api.post("/api/Auth/login", { email, password });
    localStorage.setItem("token", res.data.token);
    localStorage.setItem('user', JSON.stringify(res.data.user));

    setTimeout(() => {
      window.location.href = "/anasayfa";
    }, 2000);

    toastr["success"]("Giriş başarılı!");
  } catch (error) {
    console.log(error);
    toastr["error"]("Giriş başarısız!");

  }
};

document.getElementById("loginForm").addEventListener("submit", async (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;

  try {
    await login(email, password);

    // Başarılı giriş durumunda kullanıcı bilgisini navbar'a yazdırma
    const user = JSON.parse(localStorage.getItem('user'));
    if (user && user.FirstName) {
      document.querySelector('.navbar .text-gray-600').textContent = user.FirstName;
    }
  } catch (error) {
    console.error("Login işlemi sırasında bir hata oluştu:", error);
  }
});
