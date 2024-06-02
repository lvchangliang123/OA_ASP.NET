import axios from 'axios';

// 创建 Axios 实例
export const httpApi = axios.create({
    baseURL: 'https://localhost:7004/', // 这里填写你的基础URL
    timeout: 5000, // 设置请求超时时间，单位ms
    // 可以在这里添加其他默认配置，如 headers 等
});