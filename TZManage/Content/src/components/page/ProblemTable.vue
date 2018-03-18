<template>
    <div class="table">
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item v-for="(item, i) in breadcrumb"><i class="el-icon-menu" v-if="i === 0"></i> {{item}}</el-breadcrumb-item>
                <!--<el-breadcrumb-item>基础表格</el-breadcrumb-item>-->
            </el-breadcrumb>
        </div>
        <div class="handle-box">
            <!--<el-button type="primary" icon="delete" class="handle-del mr10" @click="delAll">批量删除</el-button>-->
            <el-select v-model="select_years" placeholder="年度" class="handle-select mr10">
                <el-option v-for="(year, i) in years" :key="i" :label="year" :value="year"></el-option>
            </el-select>
            <el-select v-model="select_adcd" placeholder="行政区划" class="handle-select mr10">
                <el-option v-for="(item, i) in adlist" :key="i" :label="item.label" :value="item.value"></el-option>
            </el-select>
            <el-input v-model="select_area" placeholder="所属区域" class="handle-input mr10"></el-input>
            <el-select v-model="select_problem" placeholder="四边" class="handle-select mr10">
                <el-option v-for="(item, i) in edgeOptions" :key="i" :label="item.label" :value="item.id"></el-option>
            </el-select>
            <el-select v-model="select_problem" placeholder="存在问题" class="handle-select mr10">
                <el-option v-for="(item, i) in proOptions" :key="i" :label="item.label" :value="item.id"></el-option>
            </el-select>
        </div>
        <div class="handle-box">
            <el-input v-model="select_problem_num" placeholder="问题编号" class="handle-input mr10"></el-input>
            <el-input v-model="select_word" placeholder="筛选关键词" class="handle-input mr10"></el-input>
            <el-button type="primary" icon="search" @click="search">搜索</el-button>
            <el-button type="primary" icon="plus" @click="search">添加项目</el-button>
            <el-button type="primary" icon="printer" @click="search">数据导出</el-button>
            <el-button type="primary" icon="upload2" @click="proAddShow = true">上报问题</el-button>

            <vProblemForm :formShow="proAddShow" :sposition="position" @closeProAdd="closePro" @selectMap="closeMap"></vProblemForm>
            <map-select :mapShow="mapSelectShow" @selectMap="closeMap" @selectPosition="setPosition"></map-select>

            <el-button type="primary" icon="" @click="search">整改完成</el-button>
        </div>
        <el-table :data="data" border style="width: 100%" ref="multipleTable" @selection-change="handleSelectionChange">
            <el-table-column type="selection" width="55"></el-table-column>
            <el-table-column prop="adnm" label="行政区划" sortable width="150">
            </el-table-column>
            <el-table-column prop="problemNum" label="问题编号" width="120">
            </el-table-column>
            <el-table-column prop="lineName" label="线路名称" >
            </el-table-column>
            <el-table-column prop="mileage" label="里程" >
            </el-table-column>
            <el-table-column prop="problemName" label="问题名称" >
            </el-table-column>
            <el-table-column prop="status" label="状态" >
            </el-table-column>
            <el-table-column label="操作" width="180">
                <template scope="scope">
                    <el-button size="small"
                            @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                    <el-button size="small" type="danger"
                            @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
        <div class="pagination">
            <el-pagination
                    @current-change ="handleCurrentChange"
                    layout="prev, pager, next"
                    :total="1000">
            </el-pagination>
        </div>
    </div>

</template>

