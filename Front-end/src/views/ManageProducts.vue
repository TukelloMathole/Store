<template>
    <div class="p-4">
      <h1 class="text-2xl font-bold">Manage Products</h1>
      <button @click="addProduct" class="mt-4 mb-4 p-2 bg-blue-600 text-white">Add Product</button>
      <table class="min-w-full">
        <thead>
          <tr>
            <th class="px-4 py-2">Product ID</th>
            <th class="px-4 py-2">Name</th>
            <th class="px-4 py-2">Price</th>
            <th class="px-4 py-2">Stock</th>
            <th class="px-4 py-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in products" :key="product.id">
            <td class="border px-4 py-2">{{ product.id }}</td>
            <td class="border px-4 py-2">{{ product.name }}</td>
            <td class="border px-4 py-2">${{ product.price.toFixed(2) }}</td>
            <td class="border px-4 py-2">{{ product.stock }}</td>
            <td class="border px-4 py-2">
              <button @click="editProduct(product)" class="text-blue-600">Edit</button>
              <button @click="deleteProduct(product.id)" class="text-red-600">Delete</button>
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
      ...mapGetters('product', ['products']) // Assuming product list is stored in Vuex
    },
    methods: {
      addProduct() {
        // Logic to add a product
      },
      editProduct(product) {
        // Logic to edit a product
        console.log('Editing product:', product);
      },
      deleteProduct(productId) {
        // Logic to delete a product
        console.log('Deleting product with ID:', productId);
      }
    }
  };
  </script>
  
  
  <style scoped>
  /* Add styles specific to this component */
  </style>
  