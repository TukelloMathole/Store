import axios from 'axios';


const API_URL = 'https://localhost:7084'; // Replace with your backend API URL

class AuthService {
  setAuthHeader(token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }

  async login(credentials) {
    const response = await axios.post(`${API_URL}/api/Account/login`, credentials);
    const { accessToken, refreshToken } = response.data;

    // Store tokens and user information in local storage
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);

    

    // Set the auth header for future requests
    this.setAuthHeader(accessToken);

    return response;
  }

  async logout() {
    // Remove tokens from local storage
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userEmail');
    
    // Clear auth header
    delete axios.defaults.headers.common['Authorization'];

    await axios.post(`${API_URL}/api/Account/logout`);
  }

  async register(userData) {
    const response = await axios.post(`${API_URL}/api/Account/register`, userData);
    return response;
  }

  async updateProfile(userData) {
    const response = await axios.put(`${API_URL}/api/Account/updateProfile`, userData);
    return response;
  }

  async getUserData() {
    // Get the access token from local storage
    const accessToken = localStorage.getItem('accessToken');
    console.log("we here now")
    if (accessToken) {
      // Set the auth header
      this.setAuthHeader(accessToken);

      // Fetch user data
      const response = await axios.get(`${API_URL}/api/Account/user`);
      return response;
    } else {
      throw new Error('No access token found.');
    }
  }
}

export default new AuthService();
