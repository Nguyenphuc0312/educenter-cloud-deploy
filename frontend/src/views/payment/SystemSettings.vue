<template>
  <v-card>
    <v-card-title class="text-subtitle-1 font-weight-bold">Cài Đặt Hệ Thống</v-card-title>
    <v-divider />
    <v-card-text class="pa-6">
      <v-list lines="two">
        <v-list-item prepend-icon="mdi-school" title="Tên trung tâm" subtitle="EduCenter - Payment Service">
          <template #append><v-btn variant="text" size="small" color="success">Sửa</v-btn></template>
        </v-list-item>
        <v-list-item prepend-icon="mdi-domain" title="Địa chỉ" subtitle="123 Nguyễn Văn Linh, Quận 7, TP.HCM" />
        <v-list-item prepend-icon="mdi-phone" title="Hotline" subtitle="028 1234 5678" />
        <v-list-item prepend-icon="mdi-email" title="Email" subtitle="contact@educenter.vn" />
        <v-list-item prepend-icon="mdi-web" title="Website" subtitle="https://educenter.vn" />
        <v-divider class="my-2" />
        <v-list-subheader>CHÍNH SÁCH</v-list-subheader>
        <v-list-item prepend-icon="mdi-cash" title="Phí trễ hạn" subtitle="2% / tháng trên số tiền còn nợ." />
        <v-list-item prepend-icon="mdi-refund" title="Chính sách hoàn tiền" subtitle="Hoàn 80% nếu rút trước 7 ngày khai giảng." />
        <v-list-item prepend-icon="mdi-calendar-check" title="Chính sách điểm danh" subtitle="Vắng quá 20% tổng buổi học sẽ không được thi cuối kỳ." />
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script setup>
</script>
