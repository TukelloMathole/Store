<template>
  <div class="container mx-auto p-6 grid grid-cols-1 md:grid-cols-2 gap-6">
    <!-- Profile Information Tile -->
    <div class="bg-white p-6 rounded-lg shadow-lg">
      <h2 class="text-xl font-semibold text-gray-800 mb-4">Profile Information</h2>
      <form @submit.prevent="updateProfile">
        <div class="mt-4">
          <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
          <input 
            type="text" 
            v-model="user.name" 
            id="name" 
            class="mt-2 p-3 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <div class="mt-4">
          <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
          <input 
            type="email" 
            v-model="user.email" 
            id="email" 
            class="mt-2 p-3 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <button 
          type="submit" 
          class="mt-6 w-full px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200"
        >
          Update Profile
        </button>
      </form>
    </div>

    <!-- Password Update Tile -->
    <div class="bg-white p-6 rounded-lg shadow-lg">
      <h2 class="text-xl font-semibold text-gray-800 mb-4">Update Password</h2>
      <form @submit.prevent="updatePassword">
        <div class="mt-4">
          <label for="currentPassword" class="block text-sm font-medium text-gray-700">Current Password</label>
          <input 
            type="password" 
            v-model="passwords.currentPassword" 
            id="currentPassword" 
            class="mt-2 p-3 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <div class="mt-4">
          <label for="newPassword" class="block text-sm font-medium text-gray-700">New Password</label>
          <input 
            type="password" 
            v-model="passwords.newPassword" 
            id="newPassword" 
            class="mt-2 p-3 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <div class="mt-4">
          <label for="confirmPassword" class="block text-sm font-medium text-gray-700">Confirm New Password</label>
          <input 
            type="password" 
            v-model="passwords.confirmPassword" 
            id="confirmPassword" 
            class="mt-2 p-3 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <button 
          type="submit" 
          class="mt-6 w-full px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors duration-200"
        >
          Update Password
        </button>
      </form>
    </div>

    <!-- Personal Settings Tile -->
    <div class="bg-white p-6 rounded-lg shadow-lg">
      <h2 class="text-xl font-semibold text-gray-800 mb-4">Personal Settings</h2>
      <form @submit.prevent="updateSettings">
        <div class="mt-4">
          <label for="language" class="block text-sm font-medium text-gray-700">Preferred Language</label>
          <select 
            v-model="settings.language" 
            id="language" 
            class="mt-2 p-3 w-full border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          >
            <option value="en">English</option>
            <option value="es">Spanish</option>
            <option value="fr">French</option>
          </select>
        </div>
        <div class="mt-4">
          <label class="block text-sm font-medium text-gray-700">Email Notifications</label>
          <input 
            type="checkbox" 
            v-model="settings.notifications" 
            id="notifications" 
            class="mt-2"
          />
          <label for="notifications" class="ml-2 text-gray-700">Receive email notifications</label>
        </div>
        <button 
          type="submit" 
          class="mt-6 w-full px-4 py-2 bg-yellow-600 text-white rounded-lg hover:bg-yellow-700 transition-colors duration-200"
        >
          Update Settings
        </button>
      </form>
    </div>
  </div>
</template>

<script>
import AuthService from '@/services/AuthService'; // Adjust the import as necessary

export default {
  data() {
    return {
      user: {
        name: '',
        email: '',
      },
      passwords: {
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
      },
      settings: {
        language: 'en', // Default to English
        notifications: true, // Default to receiving notifications
      },
    };
  },
  methods: {
    async getUserData() {
      try {
        const userData = await AuthService.getUserData();
        this.user = userData; // Populate the user data in the form
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    },
    async updateProfile() {
      try {
        const response = await AuthService.updateProfile(this.user);
        console.log('Profile updated:', response.data);
        // Handle success (e.g., show a success message)
      } catch (error) {
        console.error('Error updating profile:', error);
        // Handle error (e.g., show an error message)
      }
    },
    async updatePassword() {
      // Handle password update logic here
      console.log('Password updated:', this.passwords);
    },
    async updateSettings() {
      // Handle personal settings update logic here
      console.log('Settings updated:', this.settings);
    },
  },
  async mounted() {
    await this.getUserData(); // Fetch user data when the component mounts
  },
};
</script>

<style scoped>
/* Optional: Add custom styles */
</style>
