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
    <div class="operation">
        <el-button type="primary" style="width:200px" @click="handleBlogUpload">
            上传<el-icon class="el-icon--right"><Upload /></el-icon>
        </el-button>
    </div>
</template>

<script lang="js" setup>
    import { ref, reactive } from 'vue'
    import { Upload, Edit, Back } from '@element-plus/icons-vue'
    import { mavonEditor } from 'mavon-editor'
    import { httpApi } from '@/Utils/httpApi'
    import { useRouter } from 'vue-router'
    import 'mavon-editor/dist/css/index.css'

    // 定义选项数据
    const options = [
        { value: 'option1', label: '选项1' },
        { value: 'option2', label: '选项2' },
        { value: 'option3', label: '选项3' },
        // 更多选项...
    ];

    const router = useRouter()

    const goTo = (para) => {
        router.push(`/${para}`)
    }

    const selectedTags = ref([]);
    const Content = ref('');
    const Title = ref('');
    const OverView = ref('');

    const handleImgAdd = async (pos, $file) => {
        const formData = new FormData();
        formData.append('title', Title);
        formData.append('position', pos);
        formData.append('file', $file.file);

        try {
            const response = await httpApi.post('api/BlogEdit/UploadBlogImage', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            if (response.status === 200) {
                this.$refs.blogEditor.updateSrc(pos, response.data.url);
                ElMessage.success('图片上传成功!');
            } else {
                ElMessage.error('图片上传失败,请重试!');
            }
        } catch (e) {
            ElMessage.error('图片上传失败,请重试!');
        }

    };

    const handleImgDel = async (pos,$file)=>{
        const formData = new FormData();
        formData.append('title', Title);
        formData.append('position', pos);
        formData.append('file', $file.file);
        try {
            const response = await httpApi.post('api/BlogEdit/UploadBlogImage', formData, {
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

    const handleBlogUpload= async ()=>{
         const formData = new FormData();
         formData.append('title', Title);
         formData.append('overView', OverView);
         formData.append('content', Content);
         formData.append('tags', selectedTags);
         try {
             const response = await httpApi.post('api/BlogEdit/UploadBlogContent', formData, {
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