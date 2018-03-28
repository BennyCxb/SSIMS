<template>
  <el-dialog :title="title" :visible.sync="formShow" :before-close="handleClose" width="80%">
    <el-form :model="form" ref="probForm" class="demo-form-inline demo-ruleForm">
      <el-row>
        <el-col :span="12">
          <el-form-item label="年度月份" :label-width="formLabelWidth" prop="month">
            <el-date-picker
              v-model="form.month"
              type="month"
              placeholder="选择年度月份">
            </el-date-picker>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="四边" :label-width="formLabelWidth" prop="edge">
            <el-select v-model="form.edge">
              <el-option
                v-for="(item,i) in edgeOptions"
                :key="i"
                :label="item.label"
                :value="item.value">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item label="行政区划" :label-width="formLabelWidth" prop="adcd">
            <el-select v-model="form.adcd">
              <el-option
                v-for="(item,i) in adcdOptions"
                :key="i"
                :label="item.label"
                :value="item.value">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="乡镇街道" :label-width="formLabelWidth" prop="town">
            <el-input v-model="form.town" placeholder="请输入乡镇街道"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item label="线路名称" :label-width="formLabelWidth" prop="lineName">
            <el-input v-model="form.lineName" placeholder="请输入线路名称"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="里程" :label-width="formLabelWidth" prop="mileage">
            <el-input v-model="form.mileage" placeholder="请输入里程"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item label="定位信息" :label-width="formLabelWidth">
            <el-input v-model="form.position" @focus="openMap"
                      placeholder="点击选择定位"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="问题类型" :label-width="formLabelWidth" prop="proType">
            <el-select v-model="form.proType" placeholder="请选择问题类型">
              <el-option
                v-for="(item, i) in proOptions"
                :key="i"
                :label="item.label"
                :value="item.value">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <!--<el-col :span="12">-->
        <!--<el-form-item label="整治时限" :label-width="formLabelWidth" prop="duration">-->
        <!--<el-input v-model="form.duration" auto-complete="off" placeholder="请输入整治时限"></el-input>-->
        <!--</el-form-item>-->
        <!--</el-col>-->
      </el-row>
      <el-row>
        <el-col :span="24">
          <el-form-item label="问题描述" :label-width="formLabelWidth">
            <el-input v-model="form.proDescribe"
                      type="textarea"
                      :rows="2"
                      placeholder="请输入问题描述"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="24">
          <el-form-item label="备注" :label-width="formLabelWidth">
            <el-input v-model="form.remarks"
                      type="textarea"
                      :rows="2"
                      placeholder="请输入内容"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>

    <el-form :model="files" ref="probForm2" class="demo-form-inline demo-ruleForm">
      <el-row>
        <el-col :span="24">
          <el-form-item label="整改前照片" :label-width="formLabelWidth">
            <el-upload
              ref="upload"
              :action="url"
              :headers="files.headers"
              :auto-upload="false"
              list-type="picture-card"
              :on-preview="handlePictureCardPreview"
              :data="files.data"
              :accept="files.accept1"
              :on-success="uploadSuccess">
              <i class="el-icon-plus"></i>
            </el-upload>
            <el-dialog :visible.sync="dialogVisible"
                       append-to-body>
              <img width="100%" :src="dialogImageUrl" alt="">
            </el-dialog>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <div slot="footer" class="dialog-footer">
      <el-button @click="resetForm('probForm')">重置</el-button>
      <el-button type="primary" @click="submit('probForm')">确 定</el-button>
    </div>
  </el-dialog>
</template>

<script>
import {formatDate} from '../../assets/js/date.js'
import _ from 'lodash'

