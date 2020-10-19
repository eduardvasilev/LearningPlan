import Vue from 'vue';
import App from './App.vue';
import { store } from './store';
import { router } from "./router";

import 'bootstrap'
import 'bootstrap/dist/css/bootstrap.min.css'

Vue.config.productionTip = true;

new Vue({
    router: router,
    render: h => h(App),
    store: store
}).$mount('#app');
