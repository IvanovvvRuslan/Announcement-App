import axios from 'axios';

const API_BASE_URL = 'http://localhost:5188';

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const announcementAPI = {
    getAll: (page = 1, pageSize = 10) => api.get(`/announcement?pageNumber=${page}&pageSize=${pageSize}`),
    getById: (id) => api.get(`/announcement/${id}`),
    create: (data) => api.post('/announcement', data),
    update: (id, data) => api.patch(`/announcement/${id}`, data),
    delete: (id) => api.delete(`/announcement/${id}`),
};