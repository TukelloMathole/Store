// store/modules/user.js

import UserService from '@/services/UserService'; // Import your UserService

const state = {
  profile: null, // Holds the user's profile information
  loading: false, // Indicates whether user data is being loaded
  error: null, // Holds any error messages
};

const getters = {
  profile: state => state.profile, // Get the user's profile
  isLoading: state => state.loading, // Check if data is being loaded
  error: state => state.error, // Get the error message
};

const actions = {
  async fetchProfile({ commit }) {
    commit('setLoading', true);
    try {
      const response = await UserService.getProfile(); // Call the service to fetch user profile
      commit('setProfile', response.data.profile); // Set profile data
    } catch (error) {
      commit('setError', 'Failed to fetch profile. Please try again.');
    } finally {
      commit('setLoading', false);
    }
  },

  async updateProfile({ commit }, profileData) {
    commit('setLoading', true);
    try {
      const response = await UserService.updateProfile(profileData); // Call the service to update profile
      commit('setProfile', response.data.profile); // Update profile data
    } catch (error) {
      commit('setError', 'Failed to update profile. Please try again.');
    } finally {
      commit('setLoading', false);
    }
  },

  clearError({ commit }) {
    commit('setError', null); // Clear the error message
  }
};

const mutations = {
  setProfile(state, profile) {
    state.profile = profile; // Update the profile information
  },

  setLoading(state, loading) {
    state.loading = loading; // Set the loading state
  },

  setError(state, error) {
    state.error = error; // Set the error message
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
