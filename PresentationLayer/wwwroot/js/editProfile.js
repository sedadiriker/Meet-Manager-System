
document.addEventListener("DOMContentLoaded", () => {
    const user = JSON.parse(localStorage.getItem('user'));

        document.getElementById('firstName').value = user.firstName;
        document.getElementById('lastName').value = user.lastName;
        document.getElementById('email').value = user.email;
    }
);

document.getElementById('editBtn').addEventListener('click', () => {
    document.getElementById('firstName').removeAttribute('readonly');
    document.getElementById('lastName').removeAttribute('readonly');
    document.getElementById('email').removeAttribute('readonly');
    document.getElementById('password').removeAttribute('readonly');
    document.getElementById('profilePictureUpload').removeAttribute('disabled');
    document.getElementById('saveBtn').classList.remove('d-none');
    document.getElementById('editBtn').classList.add('d-none');
});

document.getElementById('profileForm').addEventListener('submit', async (e) => {
    e.preventDefault();
  
    const formData = new FormData(e.target);
    const user = JSON.parse(localStorage.getItem('user'));
  
    try {
      const updateResponse = await api.put(`/api/User/${user.id}`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
  
      if (updateResponse.status === 200) {
        const getUserResponse = await api.get(`/api/User/${user.id}`);
  
        if (getUserResponse.status === 200) {
          const updatedUser = getUserResponse.data;
          
          localStorage.setItem('user', JSON.stringify(updatedUser));
          
          document.querySelector('.navbar .text-gray-600').textContent = `${updatedUser.firstName.toLocaleUpperCase('tr-TR')} ${updatedUser.lastName.toLocaleUpperCase('tr-TR')}`;
          document.getElementById('firstName').value = updatedUser.firstName;
          document.getElementById('lastName').value = updatedUser.lastName;
          document.getElementById('email').value = updatedUser.email;
          const profilePictureUrl = `http://localhost:5064/uploads/${updatedUser.profilePicture.split('/').pop()}`;
          const imgElements = [...document.querySelectorAll('.img-profile')];
          imgElements.forEach(img => {
            img.src = profilePictureUrl;
          });
          document.getElementById('saveBtn').classList.add('d-none');
                document.getElementById('editBtn').classList.remove('d-none');
                document.getElementById('firstName').setAttribute('readonly', true);
                document.getElementById('lastName').setAttribute('readonly', true);
                document.getElementById('email').setAttribute('readonly', true);
                document.getElementById('password').setAttribute('readonly', true);
                document.getElementById('profilePictureUpload').setAttribute('disabled', true);
          alert('Profile updated successfully!');
        } else {
          alert('An error occurred while retrieving the updated profile.');
        }
      } else {
        alert('An error occurred while updating the profile.');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('An error occurred while updating the profile.');
    }
  });
