<template>
    <div>
        <el-container class="container">
            <el-card style="width:70%;height:80%;position:absolute">
                <el-row>
                    <el-col :span="17" style="background-color:deepskyblue">
                        123
                    </el-col>
                    <el-col :span="1"></el-col>
                    <el-col :span="6">
                        <el-input v-model="input1"
                                  placeholder="Username or e-mail">
                            <template #prefix>
                                <el-icon class="el-input__icon"><UserFilled /></el-icon>
                            </template>
                        </el-input>
                        <el-input v-model="input2" style="margin-top:15px"
                                  placeholder="Password">
                            <template #prefix>
                                <el-icon class="el-input__icon"><View /></el-icon>
                            </template>
                        </el-input>
                        <div class="btnLogin">
                            <el-button type="primary" class="flex-grow" @click="onSubmit">登录</el-button>
                        </div>
                        <div class="linkForgetPassword">
                            <a href="#">忘记密码</a>
                            <a href="#" style="margin-left:15px">注册账号</a>
                        </div>
                        <el-divider content-position="center">
                            <span style="font-size:small;">其他登录</span>
                        </el-divider>
                        <div style="width:100%;display:flex">
                            <el-button type="primary" :icon="icon-weixin" class="flex-grow"><span class="iconfont icon-weixin"></span>微信登录</el-button>
                        </div>
                        <div style="width:100%;display:flex;margin-top:15px">
                            <el-button type="primary" class="flex-grow"><span class="iconfont icon-weibo"></span>微博登录</el-button>
                        </div>
                    </el-col>
                </el-row>
            </el-card>
        </el-container>
    </div>
     

</template>

<script lang="js" setup>
    import { ref, reactive } from 'vue'
    import { UserFilled, View } from '@element-plus/icons-vue'
    import '../Iconscss/iconfont.css'

    const input1 = ref('')
    const input2 = ref('')

</script>

<style>
    .container {
        display: flex;
        align-items: center;
        justify-content: center;
        height:900px;
        width:100%;
        position:relative
    }
    .btnLogin {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top:10px
    }
    .linkForgetPassword {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 10px
    }
    .flex-grow {
        flex-grow: 1;
    }
</style>