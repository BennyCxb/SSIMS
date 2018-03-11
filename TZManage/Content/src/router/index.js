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
                    path: '/Province',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve),
                    children:[
                        {
                            path: '/Province_jj',
                            name: 'Province_jj',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_hy',
                            name: 'Province_hy',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_lq',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_lh',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_wl',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_yh',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_xj',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_sm',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/Province_tt',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        }
                    ]
                },
                {
                    path: '/City',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve),
                    children:[
                        {
                            path: '/City_jj',
                            name: 'City_jj',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_hy',
                            name: 'City_hy',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_lq',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_lh',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_wl',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_yh',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_xj',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_sm',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/City_tt',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        }
                    ]
                },
                {
                    path: '/County_',
                    component: resolve => require(['../components/page/ProblemTable.vue'], resolve),
                    children:[
                        {
                            path: '/County_jj',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_hy',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_lq',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_lh',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_wl',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_yh',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_xj',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_sm',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        },
                        {
                            path: '/County_tt',
                            component: resolve => require(['../components/page/ProblemTable.vue'], resolve)
                        }
                    ]
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
