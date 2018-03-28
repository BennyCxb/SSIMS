<template>
  <div class="table">
    <div class="crumbs">
      <el-breadcrumb separator="/">
        <el-breadcrumb-item v-for="(item, i) in breadcrumb" :key="i"><i class="el-icon-menu" v-if="i === 0"></i> {{item}}
        </el-breadcrumb-item>
        <!--<el-breadcrumb-item>基础表格</el-breadcrumb-item>-->
      </el-breadcrumb>
    </div>
    <div class="handle-box">
      <!--<el-button type="primary" icon="delete" class="handle-del mr10" @click="delAll">批量删除</el-button>-->
      <el-date-picker
        class="handle-select"
        v-model="select_years"
        align="right"
        type="year"
        placeholder="选择年">
      </el-date-picker>
      <el-date-picker
        class="handle-select"
        v-model="select_mouths"
        format="MM"
        type="month"
        placeholder="选择月">
      </el-date-picker>
      <el-select v-model="select_adcd" class="handle-select" placeholder="行政区划" clearable>
        <el-option
          v-for="item in adlist"
          :key="item.value"
          :label="item.label"
          :value="item.value">
          <span style="float: left">{{ item.label }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.value }}</span>
        </el-option>
      </el-select>
      <!--<el-select v-model="select_adcd" placeholder="行政区划" class="handle-select mr10">-->
      <!--<el-option v-for="(item, i) in adlist" :key="i" :label="item.label" :value="item.value"></el-option>-->
      <!--</el-select>-->
      <!--<el-input v-model="select_area" placeholder="所属区域" class="handle-input mr10"></el-input>-->
      <el-select v-model="select_edge" placeholder="四边" class="handle-select mr10" clearable>
        <el-option v-for="(item, i) in edgeOptions" :key="i" :label="item.label" :value="item.value"></el-option>
      </el-select>
      <el-select v-model="select_problem" placeholder="存在问题" class="handle-select mr10" clearable>
        <el-option v-for="(item, i) in proOptions" :key="i" :label="item.label" :value="item.value"></el-option>
      </el-select>
      <el-select v-model="select_status" placeholder="审核状态" class="handle-select mr10" clearable>
        <el-option v-for="(item, i) in staOptions" :key="i" :label="item.label" :value="item.value"></el-option>
      </el-select>
      <el-select v-model="select_cttatus" placeholder="整改状态" class="handle-select mr10" clearable>
        <el-option v-for="(item, i) in cstaOptions" :key="i" :label="item.label" :value="item.value"></el-option>
      </el-select>
    </div>
    <div class="handle-box">
      <el-input v-model="select_problem_num" placeholder="问题编号" class="handle-input mr10"></el-input>
      <el-button type="primary" icon="search" @click="search">搜索</el-button>
      <!--<el-button type="primary" icon="plus" @click="search">添加项目</el-button>-->
      <!--<el-button type="primary" icon="printer" @click="search">数据导出</el-button>-->
      <el-button type="primary" icon="plus" @click="addProblem">新增问题</el-button>
      <!--<router-link :to="'ProblemAdd'">-->
      <!--<el-button type="primary" icon="upload2">上报问题</el-button>-->
      <!--</router-link>-->
      <vProblemForm :fid="editFid" :billTypeId="billTypeID" :formShow="proAddShow" :sposition="position"
                    @closeProAdd="closePro" @selectMap="closeMap"></vProblemForm>
      <map-select :mapShow="mapSelectShow" @selectMap="closeMap" @selectPosition="setPosition"></map-select>

      <!--<el-button type="primary" icon="" @click="search">整改完成</el-button>-->
    </div>
    <el-table :data="data" border style="width: 100%" ref="multipleTable" @selection-change="handleSelectionChange"
              stripe>
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column prop="FAgencyName" label="行政区划">
      </el-table-column>
      <el-table-column prop="FBillNo" label="问题编号" sortable>
      </el-table-column>
      <el-table-column prop="FLineName" label="线路名称">
      </el-table-column>
      <el-table-column prop="FMileage" label="里程">
      </el-table-column>
      <el-table-column prop="FProbType" label="问题类型">
      </el-table-column>
      <el-table-column prop="FStatusName" label="审核状态" sortable>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template scope="scope">
          <el-button size="small"
          </el-button>
          <el-button size="small"
                     @click="handleEdit(scope.$index, scope.row)">编辑
          </el-button>
          <el-button size="small" type="danger"
                     @click="handleDelete(scope.$index, scope.row)">删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="pagination">
      <el-pagination
        @current-change="handleCurrentChange"
        layout="prev, pager, next"
        :total="1000">
      </el-pagination>
    </div>
  </div>

