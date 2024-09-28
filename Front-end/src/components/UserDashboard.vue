<template>
  <div class="user-dashboard">
    <div class="user-dashboard-header">
      <h1 class="title">User Dashboard</h1>
    </div>
    <div class="dashboard-content">
      <div class="stat-card">
        <h3>Recommended Games</h3>
        <ul>
          <li v-for="game in recommendedGames" :key="game.id">{{ game.name }} - R{{ game.price }}</li>
        </ul>
      </div>

      <div class="stat-card">
        <h3>Recently Played</h3>
        <ul>
          <li v-for="game in recentlyPlayed" :key="game.id">{{ game.name }} - Played on: {{ game.playedDate }}</li>
        </ul>
      </div>

      <div class="stat-card">
        <h3>Upcoming Releases</h3>
        <ul>
          <li v-for="game in upcomingReleases" :key="game.id">{{ game.name }} - Release Date: {{ game.releaseDate }}</li>
        </ul>
      </div>

      <div class="stat-card">
        <h3>Your Wishlist</h3>
        <ul>
          <li v-for="game in wishlist" :key="game.id">{{ game.name }} - R{{ game.price }}</li>
        </ul>
      </div>

      <div class="stat-card">
        <h3>Notifications</h3>
        <ul>
          <li v-for="notification in notifications" :key="notification.id">{{ notification.message }}</li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      recommendedGames: [
        { id: 1, name: 'Game A', price: 200 },
        { id: 2, name: 'Game B', price: 150 },
      ],
      recentlyPlayed: [
        { id: 1, name: 'Game C', playedDate: '2024-09-28' },
        { id: 2, name: 'Game D', playedDate: '2024-09-27' },
      ],
      upcomingReleases: [
        { id: 1, name: 'Game E', releaseDate: '2024-10-15' },
        { id: 2, name: 'Game F', releaseDate: '2024-11-01' },
      ],
      wishlist: [
        { id: 1, name: 'Game G', price: 250 },
        { id: 2, name: 'Game H', price: 300 },
      ],
      notifications: [
        { id: 1, message: 'Game A is now available!' },
        { id: 2, message: 'Don’t miss the sale on Game B!' },
      ],
    };
  },
  methods: {
    logout() {
      alert('Logged out');
    },
  },
};
</script>

<style scoped>
.user-dashboard {
  width: 100vw;
  height: 100vh;
  padding: 20px;
  background-color: #1f2937;
  color: #f9f9f9;
  overflow-y: auto;
}
.user-dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px;
  margin: 10px;
  background-color: #2a2e37;
  border-bottom: 2px solid #60a5fa;
}
.title {
  font-size: 2.5rem;
  color: #60a5fa;
  font-weight: 700;
  margin: 0;
}
.dashboard-content {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 30px;
}
.stat-card {
  background-color: #374151;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
  text-align: center;
  transition: transform 0.3s ease;
}
.stat-card:hover {
  transform: translateY(-5px);
}
.stat-card h3 {
  color: #60a5fa;
  margin-bottom: 15px;
}
.stat-card ul {
  list-style: none;
  padding: 0;
}
.stat-card li {
  margin-bottom: 10px;
}
.button {
  background-color: #ffffff00;
  color: #fff;
  width: 8.5em;
  height: 2.9em;
  border: #3654ff 0.2em solid;
  border-radius: 11px;
  text-align: right;
  transition: all 0.6s ease;
}
.button:hover {
  background-color: #3654ff;
  cursor: pointer;
}
.text {
  margin: 0 1.5em;
}
</style>
