import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: '/',
            redirect: '/login'
        },
        {
            path: '/readme',
            component: resolve => require(['../components/common/Home.vue'], resolve),
            children:[
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
                },
                {
                    path: '/ProblemAdd',
                    component: resolve => require(['../components/page/ProblemForm2.vue'], resolve)
                },

                {
                    path: '/Bridgejj',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                },
                {
                    path: '/PProvince',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                },
                {
                    path: '/PCity',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                },
                {
                    path: '/PCcounty',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                },
                {
                    path: '/basetable',
                    component: resolve => require(['../components/page/BaseTable.vue'], resolve)
                },
                {
                    path: '/vuetable',
                    component: resolve => require(['../components/page/VueTable.vue'], resolve)     // vue-datasource组件
                },
                {
                    path: '/baseform',
                    component: resolve => require(['../components/page/BaseForm.vue'], resolve)
                },
                {
                    path: '/vueeditor',
                    component: resolve => require(['../components/page/VueEditor.vue'], resolve)    // Vue-Quill-Editor组件
                },
                {
                    path: '/markdown',
                    component: resolve => require(['../components/page/Markdown.vue'], resolve)     // Vue-Quill-Editor组件
                },
                {
                    path: '/upload',
                    component: resolve => require(['../components/page/Upload.vue'], resolve)       // Vue-Core-Image-Upload组件
                },
                {
                    path: '/basecharts',
                    component: resolve => require(['../components/page/BaseCharts.vue'], resolve)   // vue-schart组件
                },
                {
                    path: '/drag',
                    component: resolve => require(['../components/page/DragList.vue'], resolve)    // 拖拽列表组件
                }
            ]
        },
        {
            path: '/login',
            component: resolve => require(['../components/page/Login.vue'], resolve)
        },
    ]
})
