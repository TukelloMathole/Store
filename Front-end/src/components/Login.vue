<template>
  <div class="login-container">
    <p class="title">Login</p>
    <p class="message">Sign in to access your dashboard.</p>
    <form @submit.prevent="handleLogin" class="form">
      <label>
        <input v-model="email" type="email" class="input" required placeholder=" ">
        <span>Email</span>
      </label>
      <label>
        <input v-model="password" type="password" class="input" required placeholder=" ">
        <span>Password</span>
      </label>
      <button type="submit" class="submit" :disabled="loading">Login</button>
      <div v-if="error" class="error">{{ error }}</div>
      <p class="signin">Don't have an account? <router-link to="/register">Register</router-link></p>
    </form>
    
    <!-- Popup for success/error messages -->
    <div v-if="showPopup" class="popup-overlay" @click="closePopup">
      <div class="popup">
        <p>{{ popupMessage }}</p>
        <button @click="closePopup">Close</button>
      </div>
    </div>
  </div>
</template>

<script>
import AuthService from '@/services/AuthService';
import { mapMutations } from 'vuex';

export default {
  name: 'UserLogin',
  data() {
    return {
      email: '',
      password: '',
      error: '',
      loading: false,
      showPopup: false,
      popupMessage: ''
    };
  },
  methods: {
  ...mapMutations('auth', ['setTokens', 'setUserRole']),
  async handleLogin() {
    this.loading = true;
    this.error = '';
    try {
      const response = await AuthService.login({ email: this.email, password: this.password });
      const { accessToken, refreshToken, role } = response.data;
      
      // Store tokens and role in Vuex
      this.setTokens({ accessToken, refreshToken });
      this.setUserRole(role); 

      // Show success popup
      this.showPopupWithMessage('Login successful!');
      
      // Delay the navigation until after the popup is shown
      setTimeout(() => {
        // Redirect based on user role
        if (role === 'Admin') {
          this.$router.push({ name: 'AdminDashboard' });
        } else if (role === 'User') {
          this.$router.push({ name: 'UserDashboard' });
        } else {
          console.error('Unknown role:', role);
        }
      }, 3000); // Wait for 3 seconds before redirecting
    } catch (err) {
      console.error('Login error:', err);
      this.showPopupWithMessage('Login failed. Please check your credentials and try again.');
    } finally {
      this.loading = false;
    }
  },
  showPopupWithMessage(message) {
    this.popupMessage = message;
    this.showPopup = true;

    // Auto-hide popup after 3 seconds
    setTimeout(() => {
      this.showPopup = false;
    }, 3000);
  },
  closePopup() {
    this.showPopup = false;
  }
}

};
</script>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 20px;
  background-color: #fff;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.title {
  font-size: 28px;
  color: royalblue;
  font-weight: 600;
  letter-spacing: -1px;
  margin-bottom: 10px;
}

.message {
  color: rgba(88, 87, 87, 0.822);
  font-size: 14px;
  margin-bottom: 20px;
  text-align: center;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 10px;
  width: 100%;
}

.form label {
  position: relative;
}

.form label .input {
  width: 100%;
  padding: 10px;
  outline: 0;
  border: 1px solid rgba(105, 105, 105, 0.397);
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

.form label .input:valid + span {
  color: green;
}

.submit {
  border: none;
  outline: none;
  background-color: royalblue;
  padding: 10px;
  border-radius: 10px;
  color: #fff;
  font-size: 16px;
  cursor: pointer;
  transform: 0.3s ease;
}

.submit:hover {
  background-color: rgb(56, 90, 194);
}

.error {
  color: red;
  margin-top: 10px;
  text-align: center;
}

.signin {
  text-align: center;
  color: rgba(88, 87, 87, 0.822);
  margin-top: 10px;
}

.signin a {
  color: royalblue;
}

.signin a:hover {
  text-decoration: underline royalblue;
}

/* Popup styles */
.popup-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2000;
}

.popup {
  background-color: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  text-align: center;
}

.popup button {
  margin-top: 15px;
  padding: 8px 12px;
  background-color: royalblue;
  border: none;
  border-radius: 5px;
  color: white;
  cursor: pointer;
}

.popup button:hover {
  background-color: rgb(56, 90, 194);
}
</style>
