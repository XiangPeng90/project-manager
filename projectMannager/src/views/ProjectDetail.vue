<template>
  <div style="padding: 20px;">
    <el-page-header @back="$router.push('/projects')" content="项目详情" />
    <div v-loading="loading" style="margin-top: 20px;">
      <el-descriptions :column="2" border v-if="project">
        <el-descriptions-item label="项目编号">{{ project.code }}</el-descriptions-item>
        <el-descriptions-item label="项目名称">{{ project.name }}</el-descriptions-item>
        <el-descriptions-item label="负责人">{{ project.manager }}</el-descriptions-item>
        <el-descriptions-item label="状态">
          <el-tag :type="project.status === 1 ? 'success' : 'info'">
            {{ project.status === 1 ? '启用' : '停用' }}
          </el-tag>
        </el-descriptions-item>
      </el-descriptions>

      <h3 style="margin-top: 24px;">工作包清单</h3>
      <el-table :data="project?.workPackages || []" border>
        <el-table-column prop="code" label="编号" width="150" />
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="owner" label="负责人" width="120" />
        <el-table-column label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">
              {{ row.status === 1 ? '启用' : '停用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="120">
          <template #default="{ row }">
            <el-button size="small" @click="$router.push(`/workpackages/${row.id}`)">进入</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getProjectDetail } from '@/api'

const route = useRoute()
const projectId = route.params.id
const project = ref(null)
const loading = ref(false)

async function loadData() {
  loading.value = true
  try {
    project.value = await getProjectDetail(projectId)
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}

onMounted(loadData)
</script>