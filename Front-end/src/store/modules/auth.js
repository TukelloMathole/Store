const state = {
  user: null, 
  accessToken: localStorage.getItem('accessToken') || null, 
  refreshToken: localStorage.getItem('refreshToken') || null,
  userRole: localStorage.getItem('role') || null // Load from local storage on init
};

const getters = {
  isAuthenticated: state => {
    return !!state.accessToken;
  }, 
  user: state => {
    return state.user;
  }, 
  accessToken: state => {
    return state.accessToken;
  },
  userRole: state => {
    return state.userRole;
  }
};

const mutations = {
  setUser(state, user) {
    state.user = user; // Update user information
  },

  setUserRole(state, role) {
    state.userRole = role;
    localStorage.setItem('role', role); // Save to local storage
  },

  setTokens(state, tokens) {
    state.accessToken = tokens.accessToken; // Update access token
    state.refreshToken = tokens.refreshToken; // Update refresh token
    localStorage.setItem('accessToken', tokens.accessToken); // Save to local storage
    localStorage.setItem('refreshToken', tokens.refreshToken); // Save to local storage
  },

  clearUser(state) {
    state.user = null; // Clear user information
  },

  clearUserRole(state) {
    state.userRole = null; // Clear user role
    localStorage.removeItem('role'); // Remove from local storage
  },

  clearTokens(state) {
    state.accessToken = null; // Clear access token
    state.refreshToken = null; // Clear refresh token
    localStorage.removeItem('accessToken'); // Remove from local storage
    localStorage.removeItem('refreshToken'); // Remove from local storage
  }
};

export default {
  namespaced: true,
  state,
  getters,
  mutations
};
