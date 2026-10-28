<template>
  <div style="padding: 20px;">
    <el-page-header @back="$router.back()" content="工作包详情" />

    <div v-loading="loading" style="margin-top: 20px;">
      <!-- 基础信息 -->
      <el-descriptions :column="2" border v-if="wp">
        <el-descriptions-item label="工作包编号">{{ wp.code }}</el-descriptions-item>
        <el-descriptions-item label="工作包名称">{{ wp.name }}</el-descriptions-item>
        <el-descriptions-item label="所属项目">{{ wp.projectName }}</el-descriptions-item>
        <el-descriptions-item label="负责人">{{ wp.owner }}</el-descriptions-item>
        <el-descriptions-item label="状态">
          <el-tag :type="wp.status === 1 ? 'success' : 'info'">
            {{ wp.status === 1 ? '启用' : '停用' }}
          </el-tag>
        </el-descriptions-item>
      </el-descriptions>

      <!-- 台账列表 -->
      <div style="margin-top: 24px; display: flex; justify-content: space-between; align-items: center;">
        <h3>月度台账</h3>
        <el-button type="primary" @click="openCreate">新增台账</el-button>
      </div>

      <el-table :data="wp?.ledgers || []" border>
        <el-table-column prop="yearMonth" label="年月" width="120" />
        <el-table-column prop="cashflow" label="现金流" />
        <el-table-column prop="funding" label="资金投入" />
        <el-table-column label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">
              {{ row.status === 1 ? '有效' : '作废' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180">
          <template #default="{ row }">
            <el-button
              v-if="row.status === 1"
              size="small"
              type="warning"
              @click="openEdit(row)"
            >编辑</el-button>
            <el-button
              v-if="row.status === 1"
              size="small"
              type="danger"
              @click="handleVoid(row.id)"
            >作废</el-button>
            <el-tag v-if="row.status !== 1" type="info">已作废</el-tag>
          </template>
        </el-table-column>
      </el-table>

      <!-- 提交汇总 -->
      <h3 style="margin-top: 24px;">提交汇总</h3>
      <el-form inline>
        <el-form-item label="年月">
          <el-select
            v-model="submitForm.yearMonth"
            placeholder="选择有效台账年月"
            style="width: 220px;"
            :disabled="activeLedgers.length === 0"
          >
            <el-option
              v-for="ledger in activeLedgers"
              :key="ledger.id"
              :label="`${ledger.yearMonth}（现金流 ${ledger.cashflow} / 资金 ${ledger.funding}）`"
              :value="ledger.yearMonth"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            @click="handleSubmit"
            :disabled="!submitForm.yearMonth"
          >提交到项目台账</el-button>
        </el-form-item>
      </el-form>
      <p v-if="activeLedgers.length === 0" style="color: #999; font-size: 12px;">
        暂无有效台账，请先新增台账后再提交汇总。
      </p>
    </div>

    <!-- 新增/编辑台账弹窗 -->
    <el-dialog
      v-model="dialogVisible"
      :title="form.id ? '编辑台账' : '新增台账'"
      width="500px"
    >
      <el-form :model="form" label-width="100px">
        <el-form-item label="年月">
          <el-input
            v-model.number="form.yearMonth"
            placeholder="如 202401"
            :disabled="!!form.id"
          />
        </el-form-item>
        <el-form-item label="现金流">
          <el-input-number v-model="form.cashflow" :min="0" style="width: 100%;" />
        </el-form-item>
        <el-form-item label="资金投入">
          <el-input-number v-model="form.funding" :min="0" style="width: 100%;" />
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="form.remark" type="textarea" :rows="2" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSave">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import {
  getWorkPackageDetail,
  createWorkPackageLedger,
  updateWorkPackageLedger,
  voidWorkPackageLedger,
  submitWorkPackage,
} from '@/api'
import { ElMessage, ElMessageBox } from 'element-plus'

const route = useRoute()
const wpId = route.params.id

const wp = ref(null)
const loading = ref(false)
const dialogVisible = ref(false)

const form = ref({
  id: null,
  yearMonth: null,
  cashflow: 0,
  funding: 0,
  remark: '',
})

const submitForm = ref({ yearMonth: null })

// 只列有效台账，供提交汇总下拉使用
const activeLedgers = computed(() =>
  (wp.value?.ledgers || []).filter((l) => l.status === 1)
)

async function loadData() {
  loading.value = true
  try {
    wp.value = await getWorkPackageDetail(wpId)
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}

function openCreate() {
  form.value = { id: null, yearMonth: null, cashflow: 0, funding: 0, remark: '' }
  dialogVisible.value = true
}

function openEdit(row) {
  form.value = {
    id: row.id,
    yearMonth: row.yearMonth,
    cashflow: row.cashflow,
    funding: row.funding,
    remark: row.remark || '',
  }
  dialogVisible.value = true
}

async function handleSave() {
  try {
    if (form.value.id) {
      await updateWorkPackageLedger(wpId, form.value.id, {
        id: form.value.id,
        cashflow: form.value.cashflow,
        funding: form.value.funding,
        remark: form.value.remark,
      })
    } else {
      await createWorkPackageLedger(wpId, {
        yearMonth: form.value.yearMonth,
        cashflow: form.value.cashflow,
        funding: form.value.funding,
        remark: form.value.remark,
      })
    }
    ElMessage.success('保存成功')
    dialogVisible.value = false
    loadData()
  } catch (e) {
    console.error(e)
  }
}

async function handleVoid(id) {
  await ElMessageBox.confirm('确定作废该台账吗？', '提示', { type: 'warning' })
  try {
    await voidWorkPackageLedger(wpId, id)
    ElMessage.success('已作废')
    loadData()
  } catch (e) {
    console.error(e)
  }
}

async function handleSubmit() {
  try {
    await submitWorkPackage(wpId, { yearMonth: submitForm.value.yearMonth })
    ElMessage.success('提交成功，已汇总到项目台账')
    submitForm.value.yearMonth = null
  } catch (e) {
    console.error(e)
  }
}

onMounted(loadData)
</script>