// store/axiosInstance.js
import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'https://localhost:7084/', // Adjust the base URL
  headers: {
    'Content-Type': 'application/json' // Ensure content type is set to JSON
  }
});

// Add a request interceptor to include the token in headers
axiosInstance.interceptors.request.use(
  config => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  error => Promise.reject(error)
);

export default axiosInstance;
