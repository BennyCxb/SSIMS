<template>
    <el-dialog :title="title" :visible.sync="formShow" :before-close="handleClose" width="80%">
        <el-form :model="form" :rules="rules" ref="probForm" class="demo-form-inline demo-ruleForm">
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
                                v-for="item in edgeOptions"
                                :key="item.value"
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
                                v-for="item in adcdOptions"
                                :key="item.value"
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
                    <el-form-item label="定位信息" :label-width="formLabelWidth" prop="position">
                        <el-input v-model="form.position" @focus="openMap"
                                  placeholder="点击选择定位"></el-input>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="问题分类" :label-width="formLabelWidth" prop="proType">
                        <el-select v-model="form.proType" placeholder="请选择问题分类">
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
                    <el-form-item label="问题描述" :label-width="formLabelWidth" prop="proDescribe">
                        <el-input v-model="form.proDescribe"
                                  type="textarea"
                                  :rows="2"
                                  placeholder="请输入问题描述"></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="24">
                    <el-form-item label="备注" :label-width="formLabelWidth" prop="remarks">
                        <el-input v-model="form.remarks"
                                  type="textarea"
                                  :rows="2"
                                  placeholder="请输入内容"></el-input>
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
    import _ from 'lodash';
    import {formatDate} from '../../assets/js/date.js';

    export default {
        data() {
            return {
                title: '新增问题',
                append: true,
                innerVisible: false,
                form: {
                    fid: this.fid,
                    billTypeId: this.billTypeId,
                    billNo: '',
                    adcd: '',
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
                    remarks: '',
                },
                formLabelWidth: '120px',
                adcdOptions: [],
                edgeOptions: [],
                proOptions: [],
                rules: {
                    adcd: [
                        {required: true, message: '请选择行政区划', trigger: 'change'}
                    ],
                    edge: [
                        {required: true, message: '请选择', trigger: 'change'}
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
                        {required: true, message: '请选择问题类型', trigger: 'change'}
                    ],
                    lineName: [
                        {required: true, message: '请输入线路名称', trigger: 'blur'}
                    ],
                    position: [
                        {required: false, message: '请选择定位', trigger: 'blur'}
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
            handleClose() {
                this.$confirm('确认关闭？')
                    .then(_ => {
                        this.$emit('closeProAdd', false);
                    })
                    .catch(_ => {});
            },
            getBillTypeId() {
                this.billTypeID = this.$route.params.btid
            },
            openMap() {
                this.$emit('selectMap', true);
            },
            getAdcd() {
                let self = this;
                this.$axios.get('Common/GetAgencyList')
                    .then(function (response) {
                        let data = response.data;
                        let adcdlist = [];
                        _.each(data.object, (obj) => {
                            adcdlist.push({
                                value: obj.FValue,
                                label: obj.FName
                            })
                        })
                        self.adcdOptions = [].concat(adcdlist);
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
                this.$axios.get('Common/GetEnumList', {
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
                this.$axios.get('Common/GetEnumList', {
                    params: {
                        EnumType: '问题类型'
                    }
                })
                    .then(response => {
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
                    .catch(error => {
                        console.log(error);
                        self.$alert(error.message, '温馨提示', {
                            confirmButtonText: '确定'
                        });
                    });
            },
            /**
             * 获取问题详情
             */
            getInfo() {
                console.log(this.fid)
                let self = this
                // this.resetForm('probForm')
                Object.assign(this.$data.form, this.$options.data().form)
                if (this.fid != '') {
                    this.title = '编辑问题'
                    this.$axios.get('LoanApply/GetApplyInfo', {
                        params: {
                            FID: this.fid
                        }
                    })
                        .then(response => {
                            let data = response.data;
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
                                this.$message({
                                    message: data.message,
                                    type: 'warning'
                                });
                            }
                        })
                        .catch(error => {
                            console.log(error);
                            this.$message.error(error.message);
                        });
                } else {
                    this.title = '新增问题'
                }

            },
            /**
             * 重置表单
             */
            resetForm(formName) {
                this.$refs[formName].resetFields();
            },
            /**
             * 提交数据
             */
            submit(formName) {
                let self = this;

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
                        if (self.form.fid != '') {
                            data.FID = self.form.fid
                        }
                        this.$axios.post('LoanApply/SaveSJApply', data)
                            .then(response => {
                                let data = response.data;
                                if (data.code === 1) {
                                    this.$message({
                                        message: self.fid != ''? '修改成功' : '新增成功！',
                                        type: 'success'
                                    });
                                    this.$emit('closeProAdd', false);
                                } else {
                                    this.$message({
                                        message: data.message,
                                        type: 'warning'
                                    });
                                }
                            })
                            .catch(error => {
                                console.log(error);
                                this.$message.error(error.message);
                            });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });

            }
        },
        props: ['fid', 'formShow', 'sposition', 'billTypeId'],
        created() {
            this.getAdcd();
            this.getEdge();
            this.getProblemType();
        },
        watch: {
            formShow(curVal) {
                if (curVal) {
                    this.getInfo()
                }
            },
            fid(curVal) {
                this.form.fid = curVal
            },
            sposition(curVal) {
                this.form.position = curVal
            },
            billTypeId(curVal) {
                this.form.billTypeId = curVal
            }
        }
    }
</script>
