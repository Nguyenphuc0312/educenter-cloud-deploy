<template>
  <div>
    <v-row class="mb-5">
      <v-col cols="12" sm="4">
        <v-card class="pa-4">
          <div class="d-flex align-center">
            <div>
              <p class="text-caption mb-1" style="color:#64748b">Tổng học phí</p>
              <h3 class="text-h6 font-weight-bold" style="color:#2E7D32">{{ fmt(totalAmount) }}</h3>
            </div>
            <v-spacer />
            <v-avatar size="44" color="success" variant="tonal" rounded="lg"><v-icon>mdi-cash-multiple</v-icon></v-avatar>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="4">
        <v-card class="pa-4">
          <div class="d-flex align-center">
            <div>
              <p class="text-caption mb-1" style="color:#64748b">Đã thanh toán</p>
              <h3 class="text-h6 font-weight-bold" style="color:#388E3C">{{ fmt(totalPaid) }}</h3>
            </div>
            <v-spacer />
            <v-avatar size="44" color="success" variant="tonal" rounded="lg"><v-icon>mdi-check-circle</v-icon></v-avatar>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="4">
        <v-card class="pa-4">
          <div class="d-flex align-center">
            <div>
              <p class="text-caption mb-1" style="color:#64748b">Còn nợ</p>
              <h3 class="text-h6 font-weight-bold" style="color:#D32F2F">{{ fmt(totalDebt) }}</h3>
            </div>
            <v-spacer />
            <v-avatar size="44" color="error" variant="tonal" rounded="lg"><v-icon>mdi-alert-circle</v-icon></v-avatar>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <h3 class="text-subtitle-1 font-weight-bold mb-3">Khóa Học Đã Đăng Ký</h3>
    <v-row class="mb-5">
      <v-col v-for="inv in studentInvoices" :key="inv.id" cols="12" md="6">
        <v-card class="billing-card h-100" :class="{ 'billing-card--unpaid': inv.status !== 'Paid' }">
          <v-card-text class="pa-5">
            <div class="d-flex align-start justify-space-between mb-3">
              <div>
                <h4 class="text-subtitle-2 font-weight-bold mb-1">{{ inv.courseName }}</h4>
                <v-chip size="x-small" variant="tonal" color="success" label>Lớp #{{ inv.courseId }}</v-chip>
              </div>
              <v-chip :color="getStatusColor(inv.status)" variant="flat" size="small" label>
                <v-icon start size="12">{{ getStatusIcon(inv.status) }}</v-icon>
                {{ getStatusText(inv.status) }}
              </v-chip>
            </div>
            <v-divider class="mb-3" />
            <div class="fee-details">
              <div class="fee-row"><span style="color:#64748b">Tổng:</span><span class="font-weight-bold">{{ fmt(inv.amount) }}</span></div>
              <div class="fee-row"><span style="color:#64748b">Đã TT:</span><span style="color:#388E3C;font-weight:medium">{{ fmt(inv.paidAmount) }}</span></div>
              <div v-if="inv.amount - inv.paidAmount > 0" class="fee-row">
                <span style="color:#64748b">Còn lại:</span><span style="color:#D32F2F;font-weight:bold">{{ fmt(inv.amount - inv.paidAmount) }}</span>
              </div>
            </div>
            <v-progress-linear :model-value="(inv.paidAmount / inv.amount) * 100" :color="getStatusColor(inv.status)" rounded height="8" class="mt-4 mb-2" />
            <div class="text-center" style="color:#64748b">{{ Math.round((inv.paidAmount / inv.amount) * 100) }}% đã thanh toán</div>
          </v-card-text>
          <v-card-actions class="pa-4 pt-0" v-if="inv.status !== 'Paid'">
            <v-btn color="success" variant="flat" block prepend-icon="mdi-credit-card-check" @click="goToCheckout">
              Thanh Toán — {{ fmt(inv.amount - inv.paidAmount) }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { computed, onMounted } from 'vue'
import { usePaymentStore } from '@/stores/payment'

const payment = usePaymentStore()

const studentInvoices = computed(() => payment.invoices)
const totalAmount = computed(() => studentInvoices.value.reduce((s, i) => s + (i.amount || 0), 0))
const totalPaid = computed(() => studentInvoices.value.reduce((s, i) => s + (i.paidAmount || 0), 0))
const totalDebt = computed(() => totalAmount.value - totalPaid.value)

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v)
const getStatusColor = s => ({ Paid: 'success', Partial: 'warning', Unpaid: 'error' })[s] || 'secondary'
const getStatusIcon = s => ({ Paid: 'mdi-check-circle', Partial: 'mdi-circle-half-full', Unpaid: 'mdi-alert-circle' })[s]
const getStatusText = s => ({ Paid: 'Đã thanh toán', Partial: 'Một phần', Unpaid: 'Chưa TT' })[s] || s

function goToCheckout() {
  window.location.href = '#checkout'
}

onMounted(() => payment.fetchInvoices())
</script>

<style scoped>
.billing-card { transition: all 0.3s ease; }
.billing-card:hover { transform: translateY(-2px); }
.billing-card--unpaid { border-left: 4px solid #F57C00 !important; }
.fee-details { background: rgba(0,0,0,0.04); border-radius: 10px; padding: 12px 16px; }
.fee-row { display: flex; justify-content: space-between; align-items: center; padding: 4px 0; }
</style>
