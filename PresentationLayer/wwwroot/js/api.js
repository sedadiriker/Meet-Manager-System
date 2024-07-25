const api = axios.create({
    baseURL: 'http://localhost:5064',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}` 
    }
  });