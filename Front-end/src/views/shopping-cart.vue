<template>
    <div class="min-h-screen flex flex-col p-6 bg-gray-100">
      <h1 class="text-4xl font-bold mb-6 text-center text-gray-800">Shopping Cart</h1>
      
      <div v-if="cartItems.length" class="bg-white shadow-md rounded-lg p-6">
        <table class="min-w-full divide-y divide-gray-200">
          <thead>
            <tr>
              <th class="px-4 py-2 text-left">Product</th>
              <th class="px-4 py-2 text-left">Quantity</th>
              <th class="px-4 py-2 text-left">Price</th>
              <th class="px-4 py-2 text-left">Total</th>
              <th class="px-4 py-2 text-left">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in cartItems" :key="item.id" class="hover:bg-gray-100">
              <td class="border px-4 py-2">{{ item.productName }}</td>
              <td class="border px-4 py-2">
                <input 
                  type="number" 
                  v-model="item.quantity" 
                  min="1" 
                  class="w-16 border rounded px-2"
                />
              </td>
              <td class="border px-4 py-2">${{ item.price.toFixed(2) }}</td>
              <td class="border px-4 py-2">${{ (item.price * item.quantity).toFixed(2) }}</td>
              <td class="border px-4 py-2">
                <button @click="removeItem(item.id)" class="text-red-500 hover:underline">Remove</button>
              </td>
            </tr>
          </tbody>
        </table>
  
        <div class="mt-6">
          <h2 class="text-2xl font-semibold">Total: ${{ totalCost.toFixed(2) }}</h2>
        </div>
  
        <div class="mt-4 flex justify-end">
          <button @click="checkout" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">Proceed to Checkout</button>
        </div>
      </div>
  
      <div v-else class="flex items-center justify-center min-h-screen">
        <p class="text-gray-600">Your cart is empty. Start shopping!</p>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        // Dummy cart items data
        cartItems: [
          {
            id: '001',
            productName: 'Product A',
            price: 29.99,
            quantity: 1
          },
          {
            id: '002',
            productName: 'Product B',
            price: 49.99,
            quantity: 2
          },
          {
            id: '003',
            productName: 'Product C',
            price: 19.99,
            quantity: 3
          }
        ]
      };
    },
    computed: {
      // Calculate total cost
      totalCost() {
        return this.cartItems.reduce((total, item) => {
          return total + item.price * item.quantity;
        }, 0);
      }
    },
    methods: {
      // Remove item from cart
      removeItem(itemId) {
        this.cartItems = this.cartItems.filter(item => item.id !== itemId);
      },
      // Handle checkout process
      checkout() {
        alert('Proceeding to checkout...');
        // Here you can add logic to navigate to the checkout page or process the order
      }
    }
  };
  </script>
  
  <style scoped>
  /* Add any additional styles specific to the shopping cart component here */
  </style>
  