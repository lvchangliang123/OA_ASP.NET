<template>
    <div style="display:flex;align-items:center">
        <div class="rounded-background" style="display: flex; align-items: center; margin-left: 10px">
            <a @click="goTo('about')" style="color:white">
                <el-icon><Edit /></el-icon>
                我的笔记
            </a>
        </div>
        <div class="rounded-background" style="display:flex;align-items:center;margin-left:10px">
            <a href="#" style="color:white">
                <el-icon><Back /></el-icon>
                回到首页
            </a>
        </div>
    </div>
    <el-divider></el-divider>
    <div class="title">
        <span style="color:red">*</span>
        <span>标题:</span>
        <el-input class="inputtext" v-model="Title"
                  style="flex:1;margin-left:10px;margin-right:10px;"
                  placeholder="请输入文章标题..." />
    </div>
    <div class="title">
        <span style="color:red">*</span>
        <span>概述:</span>
        <el-input class="inputtext" type="textarea" :rows="3" v-model="OverView"
                  style="flex:1;margin-left:10px;margin-right:10px;min-height:60px;"
                  placeholder="请输入文章概述..." />
    </div>
    <div class="body">
        <mavon-editor style="width:100%;height:65vh"
                      v-model="Content" ref="blogEditor"
                      @imgAdd="handleImgAdd"
                      @imgDel="handleImgDel"></mavon-editor>
    </div>
    <div class="tag">
        <span style="color:red">*</span>
        <span>标签:</span>
        <el-select style="flex:1;margin-left:10px;margin-right:10px;"
                   v-model="selectedTags" multiple
                   placeholder="请选择博客分类标签...">
            <el-option v-for="item in options"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
        </el-select>
    </div>
    <div class="upload-section" style="margin-top:10px">
        <el-upload class="upload-cover"
                   action="your-upload-url"
                   :on-success="handleCoverUploadSuccess">
            <el-button size="middle" type="primary">
                上传封面图片   <el-icon v-if="uploadCover" size="16" color="#95d475" style="margin-left:10px"><CircleCheck /></el-icon>
            </el-button>
        </el-upload>
        <el-upload class="upload-code"
                   action="your-upload-url"
                   :on-success="handleCodeUploadSuccess">
            <el-button size="middle" type="primary">
            上传代码文件 <el-icon v-if="uploadCode" size="16" color="#95d475" style="margin-left:10px"><CircleCheck /></el-icon>
            </el-button>
        </el-upload>
        <el-upload>
            <el-button type="primary" style="width:200px" @click="handleBlogUpload">
                上传博客<el-icon size="20" class="el-icon--right"><Upload /></el-icon>
            </el-button>
        </el-upload>
    </div>
    <!--<div class="operation">
        <el-button type="primary" style="width:200px" @click="handleBlogUpload">
            上传<el-icon class="el-icon--right"><Upload /></el-icon>
        </el-button>
    </div>-->
</template>

<script lang="js" setup>
    import { ref, reactive, computed } from 'vue'
    import { Upload, Edit, Back, CircleCheck } from '@element-plus/icons-vue'
    import { mavonEditor } from 'mavon-editor'
    import { httpApi } from '@/Utils/httpApi'
    import { useRouter } from 'vue-router'
    import { useStore } from '@/VueX/store'
    import { ElMessage } from 'element-plus';
    import 'mavon-editor/dist/css/index.css'

    // 定义选项数据
    const options = [
        { value: 'WinForm', label: 'WinForm' },
        { value: 'WPF', label: 'WPF' },
        { value: 'ASP.NET', label: 'ASP.NET' },
        { value: 'ASP.NET CORE', label: 'ASP.NET CORE' },
        { value: 'Entity Framework', label: 'Entity Framework' },
        { value: 'WCF', label: 'WCF' },
        { value: 'Blazor', label: 'Blazor' },
        { value: 'MAUI', label: 'MAUI' },
        { value: 'Azure', label: 'Azure' },
        { value: 'Others', label: 'Others' },
    ];

    const router = useRouter()

    const goTo = (para) => {
        router.push(`/${para}`)
    }

    const userIdCom = computed(() => {
        return useStore.state.currentUser?.id;
    });

    const blogEditor = ref(null);

    const selectedTags = ref('');
    const Content = ref('');
    const Title = ref('');
    const OverView = ref('');

    const uploadCover = ref(false);
    const uploadCode = ref(false);

    const handleImgAdd = async (pos, file) => {
        const formData = new FormData();
        formData.append('Title', Title.value);
        formData.append('Position', pos);
        formData.append('File', file);

        try {
            const response = await httpApi.post('api/BlogEdit/UploadBlogImage', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            if (response.status === 200) {
                blogEditor.value.$img2Url(pos, response.data.url);
                ElMessage.success('图片上传成功!');
            } else {
                ElMessage.error('图片上传失败,请重试!');
            }
        } catch (e) {
            ElMessage.error('图片上传失败,请重试!');
        }

    };

    const handleImgDel = async (file) => {
        const formData = new FormData();
        formData.append('Title', Title.value);
        formData.append('FileName', file[0]);
        try {
            const response = await httpApi.post('api/BlogEdit/DeleteBlogImage', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            if (response.status === 200) {
                this.$refs.blogEditor.updateSrc(pos, null);
                ElMessage.success('图片删除成功!');
            } else {
                ElMessage.error('图片删除失败,请重试!');
            }
        } catch (e) {
            ElMessage.error('图片删除失败,请重试!');
        }
    };

    const handleCoverUploadSuccess = (file, fileList) => {
        console.log('Cover upload success:', file, fileList);
    };

    const handleCodeUploadSuccess = (file, fileList) => {
        console.log('Code upload success:', file, fileList);
    };

    const handleBlogUpload = () => {
        const formData = new FormData();
        formData.append('Title', Title.value);
        formData.append('OverView', OverView.value);
        formData.append('Content', Content.value);
        formData.append('Tags', selectedTags.value);
        formData.append('UserId', userIdCom.value);
        try {
            const response = httpApi.post('api/BlogEdit/UploadBlogContent', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            if (response.status === 200) {
                ElMessage.success('文章上传成功!');
                goTo('about');
            } else {
                ElMessage.error('文章上传失败,请重试!');
            }
        } catch (e) {
            ElMessage.error('文章上传失败,请重试!');
        }
    };

</script>

<style>
    .body {
        margin: 10px;
    }

    .tag {
        margin-top: 10px;
        display: flex;
        align-items: center;
    }

    .title {
        margin-top: 10px;
        display: flex;
        align-items: center;
    }

    .inputtext {
        margin-left: 10px;
    }

    .operation {
        margin-top: 20px;
        margin-left: 10px;
    }

    .upload-section {
        display: flex;
        align-items:center;
        gap:10px;
    }

    .upload-section .el-upload {
        height: 100%; 
    }

    .rounded-background {
        width: 160px;
        height: 40px;
        background-color: mediumseagreen;
        border-radius: 5px;
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    a {
        text-decoration: none;
    }

        a:hover {
            cursor: pointer;
        }
</style>