<script>
    import vProblemForm from './ProblemForm.vue';
    import mapSelect from './MapSelect.vue';
    import Axios from 'axios';
    import _ from 'lodash';

    export default {
        components:{
            vProblemForm,
            mapSelect
        },
        data() {
            return {
                msg: '',
                url: './static/vuetable.json',
                position: '',
                tableData: [],
                cur_page: 1,
                multipleSelection: [],
                select_cate: '',
                select_years: '',
                select_word: '',
                select_adcd: '',
                select_area: '',
                select_problem_num: '',
                select_problem: '',
                del_list: [],
                is_search: false,
                years: [2018,2017,2016],
                adlist: [],
                proOptions: [],
                edgeOptions: [],
                proAddShow: false,
                mapSelectShow: false,
                breadcrumb: []
            }
        },
        created(){
            this.getData();
        },
        computed: {
            data(){
                const self = this;
                // return self.tableData.filter(function(d){
                //     let is_del = false;
                //     for (let i = 0; i < self.del_list.length; i++) {
                //         if(d.name === self.del_list[i].name){
                //             is_del = true;
                //             break;
                //         }
                //     }
                //     if(!is_del){
                //         if(d.adcd.indexOf(self.select_adcd) > -1 &&
                //             (d.problemNum.indexOf(self.select_problem_num) > -1 ||
                //             d.problemName.indexOf(self.select_problem) > -1)
                //         ){
                //             return d;
                //         }
                //     }
                // })
                return self.tableData
            }
        },
        methods: {
            handleCurrentChange(val){
                this.cur_page = val;
                this.getData();
            },
            getBreadcrumb(){
                let blist = JSON.parse(sessionStorage.getItem('breadcrumb'));
                this.breadcrumb = [].concat(blist);
            },
            getAdcd() {
                let self = this;
                Axios.get('Common/GetAgencyList')
                    .then(function (response) {
                        let data = response.data;
                        let adcdlist = [];
                        _.each(data.object, (obj) => {
                            adcdlist.push({
                                value: obj.FValue,
                                label: obj.FName
                            })
                        })
                        self.adlist = [].concat(adcdlist);
                    })
                    .catch(function (error) {
                        console.log(error);
                        self.$alert(error.message, '温馨提示', {
                            confirmButtonText: '确定'
                        });
                    });
            },
            getEdge() {
                let self = this;
                Axios.get('Common/GetEnumList', {
                    params: {
                        EnumType: '四边'
                    }
                })
                    .then(function (response) {
                        let data = response.data;
                        let list = [];
                        _.each(data.object, (obj) => {
                            list.push({
                                value: obj.FValue,
                                label: obj.FName
                            })
                        })
                        self.edgeOptions = [].concat(list);
                    })
                    .catch(function (error) {
                        console.log(error);
                        self.$alert(error.message, '温馨提示', {
                            confirmButtonText: '确定'
                        });
                    });
            },
            getProblemType() {
                let self = this;
                Axios.get('Common/GetEnumList', {
                    params: {
                        EnumType: '问题类型'
                    }
                })
                    .then(function (response) {
                        let data = response.data;
                        let ptypelist = [];
                        _.each(data.object, (obj) => {
                            ptypelist.push({
                                value: obj.FValue,
                                label: obj.FName
                            })
                        })
                        self.proOptions = [].concat(ptypelist);
                    })
                    .catch(function (error) {
                        console.log(error);
                        self.$alert(error.message, '温馨提示', {
                            confirmButtonText: '确定'
                        });
                    });
            },
            getData(){
                let self = this;
                if(process.env.NODE_ENV === 'development'){
                    self.url = '/ms/table/list';
                };
                var instance = self.$axios.create({
                    baseURL: 'http://localhost:8080/',
                    timeout: 1000,
                    headers: {'X-Custom-Header': 'foobar'}
                });
                instance.post(self.url, {page:self.cur_page}).then((res) => {
                    // self.tableData = res.data.list;
                    self.tableData = list;
                })
            },
            search(){
                this.is_search = true;
            },
            formatter(row, column) {
                return row.address;
            },
            // filterTag(value, row) {
            //     return row.tag === value;
            // },
            handleEdit(index, row) {
                this.$message('编辑第'+(index+1)+'行');
            },
            handleDelete(index, row) {
                this.$message.error('删除第'+(index+1)+'行');
            },
            delAll(){
                const self = this,
                    length = self.multipleSelection.length;
                let str = '';
                self.del_list = self.del_list.concat(self.multipleSelection);
                for (let i = 0; i < length; i++) {
                    str += self.multipleSelection[i].name + ' ';
                }
                self.$message.error('删除了'+str);
                self.multipleSelection = [];
            },
            handleSelectionChange(val) {
                this.multipleSelection = val;
            },
            closePro: function (msg) {
                this.proAddShow = msg;
            },
            closeMap: function (msg) {
                this.mapSelectShow = msg;
            },
            setPosition(msg) {
                this.position = msg.lng + ',' + msg.lat;
            },
            getStatus (urlStr) {
                var urlStrArr = urlStr.split('/')
                return urlStrArr[urlStrArr.length - 1]
            }
        },
        created() {
            this.getAdcd();
            this.getEdge();
            this.getProblemType();
        },
        mounted() {
            this.getBreadcrumb();
        },
        watch: {
            // $route (to, from) {
            //     console.log(to)
            //     console.log(from)
            //     this.getBreadcrumb();
            // }
            '$route' (to, from) {
                //刷新参数放到这里里面去触发就可以刷新相同界面了
                console.log(to);
                console.log(from);
                this.getStatus(this.$route.path);
                this.getBreadcrumb();
            }
        },
    }

    const list = [
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj001',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 2,
            adnm: '椒江区',
            problemNum: 'jj002',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj003',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj004',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj005',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj006',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj007',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj001',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
        {
            id: 1,
            adnm: '椒江区',
            problemNum: 'jj001',
            lineName: 'G15沈海',
            mileage: '椒江大桥下',
            problemName: '乱堆乱放',
            status: '审核通过'
        },
    ]
</script>

<style scoped>
.handle-box{
    margin-bottom: 20px;
}
.handle-select{
    width: 120px;
}
.handle-input{
    width: 300px;
    display: inline-block;
}
</style>
