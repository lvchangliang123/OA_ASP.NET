<template>
    <div>
        <el-container class="container">
            <el-card>
                <el-row style="height:450px">
                    <el-col :span="16">
                        <el-image :src="imageSrc" style="height:450px; border-radius: 4px" :fit="fill" />
                    </el-col>
                    <el-col :span="8">
                        <el-form :model="form" :rules="rules" ref="formRef" 
                                 label-width="80px" style="height:200px">
                            <el-form-item label="用户名" prop="Name">
                                <el-input v-model="form.Name">
                                    <template #prefix>
                                        <el-icon class="el-input_icon"><UserFilled /></el-icon>
                                    </template>
                                </el-input>
                            </el-form-item>
                            <el-form-item label="邮箱" prop="email" style="margin-top:15px">
                                <el-input v-model="form.Email">
                                    <template #prefix>
                                        <el-icon class="el-input_icon"><span class="iconfont icon-email"></span></el-icon>
                                    </template>
                                </el-input>
                            </el-form-item>
                            <el-form-item label="上传头像">
                                <el-upload action="https://run.mocky.io/v3/9d059bf9-4660-45f2-925d-ce80ad6c4d15"
                                           tip="点击上传" :show-file-list="false"
                                           :on-remove="handleRemove"
                                           :before-remove="beforeRemove"
                                           :on-change="handleAvatarChange"
                                           :auto-upload="false">
                                    <img v-if="imageUrl" :src="imageUrl" style="height:100px;width:100px" class="avatar" />
                                    <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
                                    <span v-else>点击上传头像</span>
                                </el-upload>
                            </el-form-item>
                            <el-form-item label="密码" prop="Password" style="margin-top:15px">
                                <el-input v-model="form.Password" type="password">
                                    <template #prefix>
                                        <el-icon class="el-input_icon"><span class="iconfont icon-mima"></span></el-icon>
                                    </template>
                                </el-input>
                            </el-form-item>
                            <el-form-item label="确认密码" prop="ConfirmPassword" style="margin-top:15px">
                                <el-input v-model="form.ConfirmPassword" type="password">
                                    <template #prefix>
                                        <el-icon class="el-input_icon"><span class="iconfont icon-querenmima"></span></el-icon>
                                    </template>
                                </el-input>
                            </el-form-item>
                            <el-form-item label="生日" prop="Birthday" style="margin-top:15px">
                                <el-date-picker style="width:100%" v-model="form.Birthday" type="date"></el-date-picker>
                            </el-form-item>
                            <div class="btnRegister" style="margin-left:15px">
                                <el-button type="primary" class="flex-grow" @click="submitForm">注册</el-button>
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
    import { ref, reactive } from 'vue'
    import { ElMessage } from 'element-plus';
    import { UserFilled,Plus } from '@element-plus/icons-vue'
    import { httpApi } from '@/Utils/httpApi'
    import '../Iconscss/iconfont.css'
    import Imagebg from "../assets/loginbg.jpg"
    import axios from 'axios'
    import { useRouter } from 'vue-router'

    const router = useRouter()

    const imageSrc = Imagebg
    const formRef = ref(null);
    const imageUrl = ref('');
    const dialogVisible = ref(false);
    const showUpload = ref(true);

    const form = reactive({
        Name: '',
        Email: '',
        Password: '',
        Avatar: null,
        ConfirmPassword: '',
        Birthday: new Date()
    });

    const rules = reactive({
        Name: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
        Email: [{ required: true, message: '请输入邮箱', trigger: 'blur' }],
        Password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
        ConfirmPassword: [{ required: true, message: '请确认密码', trigger: 'blur' }],
        Birthday: [{ required: true, message: '请输入生日', trigger: 'blur' }],
    });

    const handleAvatarChange = function(uploadFile, uploadFiles) {
      imageUrl.value = URL.createObjectURL(uploadFile.raw);
      form.Avatar = uploadFile.raw;
    }

    const handleRemove = (file, fileList) => {
        ElMessage({
            message: `删除 ${file.name}`,
            type: 'info',
        });
    };

    const beforeRemove = (file, fileList) => {
        return ElMessage.confirm(`确定移除 ${file.name}？`);
    };


    const submitForm = async () => {
        const formIsValid = await formRef.value.validate();
        if (!formIsValid) return;

        if (form.Password != form.ConfirmPassword) {
            ElMessage.error('注册失败,请确认密码输入是否正确!');
            return;
        }

        const formData = new FormData();
        formData.append('Name', form.Name);
        formData.append('Email', form.Email);
        formData.append('Password', form.Password);
        formData.append('Birthday', form.Birthday.toISOString());
        formData.append('Avatar', form.Avatar);

        const headers = {
            'Content-Type': 'multipart/form-data',
        };

        try {
            const response = await httpApi.post('api/Regis/RegisUser', formData,{headers});
            if (response.status === 200) {
                ElMessage.success('注册成功');
                router.push(`/login`);
            } else {
                ElMessage.error('注册失败，请检查您的输入或重试');
            }
        } catch (error) {
            ElMessage.error('注册失败，服务器请求失败');
        }
    };
</script>

<style>
    .container {
        position: absolute;
        top: 25%;
        left: 22%;
        width: 60%;
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
        margin-top: 10px;
    }

    .flex-grow {
        flex-grow: 1;
    }

    .el-form-item__label {
        justify-content: flex-start;
        margin-left: 10px;
    }
</style>
