<template>
    <el-dialog title="新增问题" :visible.sync="formShow" :before-close="handleClose">
        <el-form :model="formInline" :rules="rules" ref="ruleForm" class="demo-form-inline demo-ruleForm">
            <el-row>
                <el-col :span="12">
                    <el-form-item label="行政区划" :label-width="formLabelWidth" prop="adcd">
                        <el-select v-model="formInline.adcd">
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
                        <el-input v-model="formInline.town" auto-complete="off" placeholder="请输入乡镇街道"></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="年度月份" :label-width="formLabelWidth" prop="month">
                        <el-date-picker
                            v-model="formInline.month"
                            type="month"
                            placeholder="选择年度月份">
                        </el-date-picker>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="问题编号" :label-width="formLabelWidth" prop="proNum">
                        <el-input v-model="formInline.proNum" auto-complete="off" placeholder="请输入问题编号"></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="线路名称" :label-width="formLabelWidth" prop="lineName">
                        <el-input v-model="formInline.lineName" auto-complete="off" placeholder="请输入线路名称"></el-input>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="里程" :label-width="formLabelWidth">
                        <el-input v-model="formInline.mileage" auto-complete="off" placeholder="请输入里程"></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="问题描述" :label-width="formLabelWidth">
                        <el-input v-model="formInline.proDescribe" auto-complete="off" placeholder="请输入问题描述"></el-input>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="整治时限" :label-width="formLabelWidth" prop="duration">
                        <el-input v-model="formInline.duration" auto-complete="off" placeholder="请输入整治时限"></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="12">
                    <el-form-item label="问题分类" :label-width="formLabelWidth">
                        <el-select v-model="formInline.proType" placeholder="请选择问题分类">
                            <el-option
                                v-for="(item, i) in proOption"
                                :key="i"
                                :label="item.label"
                                :value="item.value">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="定位信息" :label-width="formLabelWidth">
                        <el-input v-model="sposition" auto-complete="off" @focus="openMap"
                                  placeholder="点击选择定位"></el-input>
                        <!--<el-button type="primary" @click="openMap">主要按钮</el-button>-->
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="24">
                    <el-form-item label="备注" :label-width="formLabelWidth">
                        <el-input v-model="formInline.name"
                                  auto-complete="off"
                                  type="textarea"
                                  :rows="2"
                                  placeholder="请输入内容"></el-input>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
        <div slot="footer" class="dialog-footer">
            <el-button @click="handleClose">取 消</el-button>
            <el-button type="primary" @click="handleClose">确 定</el-button>
        </div>
    </el-dialog>
</template>

<script>
    import Axios from 'axios';
    import _ from 'lodash';
    import {formatDate} from '../../assets/js/date.js';

    export default {
        data() {
            return {
                append: true,
                innerVisible: false,
                formInline: {
                    adcd: '331002',
                    town: '',
                    year: '',
                    month: formatDate(new Date(), 'yyyy-MM'),
                    name: '',
                    proNum: '',
                    lineName: '',
                    mileage: '',
                    proDescribe: '',
                    duration: '',
                    proType: '0'
                },
                formLabelWidth: '120px',
                adcdOptions: [{
                    value: '331002',
                    label: '椒江区'
                }, {
                    value: 'fankui',
                    label: '路桥区'
                }, {
                    value: 'xiaolv',
                    label: '黄岩区'
                }
                ],
                proOption: [
                    {
                        value: '0',
                        label: '乱搭乱建'
                    },
                    {
                        value: '1',
                        label: '乱堆乱放'
                    },
                    {
                        value: '2',
                        label: '废品垃圾'
                    }
                ],
                rules: {
                    adcd: [
                        {required: true, message: '请选择行政区划', trigger: 'change'}
                    ],
                    town: [
                        {required: true, message: '请输入乡镇街道', trigger: 'blur'}
                    ],
                    month: [
                        {type: 'date', required: true, message: '请选择年度月份', trigger: 'change'}
                    ],
                    proNum: [
                        {required: true, message: '请输入问题编号', trigger: 'blur'}
                    ],
                    lineName: [
                        {required: true, message: '请输入线路名称', trigger: 'blur'}
                    ],
                    duration: [
                        {required: true, message: '请输入整治时限', trigger: 'blur'}
                    ]
                }
            }
        },
        methods: {
            handleClose() {
                this.$emit('closeProAdd', false);
            },
            openMap() {
                this.$emit('selectMap', true);
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
                        self.adcdOptions = [].concat(adcdlist);
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
                        FEnumTypeID: 4
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
                    })
                    .catch(function (error) {
                        console.log(error);
                        self.$alert(error.message, '温馨提示', {
                            confirmButtonText: '确定'
                        });
                    });
            },
            save() {
                Axios.post('LoanApply/SaveSJApply', {})
            }
        },
        props: ['formShow', 'sposition'],
        created() {
            this.getAdcd();
            this.getProblemType();
        }
    }
</script>
