import { createRouter, createWebHistory } from 'vue-router';
import store from '../store'; // Import the Vuex store

import HomePage from '@/views/Home.vue';
import LoginPage from '../components/Login.vue';
import RegisterPage from '../components/Register.vue';
import UserDashboard from '../components/UserDashboard.vue';
import AdminDashboard from '../components/AdminDashboard.vue';
import UserProfile from '../views/UserProfile.vue'; // User profile component
import OrderHistory from '../views/OrderHistory.vue'; // Order history component
import ManageUsers from '../views/ManageUsers.vue'; // Component for managing users
import ManageProducts from '../views/ManageProducts.vue'; // Component for managing products
import ShoppingCart from '../views/shopping-cart.vue';
import UserProducts from '../views/UserProducts.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginPage,
    meta: { requiresGuest: true } // Only accessible when not logged in
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterPage,
    meta: { requiresGuest: true } // Only accessible when not logged in
  },
  {
    path: '/user/dashboard',
    name: 'UserDashboard',
    component: UserDashboard,
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: UserProfile, // User profile component
    meta: { requiresAuth: true } // Requires authentication
  },
  {
    path: '/orders',
    name: 'OrderHistory',
    component: OrderHistory, // Order history component
    meta: { requiresAuth: true } // Requires authentication
  },
  {
    path: '/cart',
    name: 'ShoppingCart',
    component: ShoppingCart, // Admin dashboard component
    meta: { requiresAuth: true } // Requires authentication and admin role
  },
  {
    path: '/products',
    name: 'UserProducts',
    component: UserProducts, // Admin dashboard component
    meta: { requiresAuth: true } // Requires authentication and admin role
  },
  {
    path: '/admin/dashboard',
    name: 'AdminDashboard',
    component: AdminDashboard, // Admin dashboard component
    meta: { requiresAuth: true, requiresAdmin: true } // Requires authentication and admin role
  },
  {
    path: '/admin/manage-users',
    name: 'ManageUsers',
    component: ManageUsers, // Component for managing users
    meta: { requiresAuth: true, requiresAdmin: true } // Requires authentication and admin role
  },
  {
    path: '/admin/manage-products',
    name: 'ManageProducts',
    component: ManageProducts, // Component for managing products
    meta: { requiresAuth: true, requiresAdmin: true } // Requires authentication and admin role
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

// Navigation guards for protected and guest routes
router.beforeEach((to, from, next) => {
  const isAuthenticated = store.getters['auth/isAuthenticated'];
  const userRole = store.getters['auth/userRole'];

  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    next({ path: '/login' }); // Redirect to login if not authenticated
  } else if (to.matched.some(record => record.meta.requiresAdmin) && userRole !== 'Admin') {
    next({ path: '/' }); // Redirect to home if not an admin
  } else {
    next(); // Proceed to the route
  }
});

export default router;
