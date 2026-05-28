<script setup>
import { reactive, ref } from 'vue'
import { message } from 'ant-design-vue'
import { createClass, extractClassApiErrorMessage } from '../api/classApi'

const emit = defineEmits(['created'])

const formRef = ref()
const saving = ref(false)

function createInitialFormState() {
  return {
    className: '',
    courseId: null,
    teacherId: null,
    room: '',
    capacity: null,
    startDate: null,
    endDate: null,
  }
}

const formState = reactive(createInitialFormState())

function validatePositiveInteger(label) {
  return async (_rule, value) => {
    if (value === null || value === undefined || value === '') {
      throw new Error(`Vui long nhap ${label}.`)
    }
    if (!Number.isInteger(value) || value <= 0) {
      throw new Error(`${label} phai la so nguyen duong.`)
    }
  }
}

const rules = {
  className: [{ required: true, message: 'Vui long nhap ten lop hoc.', trigger: 'blur' }],
  courseId: [{ validator: validatePositiveInteger('CourseId'), trigger: 'change' }],
  teacherId: [{ validator: validatePositiveInteger('TeacherId'), trigger: 'change' }],
}

function resetForm() {
  Object.assign(formState, createInitialFormState())
  formRef.value?.clearValidate?.()
}

async function submitForm() {
  try {
    await formRef.value?.validate()
  } catch {
    return
  }

  const payload = {
    className: formState.className.trim(),
    courseId: Number(formState.courseId),
    teacherId: Number(formState.teacherId),
    room: formState.room?.trim() || null,
    capacity: formState.capacity ? Number(formState.capacity) : null,
    startDate: formState.startDate ? `${formState.startDate}T00:00:00` : null,
    endDate: formState.endDate ? `${formState.endDate}T00:00:00` : null,
  }

  saving.value = true
  try {
    const createdClass = await createClass(payload)
    message.success('Da them lop hoc moi.')
    emit('created', createdClass)
    resetForm()
  } catch (error) {
    message.error(extractClassApiErrorMessage(error, 'Khong tao duoc lop hoc.'))
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <a-card class="module-table-card" :bordered="false">
    <template #title>
      <div class="class-form-title">Form them moi lop hoc</div>
    </template>

    <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical">
      <a-row :gutter="16">
        <a-col :xs="24" :md="12">
          <a-form-item label="Ten lop hoc" name="className">
            <a-input v-model:value="formState.className" placeholder="VD: FE-Vue3-01" />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item label="CourseId" name="courseId">
            <a-input-number
              v-model:value="formState.courseId"
              :min="1"
              :precision="0"
              :step="1"
              style="width: 100%"
              placeholder="Nhap CourseId"
            />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item label="TeacherId (so nguyen)" name="teacherId">
            <a-input-number
              v-model:value="formState.teacherId"
              :min="1"
              :precision="0"
              :step="1"
              style="width: 100%"
              placeholder="Nhap TeacherId"
            />
            <div class="form-hint">TeacherId chi can nhap so nguyen duong.</div>
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item label="Phong hoc">
            <a-input v-model:value="formState.room" placeholder="VD: A-203" />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item label="Si so toi da">
            <a-input-number
              v-model:value="formState.capacity"
              :min="1"
              :precision="0"
              :step="1"
              style="width: 100%"
              placeholder="Nhap si so"
            />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item label="Ngay bat dau">
            <a-date-picker
              v-model:value="formState.startDate"
              style="width: 100%"
              value-format="YYYY-MM-DD"
              format="DD/MM/YYYY"
              placeholder="Chon ngay bat dau"
            />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item label="Ngay ket thuc">
            <a-date-picker
              v-model:value="formState.endDate"
              style="width: 100%"
              value-format="YYYY-MM-DD"
              format="DD/MM/YYYY"
              placeholder="Chon ngay ket thuc"
            />
          </a-form-item>
        </a-col>
      </a-row>

      <div class="class-form-actions">
        <a-button @click="resetForm">Lam moi</a-button>
        <a-button type="primary" :loading="saving" @click="submitForm">Tao lop hoc</a-button>
      </div>
    </a-form>
  </a-card>
</template>

<style scoped>
.class-form-title {
  font-weight: 700;
}

.class-form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}
</style>