export default {
  computed: {
    url () {
      return this.$axios.defaults.baseURL + 'Files/UploadFileForQiNiu'
    }
  },
  data () {
    return {
      title: '新增问题',
      append: true,
      innerVisible: false,
      form: {
        isSubmited: false,
        fid: this.fid,
        billTypeId: this.billTypeId,
        billNo: '',
        adcd: 331002,
        edge: 1,
        town: '',
        year: '',
        month: formatDate(new Date(), 'yyyy-M'),
        proNum: '',
        lineName: '',
        mileage: '',
        position: this.sposition,
        proType: 1,
        proDescribe: '',
        remarks: ''
      },
      files: {
        accept1: 'image/*',
        headers: {
          Authorization: 'Bearer ' + this.$cookies.get('TZManage')
        },
        data: {
          AttachType: 'image/*',
          FBillTypeID: Number(this.billTypeId),
          FLoanID: 0
        }
      },
      formLabelWidth: '120px',
      adcdOptions: [],
      edgeOptions: [],
      proOptions: [],
      rules: {
        adcd: [
          {type: 'number', required: true, message: '请选择行政区划', trigger: 'change'}
        ],
        edge: [
          {type: 'number', required: true, message: '请选择', trigger: 'change'}
        ],
        town: [
          {required: true, message: '请输入乡镇街道', trigger: 'blur'}
        ],
        month: [
          {required: true, message: '请选择年度月份', trigger: 'change'}
        ],
        mileage: [
          {required: true, message: '请输入里程', trigger: 'blur'}
        ],
        proType: [
          {type: 'number', required: true, message: '请选择问题类型', trigger: 'change'}
        ],
        lineName: [
          {required: true, message: '请输入线路名称', trigger: 'blur'}
        ],
        position: [
          {required: false}
        ],
        proDescribe: [
          {required: false}
        ],
        remarks: [
          {required: false}
        ]
      },
      dialogImageUrl: '',
      dialogVisible: false
    }
  },
  methods: {
    handleClose () {
      this.$confirm('确认关闭？')
        .then(_ => {
          this.$emit('closeProAdd', false)
        })
        .catch(_ => {
        })
    },
    getBillTypeId () {
      this.billTypeID = this.$route.params.btid
    },
    openMap () {
      this.$emit('selectMap', true)
    },
    getAdcd () {
      let self = this
      this.$axios.get('Common/GetAgencyList')
        .then(response => {
          let data = response.data
          let adcdlist = []
          _.each(data.object, (obj) => {
            adcdlist.push({
              value: Number(obj.FValue),
              label: obj.FName
            })
          })
          self.adcdOptions = [].concat(adcdlist)
        })
        .catch(error => {
          console.log(error)
          self.$alert(error.message, '温馨提示', {
            confirmButtonText: '确定'
          })
        })
    },
    getEdge () {
      let self = this
      this.$axios.get('Common/GetEnumList', {
        params: {
          EnumType: '四边'
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
          self.edgeOptions = [].concat(list)
        })
        .catch(error => {
          console.log(error)
          self.$alert(error.message, '温馨提示', {
            confirmButtonText: '确定'
          })
        })
    },
    getProblemType () {
      let self = this
      this.$axios.get('Common/GetEnumList', {
        params: {
          EnumType: '问题类型'
        }
      })
        .then(response => {
          let data = response.data
          let ptypelist = []
          _.each(data.object, (obj) => {
            ptypelist.push({
              value: obj.FValue,
              label: obj.FName
            })
          })
          self.proOptions = [].concat(ptypelist)
        })
        .catch(error => {
          console.log(error)
          self.$alert(error.message, '温馨提示', {
            confirmButtonText: '确定'
          })
        })
    },
    /**
     * 获取问题详情
     */
    getInfo () {
      console.log(this.fid)
      let self = this
      Object.assign(this.$data.form, this.$options.data().form)
      Object.assign(this.$data.form, this.$options.data().files)
      if (this.fid !== '') {
        this.title = '编辑问题'
        this.$axios.get('LoanApply/GetApplyInfo', {
          params: {
            FID: this.fid
          }
        })
          .then(response => {
            let data = response.data
            if (data.code === 1) {
              var obj = data.object
              self.form = {
                fid: obj.FID,
                billTypeId: obj.FBillTypeID,
                billNo: obj.FBillNo,
                adcd: obj.FAgencyValue,
                edge: obj.FPerimeter,
                town: obj.FTwon,
                year: obj.FYear,
                month: obj.FYear + '-' + obj.FMonth,
                lineName: obj.FLineName,
                mileage: obj.FMileage,
                position: obj.FGPS,
                proType: obj.FProbTypeID,
                proDescribe: obj.FProbDescribe,
                remarks: obj.FRemark
              }
            } else {
              self.$message({
                message: data.message,
                type: 'warning'
              })
            }
          })
          .catch(error => {
            console.log(error)
            this.$message.error(error.message)
          })
      } else {
        this.title = '新增问题'
      }
    },
    /**
     * 重置表单
     */
    resetForm (formName) {
      this.$refs[formName].resetFields()
    },
    /**
     * 提交数据
     */
    submit (formName) {
      let self = this
      if (self.form.isSubmited === false) {
        this.$refs[formName].validate((valid) => {
          if (valid) {
            let data = {
              FBillTypeID: self.form.billTypeId,
              FBillNo: self.form.billNo,
              FAgencyValue: self.form.adcd,
              FPerimeter: self.form.edge,
              FYear: Number(self.form.month.substring(0, 4)),
              FMonth: Number(self.form.month.substring(5)),
              FLineName: self.form.lineName,
              FMileage: self.form.mileage,
              FProbTypeID: self.form.proType,
              FProbDescribe: self.form.proDescribe,
              FGPS: self.form.position,
              FTwon: self.form.town,
              FRemark: self.form.remarks
            }
            if (self.form.fid !== '') {
              data.FID = self.form.fid
            }
            this.$axios.post('LoanApply/SaveSJApply', data)
              .then(response => {
                let data = response.data
                if (data.code === 1) {
                  self.files.FLoanID = data.object
                  self.form.isSubmited = true
                  if (self.form.fid === '' || undefined) {
                    self.submitUpload()
                  } else {

                  }
                  // this.$message({
                  //     message: self.fid != ''? '修改成功' : '新增成功！',
                  //     type: 'success'
                  // })
                  // this.$emit('closeProAdd', false)
                } else {
                  this.$message({
                    message: data.message,
                    type: 'warning'
                  })
                }
              })
              .catch(error => {
                console.log(error)
                self.$message.error(error.message)
              })
          } else {
            console.log('error submit!!')
            return false
          }
        })
      } else {
        self.submitUpload()
      }
    },
    getAttachTypeList: function () {
      var self = this
      this.$axios.get('Files/GetAttachTypeList', {
        params: {
          FBillTypeID: self.form.billTypeId
        }
      })
        .then(response => {
          let data = response.data
          if (data.code === 1) {
            _.each(data.object, obj => {
              if (obj.FName === '整改前照片') {
                self.files.accept1 = obj.FFileType
              } else if (obj.FName === '整改后照片') {
                self.files.accept2 = obj.FFileType
              }
            })
          } else {
            self.$message({
              message: data.message,
              type: 'warning'
            })
          }
        })
        .catch(error => {
          console.log(error)
          self.$message.error(error.message)
        })
    },
    /**
     * 获取整改照片地址
     */
    getFilesUrl: function () {
      var self = this
      this.$axios.get('Files/GetFilesUrl', {
        params: {
          FLoanID: self.files.FLoanID,
          FBillTypeID: self.form.billTypeId,
          FAttachType: self.files.accept1
        }
      })
        .then(response => {
          let data = response.data
          console.log(data)
          if (data.code === 1) {

          } else {
            this.$message({
              message: data.message,
              type: 'warning'
            })
          }
        })
        .catch(error => {
          console.log(error)
          this.$message.error(error.message)
        })
    },
    submitUpload () {
      console.log(this.$refs.upload)
      this.$refs.upload.submit()
    },
    uploadSuccess (response, file, fileLis) {
      var self = this
      var data = response
      console.log(response)
      if (data.code === 1) {
        this.$message({
          message: self.fid !== '' ? '修改成功' : '新增成功！',
          type: 'success'
        })
        this.$emit('closeProAdd', false)
      } else {
        this.$message({
          message: data.message,
          type: 'warning'
        })
      }
    },
    handleRemove (file, fileList) {
      console.log(file, fileList)
    },
    handlePictureCardPreview (file) {
      this.dialogImageUrl = file.url
      this.dialogVisible = true
    },
    imageuploaded (res) {
      console.log(res)
    },
    handleError () {
      this.$notify.error({
        title: '上传失败',
        message: '图片上传接口上传失败，可更改为自己的服务器接口'
      })
    }
  },
  props: ['fid', 'formShow', 'sposition', 'billTypeId'],
  created () {
    this.getAdcd()
    this.getEdge()
    this.getProblemType()
    this.getAttachTypeList()
  },
  watch: {
    formShow (curVal) {
      if (curVal) {
        this.getInfo()
      }
    },
    fid (curVal) {
      this.form.fid = curVal
    },
    sposition (curVal) {
      this.form.position = curVal
    },
    billTypeId (curVal) {
      this.form.billTypeId = curVal
    }
  }
}
</script>
