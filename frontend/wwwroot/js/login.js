const api = axios.create({
  baseURL: 'http://localhost:5229',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('token')}` 
  }
});

// apiClient.interceptors.request.use(request => {
//   console.log('Starting Request', request);
//   return request;
// });

// apiClient.interceptors.response.use(response => {
//   console.log('Response:', response);
//   return response;
// }, error => {
//   console.error('Error:', error);
//   return Promise.reject(error);
// });

// apiClient.interceptors.request.use(config => {
//   const token = localStorage.getItem('token');
//   if (token) {
//     config.headers.Authorization = `Bearer ${token}`;
//   }
//   return config;
// }, error => {
//   return Promise.reject(error);
// });

const login = async (email,password) => {

  try {
    const res = await api.post("/api/Auth/login",{email,password})
    localStorage.setItem("token", res.data.token)
    localStorage.setItem('user', JSON.stringify(res.data.user));

    window.location.href = "/anasayfa"
  } catch (error) {
    console.log(error)
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
