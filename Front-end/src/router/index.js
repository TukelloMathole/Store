import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '../views/Home.vue';
import Login from '../components/Login.vue';
import Register from '../components/Register.vue';
import Profile from '../views/Profile.vue';
import AdminDashboard from '../components/AdminDashboard.vue';
import UserDashboard from '../components/UserDashboard.vue';

const routes = [
  { path: '/', name: 'HomePage', component: HomePage },
  { path: '/login', component: Login, name: 'Login' },
  { path: '/register', component: Register, name: 'Register' },
  { path: '/profile', component: Profile, name: 'Profile' },
  {
    path: '/admin-dashboard',
    component: AdminDashboard,
    name: 'AdminDashboard',
    meta: { requiresAuth: true, role: 'Admin' }
  },
  {
    path: '/user-dashboard',
    component: UserDashboard,
    name: 'UserDashboard',
    meta: { requiresAuth: true, role: 'User' }
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

// Global navigation guard for role-based access control
router.beforeEach((to, from, next) => {
  const isLoggedIn = !!localStorage.getItem('accessToken');
  const role = localStorage.getItem('role');

  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!isLoggedIn) {
      next('/login');
    } else {
      const requiredRole = to.meta.role;
      if (requiredRole && role !== requiredRole) {
        // Redirect to home page or show unauthorized message
        next('/');
      } else {
        next();
      }
    }
  } else {
    next(); // Make sure to always call next()
  }
});

export default router;
