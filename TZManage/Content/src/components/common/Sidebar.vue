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
                        <template v-for="(subItem,i) in item.subs">
                            <template v-if="subItem.subs">
                                <el-submenu :index="subItem.index">
                                    <template slot="title">
                                        <i :class="item.icon"></i>
                                        <span>{{ subItem.title }}</span>
                                    </template>
                                    <!--三级菜单-->
                                    <el-menu-item v-for="(subSubItem,i) in subItem.subs"
                                                  :key="i"
                                                  :index="subSubItem.index"
                                                  @click="routerTo(subSubItem.index, item.index, subItem.index, subSubItem.countyId)">
                                        {{ subSubItem.title }}
                                    </el-menu-item>
                                    <!--三级菜单-->
                                </el-submenu>
                            </template>
                            <template v-else>
                                <el-menu-item :index="subItem.index">
                                    <i :class="subItem.icon"></i>{{ subItem.title }}
                                </el-menu-item>
                            </template>
                        </template>

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
                        index: 'Problem',
                        title: '问题点位',
                        subs: [
                            {
                                // icon: 'el-icon-menu',
                                index: 'Province',
                                title: '省级问题',
                                subs: [
                                    {
                                        index: 'Province_jj',
                                        countyId: 'jj',
                                        title: '椒江区'
                                    },
                                    {
                                        index: 'Province_hy',
                                        countyId: 'hy',
                                        title: '黄岩区'
                                    },
                                    {
                                        index: 'Province_lq',
                                        title: '路桥区'
                                    },
                                    {
                                        index: 'Province_lh',
                                        title: '临海市'
                                    },
                                    {
                                        index: 'Province_wl',
                                        title: '温岭市'
                                    },
                                    {
                                        index: 'Province_yh',
                                        title: '玉环市'
                                    },
                                    {
                                        index: 'Province_xj',
                                        title: '仙居县'
                                    },
                                    {
                                        index: 'Province_sm',
                                        title: '三门县'
                                    },
                                    {
                                        index: 'Province_tt',
                                        title: '天台县'
                                    }
                                ]
                            },
                            {
                                // icon: 'el-icon-menu',
                                index: 'City',
                                title: '市级问题',
                                subs: [
                                    {
                                        index: 'City_jj',
                                        countyId: 'jj',
                                        title: '椒江区'
                                    },
                                    {
                                        index: 'City_hy',
                                        countyId: 'jj',
                                        title: '黄岩区'
                                    },
                                    {
                                        index: 'City_lq',
                                        title: '路桥区'
                                    },
                                    {
                                        index: 'City_lh',
                                        title: '临海市'
                                    },
                                    {
                                        index: 'City_wl',
                                        title: '温岭市'
                                    },
                                    {
                                        index: 'City_yh',
                                        title: '玉环市'
                                    },
                                    {
                                        index: 'City_xj',
                                        title: '仙居县'
                                    },
                                    {
                                        index: 'City_sm',
                                        title: '三门县'
                                    },
                                    {
                                        index: 'City_tt',
                                        title: '天台县'
                                    }
                                ]
                            },

                            {
                                // icon: 'el-icon-menu',
                                index: 'County',
                                title: '县级自查自纠点位',
                                subs: [
                                    {
                                        index: 'County_jj',
                                        title: '椒江区'
                                    },
                                    {
                                        index: 'County_hy',
                                        title: '黄岩区'
                                    },
                                    {
                                        index: 'County_lq',
                                        title: '路桥区'
                                    },
                                    {
                                        index: 'County_lh',
                                        title: '临海市'
                                    },
                                    {
                                        index: 'County_wl',
                                        title: '温岭市'
                                    },
                                    {
                                        index: 'County_yh',
                                        title: '玉环市'
                                    },
                                    {
                                        index: 'County_xj',
                                        title: '仙居县'
                                    },
                                    {
                                        index: 'County_sm',
                                        title: '三门县'
                                    },
                                    {
                                        index: 'County_tt',
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
                                index: 'Bridgejj',
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
