<template>
    <el-dialog title="新增问题" :visible.sync="formShow" :before-close="handleClose">
        <el-form :inline="true" :model="formInline" :rules="rules" ref="ruleForm" class="demo-form-inline demo-ruleForm">
            <el-form-item label="行政区划" :label-width="formLabelWidth" prop="adcd" style="width: 48%">
                <el-select v-model="formInline.adcd">
                    <el-option
                        v-for="item in adcdOptions"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="乡镇街道" :label-width="formLabelWidth" prop="town" style="width: 48%">
                <el-input v-model="formInline.town" auto-complete="off" placeholder="请输入乡镇街道"></el-input>
            </el-form-item>
            <el-form-item label="年度月份" :label-width="formLabelWidth" prop="month" style="width: 48%">
                <el-date-picker
                    v-model="formInline.month"
                    type="month"
                    placeholder="选择年度月份">
                </el-date-picker>
            </el-form-item>
            <el-form-item label="问题编号" :label-width="formLabelWidth" prop="proNum" style="width: 48%">
                <el-input v-model="formInline.proNum" auto-complete="off" placeholder="请输入问题编号"></el-input>
            </el-form-item>
            <el-form-item label="线路名称" :label-width="formLabelWidth" prop="lineName" style="width: 48%">
                <el-input v-model="formInline.lineName" auto-complete="off" placeholder="请输入线路名称"></el-input>
            </el-form-item>
            <el-form-item label="里程" :label-width="formLabelWidth" style="width: 48%">
                <el-input v-model="formInline.mileage" auto-complete="off" placeholder="请输入里程"></el-input>
            </el-form-item>
            <el-form-item label="问题描述" :label-width="formLabelWidth" style="width: 48%">
                <el-input v-model="formInline.proDescribe" auto-complete="off" placeholder="请输入问题描述"></el-input>
            </el-form-item>
            <el-form-item label="整治时限" :label-width="formLabelWidth" prop="duration" style="width: 48%">
                <el-input v-model="formInline.duration" auto-complete="off" placeholder="请输入整治时限"></el-input>
            </el-form-item>
            <el-form-item label="问题分类" :label-width="formLabelWidth" style="width: 48%">
                <el-select v-model="formInline.proType" placeholder="请选择问题分类">
                    <el-option
                        v-for="(item, i) in proOption"
                        :key="i"
                        :label="item.label"
                        :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="定位信息" :label-width="formLabelWidth" style="width: 48%">
                <el-input v-model="sposition" auto-complete="off" @focus="openMap" placeholder="点击选择定位"></el-input>
                <!--<el-button type="primary" @click="openMap">主要按钮</el-button>-->
            </el-form-item>
            <el-form-item label="备注" :label-width="formLabelWidth" style="width: 48%">
                <el-input v-model="formInline.name" auto-complete="off"></el-input>
            </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
            <el-button @click="handleClose">取 消</el-button>
            <el-button type="primary" @click="handleClose">确 定</el-button>
        </div>
    </el-dialog>
</template>

<script>
    export default {
        data() {
            return {
                append: true,
                innerVisible: false,
                formInline: {
                    adcd: '331002',
                    town: '',
                    year: '',
                    month: '',
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
                        { required: true, message: '请选择行政区划', trigger: 'change' }
                    ],
                    town: [
                        { required: true, message: '请输入乡镇街道', trigger: 'blur' }
                    ],
                    month: [
                        { type: 'date', required: true, message: '请选择年度月份', trigger: 'change' }
                    ],
                    proNum: [
                        { required: true, message: '请输入问题编号', trigger: 'blur' }
                    ],
                    lineName: [
                        { required: true, message: '请输入线路名称', trigger: 'blur' }
                    ],
                    duration: [
                        { required: true, message: '请输入整治时限', trigger: 'blur' }
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
            }
        },
        props: ['formShow', 'sposition']
    }
</script>
