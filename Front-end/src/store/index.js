// store/index.js
import { createStore } from 'vuex';
import products from './modules/products';
import auth from './modules/auth'; // Import your Vuex modules

const store = createStore({
  modules: {
    products,
    auth // Add your modules here
  }
});

export default store;
