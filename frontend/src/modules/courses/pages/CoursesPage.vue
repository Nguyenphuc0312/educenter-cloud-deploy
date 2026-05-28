<script setup>
import CourseList from '../components/CourseList.vue'
import ClassCreateForm from '../components/ClassCreateForm.vue'
</script>

<template>
  <section class="course-module">
    <a-card class="module-hero" :bordered="false">
      <div class="module-hero__body">
        <p class="eyebrow">Course & Schedule Service</p>
        <h2>Course Management</h2>
        <p>
          Quan ly danh sach khoa hoc va tao lop hoc moi qua API Gateway.
          Tat ca request tu dong gui kem Authorization Bearer token.
        </p>
      </div>
    </a-card>

    <a-tabs>
      <a-tab-pane key="course-list" tab="Danh sach khoa hoc">
        <CourseList />
      </a-tab-pane>
      <a-tab-pane key="create-class" tab="Them moi lop hoc">
        <ClassCreateForm />
      </a-tab-pane>
    </a-tabs>
  </section>
</template>

<style scoped>
.course-module {
  display: grid;
  gap: 24px;
}
</style>
