<template>
    <div>
        <el-container class="container">
            <el-card>
                <el-row style="height:400px">
                    <el-col :span="18">
                        <el-image :src="imageSrc" style="height:400px;border-radius:4px" :fit="fill" />
                    </el-col>
                    <el-col :span="6">
                        <el-form style="height:200px" :model="formLogin" :rules="rules" ref="formLoginRef">
                            <el-input v-model="formLogin.Identifier"
                                      placeholder="用户名或邮箱">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><UserFilled /></el-icon>
                                </template>
                            </el-input>
                            <el-input v-model="formLogin.Password"
                                      type="password"
                                      style="margin-top:15px"
                                      placeholder="账户密码">
                                <template #prefix>
                                    <el-icon class="el-input__icon"><View /></el-icon>
                                </template>
                            </el-input>
                            <el-checkbox style="margin-top:15px">记住密码</el-checkbox>
                            <div class="btnLogin">
                                <el-button type="primary" class="flex-grow" @click="onSubmitLogin">登录</el-button>
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
    import { ElMessage } from 'element-plus';
    import { UserFilled, View } from '@element-plus/icons-vue'
    import { httpApi } from '@/Utils/httpApi'
    import '../Iconscss/iconfont.css'
    import Imagebg from "../assets/loginbg.jpg"
    import { useRouter } from 'vue-router'
    import { useStore } from '@/VueX/store';

    const formLoginRef = ref(null);
    const router = useRouter();

    const imageSrc = Imagebg
    const formLogin = reactive({
        Identifier: '',
        Password: '',
    });

    const rules = reactive({
        Identifier: [{ required: true, message: '请输入用户名或邮箱', trigger: 'blur' }],
        Password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
    });

    const onSubmitLogin = async () => {
        const formIsValid = await formLoginRef.value.validate();
        if (!formIsValid) return;
        const formData = new FormData();
        formData.append('Identifier', formLogin.Identifier);
        formData.append('Password', formLogin.Password);
        const headers = {
            'Content-Type': 'multipart/form-data',
        };
        const headerJson = {
            'Content-Type': 'application/json',
        };
        try {
            const response = await httpApi.post('api/Login/UserLogin', formData, { headers });
            if (response.status === 200) {
                if (response.data.PageName.toString() == "RedirectToBlogHome") {
                    useStore.commit('SET_CURRENT_USER', response.data.User);
                    //存储用户信息持久化
                    localStorage.setItem("VueBlogUser", JSON.stringify(response.data.User));
                    router.push('/bloghome');
                }
            } else {
                ElMessage.error('登录失败' + response.data.toString());
            }
        } catch (error) {
            ElMessage.error('登录失败,登录请求未正确响应');
        }
    }

</script>

<style>
    .container {
        position: absolute;
        top: 25%;
        left: 25%;
        width: 50%;
    }

    .btnLogin {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 25px
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