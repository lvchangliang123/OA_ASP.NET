<template>
    <div>
        <el-row>
            <el-col :span="18">
                <div>
                    <div style="text-align:center;font-family:STSong">
                        <span style="font-size:x-large;font-weight:bold;">{{BlogDetail.BlogTitle}}</span>
                    </div>
                    <el-card>
                        <div style="display: flex; align-items: center; justify-content: center;color:darkgray">
                            <el-avater>
                                <el-icon>
                                    <User />
                                </el-icon>
                                <span>{{BlogDetail.BlogAuther}}</span>
                            </el-avater>
                            <span style="margin-left:5px">{{BlogDetail.BlogCreateDateTime}}</span>
                            <span style="margin-left:5px">{{BlogDetail.BlogTag}}</span>
                            <span style="margin-left: 5px;display:flex;align-items:center">{{BlogDetail.BlogViews}}浏览<el-icon :size="20"><View /></el-icon></span>
                            <span class="clickable-span" style="margin-left:5px;display:flex;align-items:center" 
                                  @click="drawer = true">{{BlogDetail.BlogCommentCount}}评论<el-icon :size="20"><ChatDotSquare /></el-icon></span>
                        </div>
                    </el-card>
                    <el-card style="margin-top:5px;background-color:#f6f6f6;color:#8b94a0;">
                        <div>
                            <span><span style="color:black;font-weight:bold">【概述】</span>{{BlogDetail.BlogOverView}}</span>
                        </div>
                    </el-card>
                    <div>
                        <el-card style="height:60vh;margin-top:10px;overflow:auto">
                            <el-scrollbar style="height:100%">
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是
                                测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文
                                章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                                这是测试的文章内容这是测试的文章内容这是测试的文章内容这是测试的文章内容
                            </el-scrollbar>
                        </el-card>

                    </div>
                    <div style="text-align:center;margin-top:5px">
                        <a href="#" style="margin-right:10px"><span>上一篇文章</span></a>
                        <a href="#" style="margin-left:10px"><span>下一篇文章</span></a>
                    </div>
                    <el-row>
                        <el-divider content-position="left" style="width:100%">
                            <span style="font-size:large;font-weight:bold">文章评论</span>
                        </el-divider>
                        <el-input v-model="BlogDetail.BlogCommentContent"
                                  style="width:100%"
                                  :autosize="{ minRows: 3, maxRows: 10 }"
                                  type="textarea"
                                  placeholder="Please input" />
                        <div style="margin-left:auto">
                            <el-button style="margin-top:5px" color="#626aef" :dark="isDark">发表评论</el-button>
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
                        <span style="margin-left:5px">{{comment.CommentUserName}}</span>
                        <span style="margin-left:10px">{{comment.CommentDate}}</span>
                    </el-avater>
                    <div style="margin-left:20px;margin-top:5px;margin-bottom:10px">
                        <span>{{comment.CommentBody}}</span>
                    </div>
                </div>
        </el-drawer>
    </div>
</template>

<script lang="js" setup>
    import { ref, reactive } from 'vue'
    import { User } from '@element-plus/icons-vue'
    import { ChatDotSquare } from '@element-plus/icons-vue'
    import { View } from '@element-plus/icons-vue'

    const drawer = ref(false)

    const BlogDetail = ref(
        {
            BlogTitle: '我的第一篇博客',
            BlogAuther: 'Victor',
            BlogCreateDateTime: '2024-10-12',
            BlogTag: '.NET CORE',
            BlogViews: '123',
            BlogCommentCount: '12',
            BlogOverView: '这是我的第一篇测试文章的概述部分',
            BlogCommentContent: '这是我的评论'
        }
    )

    const BlogComments = ref([{
        CommentUserName: 'User01',
        CommentDate: '2023-10-03',
        CommentBody: '文章写得好1'
    },
    {
        CommentUserName: 'User02',
        CommentDate: '2023-10-03',
        CommentBody: '文章写得好2'
    },
    {
        CommentUserName: 'User03',
        CommentDate: '2023-10-03',
        CommentBody: '文章写得好3'
    }]
    )

    //const ViewBlogCommentClick = (() => {
    //    drawer = true
    //})

</script>

<style>
    .clickable-span {
        cursor: pointer;
        &:hover
        {
            color: #0056b3;
            text-decoration: none;
        }
        transition: background-color 0.3s ease, color 0.3s ease;
    }
</style>