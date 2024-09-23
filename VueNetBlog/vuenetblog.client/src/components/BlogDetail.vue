<template>
    <div>
        <el-row>
            <el-col :span="18">
                <div>
                    <div style="text-align:center;font-family:STSong">
                        <span style="font-size:xx-large;font-weight:bold">{{blogData ? blogData.title : ''}}</span>
                    </div>
                    <el-card>
                        <div style="display: flex; align-items: center; justify-content: center;color:darkgray">
                            <el-avater style="display:flex;align-items:center">
                                <el-avatar v-if="!BlogUserAvatarPath" :icon="UserFilled" :size="30" />
                                <el-avatar v-else :size="30" :src="$hostURL+BlogUserAvatarPath"></el-avatar>
                                <span style="margin-left:5px">{{BlogUserName}}</span>
                            </el-avater>
                            <span style="margin-left:15px">{{BlogCreateTime}}</span>
                            <span style="margin-left:15px">{{BlogTagFirst}}</span>
                            <span style="margin-left:15px;display:flex;align-items:center">{{BlogViewCount}}浏览<el-icon :size="20"><View /></el-icon></span>
                            <span class="clickable-span" style="margin-left:15px;display:flex;align-items:center;color:lightskyblue"
                                  @click="drawer = true">{{BlogCommentsCount}}评论<el-icon :size="20"><ChatDotSquare /></el-icon></span>
                        </div>
                    </el-card>
                    <el-card style="margin-top:5px;background-color:#f6f6f6;color:#8b94a0;">
                        <div>
                            <span><span style="color:black;font-weight:bold">【概述】</span>{{blogData ? blogData.overView : ''}}</span>
                        </div>
                    </el-card>
                    <div>
                        <el-card style="height:60vh;margin-top:10px;overflow:auto">
                            <el-scrollbar style="height:100%">
                                <mavon-editor style="width:100%;height:65vh"
                                              v-model="blogContent"
                                              :editable="false"
                                              :defaultOpen="'preview'"
                                              :subfield="false"
                                              :toolbarsFlag="false"
                                              :navigation="false"
                                              codeStyle="docco" 
                                              :ishljs="true" 
                                              :scroll-style="true" 
                                              :box-shadow="false"
                                              ref="blogEditor">
                                </mavon-editor>
                            </el-scrollbar>
                        </el-card>

                    </div>
                    <div style="text-align:center;margin-top:5px">
                        <el-link type="primary" style="margin-right: 10px ">上一篇文章</el-link>
                        <el-link type="primary" style="margin-left: 10px ">下一篇文章</el-link>
                    </div>
                    <el-row>
                        <el-divider content-position="left" style="width:100%">
                            <span style="font-size:large;font-weight:bold">文章评论</span>
                        </el-divider>
                        <el-input v-model="currentBlogComment"
                                  style="width:100%"
                                  :autosize="{ minRows: 3, maxRows: 10 }"
                                  type="textarea"
                                  placeholder="Please input" />
                        <div style="margin-left:auto">
                            <el-button type="primary" style="margin-top:5px" 
                                       :dark="isDark" @click="handleBlogComment">发表评论</el-button>
                        </div>
                    </el-row>
                </div>
            </el-col>
            <el-col :span="0.5">
                <el-divider direction="vertical" style="height:100%"></el-divider>
            </el-col>
            <el-col :span="5">
                <el-divider content-position="left" style="width:100%">
                    <span style="font-size:large;font-weight:bold"><span style="color:lightgreen">相关</span>文章</span>
                </el-divider>
                <el-divider content-position="left" style="width:100%">
                    <span style="font-size:large;font-weight:bold"><span style="color:lightgreen">点击</span>排行</span>
                </el-divider>
            </el-col>
        </el-row>
        <el-drawer v-model="drawer" :with-header="false"
                   :direction="rtl">
            <el-divider content-position="left" style="width:100%">
                <span style="font-weight:bold;font-size:large">文章评论</span>
            </el-divider>
                <div v-for="(comment, index) of BlogComments" :key="index">
                    <el-avater>
                        <el-icon>
                            <User />
                        </el-icon>
                        <span style="margin-left:5px">{{comment.user.name}}</span>
                        <span style="margin-left:10px">{{formattedCreateTime(comment.createTime)}}</span>
                    </el-avater>
                    <div style="margin-left:20px;margin-top:5px;margin-bottom:10px">
                        <span>{{comment.content}}</span>
                    </div>
                </div>
        </el-drawer>
    </div>
</template>

<script lang="js" setup>
    import { ref, reactive, onMounted, computed } from 'vue'
    import { User } from '@element-plus/icons-vue'
    import { ChatDotSquare } from '@element-plus/icons-vue'
    import { mavonEditor } from 'mavon-editor'
    import { View } from '@element-plus/icons-vue'
    import { useStore } from '@/VueX/store'
    import { httpApi } from '@/Utils/httpApi'
    import { ElMessage } from 'element-plus'

    const blogData = ref(null);

    const blogContent = ref('');

    const currentBlogComment = ref('');

    const drawer = ref(false)

    const handleBlogComment = async () => {
        if (currentBlogComment.value.trim() !== '') {
            const blogId = useStore.state.currentBlog.blogId;
            const userId = useStore.state.currentUser.id;
            const formData = new FormData();
            formData.append('BlogId', blogId);
            formData.append('UserId', userId);
            formData.append('CommentContent', currentBlogComment.value);
            try {
                const response = await httpApi.post('api/BlogDetail/SubmitBlogComment', formData, {
                    headers: { 'Content-Type': 'multipart/form-data' }
                });
                if (response.status === 200) {
                    ElMessage.success('文章评论成功!');
                } else {
                    ElMessage.error('文章评论失败,请重试!');
                }
            } catch (e) {
                ElMessage.error('文章评论失败,请重试!');
            }
        } else {
            ElMessage.error('文章评论内容不能为空!请重试!');
        }
    }

    const formattedCreateTime = (dateTime)=> {
        if (!dateTime) return '';
        const date = new Date(dateTime);
        if (isNaN(date.getTime())) return '';
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
    }

    onMounted(async () => {
        const blogInfo = useStore.state.currentBlog;
        try {
            if (blogInfo) {
                const url = `api/BlogDetail/GetBlogData?userid=${encodeURIComponent(blogInfo.userId)}&blogid=${encodeURIComponent(blogInfo.blogId)}`;
                const response = await httpApi.get(url);
                blogData.value = response.data;
                blogContent.value = blogData.value.content;
                BlogComments.value = blogData.value.comments;
                BlogCommentsCount.value = BlogComments.value.length;
                BlogViewCount.value = blogData.value.viewCount;
                BlogCreateTime.value = formattedCreateTime(blogData.value.createTime);
                BlogUserName.value = blogData.value.user.name;
                BlogTagFirst.value = blogData.value.tags;
            }
        } catch (e) {
            ElMessage.error('博客信息获取失败!请重试!');
        }
    })

    const BlogComments = ref([])

    const BlogCommentsCount = ref(0)

    const BlogViewCount = ref(0)

    const BlogCreateTime = ref('')

    const BlogUserName = ref('')

    const BlogTagFirst = ref('')

    const BlogUserAvatarPath = computed(() => {
        if (blogData.value) {
            return blogData.value.user.avatarPath;
        } else {
            return null;
        }
    });

</script>

<style>
    .clickable-span {
        cursor: pointer;
        transition: background-color 0.3s ease, color 0.3s ease;
        &:hover {
            color: #0056b3;
            text-decoration: none;
        }
    }
</style>