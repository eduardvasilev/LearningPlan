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
                                <a class="nav-link active" id="login-tab" data-toggle="pill" href="#login" role="tab" aria-controls="login" aria-selected="false">Sign In</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="register-tab" data-toggle="pill" href="#register" role="tab" aria-controls="register" aria-selected="true">Sign Up</a>
                            </li>
                        </ul>
                        <div class="tab-content" id="myTabContent">
                           <div class="tab-pane fade show active" id="login" role="tabpanel" aria-labelledby="login-tab">
                                <Login />
                            </div>
                            <div class="tab-pane fade" id="register" role="tabpanel" aria-labelledby="register-tab">
                                <Register />
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
/* https://www.canva.com/colors/color-palettes/northern-lights-3/ */
    body, .modal-content {
        background-color: #EAEAE0 !important;
        /* color: #EAEAE0 !important; */
    }
    hr {
        background-color: #EAEAE0 !important;
    }

    svg{
        color: #1C4670;
    }

    .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
        background-color: #278AB0 !important;
    }

.nav-link:hover{
    /* background-color: #1C4670 !important; */
    color: black !important;
}
.logout{
    cursor: pointer;
}

    .login-form {
        margin: 0;
        position: absolute;
        top: 30%;
        left: 40%;
        margin-right: -50%;
    }
    a {
        color: #000000 !important;
        text-decoration: none !important;
    }
    a:hover {
        color: #EAEAE0 !important;
    }
   
   .btn{
         border: none !important;
        border-radius: 0% !important;
   }

    .btn-primary {
        background-color: #278AB0 !important;
        color: #EAEAE0 !important;
      
    }
    .card {
        background-color: #1DC690 !important;
        /* color: black !important; */
       /* border: 2px solid;
        border-color: #EAEAE0 !important;*/
    }
    .navbar {
        background-color: #1DC690 !important;
    }

    .close > span {
        color: #1DC690 !important;
    }

    .app-body{
        margin-left: 100px;
        margin-right: 100px;
        padding-top: 100px;
    }

    .btn {
        color: #EAEAE0 !important;
    }
   .btn:hover {
     text-decoration: none !important;
    }
    .jumbotron {
        background-color: #373738 !important;
    }

    .plan-card {
        border: 2px solid;
        border-color: #EAEAE0;
        /* box-shadow: 0px 0px 20px -3px #000000; */
    }

    .home{
        position: fixed;
        width: 100%;
        z-index: 1;
        border-bottom: solid 1px;
    }
</style>
