import Cookies from 'js-cookie';

class TokenService {
  constructor() {
    this.token = null;
    this.refreshToken = null;
    this.tokenExpiration = null;
  }

  saveTokens({ accessToken, refreshToken, expiration }) {
    this.token = accessToken;
    this.refreshToken = refreshToken;
    this.tokenExpiration = Date.now() + expiration * 1000; // Assuming 'exp' is in seconds

    // Save tokens as cookies
    Cookies.set('accessToken', accessToken, { expires: expiration / 86400, secure: true, sameSite: 'Strict' }); 
    Cookies.set('refreshToken', refreshToken, { expires: 30, secure: true, sameSite: 'Strict' }); 
    Cookies.set('tokenExpiration', this.tokenExpiration);
  }

  getToken() {
    if (this.isTokenValid()) {
      return this.token;
    }
    return Cookies.get('accessToken') || null;
  }

  isTokenValid() {
    return this.token && this.tokenExpiration > Date.now();
  }

  clearTokens() {
    this.token = null;
    this.refreshToken = null;
    this.tokenExpiration = null;

    // Remove cookies
    Cookies.remove('accessToken');
    Cookies.remove('refreshToken');
    Cookies.remove('tokenExpiration');
  }

  async refreshToken() {
    const refreshToken = this.refreshToken || Cookies.get('refreshToken');
    
    if (!refreshToken) {
      this.clearTokens();
      return null; 
    }

    try {
      const response = await fetch('https://yourapi.com/refresh', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ refreshToken }),
      });

      if (!response.ok) {
        throw new Error('Failed to refresh token');
      }

      const { accessToken, refreshToken, exp } = await response.json();
      this.saveTokens({ accessToken, refreshToken, expiration: exp });
      return accessToken; // Return the new access token
    } catch (error) {
      this.clearTokens();
      console.error('Error refreshing token:', error);
      return null; // Refresh failed
    }
  }
}

export default new TokenService();
