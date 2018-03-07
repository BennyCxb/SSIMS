<template>
    <div class="header">
        <div class="logo">四边三化信息管理系统</div>
        <!--<div class="menu">-->
            <!--<el-menu :default-active="activeIndex2" class="el-menu-demo" mode="horizontal" @select="handleSelect" background-color="#545c64" text-color="#fff" active-text-color="#ffd04b">-->
                <!--<el-menu-item :index="item.index" v-for="item in items">{{item.title}}</el-menu-item>-->
            <!--</el-menu>-->
        <!--</div>-->
        <div class="user-info">
            <el-dropdown trigger="click" @command="handleCommand">
                <span class="el-dropdown-link">
                    <img class="user-logo" src="../../../static/img/img.jpg">
                    {{username}}
                </span>
                <el-dropdown-menu slot="dropdown">
                    <el-dropdown-item command="loginout">退出</el-dropdown-item>
                </el-dropdown-menu>
            </el-dropdown>
        </div>
    </div>
</template>
<script>
    export default {
        data() {
            return {
                name: 'linxin',
                activeIndex: '1',
                activeIndex2: '1',
                items: [
                    {
                        index: '1',
                        title: '问题点位'
                    },
                    {
                        index: '2',
                        title: '桥下空间利用'
                    },
                    {
                        index: '3',
                        title: '精品示范道路'
                    },
                    {
                        index: '4',
                        title: '精品示范入城口'
                    },

                ]
            }
        },
        computed:{
            username(){
                let username = localStorage.getItem('ms_username');
                return username ? username : this.name;
            }
        },
        methods:{
            handleCommand(command) {
                if(command == 'loginout'){
                    localStorage.removeItem('ms_username')
                    this.$router.push('/login');
                }
            },
            handleSelect(key, keyPath) {
                console.log(key, keyPath);
            }
        }
    }
</script>
<style scoped>
    .header {
        position: relative;
        box-sizing: border-box;
        width: 100%;
        height: 70px;
        font-size: 22px;
        line-height: 70px;
        color: #fff;
    }
    .header .logo{
        float: left;
        width:250px;
        text-align: center;
    }
    .header .menu {
        float: left;
        width: calc(100% - 500px);
    }
    .header .menu .el-menu {
        padding: 0 20px;
        background: transparent;
    }
    .el-menu--horizontal .el-menu-item {
        height: 70px;
        line-height: 70px;
        color: #FFF;
    }
    .el-menu--horizontal .el-menu-item:hover {
        background: #161e2d;
    }
    .el-menu--horizontal.el-menu--dark .el-submenu .el-menu-item.is-active, .el-menu-item.is-active {
        color: #20a0ff;
    }
    .user-info {
        float: right;
        padding-right: 50px;
        font-size: 16px;
        color: #fff;
    }
    .user-info .el-dropdown-link{
        position: relative;
        display: inline-block;
        padding-left: 50px;
        color: #fff;
        cursor: pointer;
        vertical-align: middle;
    }
    .user-info .user-logo{
        position: absolute;
        left:0;
        top:15px;
        width:40px;
        height:40px;
        border-radius: 50%;
    }
    .el-dropdown-menu__item{
        text-align: center;
    }
</style>
