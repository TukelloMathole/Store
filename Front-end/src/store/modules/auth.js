import Cookies from 'js-cookie';

const state = {
  user: null,
  accessToken: Cookies.get('accessToken') || null, 
  refreshToken: Cookies.get('refreshToken') || null,
  userRole: Cookies.get('role') || null // Load from cookies on init
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
    Cookies.set('role', role, { secure: true, sameSite: 'Strict' }); // Save to cookies
  },

  setTokens(state, tokens) {
    state.accessToken = tokens.accessToken; // Update access token
    state.refreshToken = tokens.refreshToken; // Update refresh token
    Cookies.set('accessToken', tokens.accessToken, { secure: true, sameSite: 'Strict' }); // Save to cookies
    Cookies.set('refreshToken', tokens.refreshToken, { secure: true, sameSite: 'Strict' }); // Save to cookies
  },

  clearUser(state) {
    state.user = null; // Clear user information
  },

  clearUserRole(state) {
    state.userRole = null; // Clear user role
    Cookies.remove('role'); // Remove from cookies
  },

  clearTokens(state) {
    state.accessToken = null; // Clear access token
    state.refreshToken = null; // Clear refresh token
    Cookies.remove('accessToken'); // Remove from cookies
    Cookies.remove('refreshToken'); // Remove from cookies
  }
};

export default {
  namespaced: true,
  state,
  getters,
  mutations
};
