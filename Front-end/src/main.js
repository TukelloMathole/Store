// main.js
import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from './store'; // Import the Vuex store

const app = createApp(App);

app.use(router);
app.use(store); // Add Vuex to the app

app.mount('#app');
