<template>
    <div id="app">
        <div v-if="this.$store.state.user.isAuthenticated">
            <Home />
            <div class="app-body">
                <router-view></router-view>
            </div>
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
        background-color: #1d1d1d !important;
        color: #fefefe !important;
    }
    hr {
        background-color: #fefefe !important;
    }
    .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
        background-color: #e48900 !important;
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
        background-color: #e48900 !important;
        color: #fefefe !important;
    }
    .card {
        background-color: #1d1d1d !important;
        color: #fefefe !important;
       /* border: 2px solid;
        border-color: #fefefe !important;*/
    }
    .navbar {
        background-color: #e48900 !important;
    }

    .close > span {
        color: #fefefe !important;
    }

    .app-body{
        margin-left: 100px;
        margin-right: 100px;
    }

    .btn {
        color: #fefefe !important;
    }

    .jumbotron {
        background-color: #373738 !important;
    }

    .plan-card {
        border: 2px solid;
        border-color: #fefefe;
        box-shadow: 0px 0px 20px -3px #000000;
    }
</style>
