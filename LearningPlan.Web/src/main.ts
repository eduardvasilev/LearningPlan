import Vue from 'vue';
import App from './App.vue';
import { store } from './store';
import { router } from "./router";

Vue.config.productionTip = true;

new Vue({
    router: router,
    render: h => h(App),
    store: store
}).$mount('#app');
