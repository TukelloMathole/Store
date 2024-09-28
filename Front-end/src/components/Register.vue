<template>
  <div class="register-container">
    <p class="title">Register</p>
    <p class="message">Signup now and get full access to our app.</p>
    <form @submit.prevent="handleRegister" class="form">
      <!-- Form Inputs -->
      <label>
        <input v-model="firstName" type="text" class="input" required placeholder=" ">
        <span>Firstname</span>
      </label>
      <label>
        <input v-model="lastName" type="text" class="input" required placeholder=" ">
        <span>Lastname</span>
      </label>
      <label>
        <input v-model="email" type="email" class="input" required placeholder=" ">
        <span>Email</span>
      </label>
      <label>
        <input v-model="password" type="password" class="input" required placeholder=" ">
        <span>Password</span>
      </label>
      <label>
        <input v-model="confirmPassword" type="password" class="input" required placeholder=" ">
        <span>Confirm Password</span>
      </label>
      <button type="submit" class="submit" :disabled="loading">Register</button>
      <div v-if="error" class="error">{{ error }}</div>
      <p class="signin">Already have an account? <router-link to="/login">Login</router-link></p>
    </form>

    <!-- Pop-up for response messages -->
    <div v-if="showPopup" class="popup">
      <div class="popup-content">
        <p>{{ popupMessage }}</p>
        <button @click="closePopup">Close</button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from 'vuex';
import AuthService from '@/services/AuthService';

export default {
  name: 'UserRegister',
  data() {
    return {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
      error: '',
      loading: false,
      popupMessage: '', // Message to display in the pop-up
      showPopup: false  // Control visibility of the pop-up
    };
  },
  methods: {
  ...mapActions('auth', ['register']),
  async handleRegister() {
    this.loading = true;
    this.error = '';
    
    // Check if passwords match
    if (this.password !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      this.loading = false;
      return;
    }
    
    try {
      // Call the AuthService to register the user
      await AuthService.register({
        firstName: this.firstName,
        lastName: this.lastName,
        email: this.email,
        password: this.password
      });
      
      // Show success pop-up
      this.showPopupWithMessage('Registration successful!');
      
      // Delay the navigation to login until after the popup
      setTimeout(() => {
        this.$router.push({ name: 'Login' });
      }, 3000); // Wait for 3 seconds before redirecting to login
    } catch (err) {
      console.error(err);
      // Show error pop-up if registration fails
      this.showPopupWithMessage(err.message || 'Registration failed. Please check your details and try again.');
    } finally {
      this.loading = false;
    }
  },
  
  // Method to show pop-up messages
  showPopupWithMessage(message) {
    this.popupMessage = message;
    this.showPopup = true;
    
    // Auto-hide pop-up after 3 seconds
    setTimeout(() => {
      this.showPopup = false;
    }, 3000);
  },
  
  // Method to close the pop-up
  closePopup() {
    this.showPopup = false;
  }
}


};
</script>

<style scoped>
.register-container {
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
.popup {
  position: fixed;
  top: 20px;
  left: 50%;
  transform: translateX(-50%);
  background-color: rgba(0, 0, 0, 0.8);
  color: #fff;
  padding: 20px;
  border-radius: 10px;
  z-index: 1000;
}

.popup-content {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.popup-content button {
  margin-top: 10px;
  background-color: #fff;
  color: black;
  border: none;
  padding: 5px 10px;
  cursor: pointer;
}

.popup-content button:hover {
  background-color: #ddd;
}
</style>