</template>

<script>
import _ from 'lodash'
import vProblemForm from './ProblemForm.vue'
import mapSelect from './MapSelect.vue'

export default {
  components: {
    vProblemForm,
    mapSelect
  },
  data () {
    return {
      editFid: '',
      billTypeID: '',
      position: '',
      tableData: [],
      cur_page: 1,
      multipleSelection: [],
      select_cate: '',
      select_years: '',
      select_mouths: '',
      select_adcd: '',
      select_edge: '',
      select_problem_num: '',
      select_problem: '',
      select_status: '',
      select_cttatus: '',
      del_list: [],
      is_search: false,
      years: [2018, 2017, 2016],
      mouths: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
      adlist: [],
      proOptions: [],
      edgeOptions: [],
      staOptions: [],
      cstaOptions: [],
      proAddShow: false,
      mapSelectShow: false,
      breadcrumb: []
    }
  },
  created () {
    this.getBillTypeId()
    this.getBreadcrumb()

    this.getAdcd()
    this.getEdge()
    this.getData()
    this.getProblemType()
    this.getStatusData()
    this.getChangeStatusData()
  },
  computed: {
    data () {
      return this.tableData
    }
  },
  methods: {
    handleCurrentChange (val) {
      this.cur_page = val
      this.getData()
    },
    getStatus (urlStr) {
      var urlStrArr = urlStr.split('/')
      return urlStrArr[urlStrArr.length - 1]
    },
    /**
     * 获取BiilTypeID
     */
    getBillTypeId () {
      this.billTypeID = this.$route.params.btid
    },
    /**
     * 获取面包屑
     */
    getBreadcrumb () {
      let blist = JSON.parse(sessionStorage.getItem('breadcrumb'))
      this.breadcrumb = [].concat(blist)
    },
    /**
     * 获取行政区划
     */
    getAdcd () {
      let self = this
      this.$axios.get('Common/GetAgencyList')
        .then(function (response) {
          let data = response.data
          let adcdlist = []
          _.each(data.object, (obj) => {
            adcdlist.push({
              value: obj.FValue,
              label: obj.FName
            })
          })
          self.adlist = [].concat(adcdlist)
        })
        .catch(function (error) {
          console.log(error)
          self.$alert(error.message, '温馨提示', {
            confirmButtonText: '确定'
          })
        })
    },
    /**
     * 获取四边
     */
    getEdge () {
      let self = this
      this.$axios.get('Common/GetEnumList', {
        params: {
          EnumType: '四边'
        }
      })
        .then(function (response) {
          let data = response.data
          let list = []
          _.each(data.object, (obj) => {
            list.push({
              value: obj.FValue,
              label: obj.FName
            })
          })
          self.edgeOptions = [].concat(list)
        })
        .catch(function (error) {
          console.log(error)
          this.$message.error(error.message)
        })
    },
    /**
     * 获取审核状态
     */
    getStatusData () {
      let self = this
      this.$axios.get('Common/GetEnumList', {
        params: {
          EnumType: '审核状态'
        }
      })
        .then(function (response) {
          let data = response.data
          let list = []
          _.each(data.object, (obj) => {
            list.push({
              value: obj.FValue,
              label: obj.FName
            })
          })
          self.staOptions = [].concat(list)
        })
        .catch(function (error) {
          console.log(error)
          this.$message.error(error.message)
        })
    },
    /**
     * 获取整改状态
     */
    getChangeStatusData () {
      let self = this
      this.$axios.get('Common/GetEnumList', {
        params: {
          EnumType: '整改状态'
        }
      })
        .then(function (response) {
          let data = response.data
          let list = []
          _.each(data.object, (obj) => {
            list.push({
              value: obj.FValue,
              label: obj.FName
            })
          })
          self.cstaOptions = [].concat(list)
        })
        .catch(function (error) {
          console.log(error)
          this.$message.error(error.message)
        })
    },
    /**
     * 获取问题类型
     */
    getProblemType () {
      let self = this
      this.$axios.get('Common/GetEnumList', {
        params: {
          EnumType: '问题类型'
        }
      })
        .then(response => {
          let data = response.data
          let list = []
          _.each(data.object, (obj) => {
            list.push({
              value: obj.FValue,
              label: obj.FName
            })
          })
          self.proOptions = [].concat(list)
        })
        .catch(error => {
          console.log(error)
          this.$message.error(error.message)
        })
    },
    /**
     * 获取列表
     */
    getData () {
      let self = this
      this.$axios.post('LoanApply/GetSJList', {
        curr: this.cur_page,
        pageSize: 15,
        FAgencyValue: this.select_adcd,
        FBillTypeID: this.billTypeID,
        FBillNo: this.select_problem_num,
        FStatus: this.select_status,
        FYear: this.select_years,
        FMonth: this.select_mouths,
        FChangeStatus: this.select_cttatus,
        strSortFiled: '',
        strSortType: ''
      })
        .then(response => {
          let data = response.data
          self.tableData = data.object
        })
        .catch(error => {
          console.log(error)
          this.$message.error(error.message)
        })
    },
    /**
     * 搜索事件
     */
    search () {
      this.is_search = true
      this.getData()
    },
    formatter (row, column) {
      return row.address
    },
    // filterTag(value, row) {
    //     return row.tag === value
    // },

    /**
     * 修改信息触发
     */
    handleEdit (index, row) {
      this.editProblem(row.FID)
    },
    /**
     * 删除信息触发
     */
    handleDelete (index, row) {
      let self = this
      this.$confirm('确认删除？')
        .then(_ => {
          self.$axios.get('LoanApply/DeleteApply', {
            params: {
              FID: row.FID
            }
          })
            .then(function (response) {
              self.$message.error('删除成功！')
              self.getData()
            })
            .catch(function (error) {
              console.log(error)
              self.$alert(error.message, '温馨提示', {
                confirmButtonText: '确定'
              })
            })
        })
        .catch(_ => {
        })
    },
    // delAll(){
    //     const self = this,
    //         length = self.multipleSelection.length
    //     let str = ''
    //     self.del_list = self.del_list.concat(self.multipleSelection)
    //     for (let i = 0 i < length i++) {
    //         str += self.multipleSelection[i].name + ' '
    //     }
    //     self.$message.error('删除了'+str)
    //     self.multipleSelection = []
    // },
    handleSelectionChange (val) {
      this.multipleSelection = val
    },
    /**
     * 新增问题点位
     */
    addProblem () {
      this.proAddShow = true
      this.editFid = ''
    },
    /**
     * 修改问题点位
     */
    editProblem (fid) {
      this.proAddShow = true
      this.editFid = fid
    },
    /**
     * 关闭问题信息框
     */
    closePro: function (msg) {
      this.proAddShow = msg
      this.getData()
    },
    /**
     * 关闭地图选择框
     */
    closeMap: function (msg) {
      this.mapSelectShow = msg
    },
    /**
     * 设置定位信息
     */
    setPosition (msg) {
      this.position = msg.lng + ',' + msg.lat
    }
  },
  mounted () {

  },
  watch: {
    '$route' (to, from) {
      console.log(to)
      console.log(from)
      this.getStatus(this.$route.path)
      this.getBillTypeId()
      this.getBreadcrumb()
    }
  }
}
</script>

<style scoped>
  .handle-box {
    margin-bottom: 20px;
  }

  .handle-select {
    width: 120px;
  }

  .handle-input {
    width: 300px;
    display: inline-block;
  }
</style>
