<template>
  <div id="app">
    <main>
      <Navbar />
      <router-view></router-view> <!-- This renders the current route's component -->
    </main>
  </div>
</template>

<script>
import Navbar from './components/NavBar.vue';

export default {
  name: 'App',
  components: {
    Navbar,
  },
};
</script>

<style>
/* General Styles */
#app {
  background-color: #f5f5f5;
  min-height: 100vh;
  font-family: 'Arial', sans-serif;
  margin: 0;
  padding: 0;
}

/* Main Content */
main {
  padding: 0;
}

/* Responsive Design */
@media (max-width: 600px) {
  main {
    padding: 10px;
  }
}
</style>
