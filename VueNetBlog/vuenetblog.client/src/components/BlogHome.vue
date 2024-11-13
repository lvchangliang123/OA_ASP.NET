<template>
    <div class="home-layout">
        <el-backtop :bottom="80"></el-backtop>
        <el-container>
            <el-header>
                <el-menu :default-active="activeIndex2"
                         mode="horizontal"
                         background-color="#545c64"
                         text-color="#fff"
                         :ellipsis="false"
                         active-text-color="#ffd04b"
                         style="display:flex">
                    <el-menu-item tabindex="0" style="font-size:large">.NET Develop Blog</el-menu-item>
                    <div class="flex-grow" />
                    <el-menu-item index="1">
                        <div v-if="!currentUserAvatarPath">
                            <el-avater size="30">
                                <el-icon>
                                    <User />
                                </el-icon>
                            </el-avater>
                        </div>
                        <el-avatar v-else :size="30" :src="$hostURL+currentUserAvatarPath"/>
                    </el-menu-item>
                    <el-menu-item index="2" v-if="currentUserName" @click="goTo('about')">
                        <span>{{currentUserName}}</span>
                    </el-menu-item>
                    <el-menu-item index="3" @click="goTo('bloghome')">首页</el-menu-item>
                    <el-menu-item index="4">分类</el-menu-item>
                    <el-menu-item index="5" @click="goTo('regis')">注册</el-menu-item>
                    <el-menu-item v-if="!currentUser" index="6" @click="goTo('login')">登录</el-menu-item>
                    <el-menu-item v-else index="6" @click="logout">注销</el-menu-item>
                    <el-menu-item index="7" @click="goTo('about')">关于</el-menu-item>
                </el-menu>
            </el-header>
            <el-main>
                <el-carousel :interval="4000" type="card">
                    <el-carousel-item v-for="blogData in HomeCarouselBlogs" :key="blogData" class="carousel-item">
                        <el-image :src="getFullFilePath(blogData.coverPath)" 
                                :fit="contain" class="imageCarouselItem"
                                  @click="handleBlogImageClick(blogData)"/>
                    </el-carousel-item>
                </el-carousel>
                <el-divider content-position="left">
                    <span style="font-size:large;font-weight:bold;color:red">文章推荐</span>
                </el-divider>
                <el-row>
                    <el-col :span="18">
                        <el-row>
                            <el-col :span="24" v-for="(blog, index) of paginatedBlogs" 
                                    :key="index" style="width:100%">
                                <el-space :style="{marginBottom: '15px', width: '100%' }"
                                          :span="24"
                                          :direction="vertical" >
                                    <el-card shadow="hover" 
                                             style="width: 100%; box-sizing: border-box; padding: 15px;">
                                        <div slot="header">
                                            <h3 class="title">{{ blog.title }}</h3>
                                            <p>{{ blog.user.name }} / {{ blog.createTime }}</p>
                                        </div>
                                        <div v-html="blog.overView" style="min-width:1300px;max-width:1300px"></div>
                                        <div style="text-align:right">
                                            <a href="#">
                                                <el-button color="orange"
                                                           style="color:white;border-radius:0;"
                                                           :dark="isDark"
                                                           @click="handleBlogImageClick(blog)">
                                                    阅读全文>>
                                                </el-button>
                                            </a>
                                        </div>
                                    </el-card>
                                </el-space>
                            </el-col>
                        </el-row>
                        <div class="example-pagination-block" style="justify-content:center;display:flex">
                            <el-pagination layout="prev, pager, next"
                                           :total="totalBlogCount"
                                           :page-size="pageSize"
                                           :current-page="currentPage"
                                           @current-change="handlePageChange"/>
                        </div>
                    </el-col>
                    <el-span :span="2">
                        <el-divider direction="vertical" style="height:100%"></el-divider>
                    </el-span>
                    <el-span :span="4">
                        <el-card style="width:100%">
                            weatherforecast
                        </el-card>
                        <el-divider content-position="left" style="width:100%">
                            <span style="font-size:large;font-weight:bold"><span style="color:lightgreen">最新</span>文章</span>
                        </el-divider>
                        <el-row v-for="(item, index) of newBlogList" :key="index">
                            <el-space :style="{ marginBottom: '15px' }">
                                <span><span style="font-weight:bold">· </span>{{item}}</span>
                            </el-space>
                        </el-row>
                        <el-divider content-position="left" style="width:100%">
                            <span style="font-size:large;font-weight:bold"><span style="color:lightgreen">点击</span>排行</span>
                        </el-divider>
                        <el-row v-for="(item, index) of newBlogList" :key="index">
                            <el-space :style="{ marginBottom: '15px' }">
                                <span><span style="font-weight:bold">· </span>{{item}}</span>
                            </el-space>
                        </el-row>
                        <el-divider content-position="left" style="width:100%">
                            <span style="font-size:large;font-weight:bold">友情链接</span>
                        </el-divider>
                        <el-row v-for="(item, index) of friendlyLinks" :key="index">
                            <el-space :style="{ marginBottom: '15px' }">
                                <el-link>{{item}}</el-link>
                            </el-space>
                        </el-row>
                    </el-span>
                </el-row>
            </el-main>
            <el-footer style="display:flex;align-items:center;justify-content:center; background-color: #545c64;color:white;">
                @2024 Design By Vue3 & ASP.NET CORE
            </el-footer>
        </el-container>
    </div>

