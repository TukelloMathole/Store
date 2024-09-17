<template>
    <div class="home-container">
      <h1>Welcome to Our Application</h1>
      <p>
        This is the home page where you can find information about our application and its features.
      </p>
      <p>
        <router-link to="/login">Login</router-link> or <router-link to="/register">Register</router-link> to get started.
      </p>
    </div>
  </template>
  
  <script>
  export default {
    name: 'HomePage',
  };
  </script>
  
  <style scoped>
  .home-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #f9f9f9;
    text-align: center;
  }
  
  h1 {
    margin-bottom: 20px;
  }
  
  p {
    margin-bottom: 15px;
  }
  
  a {
    color: #007bff;
    text-decoration: none;
  }
  
  a:hover {
    text-decoration: underline;
  }
  </style>
  