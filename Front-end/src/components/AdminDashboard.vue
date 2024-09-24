<template>
  <div class="admin-dashboard">
    <div class="admin-dashboard-header">
      <h1 class="title">Admin Dashboard</h1>
      <button class="button float-right">
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"
          class="w-6 h-6">
          <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 12h15m0 0l-6.75-6.75M19.5 12l-6.75 6.75"></path>
        </svg>
        <div class="text">
          Export
        </div>
      </button>
    </div>
    <div class="dashboard-content">
      <!-- User Registration Trends -->
      <div class="stat-card">
        <h3>User Registration Trends</h3>
        <canvas id="userTrendChart"></canvas>
      </div>

      <!-- Active Users -->
      <div class="stat-card">
        <h3>Active Users</h3>
        <p>{{ activeUsersCount }}</p>
      </div>

      <!-- Sales Overview -->
      <div class="stat-card">
        <h3>Sales Overview</h3>
        <canvas id="salesOverviewChart"></canvas>
      </div>

      <!-- Top Products -->
      <div class="stat-card">
        <h3>Top Products</h3>
        <ul>
          <li v-for="product in topProducts" :key="product.id">{{ product.name }} - {{ product.sales }} sold</li>
        </ul>
      </div>

      <!-- Pending Orders -->
      <div class="stat-card">
        <h3>Pending Orders</h3>
        <p>{{ pendingOrdersCount }}</p>
      </div>

      <!-- Customer Feedback Rating -->
      <div class="stat-card">
        <h3>Average Feedback Rating</h3>
        <p>{{ averageRating }} ★</p>
      </div>

      <!-- Traffic Sources -->
      <div class="stat-card">
        <h3>Traffic Sources</h3>
        <canvas id="trafficSourcesChart"></canvas>
      </div>

      <!-- Session Duration -->
      <div class="stat-card">
        <h3>Average Session Duration</h3>
        <p>{{ averageSessionDuration }} minutes</p>
      </div>

      <!-- Tasks Overview -->
      <div class="stat-card">
        <h3>Tasks Overview</h3>
        <p>{{ tasksCompletionPercentage }}% completed</p>
      </div>
    </div>
  </div>
</template>

<script>
// Import Chart.js
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

export default {
  data() {
    return {
      activeUsersCount: 120,
      pendingOrdersCount: 5,
      averageRating: 4.5,
      averageSessionDuration: 5,
      tasksCompletionPercentage: 75,
      topProducts: [
        { id: 1, name: 'Game A', sales: 150 },
        { id: 2, name: 'Game B', sales: 90 },
        { id: 3, name: 'Game C', sales: 70 },
      ],
    };
  },
  mounted() {
    this.renderCharts();
  },
  methods: {
    renderCharts() {
      // User trend chart
      const ctx1 = document.getElementById('userTrendChart').getContext('2d');
      new Chart(ctx1, {
        type: 'line',
        data: {
          labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
          datasets: [{
            label: 'User Registrations',
            data: [10, 20, 15, 25],
            borderColor: '#60a5fa',
            backgroundColor: 'rgba(96, 165, 250, 0.2)',
            fill: true,
          }]
        },
      });

      // Sales overview chart
      const ctx2 = document.getElementById('salesOverviewChart').getContext('2d');
      new Chart(ctx2, {
        type: 'bar',
        data: {
          labels: ['Product A', 'Product B', 'Product C'],
          datasets: [{
            label: 'Sales',
            data: [150, 90, 70],
            backgroundColor: '#34d399',
          }]
        },
      });

      // Traffic sources chart
      const ctx3 = document.getElementById('trafficSourcesChart').getContext('2d');
      new Chart(ctx3, {
        type: 'pie',
        data: {
          labels: ['Direct', 'Referral', 'Social'],
          datasets: [{
            label: 'Traffic Sources',
            data: [40, 30, 30],
            backgroundColor: ['#60a5fa', '#34d399', '#fbbf24'],
          }]
        },
      });
    },
  },
};
</script>

