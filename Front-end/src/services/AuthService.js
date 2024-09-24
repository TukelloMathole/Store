import axios from 'axios';
import store from '@/store'; // Import your Vuex store

const API_URL = 'https://localhost:7084'; // Replace with your backend API URL

class AuthService {
  setAuthHeader(token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }

  async login(credentials) {
    try {
      const response = await axios.post(`${API_URL}/api/Account/login`, credentials);
      
      const { accessToken, refreshToken, role, user } = response.data;
      
      // Commit the tokens and user data to the Vuex store
      store.commit('auth/setTokens', { accessToken, refreshToken });
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
    // Remove tokens from local storage and Vuex state
    store.commit('auth/clearTokens');
    store.commit('auth/clearUser');
    store.commit('auth/clearUserRole');
    delete axios.defaults.headers.common['Authorization'];
  }

  async register(userData) {
    const response = await axios.post(`${API_URL}/api/Account/register`, userData);
    
    const { accessToken, refreshToken, role, user } = response.data;
    
    // Commit the tokens and user data to the Vuex store
    store.commit('auth/setTokens', { accessToken, refreshToken });
    store.commit('auth/setUser', user);
    store.commit('auth/setUserRole', role);
    
    this.setAuthHeader(accessToken); // Set the auth header after registration
    
    return response;
  }

  async updateProfile(userData) {
    const response = await axios.put(`${API_URL}/api/Account/updateProfile`, userData);
    
    // Commit the updated user data to the Vuex store
    store.commit('auth/setUser', response.data.user);
    
    return response;
  }

  async getUserData() {
    // Get the access token from Vuex store
    console.log("we are here");
    const accessToken = store.getters['auth/accessToken'];
    if (accessToken) {
      // Set the auth header
      this.setAuthHeader(accessToken);

      // Fetch user data
      const response = await axios.get(`${API_URL}/api/Account/user`);
      console.log(response.data)
      return response;
    } else {
      throw new Error('No access token found.');
    }
  }
}

export default new AuthService();
