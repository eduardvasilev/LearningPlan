<template>
    <div id="app">
        <div v-if="this.$store.state.user.isAuthenticated">
            <Home />
            <router-view></router-view>
        </div>
        <div v-else>
            <div class="login-form">
                <div class="row">
                    <div class="center-block">
                        <ul class="nav nav-pills" id="login-tabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="register-tab" data-toggle="pill" href="#register" role="tab" aria-controls="register" aria-selected="true">Register</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="login-tab" data-toggle="pill" href="#login" role="tab" aria-controls="login" aria-selected="false">Login</a>
                            </li>
                        </ul>
                        <div class="tab-content" id="myTabContent">
                            <div class="tab-pane fade show active" id="register" role="tabpanel" aria-labelledby="register-tab">
                                <Register />
                            </div>
                            <div class="tab-pane fade" id="login" role="tabpanel" aria-labelledby="login-tab">
                                <Login />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import Home from './components/Home.vue';
    import Register from './components/Register.vue';
    import Login from './components/Login.vue';

    @Component({
        components: {
            Home,
            Register,
            Login
        }
    })
    export default class App extends Vue {
        data: any = {
            isAuthenticated: this.$store.state.user.isAuthenticated
        };
    }
</script>

<style>

    body, .modal-content {
        background-color: #0a0a0a !important;
        color: #fefefe !important;
    }
    .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
        background-color: #ff9900 !important;
    }
    .login-form {
        margin: 0;
        position: absolute;
        top: 30%;
        left: 40%;
        margin-right: -50%;
    }
    a {
        color: #fefefe !important;
    }
    a:hover {
        color: whitesmoke !important;
    }
   
    .btn-primary {
        background-color: #ff9900 !important;
        color: #fefefe !important;
    }
    .card {
        background-color: #0a0a0a !important;
        color: #fefefe !important;
        border: 2px solid;
        border-color: #fefefe !important;
    }
    .navbar {
        background-color: #ff9900 !important;
    }

    .close > span {
        color: #fefefe !important;
    }

</style>
