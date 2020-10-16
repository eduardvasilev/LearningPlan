import Vue from 'vue';
import App from './App.vue';
import { store } from './store';

Vue.config.productionTip = true;

new Vue({
    render: h => h(App),
    store: store
}).$mount('#app');
