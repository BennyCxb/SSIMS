import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      redirect: '/login'
    },
    {
      path: '/readme',
      component: resolve => require(['../components/common/Home.vue'], resolve),
      children: [
        {
          path: '/',
          component: resolve => require(['../components/page/Readme.vue'], resolve)
        },
        {
          path: '/ptable:fid-:sid-:tid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/ptable:fid-:sid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/ptable:fid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/btable:fid-:sid-:tid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/btable:fid-:sid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/btable:fid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/rtable:fid-:sid-:tid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/rtable:fid-:sid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/rtable:fid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/ctable:fid-:sid-:tid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/ctable:fid-:sid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        },
        {
          path: '/ctable:fid-:btid',
          component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
        }
      ]
    },
    {
      path: '/login',
      component: resolve => require(['../components/page/Login.vue'], resolve)
    }
  ]
})
