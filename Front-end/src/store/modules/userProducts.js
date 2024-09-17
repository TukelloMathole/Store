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
      const response = await axiosInstance.get('/api/UserProducts');
      commit('SET_PRODUCTS', response.data);
    } catch (error) {
      console.error('Failed to fetch user products:', error);
    }
  },
  
  async fetchProductById({ commit }, id) {
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
