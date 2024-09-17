<template>
  <div class="admin-dashboard">
    <p class="title">Admin Dashboard</p>
    <p class="message">Welcome, Admin! You have full access to the system.</p>
    <button @click="handleLogout" class="logout">Logout</button>

    <div class="product-management">
      <h2>Product Management</h2>

      <form @submit.prevent="handleAddProduct" class="form">
        <label>
          <input v-model="newProduct.name" type="text" class="input" required placeholder=" ">
          <span>Name</span>
        </label>
        <label>
          <input v-model="newProduct.description" type="text" class="input" required placeholder=" ">
          <span>Description</span>
        </label>
        <label>
          <input v-model="newProduct.price" type="number" class="input" step="0.01" required placeholder=" ">
          <span>Price</span>
        </label>
        <label>
          <input v-model="newProduct.stock" type="number" class="input" required placeholder=" ">
          <span>Stock</span>
        </label>
        <button type="submit" class="submit" :disabled="loading">Add Product</button>
      </form>

      <div v-if="products.length" class="product-list">
        <div v-for="product in products" :key="product.id" class="product-card">
          <h3>{{ product.name }}</h3>
          <p>Description: {{ product.description }}</p>
          <p>Price: {{ product.price }}</p>
          <p>Stock: {{ product.stock }}</p>
          <button @click="handleEditProduct(product.id)" class="edit-button">Edit</button>
          <button @click="handleDeleteProduct(product.id)" class="delete-button">Delete</button>
        </div>
      </div>

      <!-- Modal Overlay -->
      <div v-if="editMode" class="modal-overlay"></div>

      <!-- Edit Product Form (Modal) -->
      <div v-if="editMode" class="edit-product">
        <h2>Edit Product</h2>
        <form @submit.prevent="handleUpdateProduct" class="form">
          <label>
            <input v-model="editProduct.name" type="text" class="input" required placeholder=" ">
            <span>Name</span>
          </label>
          <label>
            <input v-model="editProduct.description" type="text" class="input" required placeholder=" ">
            <span>Description</span>
          </label>
          <label>
            <input v-model="editProduct.price" type="number" class="input" step="0.01" required placeholder=" ">
            <span>Price</span>
          </label>
          <label>
            <input v-model="editProduct.stock" type="number" class="input" required placeholder=" ">
            <span>Stock</span>
          </label>
          <button type="submit" class="submit">Update Product</button>
          <button @click="cancelEdit" class="cancel">Cancel</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'AdminDashboard',
  data() {
    return {
      newProduct: {
        name: '',
        description: '',
        price: 0,
        stock: 0
      },
      editProduct: null,
      editMode: false
    };
  },
  computed: {
    ...mapGetters('products', ['products', 'selectedProduct'])
  },
  methods: {
    ...mapActions('products', ['fetchProducts', 'addProduct', 'updateProduct', 'deleteProduct', 'fetchProductById']),

    async handleAddProduct() {
      try {
        await this.addProduct(this.newProduct);
        this.newProduct = { name: '', description: '', price: 0, stock: 0 }; // Reset form
        // Optional: Show success notification
      } catch (error) {
        console.error('Failed to add product:', error);
        // Optional: Show user-friendly error notification
      }
    },

    async handleEditProduct(id) {
      try {
        await this.fetchProductById(id);
        this.editProduct = { ...this.selectedProduct }; // Initialize edit form with selected product data
        this.editMode = true;
      } catch (error) {
        console.error('Failed to fetch product:', error);
      }
    },

    async handleUpdateProduct() {
      try {
        await this.updateProduct(this.editProduct);
        this.editProduct = null;
        this.editMode = false;
      } catch (error) {
        console.error('Failed to update product:', error);
      }
    },

    async handleDeleteProduct(id) {
      try {
        await this.deleteProduct(id);
      } catch (error) {
        console.error('Failed to delete product:', error);
      }
    },

    cancelEdit() {
      this.editProduct = null;
      this.editMode = false;
    },

    handleLogout() {
      try {
        // Remove tokens from local storage
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        
        // Clear Vuex store
        this.$store.commit('auth/clearUser');
        this.$store.commit('auth/clearTokens');
        
        // Redirect to login page
        this.$router.push('/login');
      } catch (error) {
        console.error('Logout failed:', error.message); // Log the error message
        alert('Logout failed. Please try again.'); // Notify user of the error
      }
    }
  },
  created() {
    this.fetchProducts();
  }
};
</script>

<style scoped>
.admin-dashboard {
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

button {
  padding: 12px 24px;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-size: 16px;
  font-weight: 500;
  transition: background-color 0.3s ease;
}

button.logout {
  background-color: #e53e3e;
  color: white;
}

button.logout:hover {
  background-color: #c53030;
}

button.submit {
  background-color: royalblue;
  color: white;
}

button.submit:hover {
  background-color: rgb(56, 90, 194);
}

button.cancel {
  background-color: #f56565;
  color: white;
}

button.cancel:hover {
  background-color: #e53e3e;
}

.product-management {
  margin-top: 20px;
}

.product-management .form {
  margin-bottom: 20px;
  background-color: #ffffff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.form label {
  position: relative;
  display: block;
  margin-bottom: 20px;
}

.form label .input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 10px;
}

.form label .input + span {
  position: absolute;
  left: 10px;
  top: 15px;
  color: grey;
  font-size: 0.9em;
  cursor: text;
  transition: 0.3s ease;
}

.form label .input:placeholder-shown + span {
  top: 15px;
  font-size: 0.9em;
}

.form label .input:focus + span,
.form label .input:valid + span {
  top: 30px;
  font-size: 0.7em;
  font-weight: 600;
}

.product-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 20px;
  margin-top: 20px;
  max-height: 400px;
  overflow-y: auto;
}

.product-card {
  background-color: #ffffff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.product-card h3 {
  font-size: 1.25rem;
  color: royalblue;
  margin-bottom: 10px;
}

.product-card p {
  color: rgba(88, 87, 87, 0.822);
  margin-bottom: 8px;
}

.product-card button {
  align-self: flex-start;
  margin-top: 10px;
  padding: 8px 16px;
}

.edit-button {
  background-color: #48bb78;
  color: white;
}

.edit-button:hover {
  background-color: #38a169;
}

.delete-button {
  background-color: #f56565;
  color: white;
}

.delete-button:hover {
  background-color: #e53e3e;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 1000;
}

.edit-product {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: #ffffff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  z-index: 1001;
  width: 80%;
  max-width: 500px;
}
</style>
