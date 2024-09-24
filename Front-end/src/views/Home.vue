<template>
  <div class="container mx-auto p-8 bg-gray-100 rounded-lg shadow-lg text-center">
    <h1 class="text-5xl font-bold text-gray-800 mb-6">Welcome to Game Haven!</h1>
    <p class="text-xl text-gray-700 mb-4">
      Discover the latest games, consoles, and accessories tailored just for you.
    </p>
    <p class="text-lg text-gray-600 mb-6">
      Join our community of gamers and enjoy exclusive offers, game reviews, and more!
    </p>
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-xl font-semibold mb-2">Latest Releases</h2>
        <p class="text-gray-600">Stay updated with the newest games hitting the shelves.</p>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-xl font-semibold mb-2">Exclusive Offers</h2>
        <p class="text-gray-600">Get special deals on your favorite games and accessories.</p>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-xl font-semibold mb-2">Community Events</h2>
        <p class="text-gray-600">Participate in gaming events and tournaments.</p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'HomePage',
};
</script>

<style scoped>
/* No additional styles needed, everything is handled by Tailwind CSS */
</style>
