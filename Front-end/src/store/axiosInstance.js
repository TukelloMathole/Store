import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; // Import a library to decode JWT tokens
import Cookies from 'js-cookie'; // Import js-cookie to manage cookies

const axiosInstance = axios.create({
  baseURL: 'https://localhost:7084/', // Adjust the base URL
  headers: {
    'Content-Type': 'application/json' // Ensure content type is set to JSON
  }
});

// Add a request interceptor to include the token in headers and validate it
axiosInstance.interceptors.request.use(
  config => {
    const token = Cookies.get('accessToken'); // Get the token from cookies
    console.log(token);
    if (token) {
      // Check if the token is valid
      console.log(token)
      try {
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000; // Get current time in seconds
        console.log(decodedToken);
        // If the token is expired
        if (decodedToken.exp < currentTime) {
          // Token is invalid, log the user out
          Cookies.remove('accessToken'); // Remove access token from cookies
          Cookies.remove('refreshToken'); // Clear refresh token if applicable
          
          // Optionally, redirect to login
          window.location.href = '/login'; // Change this to your login route
          return Promise.reject('Token has expired. User logged out.');
        }

        // If valid, set the Authorization header
        config.headers.Authorization = `Bearer ${token}`;
      } catch (error) {
        // If decoding fails, treat it as an invalid token
        Cookies.remove('accessToken'); // Remove access token from cookies
        Cookies.remove('refreshToken'); // Clear refresh token if applicable
        window.location.href = '/login'; // Change this to your login route
        return Promise.reject('Invalid token. User logged out.');
      }
    }
    return config;
  },
  error => Promise.reject(error)
);

export default axiosInstance;
