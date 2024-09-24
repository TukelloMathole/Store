<template>
    <div class="p-4">
      <h1 class="text-2xl font-bold">User Profile</h1>
      <div class="mt-4">
        <p><strong>Name:</strong> {{ user.name }}</p>
        <p><strong>Email:</strong> {{ user.email }}</p>
        <p><strong>Role:</strong> {{ user.role }}</p>
        <!-- Add more user details as needed -->
      </div>
    </div>
  </template>
  
  <script>
  import { mapGetters } from 'vuex';
  
  export default {
    computed: {
      ...mapGetters('auth', ['user']) // Assuming user data is stored in Vuex
    }
  };
  </script>
  
  <style scoped>
  /* Add styles specific to this component */
  </style>
  