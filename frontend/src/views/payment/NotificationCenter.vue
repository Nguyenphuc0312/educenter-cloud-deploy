<template>
  <div class="notification-center">
    <!-- Header -->
    <div class="page-header mb-6">
      <div>
        <h2 class="text-h4 font-weight-bold">Trung Tâm Thông Báo</h2>
        <p class="text-body-2" style="color:#64748b">Theo dõi các thông báo và cập nhật quan trọng</p>
      </div>
      <div class="header-actions">
        <v-btn variant="text" size="small" @click="markAllRead" :disabled="allRead">
          <v-icon start>mdi-check-all</v-icon>
          Đánh dấu tất cả đã đọc
        </v-btn>
      </div>
    </div>

    <!-- Stats -->
    <v-row class="mb-6">
      <v-col cols="12" sm="4">
        <v-card class="stat-card" rounded="xl">
          <div class="d-flex align-center pa-4">
            <v-avatar size="48" color="success" variant="tonal" class="mr-4">
              <v-icon color="success">mdi-bell</v-icon>
            </v-avatar>
            <div>
              <p class="text-caption" style="color:#64748b">Tổng thông báo</p>
              <h3 class="text-h4 font-weight-bold">{{ notifications.length }}</h3>
            </div>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="4">
        <v-card class="stat-card" rounded="xl">
          <div class="d-flex align-center pa-4">
            <v-avatar size="48" color="error" variant="tonal" class="mr-4">
              <v-icon color="error">mdi-bell-alert</v-icon>
            </v-avatar>
            <div>
              <p class="text-caption" style="color:#64748b">Chưa đọc</p>
              <h3 class="text-h4 font-weight-bold">{{ unreadCount }}</h3>
            </div>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="4">
        <v-card class="stat-card" rounded="xl">
          <div class="d-flex align-center pa-4">
            <v-avatar size="48" color="info" variant="tonal" class="mr-4">
              <v-icon color="info">mdi-bell-check</v-icon>
            </v-avatar>
            <div>
              <p class="text-caption" style="color:#64748b">Đã đọc</p>
              <h3 class="text-h4 font-weight-bold">{{ readCount }}</h3>
            </div>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Notifications List -->
    <div class="notifications-list">
      <v-card 
        v-for="n in notifications" 
        :key="n.id" 
        class="notification-card mb-3"
        :class="{ 'unread': !n.read }"
        rounded="xl"
        @click="markRead(n)"
      >
        <div class="notification-content">
          <v-avatar :color="n.color" variant="tonal" class="notification-icon" size="48">
            <v-icon :color="n.color" size="24">{{ n.icon }}</v-icon>
          </v-avatar>
          
          <div class="notification-body">
            <div class="d-flex align-center justify-space-between mb-1">
              <span class="text-subtitle-2 font-weight-bold">{{ n.title }}</span>
              <span class="text-caption" style="color:#64748b">{{ n.time }}</span>
            </div>
            <p class="text-body-2 mb-0" style="color:#64748b">{{ n.message }}</p>
          </div>
          
          <v-btn 
            v-if="!n.read" 
            icon="mdi-check" 
            variant="text" 
            size="small" 
            color="success"
            class="mark-read-btn"
            @click.stop="markRead(n)"
          />
        </div>
      </v-card>
    </div>

    <!-- Empty State -->
    <v-card v-if="notifications.length === 0" class="empty-state" rounded="xl">
      <v-icon size="64" color="success" class="mb-4">mdi-bell-off-outline</v-icon>
      <h3 class="text-h6 mb-2">Không có thông báo nào</h3>
      <p class="text-body-2" style="color:#64748b">Bạn sẽ nhận được thông báo khi có cập nhật mới</p>
    </v-card>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const notifications = ref([
  { id: 1, title: 'Thanh toán thành công', message: 'Lê Minh Châu đã thanh toán 3.500.000đ cho khóa Tiếng Anh Căn bản', time: '5 phút trước', icon: 'mdi-cash-check', color: 'success', read: false },
  { id: 2, title: 'Công nợ mới được ghi nhận', message: 'Nguyễn Hoàng Dũng chưa thanh toán khóa IELTS 6.5+ - Hạn: 30/06/2026', time: '1 giờ trước', icon: 'mdi-alert-circle', color: 'warning', read: false },
  { id: 3, title: 'Bảng lương tháng 5/2026', message: 'Bảng lương tháng 5/2026 đã được duyệt và sẵn sàng chi trả', time: '2 giờ trước', icon: 'mdi-cash-multiple', color: 'info', read: true },
  { id: 4, title: 'Nhắc nợ tự động', message: 'Đã gửi 3 email nhắc nợ cho học viên quá hạn thanh toán', time: '3 giờ trước', icon: 'mdi-email-fast', color: 'warning', read: false },
  { id: 5, title: 'Cập nhật hệ thống', message: 'Hệ thống Payment Service đã được cập nhật lên phiên bản 2.0', time: '1 ngày trước', icon: 'mdi-update', color: 'primary', read: true },
  { id: 6, title: 'Báo cáo tài chính', message: 'Báo cáo doanh thu tháng 4/2026 đã sẵn sàng tải xuống', time: '2 ngày trước', icon: 'mdi-file-chart', color: 'success', read: true },
])

const unreadCount = computed(() => notifications.value.filter(n => !n.read).length)
const readCount = computed(() => notifications.value.filter(n => n.read).length)
const allRead = computed(() => unreadCount.value === 0)

function markRead(n) {
  n.read = true
}

function markAllRead() {
  notifications.value.forEach(n => n.read = true)
}
</script>

<style scoped>
.notification-center {
  animation: fadeIn 0.5s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.header-actions {
  display: flex;
  gap: 12px;
}

/* Stat Cards */
.stat-card {
  border: 1px solid rgba(0,0,0,0.05);
  transition: all 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 10px 30px rgba(0,0,0,0.08) !important;
}

/* Notifications */
.notification-card {
  border: 1px solid rgba(0,0,0,0.05);
  cursor: pointer;
  transition: all 0.3s ease;
}

.notification-card:hover {
  transform: translateX(5px);
  box-shadow: 0 5px 20px rgba(0,0,0,0.08) !important;
}

.notification-card.unread {
  border-left: 4px solid #2E7D32;
  background: linear-gradient(135deg, rgba(46,125,50,0.03), transparent);
}

.notification-content {
  display: flex;
  align-items: center;
  padding: 16px 20px;
  gap: 16px;
}

.notification-icon {
  flex-shrink: 0;
}

.notification-body {
  flex-grow: 1;
}

.mark-read-btn {
  flex-shrink: 0;
  opacity: 0;
  transition: opacity 0.2s ease;
}

.notification-card:hover .mark-read-btn {
  opacity: 1;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 60px 20px;
  border: 1px solid rgba(0,0,0,0.05);
}
</style>
