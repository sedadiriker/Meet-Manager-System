const token = localStorage.getItem('token');
const user = JSON.parse(localStorage.getItem('user'));
    console.log(user)
    console.log(token)
    const username = `${(user.firstName).toLocaleUpperCase('tr-TR')} ${(user.lastName).toLocaleUpperCase('tr-TR')}`
    const profilePictureUrl = `http://localhost:5064/uploads/${user.profilePicture.split('/').pop()}`;
;

  
    if (user) {
      document.querySelector('.navbar .text-gray-600').textContent = username;
      const imgElements = document.querySelectorAll('.img-profile');
      imgElements.forEach(img => (
        img.src = profilePictureUrl
      ))
    }


//Toast
toastr.options = {
  "closeButton": true,
  "debug": false,
  "newestOnTop": false,
  "progressBar": true,
  "positionClass": "toast-top-right",
  "preventDuplicates": false,
  "onclick": null,
  "showDuration": "300",
  "hideDuration": "1000",
  "timeOut": "5000",
  "extendedTimeOut": "1000",
  "showEasing": "swing",
  "hideEasing": "linear",
  "showMethod": "fadeIn",
  "hideMethod": "fadeOut"
};
