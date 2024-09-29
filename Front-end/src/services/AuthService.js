import axios from 'axios';
import store from '@/store'; // Import your Vuex store
import TokenService from '@/services/TokenService'; // Import your TokenService
import { jwtDecode } from 'jwt-decode';

const API_URL = 'https://localhost:5000/'; // Replace with your backend API URL

class AuthService {
  setAuthHeader(token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }

  async login(credentials) {
    try {
      const response = await axios.post(`${API_URL}UserAuthentication/login`, credentials);
      
      const { accessToken, refreshToken, role, user, exp } = response.data;

      // Save tokens using TokenService
      TokenService.saveTokens({ accessToken, refreshToken, expiration: exp });
      
      // Commit the user data to the Vuex store
      store.commit('auth/setUser', user);
      store.commit('auth/setUserRole', role);
      
      // Set the auth header for future requests
      this.setAuthHeader(accessToken);
      
      return response;
    } catch (error) {
      throw new Error(error.response ? error.response.data.message || 'Login failed' : 'Network error');
    }
  }

  async logout() {
    // Clear tokens using TokenService
    TokenService.clearTokens();
    
    // Remove user data from Vuex state
    store.commit('auth/clearUser');
    store.commit('auth/clearUserRole');
    delete axios.defaults.headers.common['Authorization'];
  }

  async register(userData) {
    const response = await axios.post(`${API_URL}UserRegistration/register`, userData);
    console.log(response);
    const { accessToken, refreshToken, role, user, exp } = response.data;

    // Save tokens using TokenService
    TokenService.saveTokens({ accessToken, refreshToken, expiration: exp });
    
    // Commit the user data to the Vuex store
    store.commit('auth/setUser', user);
    store.commit('auth/setUserRole', role);
    
    this.setAuthHeader(accessToken); // Set the auth header after registration
    
    return response;
  }

  async updateProfile(userData) {
    console.log(userData);
    const response = await axios.put(`${API_URL}userManagement/user/update`, userData);
    console.log(response);
    // Commit the updated user data to the Vuex store
    store.commit('auth/setUser', response.data.user);
    
    return response;
  }

  async getUserData() {
    const accessToken = TokenService.getToken();
    if (accessToken) {
        const decodedToken = jwtDecode(accessToken);
        const email = decodedToken.sub;
        console.log('the email',email);
        
        // Set the auth header
        this.setAuthHeader(accessToken);

        // Fetch user data using the extracted email
        const response = await axios.get(`${API_URL}UserManagement/user/email/${email}`);
        console.log(response);
        return response.data; // Return the actual data
    } else {
        throw new Error('No access token found.');
    }
}
}

export default new AuthService();
