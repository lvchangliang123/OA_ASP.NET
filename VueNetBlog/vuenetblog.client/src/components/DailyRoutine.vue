<!--使用时间轴显示随笔-->
<template>
    <div style="width:160px;height:40px;background-color:#5ea51b;color:white;margin-left:40px;margin-top:20px;
justify-content:center;display:flex;align-items:center;border-radius:3px">
        <span style="text-align:center">日常随笔</span>
    </div>
    <el-timeline style="width:90%;margin-top:20px;height:80vh">
        <el-timeline-item center timestamp="2018/4/12" placement="top" color="#0bbd87" size="large">
            <el-card>
                <h4>Update Github template</h4>
                <p>Tom committed 2018/4/12 20:46</p>
            </el-card>
        </el-timeline-item>
        <el-timeline-item timestamp="2018/4/3" placement="top" color="#0bbd87" size="large">
            <el-card>
                <h4>Update Github template</h4>
                <p>Tom committed 2018/4/3 20:46</p>
            </el-card>
        </el-timeline-item>
        <el-timeline-item center timestamp="2018/4/2" placement="top" color="#0bbd87" size="large">
            Event start
        </el-timeline-item>
        <el-timeline-item timestamp="2018/4/2" placement="top" color="#0bbd87" size="large">
            Event end
        </el-timeline-item>
        <el-timeline-item timestamp="2017/4/2" placement="top" color="#0bbd87" size="large">
            Event end
        </el-timeline-item>
        <el-timeline-item timestamp="2016/4/2" placement="top" color="#0bbd87" size="large">
            Event end
        </el-timeline-item>
    </el-timeline>
    <div style="justify-content:center;display:flex;align-items:center">
        <el-pagination background layout="prev, pager, next" :total="100" />
    </div>
</template>