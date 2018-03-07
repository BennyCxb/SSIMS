<template>
    <el-dialog title="新增问题" :visible.sync="formShow" :before-close="handleClose">
        <el-form :model="form">
            <el-form-item class="el-col-12" label="行政区划" :label-width="formLabelWidth">
                <el-cascader
                    :options="options"
                    v-model="form.adcd"
                ></el-cascader>
            </el-form-item>
            <el-form-item class="el-col-12" label="乡镇街道" :label-width="formLabelWidth">
                <el-input v-model="form.name" auto-complete="off"></el-input>
            </el-form-item>
            <el-form-item class="el-col-12" label="年度月份" :label-width="formLabelWidth">
                <el-date-picker
                    v-model="form.month"
                    type="month"
                    placeholder="选择年度月份">
                </el-date-picker>
            </el-form-item>
            <el-form-item class="el-col-12" label="问题编号" :label-width="formLabelWidth">
                <el-input v-model="form.name" auto-complete="off"></el-input>
            </el-form-item>
            <el-form-item class="el-col-12" label="线路名称" :label-width="formLabelWidth">
                <el-input v-model="form.name" auto-complete="off"></el-input>
            </el-form-item>
            <el-form-item class="el-col-12" label="里程" :label-width="formLabelWidth">
                <el-input v-model="form.name" auto-complete="off"></el-input>
            </el-form-item>
            <el-form-item class="el-col-12" label="问题分类" :label-width="formLabelWidth">
                <el-select v-model="form.proType" placeholder="请选择">
                    <el-option
                        v-for="(item, i) in proOption"
                        :key="i"
                        :label="item.label"
                        :value="item.value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item class="el-col-12" label="问题描述" :label-width="formLabelWidth">
                <el-input v-model="form.name" auto-complete="off"></el-input>
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
        data: function(){
            return {
                form: {
                    adcd: ['33','331000','331002'],
                    year: '',
                    month: '',
                    name: '',
                    proType: '0',
                    region: '',
                    date1: '',
                    date2: '',
                    delivery: false,
                    type: [],
                    resource: '',
                    desc: '',
                },
                formLabelWidth: '120px',
                options: [{
                    value: '33',
                    label: '浙江省',
                    children: [{
                        value: '331000',
                        label: '台州市',
                        children: [{
                            value: '331002',
                            label: '椒江区'
                        }, {
                            value: 'fankui',
                            label: '路桥区'
                        }, {
                            value: 'xiaolv',
                            label: '黄岩区'
                        }]
                    }, {
                        value: 'daohang',
                        label: '杭州市',
                        children: [{
                            value: 'cexiangdaohang',
                            label: '下城区'
                        }, {
                            value: 'dingbudaohang',
                            label: '上城区'
                        }]
                    }]
                }],
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
                ]
            }
        },
        methods: {
            handleClose: function () {
                this.$emit('closeProAdd', false);
            }
        },
        props: ['formShow']
    }
</script>
