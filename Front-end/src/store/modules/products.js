// store/modules/products.js
import axiosInstance from '../axiosInstance';

const state = {
  products: [],
  selectedProduct: null
};

const mutations = {
  SET_PRODUCTS(state, products) {
    state.products = products;
  },
  SET_SELECTED_PRODUCT(state, product) {
    state.selectedProduct = product;
  }
};

const actions = {
    
  async fetchProducts({ commit }) {
    try {
      const response = await axiosInstance.get('/api/AdminProducts'); // Adjust the URL as needed
      commit('SET_PRODUCTS', response.data);
    } catch (error) {
      console.error('Failed to fetch products:', error);
    }
  },
  async fetchProductById({ commit }, id) {
    try {
      const response = await axiosInstance.get(`/api/AdminProducts/${id}`); // Adjust the URL as needed
      commit('SET_SELECTED_PRODUCT', response.data);
    } catch (error) {
      console.error('Failed to fetch product:', error);
    }
  },
  async addProduct({ dispatch }, product) {
    try {
      await axiosInstance.post('/api/AdminProducts', product); // Adjust the URL as needed
      dispatch('fetchProducts');
    } catch (error) {
      console.error('Failed to add product:', error);
    }
  },
  async updateProduct({ dispatch }, product) {
    try {
      await axiosInstance.put(`/api/AdminProducts/${product.id}`, product);
      dispatch('fetchProducts');
    } catch (error) {
      console.error('Failed to update product:', error);
    }
  },
  async deleteProduct({ dispatch }, id) {
    try {
      await axiosInstance.delete(`/api/AdminProducts/${id}`); // Adjust the URL as needed
      dispatch('fetchProducts');
    } catch (error) {
      console.error('Failed to delete product:', error);
    }
  },
  async fetchUserProducts({ commit }) {
    try {
      const response = await axiosInstance.get('/api/UserProducts');
      commit('SET_PRODUCTS', response.data);
    } catch (error) {
      console.error('Failed to fetch user products:', error);
    }
  },
  async fetchUserProductById({ commit }, id) {
    try {
      const response = await axiosInstance.get(`/api/UserProducts/${id}`);
      commit('SET_SELECTED_PRODUCT', response.data);
    } catch (error) {
      console.error('Failed to fetch user product:', error);
    }
  }
};

const getters = {
  products: (state) => state.products,
  selectedProduct: (state) => state.selectedProduct
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
};
