<template>
  <div>
    <v-row>
      <v-col cols="12" md="7">
        <v-card class="mb-5">
          <v-card-title class="px-5 pt-4 pb-2">
            <v-icon color="success" class="mr-2">mdi-receipt-text</v-icon>
            <span class="text-subtitle-1 font-weight-bold">Hóa Đơn Thanh Toán</span>
          </v-card-title>
          <v-divider />
          <v-card-text class="pa-5">
            <div v-for="inv in unpaidInvoices" :key="inv.id" class="invoice-item mb-4">
              <div class="d-flex align-center justify-space-between mb-2">
                <div class="d-flex align-center">
                  <v-checkbox v-model="selected" :value="inv.id" hide-details density="compact" color="success" class="mr-2" />
                  <div>
                    <span class="font-weight-medium text-body-2">{{ inv.courseName }}</span>
                    <br />
                    <span class="text-caption" style="color:#64748b">Hạn: {{ fmtDate(inv.dueDate) }}</span>
                  </div>
                </div>
                <div class="text-right">
                  <div class="text-body-2 font-weight-bold">{{ fmt(inv.amount - inv.paidAmount) }}</div>
                  <v-chip v-if="inv.paidAmount > 0" size="x-small" color="info" variant="tonal" label>Đã TT: {{ fmt(inv.paidAmount) }}</v-chip>
                </div>
              </div>
              <v-divider />
            </div>
            <div v-if="unpaidInvoices.length === 0" class="text-center pa-6">
              <v-icon size="64" color="success" class="mb-2">mdi-check-circle</v-icon>
              <p class="text-body-1 font-weight-medium" style="color:#388E3C">Bạn đã thanh toán hết!</p>
            </div>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="5">
        <v-card class="mb-5" color="success" variant="tonal">
          <v-card-text class="pa-5 text-center">
            <p class="text-caption mb-1" style="color:#64748b">TỔNG THANH TOÁN</p>
            <h2 class="text-h4 font-weight-bold" style="color:#2E7D32">{{ fmt(totalSelected) }}</h2>
            <p class="text-caption mt-1" style="color:#64748b">{{ selected.length }} hóa đơn được chọn</p>
          </v-card-text>
        </v-card>

        <v-card class="mb-5">
          <v-card-title class="px-5 pt-4 pb-2">
            <v-icon color="success" class="mr-2">mdi-credit-card-outline</v-icon>
            <span class="text-subtitle-1 font-weight-bold">Phương Thức</span>
          </v-card-title>
          <v-divider />
          <v-card-text class="pa-5">
            <v-radio-group v-model="method" class="mb-3">
              <v-radio value="card" class="mb-3"><template #label><div class="d-flex align-center"><v-icon class="mr-2" color="blue">mdi-credit-card</v-icon><span class="text-body-2">Thẻ tín dụng</span></div></template></v-radio>
              <v-radio value="vnpay" class="mb-3"><template #label><div class="d-flex align-center"><v-icon class="mr-2" color="red">mdi-qrcode</v-icon><span class="text-body-2">VNPay QR</span></div></template></v-radio>
              <v-radio value="momo"><template #label><div class="d-flex align-center"><v-icon class="mr-2" color="pink">mdi-cellphone</v-icon><span class="text-body-2">MoMo</span></div></template></v-radio>
            </v-radio-group>
            <div v-if="method === 'card'" class="card-form">
              <v-text-field v-model="card.number" label="Số thẻ" placeholder="4242 4242 4242 4242" prepend-inner-icon="mdi-credit-card" density="compact" class="mb-2" />
              <v-row>
                <v-col cols="6"><v-text-field v-model="card.expiry" label="MM/YY" placeholder="12/28" density="compact" /></v-col>
                <v-col cols="6"><v-text-field v-model="card.cvv" label="CVV" placeholder="123" density="compact" type="password" /></v-col>
              </v-row>
              <v-text-field v-model="card.name" label="Tên chủ thẻ" placeholder="NGUYEN VAN A" density="compact" class="mt-1" />
            </div>
          </v-card-text>
        </v-card>

        <v-btn color="success" variant="flat" block size="x-large" :disabled="selected.length === 0" :loading="processing" prepend-icon="mdi-lock" @click="processPayment" class="pay-btn">
          Thanh Toán {{ fmt(totalSelected) }}
        </v-btn>
      </v-col>
    </v-row>

    <v-dialog v-model="showSuccess" max-width="440" persistent>
      <v-card rounded="xl" class="text-center pa-6">
        <v-avatar size="80" color="success" variant="tonal" class="mb-4"><v-icon size="48" color="success">mdi-check-circle</v-icon></v-avatar>
        <h3 class="text-h5 font-weight-bold mb-2">Thanh Toán Thành Công!</h3>
        <p class="text-body-2 mb-4" style="color:#64748b">Số tiền: <strong>{{ fmt(totalSelected) }}</strong></p>
        <v-btn color="success" variant="flat" block prepend-icon="mdi-check" @click="showSuccess = false">Hoàn tất</v-btn>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import { usePaymentStore } from '@/stores/payment'

const payment = usePaymentStore()
const selected = ref([])
const method = ref('card')
const processing = ref(false)
const showSuccess = ref(false)
const card = reactive({ number: '', expiry: '', cvv: '', name: '' })

const unpaidInvoices = computed(() => payment.invoices.filter(i => i.status !== 'Paid'))
const totalSelected = computed(() => unpaidInvoices.value.filter(i => selected.value.includes(i.id)).reduce((s, i) => s + ((i.amount || 0) - (i.paidAmount || 0)), 0))

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v)
const fmtDate = d => d ? new Date(d).toLocaleDateString('vi-VN') : '—'

onMounted(() => payment.fetchInvoices())

async function processPayment() {
  processing.value = true
  await new Promise(r => setTimeout(r, 2000))
  for (const id of selected.value) {
    const inv = payment.invoices.find(i => i.id === id)
    if (inv) {
      await payment.payInvoice(id, { paidAmount: inv.amount - inv.paidAmount, paymentMethod: method.value.toUpperCase() })
    }
  }
  selected.value = []
  processing.value = false
  showSuccess.value = true
}
</script>

<style scoped>
.invoice-item { transition: background 0.2s; border-radius: 8px; padding: 8px; }
.card-form { background: rgba(0,0,0,0.04); border-radius: 12px; padding: 16px; }
.pay-btn { text-transform: none; letter-spacing: 0; font-weight: 700; font-size: 1.05rem; }
</style>
