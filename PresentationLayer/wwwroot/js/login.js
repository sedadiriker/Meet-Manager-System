
const login = async (email,password) => {
  try {
    const res = await api.post("/api/Auth/login",{email,password})
    localStorage.setItem("token", res.data.token)
    localStorage.setItem('user', JSON.stringify(res.data.user));
    setTimeout(() => {
      window.location.href = "/anasayfa";
    }, 3000)
     toastr.info("Giriş başarılı!", "Başarılı");
  } catch (error) {
    console.log(error)
    
    if (error.response && error.response.data) {
      const { message } = error.response.data;

      if (message.includes("email") || message.includes("parola")) {
        toastr.error("E-posta veya şifre hatalı. Lütfen tekrar deneyin.", "Hata");
      } else if (message.includes("user")) {
        toastr.error("Kullanıcı bulunamadı. Lütfen bilgilerinizi kontrol edin.", "Hata");
      } else {
        toastr.error("Giriş sırasında bir hata oluştu. Lütfen tekrar deneyin.", "Hata");
      }
    } else {
      toastr.error("Giriş sırasında bir hata oluştu. Lütfen tekrar deneyin.", "Hata");
    }
  }
}

document.getElementById("loginForm").addEventListener("submit", (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;

login(email,password)

const user = JSON.parse(localStorage.getItem('user'));
console.log(user)

  if (user && user.FirstName) {
    document.querySelector('.navbar .text-gray-600').textContent = user.FirstName;
  }
});
