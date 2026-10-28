<template>
  <div>
    <div v-if="$route.path !== '/login'" style="padding: 12px 20px; background: #409eff; color: white; display: flex; justify-content: space-between;">
      <span style="cursor: pointer;" @click="$router.push('/projects')">项目管理系统</span>
      <el-button text style="color: white;" @click="handleLogout">退出</el-button>
    </div>
    <router-view />
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router'
const router = useRouter()
function handleLogout() {
  localStorage.removeItem('token')
  router.push('/login')
}
</script>

<style>
body { margin: 0; font-family: Arial, sans-serif; }
</style>
