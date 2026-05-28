<template>
  <v-card>
    <v-card-title class="text-subtitle-1 font-weight-bold">Nhật Ký Hệ Thống</v-card-title>
    <v-divider />
    <v-card-text class="pa-0">
      <v-timeline density="compact" side="end" class="pa-4">
        <v-timeline-item v-for="log in logs" :key="log.id" :dot-color="log.color" size="small">
          <div class="d-flex align-center justify-space-between">
            <div>
              <span class="font-weight-medium text-body-2">{{ log.action }}</span>
              <p class="text-caption mb-0" style="color:#64748b">{{ log.user }} · {{ log.time }}</p>
            </div>
            <v-chip size="x-small" :color="log.color" variant="tonal" label>{{ log.type }}</v-chip>
          </div>
        </v-timeline-item>
      </v-timeline>
    </v-card-text>
  </v-card>
</template>

<script setup>
const logs = [
  { id: 1, action: 'Thanh toán hóa đơn INV003', user: 'Lê Minh Châu', time: '5 phút trước', type: 'Payment', color: 'success' },
  { id: 2, action: 'Đăng nhập hệ thống', user: 'admin@educenter.vn', time: '15 phút trước', type: 'Auth', color: 'info' },
  { id: 3, action: 'Gửi nhắc nợ hàng loạt', user: 'Admin', time: '1 giờ trước', type: 'Debt', color: 'warning' },
  { id: 4, action: 'Cập nhật bảng lương GV', user: 'Admin', time: '2 giờ trước', type: 'Payroll', color: 'success' },
  { id: 5, action: 'Tạo hóa đơn mới', user: 'Admin', time: '3 giờ trước', type: 'Payment', color: 'success' },
]
</script>
