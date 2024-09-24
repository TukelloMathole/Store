<template>
    <div class="p-4">
      <h1 class="text-2xl font-bold">Order History</h1>
      <table class="min-w-full mt-4">
        <thead>
          <tr>
            <th class="px-4 py-2">Order ID</th>
            <th class="px-4 py-2">Date</th>
            <th class="px-4 py-2">Total</th>
            <th class="px-4 py-2">Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="order in orders" :key="order.id">
            <td class="border px-4 py-2">{{ order.id }}</td>
            <td class="border px-4 py-2">{{ order.date }}</td>
            <td class="border px-4 py-2">${{ order.total.toFixed(2) }}</td>
            <td class="border px-4 py-2">{{ order.status }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <script>
  import { mapGetters } from 'vuex';
  
  export default {
    computed: {
      ...mapGetters('order', ['orders']) // Assuming order history is stored in Vuex
    }
  };
  </script>
  
  <style scoped>
  /* Add styles specific to this component */
  </style>
  