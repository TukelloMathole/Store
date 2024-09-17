<template>
  <div class="dashboard-container">
    <p class="title">User Dashboard</p>
    <p class="message">Manage your products and settings here.</p>
    <button @click="logout" class="logout-button">Log Out</button>
    
    <!-- Sorting and Filtering -->
    <div class="filters">
      <label for="sort">
        <input id="sort" v-model="sortOption" type="text" class="input" placeholder=" ">
        <span>Sort by:</span>
        <select v-model="sortOption">
          <option value="name">Name</option>
          <option value="price">Price</option>
        </select>
      </label>
    </div>

    <!-- Product List and Pagination -->
    <div class="product-management">
      <ul class="product-list" v-if="currentProducts.length">
        <li class="product-item" v-for="product in currentProducts" :key="product.id" @click="showProductDetails(product)">
          <h3 class="product-name">{{ product.name }}</h3>
          <p class="product-description"><strong>Description:</strong> {{ product.description }}</p>
          <p class="product-price"><strong>Price:</strong> R{{ product.price }}</p>
          <p class="product-stock"><strong>Stock:</strong> {{ product.stock }}</p>
        </li>
      </ul>
      <div v-else class="no-products">No products available</div>

      <!-- Pagination Controls -->
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
        <span>Page {{ currentPage }} of {{ totalPages }}</span>
        <button @click="nextPage" :disabled="currentPage === totalPages">Next</button>
      </div>
    </div>

    <!-- Product Details Modal -->
    <div v-if="selectedProduct" class="modal-overlay">
      <div class="product-details-modal">
        <h2>{{ selectedProduct.name }}</h2>
        <p><strong>Description:</strong> {{ selectedProduct.description }}</p>
        <p><strong>Price:</strong> R{{ selectedProduct.price }}</p>
        <p><strong>Stock:</strong> {{ selectedProduct.stock }}</p>
        <button @click="selectedProduct = null">Close</button>
      </div>
    </div>
  </div>
</template>

<script>
import { useStore } from 'vuex';
import { onMounted, ref, computed } from 'vue';
import { useRouter } from 'vue-router';

export default {
  name: 'UserDashboard',
  setup() {
    const store = useStore();
    const router = useRouter();
    const selectedProduct = ref(null);
    const sortOption = ref('name');
    const currentPage = ref(1);
    const pageSize = 10; // Number of products per page
    const products = computed(() => store.state.products.products);

    onMounted(() => {
      store.dispatch('products/fetchUserProducts');
    });

    const totalPages = computed(() => Math.ceil(products.value.length / pageSize));

    // Sort products based on the selected option
    const sortedProducts = computed(() => {
      return products.value.slice().sort((a, b) => {
        if (sortOption.value === 'price') {
          return a.price - b.price;
        }
        return a.name.localeCompare(b.name);
      });
    });

    const currentProducts = computed(() => {
      const start = (currentPage.value - 1) * pageSize;
      const end = start + pageSize;
      return sortedProducts.value.slice(start, end);
    });

    const prevPage = () => {
      if (currentPage.value > 1) currentPage.value--;
    };

    const nextPage = () => {
      if (currentPage.value < totalPages.value) currentPage.value++;
    };

    const showProductDetails = (product) => {
      selectedProduct.value = product;
    };

    // Log out method
    const logout = () => {
      try {
        // Remove tokens from local storage
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        
        // Clear Vuex store
        store.commit('auth/clearUser');
        store.commit('auth/clearTokens');
        
        // Redirect to login page
        router.push('/login');
      } catch (error) {
        console.error('Logout failed:', error.message); // Log the error message
        alert('Logout failed. Please try again.'); // Notify user of the error
      }
    };

    return {
      currentProducts,
      totalPages,
      currentPage,
      prevPage,
      nextPage,
      sortOption,
      showProductDetails,
      selectedProduct,
      logout
    };
  }
};
</script>

<style scoped>
.dashboard-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.title {
  font-size: 2rem;
  color: royalblue;
  font-weight: 600;
  margin-bottom: 10px;
}

.message {
  color: rgba(88, 87, 87, 0.822);
  font-size: 14px;
  margin-bottom: 20px;
  text-align: center;
}

.filters {
  margin-bottom: 20px;
}

.filters label {
  display: flex;
  flex-direction: column;
  margin-bottom: 10px;
}

.filters span {
  font-size: 0.9em;
  color: grey;
}

.filters select {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 10px;
}

.product-management {
  margin-top: 20px;
}

.product-list {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

.product-item {
  border: 1px solid #ddd;
  border-radius: 8px;
  margin-bottom: 15px;
  padding: 15px;
  background-color: #ffffff;
}

.product-name {
  font-size: 1.25rem;
  color: royalblue;
  margin-bottom: 10px;
}

.product-description,
.product-price,
.product-stock {
  color: rgba(88, 87, 87, 0.822);
  font-size: 0.9em;
}

.no-products {
  text-align: center;
  font-size: 1.1em;
  color: #666;
}

.pagination {
  text-align: center;
  margin: 20px 0;
}

.pagination button {
  padding: 10px 20px;
  margin: 0 5px;
  border: none;
  background-color: royalblue;
  color: #fff;
  border-radius: 10px;
  cursor: pointer;
  font-size: 0.9em;
}

.pagination button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.logout-button {
  padding: 12px 24px;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-size: 16px;
  font-weight: 500;
  transition: background-color 0.3s ease;
  background-color: #e53e3e;
  color: white;
}

.logout-button:hover {
  background-color: rgb(56, 90, 194);
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.product-details-modal {
  background-color: #ffffff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  width: 80%;
  max-width: 500px;
}

.product-details-modal h2 {
  font-size: 1.25rem;
  color: royalblue;
}

.product-details-modal button {
  padding: 10px 20px;
  border: none;
  background-color: royalblue;
  color: white;
  border-radius: 10px;
  cursor: pointer;
  font-size: 0.9em;
}

.product-details-modal button:hover {
  background-color: rgb(56, 90, 194);
}
</style>
