<template>
    <div class="sidebar">
        <el-menu :default-active="onRoutes" class="el-menu-vertical-demo" theme="dark" unique-opened router>
            <!--<template v-for="item in items">-->
            <!--<template v-if="item.subs">-->
            <!--<el-submenu :index="item.index">-->
            <!--<template slot="title"><i :class="item.icon"></i>{{ item.title }}</template>-->
            <!--<el-menu-item v-for="(subItem,i) in item.subs" :key="i" :index="subItem.index">{{ subItem.title-->
            <!--}}-->
            <!--</el-menu-item>-->
            <!--</el-submenu>-->
            <!--</template>-->
            <!--<template v-else>-->
            <!--<el-menu-item :index="item.index">-->
            <!--<i :class="item.icon"></i>{{ item.title }}-->
            <!--</el-menu-item>-->
            <!--</template>-->
            <!--</template>-->

            <!--一级菜单-->
            <template v-for="item in items">
                <template v-if="item.subs">
                    <el-submenu :index="item.index">
                        <template slot="title">
                            <i :class="item.icon"></i>
                            <span>{{ item.title }}</span>
                        </template>
                        <!--二级菜单-->
                        <el-submenu v-for="(subItem,i) in item.subs" :index="subItem.index">
                            <template slot="title">
                                <i :class="item.icon"></i>
                                <span>{{ subItem.title }}</span>
                            </template>
                            <!--三级菜单-->
                            <el-menu-item v-for="(subSubItem,i) in subItem.subs"
                                          :key="i"
                                          :index="subSubItem.index">
                                {{ subSubItem.title }}
                            </el-menu-item>
                            <!--三级菜单-->
                        </el-submenu>
                        <!--二级菜单-->
                    </el-submenu>
                </template>
                <template v-else>
                    <el-menu-item :index="item.index">
                        <i :class="item.icon"></i>{{ item.title }}
                    </el-menu-item>
                </template>
            </template>
            <!--一级菜单-->
        </el-menu>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                items: [
                    {
                        icon: 'el-icon-setting',
                        index: 'readme',
                        title: '自述'
                    },
                    {
                        icon: 'el-icon-menu',
                        index: '1',
                        title: '问题点位',
                        subs: [
                            {
                                // icon: 'el-icon-menu',
                                index: 'table',
                                title: '省级问题',
                                subs: [
                                    {
                                        index: 'table?P_P_JJ',
                                        title: '椒江区'
                                    },
                                    {
                                        index: 'table?pphy',
                                        title: '黄岩区'
                                    },
                                    {
                                        index: '/Problem/province?lq',
                                        title: '路桥区'
                                    },
                                    {
                                        index: '/Problem/province?lh',
                                        title: '临海市'
                                    },
                                    {
                                        index: '/Problem/province?wl',
                                        title: '温岭市'
                                    },
                                    {
                                        index: '/Problem/province?yh',
                                        title: '玉环市'
                                    },
                                    {
                                        index: '/Problem/province?xj',
                                        title: '仙居县'
                                    },
                                    {
                                        index: '/Problem/province?sm',
                                        title: '三门县'
                                    },
                                    {
                                        index: '/Problem/province?tt',
                                        title: '天台县'
                                    }
                                ]
                            },
                            {
                                // icon: 'el-icon-menu',
                                index: 'PCity',
                                title: '市级问题',
                                subs: [
                                    {
                                        index: 'PCity',
                                        title: '椒江区'
                                    },
                                    {
                                        index: 'hy',
                                        title: '黄岩区'
                                    },
                                    {
                                        index: '/Problem/city?lq',
                                        title: '路桥区'
                                    },
                                    {
                                        index: '/Problem/city?lh',
                                        title: '临海市'
                                    },
                                    {
                                        index: '/Problem/city?wl',
                                        title: '温岭市'
                                    },
                                    {
                                        index: '/Problem/city?yh',
                                        title: '玉环市'
                                    },
                                    {
                                        index: '/Problem/city?xj',
                                        title: '仙居县'
                                    },
                                    {
                                        index: '/Problem/city?sm',
                                        title: '三门县'
                                    },
                                    {
                                        index: '/Problem/city?tt',
                                        title: '天台县'
                                    }
                                ]
                            },

                            {
                                // icon: 'el-icon-menu',
                                index: '/Problem/county',
                                title: '县级自查自纠点位',
                                subs: [
                                    {
                                        index: '/Problem/county?jj',
                                        title: '椒江区'
                                    },
                                    {
                                        index: '/Problem/county?hy',
                                        title: '黄岩区'
                                    },
                                    {
                                        index: '/Problem/county?lq',
                                        title: '路桥区'
                                    },
                                    {
                                        index: '/Problem/county?lh',
                                        title: '临海市'
                                    },
                                    {
                                        index: '/Problem/county?wl',
                                        title: '温岭市'
                                    },
                                    {
                                        index: '/Problem/county?yh',
                                        title: '玉环市'
                                    },
                                    {
                                        index: '/Problem/county?xj',
                                        title: '仙居县'
                                    },
                                    {
                                        index: '/Problem/county?sm',
                                        title: '三门县'
                                    },
                                    {
                                        index: '/Problem/county?tt',
                                        title: '天台县'
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        icon: 'el-icon-menu',
                        index: '2',
                        title: '桥下空间利用',
                        subs: [
                            {
                                icon: 'el-icon-menu',
                                index: '1-1',
                                title: '省级问题',
                                subs: [
                                    {
                                        index: 'problem/all',
                                        title: '全部'
                                    },
                                    {
                                        index: 'vuetable',
                                        title: '待审核'
                                    }
                                ]
                            },
                            {
                                icon: 'el-icon-menu',
                                index: '1-2',
                                title: '市级问题',
                                subs: [
                                    {
                                        index: 'problem/all',
                                        title: '全部'
                                    },
                                    {
                                        index: 'vuetable',
                                        title: '待审核'
                                    }
                                ]
                            },

                            {
                                icon: 'el-icon-menu',
                                index: '1-3',
                                title: '县级自查自纠点位',
                                subs: [
                                    {
                                        index: 'problem/all',
                                        title: '全部'
                                    },
                                    {
                                        index: 'vuetable',
                                        title: '待审核'
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        icon: 'el-icon-menu',
                        index: '3',
                        title: '精品示范道路',
                        subs: [
                            {
                                icon: 'el-icon-menu',
                                index: '3-1',
                                title: '省级问题',
                                subs: [
                                    {
                                        index: 'problem/all',
                                        title: '全部'
                                    },
                                    {
                                        index: 'vuetable',
                                        title: '待审核'
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        icon: 'el-icon-menu',
                        index: '4',
                        title: '精品示范入城口',
                        subs: [
                            {
                                icon: 'el-icon-menu',
                                index: '4-1',
                                title: '省级问题',
                                subs: [
                                    {
                                        index: 'problem/all',
                                        title: '全部'
                                    },
                                    {
                                        index: 'vuetable',
                                        title: '待审核'
                                    }
                                ]
                            },
                            {
                                icon: 'el-icon-menu',
                                index: '4-2',
                                title: '市级问题',
                                subs: [
                                    {
                                        index: 'problem/all',
                                        title: '全部'
                                    },
                                    {
                                        index: 'vuetable',
                                        title: '待审核'
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        },
        computed: {
            onRoutes() {
                return this.$route.path.replace('/', '');
            }
        }
    }
</script>

<style scoped>
    .sidebar {
        display: block;
        position: absolute;
        width: 250px;
        left: 0;
        top: 70px;
        bottom: 0;
        background: #2E363F;
    }

    .sidebar > ul {
        height: 100%;
    }
</style>
