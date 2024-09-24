<template>
    <div class="p-4">
      <h1 class="text-2xl font-bold">Manage Users</h1>
      <button @click="addUser" class="mt-4 mb-4 p-2 bg-blue-600 text-white">Add User</button>
      <table class="min-w-full">
        <thead>
          <tr>
            <th class="px-4 py-2">User ID</th>
            <th class="px-4 py-2">Name</th>
            <th class="px-4 py-2">Email</th>
            <th class="px-4 py-2">Role</th>
            <th class="px-4 py-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id">
            <td class="border px-4 py-2">{{ user.id }}</td>
            <td class="border px-4 py-2">{{ user.name }}</td>
            <td class="border px-4 py-2">{{ user.email }}</td>
            <td class="border px-4 py-2">{{ user.role }}</td>
            <td class="border px-4 py-2">
              <button @click="editUser(user)" class="text-blue-600">Edit</button>
              <button @click="deleteUser(user.id)" class="text-red-600">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <script>
  import { mapGetters } from 'vuex';
  
  export default {
    computed: {
      ...mapGetters('user', ['users']) // Assuming user list is stored in Vuex
    },
    methods: {
      addUser() {
        // Logic to add a user
      },
      editUser(user) {
        // Logic to edit a user
        console.log('Editing user:', user);
      },
      deleteUser(userId) {
        // Logic to delete a user
        console.log('Deleting user with ID:', userId);
      }
    }
  };
  </script>
  
  
  <style scoped>
  /* Add styles specific to this component */
  </style>
  