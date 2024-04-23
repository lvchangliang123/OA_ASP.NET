<template>
    <div>
        <el-container class="container">
            <el-card>
                <el-row style="height:400px">
                    <el-col :span="18">
                        <el-image :src="imageSrc" style="height:400px;border-radius:4px" :fit="fill" />
                    </el-col>
                    <el-col :span="6">
                        <el-form style="height:200px">
                            <el-input v-model="input1"
                                      placeholder="用户名或邮箱">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><UserFilled /></el-icon>
                                </template>
                            </el-input>
                            <el-input v-model="input2" style="margin-top:15px"
                                      placeholder="账户密码">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><View /></el-icon>
                                </template>
                            </el-input>
                            <el-checkbox style="margin-top:15px">记住密码</el-checkbox>
                            <div class="btnLogin">
                                <el-button type="primary" class="flex-grow" @click="onSubmit">登录</el-button>
                            </div>
                            <div class="linkForgetPassword">
                                <a href="#">忘记密码</a>
                                <a href="#" style="margin-left:15px">注册账号</a>
                            </div>
                        </el-form>
                        <el-divider content-position="center" style="margin-top:90px">
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
    import Imagebg from "../assets/loginbg.jpg"

    const imageSrc = Imagebg
    const input1 = ref('')
    const input2 = ref('')

</script>

<style>
    .container {
       position:absolute;
       top:25%;
       left:25%;
       width:50%;
    }
    .btnLogin {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top:25px
    }
    .linkForgetPassword {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 30px
    }
    .flex-grow {
        flex-grow: 1;
    }
</style>