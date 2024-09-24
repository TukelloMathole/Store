<template>
    <nav class="flex justify-between items-center p-4 bg-gray-800 text-white shadow-lg">
        <div class="text-lg font-bold">Game Haven</div>
        <ul class="flex space-x-6">
            <li>
                <router-link to="/" class="hover:underline hover:text-blue-400 transition">Home</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'User'">
                <router-link to="/products" class="hover:underline hover:text-blue-400 transition">Products</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'User'">
                <router-link to="/cart" class="hover:underline hover:text-blue-400 transition">Shopping Cart</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'User'">
                <router-link to="/profile" class="hover:underline hover:text-blue-400 transition">Profile</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'User'">
                <router-link to="/orders" class="hover:underline hover:text-blue-400 transition">Order History</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'Admin'">
                <router-link to="/admin/dashboard" class="hover:underline hover:text-blue-400 transition">Admin Dashboard</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'Admin'">
                <router-link to="/admin/manage-users" class="hover:underline hover:text-blue-400 transition">Manage Users</router-link>
            </li>
            <li v-if="isAuthenticated && userRole === 'Admin'">
                <router-link to="/admin/manage-products" class="hover:underline hover:text-blue-400 transition">Manage Products</router-link>
            </li>
            <li v-if="!isAuthenticated">
                <router-link to="/login" class="hover:underline hover:text-blue-400 transition">Login</router-link>
            </li>
            <li v-if="isAuthenticated">
                <a href="#" @click.prevent="handleLogout" class="hover:underline hover:text-blue-400 transition">Logout</a>
            </li>
        </ul>
    </nav>
</template>

<script>
import { mapGetters } from 'vuex';
import AuthService from '@/services/AuthService'; // Import AuthService

export default {
    computed: {
        ...mapGetters('auth', ['isAuthenticated', 'userRole']),
    },
    methods: {
        async handleLogout() {
            try {
                await AuthService.logout(); // Call the AuthService logout method
                this.$router.push('/login'); // Redirect to login after successful logout
            } catch (error) {
                console.error('Logout failed:', error.message);
            }
        }
    }
};
</script>

<style scoped>
/* Add any additional styles here if needed */
</style>
