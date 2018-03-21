import Vue from 'vue';
import App from './App';
import router from './router';
import axios from 'axios';
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-default/index.css';    // 默认主题
// import '../static/css/theme-green/index.css';       // 浅绿色主题
import "babel-polyfill";
Vue.use(ElementUI);

// 引入cookie
import {setCookie, getCookie} from 'assets/js/cookie.js';

// 引入vue-amap
import VueAMap from 'vue-amap';
Vue.use(VueAMap);

// 引入lodash
import lodash from 'lodash';
// import VueLodash from 'vue-lodash';
Vue.use(lodash);


// 初始化vue-amap
VueAMap.initAMapApiLoader({
    key: '4cf9ecbcb9448825d9b3f92064db8045',
    plugin: ['AMap.Scale', 'AMap.OverView', 'AMap.ToolBar', 'AMap.MapType', 'AMap.Geocoder', 'AMap.Geolocation'],
    v: '1.4.4'
});


Vue.prototype.$axios = axios;
// Vue.prototype.$axios.defaults.baseURL = 'http://localhost:8088/api/';
Vue.prototype.$axios.defaults.baseURL = 'http://tzsgyc.iok.la/api/';
// 添加请求拦截器
Vue.prototype.$axios.interceptors.request.use(function (config) {
    // 在发送请求之前做些什么
    if (getCookie('TZManage')) {
        config.headers.common['Authorization'] = "Bearer " + getCookie('TZManage');
    } else {
        parent.location.href = '#/login'
    }

    return config;
}, function (error) {
    // 对请求错误做些什么
    return Promise.reject(error);
});
// axios.defaults.timeout = 1000;
//默认的contenttype为json以及utf-8；
// axios.defaults.headers={'Content-Type': 'text/html;charset=gb2312'}
new Vue({
    router,
    render: h => h(App),
    components: {Map}
}).$mount('#app');
