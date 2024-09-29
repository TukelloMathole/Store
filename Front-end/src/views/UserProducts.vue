<template>
  <div class="min-h-screen p-6 bg-gray-100">
    <h1 class="text-4xl font-bold mb-6 text-center text-gray-800">Available Products</h1>
    
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="product in products" 
        :key="product.id" 
        class="bg-white shadow-md rounded-lg overflow-hidden transition-transform transform hover:scale-105"
      >
        <img :src="product.image" alt="Product Image" class="w-full h-48 object-cover" />
        <div class="p-4">
          <h2 class="text-xl font-semibold text-gray-800">{{ product.name }}</h2>
          <p class="text-gray-600">{{ product.description }}</p>
          <p class="mt-2 text-lg font-bold text-gray-800">${{ product.price.toFixed(2) }}</p>
          <button 
            @click="addToCart(product.id)" 
            class="mt-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition-colors"
          >
            Add to Cart
          </button>
        </div>
      </div>
    </div>

    <div v-if="!products.length" class="flex items-center justify-center min-h-screen">
      <p class="text-gray-600">No products available at the moment.</p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UserProducts', // Updated to multi-word component name
  data() {
    return {
      // Dummy product data
      products: [
        {
          id: '001',
          name: 'Game A',
          description: 'An exciting adventure game.',
          price: 59.99,
          image: 'https://via.placeholder.com/300x200.png?text=Game+A'
        },
        {
          id: '002',
          name: 'Game B',
          description: 'A thrilling action game.',
          price: 49.99,
          image: 'https://via.placeholder.com/300x200.png?text=Game+B'
        },
        {
          id: '003',
          name: 'Game C',
          description: 'An immersive RPG experience.',
          price: 39.99,
          image: 'https://via.placeholder.com/300x200.png?text=Game+C'
        },
        {
          id: '004',
          name: 'Game D',
          description: 'A multiplayer online battle game.',
          price: 29.99,
          image: 'https://via.placeholder.com/300x200.png?text=Game+D'
        },
        {
          id: '005',
          name: 'Game E',
          description: 'A strategic simulation game.',
          price: 19.99,
          image: 'https://via.placeholder.com/300x200.png?text=Game+E'
        }
      ]
    };
  },
  methods: {
    // Placeholder for adding products to the cart
    addToCart(productId) {
      alert(`Product ${productId} added to cart!`);
      // Implement actual cart logic here
    }
  }
};
</script>

<style scoped>
/* Add any additional styles specific to the UserProducts component here */
</style>
