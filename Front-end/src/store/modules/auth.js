import AuthService from '@/services/AuthService'; // Import your AuthService

const state = {
  user: null, 
  accessToken: null, 
  refreshToken: null,
  userRole: null
};

const getters = {
  isAuthenticated: state => !!state.user, 
  user: state => state.user, 
  accessToken: state => state.accessToken, 
};

const actions = {
  async login({ commit }, credentials) {
    try {
      const response = await AuthService.login(credentials); 
      commit('setUser', response.data.user);
      commit('setTokens', { accessToken: response.data.accessToken, refreshToken: response.data.refreshToken });
    } catch (error) {
      throw new Error('Login failed. Please check your credentials.');
    }
  },

  async logout({ commit }) {
    try {
      await AuthService.logout();
      commit('clearUser'); 
      commit('clearTokens');
    } catch (error) {
      throw new Error('Logout failed. Please try again.');
    }
  },

  async register({ commit }, userData) {
    try {
      const response = await AuthService.register(userData); // Call the register service
      commit('setUser', response.data.user); // Set user data
      commit('setTokens', { accessToken: response.data.accessToken, refreshToken: response.data.refreshToken });
    } catch (error) {
      throw new Error('Registration failed. Please check your details.');
    }
  },

  async updateUser({ commit }, userData) {
    try {
      const response = await AuthService.updateProfile(userData); // Call the update profile service
      commit('setUser', response.data.user); // Update user data
    } catch (error) {
      throw new Error('Update failed. Please try again.');
    }
  }
};

const mutations = {
  setUser(state, user) {
    state.user = user; // Update user information
  },

  setTokens(state, tokens) {
    state.accessToken = tokens.accessToken; // Update access token
    state.refreshToken = tokens.refreshToken; // Update refresh token
  },

  clearUser(state) {
    state.user = null; // Clear user information
  },

  clearTokens(state) {
    state.accessToken = null; // Clear access token
    state.refreshToken = null; // Clear refresh token
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
