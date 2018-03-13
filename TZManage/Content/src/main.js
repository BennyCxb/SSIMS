import Vue from 'vue';
import App from './App';
import router from './router';
import axios from 'axios';
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-default/index.css';    // 默认主题
// import '../static/css/theme-green/index.css';       // 浅绿色主题
import "babel-polyfill";
// import Map from './components/common/Map'

// 引入vue-amap
import VueAMap  from 'vue-amap';
Vue.use(VueAMap );
// 初始化vue-amap
VueAMap.initAMapApiLoader({
    key: '4cf9ecbcb9448825d9b3f92064db8045',
    plugin: ['AMap.Scale', 'AMap.OverView', 'AMap.ToolBar', 'AMap.MapType', 'AMap.Geocoder', 'AMap.Geolocation'],
    v: '1.4.4'
});

Vue.use(ElementUI);
Vue.prototype.$axios = axios;
axios.defaults.baseURL = 'http://localhost:8088/api/';
// axios.defaults.timeout = 1000;
//默认的contenttype为json以及utf-8；
// axios.defaults.headers={'Content-Type': 'text/html;charset=gb2312'}
new Vue({
    router,
    render: h => h(App),
    components: { Map }
}).$mount('#app');