<style scoped>
.admin-dashboard {
  width: 100vw;
  /* Full screen width */
  height: 100vh;
  /* Full screen height */
  padding: 20px;
  background-color: #1f2937;
  /* Dark background for modern look */
  color: #f9f9f9;
  /* Lighter text color for contrast */
  overflow-y: auto;
  /* Enable vertical scrolling */
}
.admin-dashboard-header {
  display: flex; /* Use flexbox for alignment */
  justify-content: space-between; /* Space between items */
  align-items: center; /* Center items vertically */
  padding: 10px; /* Add padding */
  margin: 10px;
  background-color: #2a2e37; /* Dark background */
  border-bottom: 2px solid #60a5fa; /* Light blue border at the bottom */
}
.title {
  font-size: 2.5rem; /* Larger font for the title */
  color: #60a5fa; /* Light blue color */
  font-weight: 700; /* Bold text */
  margin: 0; /* Remove margin */
}
.float-right {
  margin-left: auto; /* Float button to the right */
}

.message {
  font-size: 16px;
  color: rgba(229, 231, 235, 0.9);
  /* Light gray text */
  margin-bottom: 30px;
  text-align: center;
}

button.logout {
  background-color: #ef4444;
  /* Modern red */
  color: white;
  padding: 12px 30px;
  border: none;
  border-radius: 50px;
  /* Rounded button */
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s ease;
  display: block;
  margin: 0 auto 30px auto;
  /* Centered button */
}

button.logout:hover {
  background-color: #dc2626;
  /* Darker red on hover */
}

.dashboard-content {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  /* Flexible grid layout */
  gap: 30px;
}

/* Statistics Section */
.stats {
  grid-column: span 2;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  /* Flexible columns for tile view */
  gap: 20px;
  /* Gap between cards */
  margin-top: 20px;
  /* Add some space above */
}

.stat-card {
  background-color: #374151;
  /* Dark card background */
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
  /* Soft shadow */
  text-align: center;
  transition: transform 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
  /* Slight lift on hover */
}

.stat-card h3 {
  color: #60a5fa;
  /* Light blue for headers */
  margin-bottom: 15px;
  font-size: 1.2rem;
}

.stat-card p {
  font-size: 2rem;
  font-weight: 700;
  color: #f9f9f9;
}

/* Circle Chart for Actions Needed */
.circle-chart {
  position: relative;
  width: 120px;
  height: 120px;
  margin: 0 auto;
}

.circular-chart {
  width: 120px;
  height: 120px;
  transform: rotate(-90deg);
}

.circle-bg {
  fill: none;
  stroke: #e6e6e6;
  stroke-width: 3.8;
}

.circle {
  fill: none;
  stroke: #34d399;
  /* Green for progress */
  stroke-width: 4;
  stroke-linecap: round;
  animation: progress 1s ease-out forwards;
}

.actions-card p {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  font-size: 1.2rem;
  color: #34d399;
  /* Green text inside circle */
}

/* Quick Links Section */
.quick-links {
  grid-column: span 2;
}

.quick-links ul {
  list-style: none;
  padding: 0;
}

.quick-links li {
  margin-bottom: 15px;
  cursor: pointer;
  color: #60a5fa;
  /* Modern blue */
  font-weight: 600;
  font-size: 1rem;
}

.quick-links li:hover {
  text-decoration: underline;
}

/* Task Management Section */
.tasks {
  grid-column: span 2;
}

.tasks ul {
  list-style: none;
  padding: 0;
}

.tasks li {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
}

.tasks li .completed {
  text-decoration: line-through;
  color: gray;
}

/* Utility for full-screen layout */
body {
  margin: 0;
  font-family: 'Inter', sans-serif;
  /* Modern font */
  background-color: #1f2937;
  /* Match background */
  color: white;
}


/* From Uiverse.io by reshades */
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

.button svg {
  width: 1.6em;
  margin: -0.2em 0.8em 1em;
  position: absolute;
  display: flex;
  transition: all 0.6s ease;
}

.button:hover svg {
  transform: translateX(5px);
}

.text {
  margin: 0 1.5em
}
</style>
