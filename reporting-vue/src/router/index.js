import { createRouter, createWebHistory } from 'vue-router'
import ReportViewerView from '../views/ReportViewerView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'viewer',
      component: ReportViewerView
    },
    {
      path: '/designer',
      name: 'designer',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/ReportDesignerView.vue')
    }
  ]
})

export default router
