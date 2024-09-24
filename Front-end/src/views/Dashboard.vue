<template>
    <div class="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <h2 class="text-2xl font-bold mb-4">Dashboard</h2>
      <p class="mb-4">Welcome to your dashboard!</p>
      <p>Your user information will be displayed here.</p>
      <button @click="logout" class="mt-4 bg-red-600 text-white font-bold py-2 px-4 rounded">Logout</button>
    </div>
  </template>
  
  <script>
  export default {
    name: 'DashBoard',
    methods: {
      logout() {
        // Handle logout logic here (call Vuex action, etc.)
        console.log('User logged out');
      }
    }
  };
  </script>
  
  <style scoped>
  /* Add any additional styles here if needed */
  </style>
  