</template>

<script lang="js" setup>
    import { ref, reactive, computed, onMounted } from 'vue'
    import { User } from '@element-plus/icons-vue'
    import { httpApi } from '@/Utils/httpApi'
    import { useRouter } from 'vue-router'
    import { ElMessage } from 'element-plus'
    import { useStore } from '@/VueX/store';

    const router = useRouter()

    const goTo = (para) => {
        router.push(`/${para}`)
    }

    router.onError((err) => {
        console.error('Routing error:', err)
    })

    const currentUser = computed(() => {
        return useStore.state.currentUser;
    });

    const currentUserName = computed(() => {
        return useStore.state.currentUser?.name;
    });

    const currentUserAvatarPath = computed(() => {
        console.log(useStore.state.currentUser?.avatarPath);
        return useStore.state.currentUser?.avatarPath;
    });

    const logout = ()=> {
        useStore.commit('SET_CURRENT_USER', null);
        localStorage.removeItem("VueBlogUser");
    }

    const testVal = ref('');

    const getValue = httpApi.get('api/BlogHome/BlogHomeView')
        .then(response => testVal.value = response.data.UserName).catch(function (error) {
            console.log(error);
        })

    const activeIndex = ref('2')
    const activeIndex2 = ref('2')
    const totalBlogCount = ref(4)
    const currentPage = ref(1)
    const pageSize = ref(3)

    // 计算属性：分页后的博客列表
    const paginatedBlogs = computed(() => {
        const start = (currentPage.value - 1) * pageSize.value;
        const end = start + pageSize.value;
        return HomeListlBlogs.value.slice(start, end);
    });

    // 监听分页变化
    const handlePageChange = (newPage) => {
        currentPage.value = newPage;
    };

    const handleBlogImageClick = (blog) => {
        if (blog && blog.userId && blog.id) {
            useStore.commit('SET_CURRENT_BLOG', { userId: blog.userId, blogId: blog.id });
            router.push('/blogdetail');
        }
    }

    const HomeCarouselBlogs = ref([])
    const HomeListlBlogs = ref([])

    onMounted(async () => {
        var loginUser = JSON.parse(localStorage.getItem('VueBlogUser'));
        if (loginUser) {
            useStore.commit('SET_CURRENT_USER', loginUser);
        }
        //请求最新的3个博客信息并展示
        try {
            const url = `api/BlogHome/GetHomeCarouselBlogs`;
            const response = await httpApi.get(url);
            HomeCarouselBlogs.value = response.data;
        } catch (e) {
            ElMessage.error('博客信息获取失败!请重试!');
        }
        try {
            const url2 = `api/BlogHome/GetHomeListlBlogs`;
            const response2 = await httpApi.get(url2);
            HomeListlBlogs.value = response2.data;
            totalBlogCount.value = HomeListlBlogs.value.length;
        } catch (e) {
            ElMessage.error('博客列表获取失败!请重试!');
        }
    })

    const hostURLPath = useStore.getters['global/hostURL'];

    const getFullFilePath = (filePath) => {
        return `${hostURLPath}/${filePath}`;
    };

    const friendlyLinks = reactive([
        'CSDN',
        'Vue',
        'ASP.NET CORE',
        'Mysql'
    ])

    const newBlogList = reactive([
        '基于VUE的ASP.NET CORE应用程序',
        '基于VUE的ASP.NET CORE应用程序',
        '基于VUE的ASP.NET CORE应用程序',
        '基于VUE的ASP.NET CORE应用程序',
        '基于VUE的ASP.NET CORE应用程序',
        '基于VUE的ASP.NET CORE应用程序',
    ])

    const blogList = reactive([
        {
            title: '程序员请放下你的技术情节，与你的同伴一起进步',
            author: 'xcLiegh',
            date: '2013-11-04',
            content: '如果说掌握一门赖以生存的技术是技术人员要学会的第一课的话，那么我觉得技术人员要真正学会的第二课，不是技术，而是业务、交流与协作，学会关心其他工作伙伴的工作情况和进展…',
            background: 'url(https://example.com/image1.jpg)',
        },
        {
            title: '程序员请放下你的技术情节，与你的同伴一起进步',
            author: 'xcLiegh',
            date: '2013-11-04',
            content: '如果说掌握一门赖以生存的技术是技术人员要学会的第一课的话，那么我觉得技术人员要真正学会的第二课，不是技术，而是业务、交流与协作，学会关心其他工作伙伴的工作情况和进展…',
            background: 'url(https://example.com/image2.jpg)',
        },
        {
            title: '程序员请放下你的技术情节，与你的同伴一起进步',
            author: 'xcLiegh',
            date: '2013-11-04',
            content: '如果说掌握一门赖以生存的技术是技术人员要学会的第一课的话，那么我觉得技术人员要真正学会的第二课，不是技术，而是业务、交流与协作，学会关心其他工作伙伴的工作情况和进展…',
            background: 'url(https://example.com/image3.jpg)',
        },
        {
            title: '程序员请放下你的技术情节，与你的同伴一起进步',
            author: 'xcLiegh',
            date: '2013-11-04',
            content: '如果说掌握一门赖以生存的技术是技术人员要学会的第一课的话，那么我觉得技术人员要真正学会的第二课，不是技术，而是业务、交流与协作，学会关心其他工作伙伴的工作情况和进展…',
            background: 'url(https://example.com/image3.jpg)',
        },
    ])

</script>

<style>
    .flex-grow {
        flex-grow: 1;
    }

    .el-carousel__item h3 {
        color: #475669;
        opacity: 0.75;
        line-height: 150px;
        margin: 0;
        text-align: center;
    }

    .el-carousel__item:nth-child(2n) {
        background-color: #99a9bf;
    }

    .el-carousel__item:nth-child(2n + 1) {
        background-color: #d3dce6;
    }

    .example-pagination-block + .example-pagination-block {
        margin-top: 10px;
    }

    .carousel-item {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100%; /* 确保 carousel-item 占据整个 carousel 的高度 */
    }

    .imageCarouselItem {
        max-width: 100%; /* 确保图片最大宽度不超过 carousel-item */
        height: auto; /* 保持图片的原始比例 */
    }


</style>
