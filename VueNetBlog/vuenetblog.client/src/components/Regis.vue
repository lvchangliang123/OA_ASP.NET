<template>
    <div>
        <el-container class="container">
            <el-card>
                <el-row style="height: 400px">
                    <el-col :span="18">
                        <el-image :src="imageSrc" style="height: 400px; border-radius: 4px" :fit="fill" />
                    </el-col>
                    <el-col :span="6">
                        <el-form style="height: 200px">
                            <el-input v-model="username" placeholder="用户名">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><UserFilled /></el-icon>
                                </template>
                            </el-input>
                            <el-input v-model="email" style="margin-top: 15px" placeholder="邮箱">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><span class="iconfont icon-email"></span></el-icon>
                                </template>
                            </el-input>
                            <el-input v-model="password" style="margin-top: 15px" placeholder="密码" type="password">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><span class="iconfont icon-mima"></span></el-icon>
                                </template>
                            </el-input>
                            <el-input v-model="confirmPassword" style="margin-top: 15px" placeholder="确认密码" type="password">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><span class="iconfont icon-querenmima"></span></el-icon>
                                </template>
                            </el-input>
                            <el-date-picker v-model="birthday" style="margin-top: 15px" placeholder="生日" type="date"></el-date-picker>
                            <el-checkbox style="margin-top: 15px">我已阅读并同意 <a href="#">用户协议</a></el-checkbox>
                            <div class="btnRegister">
                                <el-button type="primary" class="flex-grow" @click="onSubmit">注册</el-button>
                            </div>
                            <div class="linkLogin">
                                <router-link to="/login">已有账号？立即登录</router-link>
                            </div>
                        </el-form>
                    </el-col>
                </el-row>
            </el-card>
        </el-container>
    </div>
</template>

<script lang="js" setup>
import { ref } from 'vue'
import { UserFilled } from '@element-plus/icons-vue'
import '../Iconscss/iconfont.css'
import Imagebg from "../assets/loginbg.jpg"

const imageSrc = Imagebg
const username = ref('')
const email = ref('')
const password = ref('')

function onSubmit() {
  // 注册逻辑
}
</script>

<style>
    .container {
        position: absolute;
        top: 25%;
        left: 25%;
        width: 50%;
    }

    .btnRegister {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 25px;
    }

    .linkLogin {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 30px;
    }

    .flex-grow {
        flex-grow: 1;
    }
</style>
