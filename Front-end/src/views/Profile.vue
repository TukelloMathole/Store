<template>
  <div class="profile-container">
    <h1>Your Profile</h1>
    <form @submit.prevent="updateProfile">
      <div class="form-group">
        <label for="firstName">First Name:</label>
        <input type="text" v-model="user.firstName" id="firstName" required />
      </div>
      <div class="form-group">
        <label for="lastName">Last Name:</label>
        <input type="text" v-model="user.lastName" id="lastName" required />
      </div>
      <div class="form-group">
        <label for="email">Email:</label>
        <input type="email" v-model="user.email" id="email" required />
      </div>
      <button type="submit" :disabled="loading">Update Profile</button>
      <div v-if="error" class="error">{{ error }}</div>
    </form>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'UserProfile', // Updated component name
  computed: {
    ...mapGetters('auth', ['user']),
  },
  data() {
    return {
      user: { ...this.user }, // Clone the user data for local editing
      error: '',
      loading: false
    };
  },
  methods: {
    ...mapActions('auth', ['updateUser']),
    async updateProfile() {
      this.loading = true;
      this.error = '';
      try {
        await this.updateUser(this.user);
        this.$router.push({ name: 'Dashboard' }); // Redirect after update
      } catch (err) {
        this.error = 'Update failed. Please try again.';
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped>
.profile-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #f9f9f9;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
}

input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
}

button {
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  background-color: #007bff;
  color: white;
  cursor: pointer;
}

button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.error {
  color: red;
  margin-top: 10px;
}
</style>
``
