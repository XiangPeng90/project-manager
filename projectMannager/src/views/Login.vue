<template>
  <div style="width: 400px; margin: 120px auto;">
    <h2>项目管理系统登录</h2>
    <el-form :model="form" label-width="80px">
      <el-form-item label="用户名">
        <el-input v-model="form.username" />
      </el-form-item>
      <el-form-item label="密码">
        <el-input v-model="form.password" type="password" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="handleLogin" :loading="loading">登录</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { login } from '@/api'
import { ElMessage } from 'element-plus'

const router = useRouter()
const loading = ref(false)
const form = ref({ username: 'admin', password: '123456' })

async function handleLogin() {
  loading.value = true
  try {
    const data = await login(form.value)
    localStorage.setItem('token', data.token)
    localStorage.setItem('displayName', data.displayName)
    ElMessage.success('登录成功')
    router.push('/projects')
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}
</script>
