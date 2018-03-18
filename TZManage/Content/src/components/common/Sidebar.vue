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
            <template v-for="(item, i) in items">
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
                                                  @click="routerTo(item, subItem, subSubItem)"
                                                  :route="{params: {
                                                    id: 1
                                                    }
                                                  }">
                                        {{ subSubItem.title }}
                                    </el-menu-item>
                                    <!--三级菜单-->
                                </el-submenu>
                            </template>
                            <template v-else>
                                <el-menu-item :key="i"
                                              :index="subItem.index"
                                              @click="routerTo(item, subItem)">
                                    <i :class="subItem.icon"></i>{{ subItem.title }}
                                </el-menu-item>
                            </template>
                        </template>

                        <!--二级菜单-->
                    </el-submenu>
                </template>
                <template v-else>
                    <el-menu-item :index="item.index"
                                  @click="routerTo(item)">
                        <i :class="item.icon"></i>{{ item.title }}
                    </el-menu-item>
                </template>
            </template>
            <!--一级菜单-->
        </el-menu>
    </div>
</template>

<script>
    import Axios from 'axios';
    import _ from 'lodash';
    export default {
        data() {
            return {
                items: [
                    {
                        icon: 'el-icon-setting',
                        index: 'readme',
                        title: '自述',
                        url: 'readme'
                    },
                ]
            }
        },
        computed: {
            onRoutes() {
                return this.$route.path.replace('/', '');
            }
        },
        methods: {
            routerTo(f, s, t) {
                // console.log(f,s,t)
                let path;
                let breadcrumb = [];
                let billTypeID;
                if (f.url === undefined) {
                    breadcrumb.push(f.title);
                    if (s.url === undefined) {
                        breadcrumb.push(s.title);
                        if (t.url === undefined) {
                            breadcrumb.push(t.title);
                        } else {
                            // path = t.url + '/' + f.id + '/'  + s.id + '/'  + t.id;
                            path = f.id + '-'  + s.id + '-'  + t.id;
                            // path = t.url;
                            breadcrumb.push(t.title);
                            billTypeID = t.billTypeID;
                        }
                    } else {
                        // path = s.url + '/'  + f.parentId + '/'  + s.parentId;
                        path = f.parentId + '-'  + s.parentId;
                        // path = s.url;
                        breadcrumb.push(s.title);
                        billTypeID = s.billTypeID;
                    }
                } else {
                    // path = f.url + '/'  + f.parentId;
                    path = f.parentId;
                    // path = f.url;
                    breadcrumb.push(f.title);
                    billTypeID = f.billTypeID;
                }
                sessionStorage.setItem('breadcrumb', JSON.stringify(breadcrumb));
                this.$router.push({
                    path: path
                })
            }
        },
        beforeCreate() {
            let self = this;
            Axios.get('Login/IsLogin')
                .then(function (response) {
                    let data = response.data.object;
                    // console.log(data);
                    let items1 = [],
                        items2 = [],
                        items3 = [];

                    // 一级菜单
                    _.each(data.MenuJson, (obj1, index1) => {
                        if (obj1.FChild.length > 0) {
                            // 二级菜单
                            _.each(obj1.FChild, (obj2, index2) => {
                                if (obj2.FChild.length > 0) {
                                    // 三级菜单
                                    _.each(obj2.FChild, (obj3, index3) => {
                                        items3.push({
                                            id: obj3.FID,
                                            billTypeID: obj3.FBillTypeID,
                                            index: obj1.FID.toString() + '-' + obj2.FID.toString() + '-' + obj3.FID.toString(),
                                            parentId: obj3.FParentID,
                                            title: obj3.FName,
                                            // url: obj3.FUrlPath
                                            url: '/ptable'
                                        })
                                    })
                                    items2.push({
                                        id: obj2.FID,
                                        index: obj1.FID.toString() + '-' + obj2.FID.toString(),
                                        parentId: obj2.FParentID,
                                        title: obj2.FName,
                                        subs: items3
                                    })
                                } else {
                                    items2.push({
                                        id: obj2.FID,
                                        billTypeID: obj2.FBillTypeID,
                                        index: obj1.FID.toString() + '-' + obj2.FID.toString(),
                                        parentId: obj2.FParentID,
                                        title: obj2.FName,
                                        url: obj2.FUrlPath,
                                    })
                                }
                            })
                            items1.push({
                                id: obj1.FID,
                                icon: 'el-icon-menu',
                                index: obj1.FID.toString(),
                                parentId: obj1.FParentID,
                                title: obj1.FName,
                                subs: items2
                            })
                        } else {
                            items1.push({
                                id: obj1.FID,
                                billTypeID: obj1.FBillTypeID,
                                icon: 'el-icon-menu',
                                index: obj1.FID.toString(),
                                parentId: obj1.FParentID,
                                title: obj1.FName,
                                // url: obj1.FUrlPath,
                                url: 'qxkj',
                            })
                        }
                    })

                    console.log(items1)
                    self.items = [{
                        icon: 'el-icon-setting',
                        index: 'readme',
                        title: '自述',
                        url: 'readme'
                    }].concat(items1);
                })
                .catch(function (error) {
                    console.log(error);
                    self.$alert(error.message, '温馨提示', {
                        confirmButtonText: '确定'
                    });
                });
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
