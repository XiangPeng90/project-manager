<template>
  <div style="padding: 20px;">
    <div style="display: flex; justify-content: space-between; margin-bottom: 16px;">
      <h2>项目列表</h2>
      <el-button type="primary" @click="openCreate">新增项目</el-button>
    </div>

    <el-table :data="tableData" border v-loading="loading" style="width: 100%;">
      <el-table-column prop="id" label="ID" width="80" />
      <el-table-column prop="code" label="项目编号" width="150" />
      <el-table-column prop="name" label="项目名称" />
      <el-table-column prop="manager" label="负责人" width="120" />
      <el-table-column label="状态" width="100">
        <template #default="{ row }">
          <el-tag :type="row.status === 1 ? 'success' : 'info'">
            {{ row.status === 1 ? '启用' : '停用' }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="280">
        <template #default="{ row }">
          <el-button size="small" @click="$router.push(`/projects/${row.id}`)">详情</el-button>
          <el-button size="small" type="warning" @click="openEdit(row)">编辑</el-button>
          <el-button size="small" type="danger" @click="handleDisable(row.id)">停用</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="dialogVisible" :title="form.id ? '编辑项目' : '新增项目'" width="500px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="编号"><el-input v-model="form.code" /></el-form-item>
        <el-form-item label="名称"><el-input v-model="form.name" /></el-form-item>
        <el-form-item label="负责人"><el-input v-model="form.manager" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSave">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getProjectList, createProject, updateProject, disableProject } from '@/api'
import { ElMessage, ElMessageBox } from 'element-plus'

const loading = ref(false)
const tableData = ref([])
const dialogVisible = ref(false)
const form = ref({ id: null, code: '', name: '', manager: '' })

async function loadData() {
  loading.value = true
  try {
    const res = await getProjectList({ page: 1, size: 100 })
    tableData.value = res.items || []
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}

function openCreate() {
  form.value = { id: null, code: '', name: '', manager: '' }
  dialogVisible.value = true
}

function openEdit(row) {
  form.value = { ...row }
  dialogVisible.value = true
}

async function handleSave() {
  try {
    if (form.value.id) {
      await updateProject(form.value.id, form.value)
    } else {
      await createProject(form.value)
    }
    ElMessage.success('保存成功')
    dialogVisible.value = false
    loadData()
  } catch (e) {}
}

async function handleDisable(id) {
  await ElMessageBox.confirm('确定停用该项目吗？', '提示', { type: 'warning' })
  await disableProject(id)
  ElMessage.success('已停用')
  loadData()
}

onMounted(loadData)
</script>