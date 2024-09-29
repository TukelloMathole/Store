<template>
  <div class="min-h-screen flex flex-col p-6 bg-gray-100">
    <h1 class="text-4xl font-bold mb-6 text-center text-gray-800">Order History</h1>
    
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <!-- Orders On the Way -->
      <div class="bg-white shadow-md rounded-lg p-6">
        <h2 class="text-2xl font-semibold mb-4 text-gray-800">Orders on the Way</h2>
        <div v-if="ordersOnTheWay.length" class="space-y-4">
          <div v-for="order in ordersOnTheWay" :key="order.id" class="border-b last:border-0 pb-4">
            <p class="font-medium text-gray-700"><strong>Order ID:</strong> {{ order.id }}</p>
            <p class="text-gray-600"><strong>Date:</strong> {{ formatDate(order.date) }}</p>
            <p class="text-gray-600"><strong>Total:</strong> ${{ order.total.toFixed(2) }}</p>
            <p class="text-gray-600"><strong>Status:</strong> <span :class="statusClass(order.status)">{{ order.status }}</span></p>
          </div>
        </div>
        <p v-else class="text-gray-500">No orders on the way.</p>
      </div>

      <!-- Past Orders -->
      <div class="bg-white shadow-md rounded-lg p-6">
        <h2 class="text-2xl font-semibold mb-4 text-gray-800">Past Orders</h2>
        <div v-if="pastOrders.length" class="space-y-4">
          <div v-for="order in pastOrders" :key="order.id" class="border-b last:border-0 pb-4">
            <p class="font-medium text-gray-700"><strong>Order ID:</strong> {{ order.id }}</p>
            <p class="text-gray-600"><strong>Date:</strong> {{ formatDate(order.date) }}</p>
            <p class="text-gray-600"><strong>Total:</strong> ${{ order.total.toFixed(2) }}</p>
            <p class="text-gray-600"><strong>Status:</strong> <span :class="statusClass(order.status)">{{ order.status }}</span></p>
          </div>
        </div>
        <p v-else class="text-gray-500">No past orders available.</p>
      </div>
    </div>
  </div>

  <!-- Loader or message if orders aren't loaded yet -->
  <div v-if="!orders.length" class="flex items-center justify-center min-h-screen">
    <p class="text-gray-600">Loading orders...</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      // Dummy data for the orders
      orders: [
        {
          id: '001',
          date: '2024-09-20',
          total: 149.99,
          status: 'Pending'
        },
        {
          id: '002',
          date: '2024-09-18',
          total: 89.49,
          status: 'Completed'
        },
        {
          id: '003',
          date: '2024-09-17',
          total: 24.99,
          status: 'Cancelled'
        },
        {
          id: '004',
          date: '2024-09-25',
          total: 249.00,
          status: 'In Transit'
        },
        {
          id: '005',
          date: '2024-09-26',
          total: 79.99,
          status: 'Completed'
        }
      ]
    };
  },
  computed: {
    // Filter orders that are on the way (Pending or In Transit)
    ordersOnTheWay() {
      return this.orders.filter(order => ['Pending', 'In Transit'].includes(order.status));
    },
    // Filter orders that are past (Completed or Cancelled)
    pastOrders() {
      return this.orders.filter(order => ['Completed', 'Cancelled'].includes(order.status));
    }
  },
  methods: {
    // Format the date to a readable format
    formatDate(date) {
      const options = { year: 'numeric', month: 'long', day: 'numeric' };
      return new Date(date).toLocaleDateString(undefined, options);
    },
    // Apply different styles to the order status
    statusClass(status) {
      return {
        'text-green-600 font-bold': status === 'Completed',
        'text-red-600 font-bold': status === 'Cancelled',
        'text-yellow-600 font-bold': status === 'Pending',
        'text-blue-600 font-bold': status === 'In Transit'
      };
    }
  }
};
</script>

<style scoped>
/* Add any additional styles specific to the order history component here */
</style>
