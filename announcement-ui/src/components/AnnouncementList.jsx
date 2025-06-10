import React, { useState, useEffect } from 'react';
import { Card, Button, List, message, Popconfirm } from 'antd';
import { EditOutlined, DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import { announcementAPI } from '../servi1ces/api.js';

const AnnouncementList = () => {
    const [announcements, setAnnouncements] = useState([]);
    const [loading, setLoading] = useState(true);

    const [pagination, setPagination] = useState({
        current: 1,
        pageSize: 10,
        total: 0
    });

    useEffect(() => {
        fetchAnnouncements(1);
    }, []);

    const fetchAnnouncements = async (page = 1) => {
        try {
            const response = await announcementAPI.getAll(page, 10);
            const data = response.data?.items || [];
            setAnnouncements(data);

            setPagination({
                current: response.data?.pageNumber || 1,
                pageSize: response.data?.pageSize || 10,
                total: response.data?.totalCount || 0
            });
        } catch (error) {
            message.error('Failed to load announcements');
        } finally {
            setLoading(false);
        }
    };

    const handlePageChange = (page) => {
        fetchAnnouncements(page);
    };

    const handleDelete = async (id) => {
        try {
            await announcementAPI.delete(id);
            message.success('Announcement deleted successfully');
            fetchAnnouncements();
        } catch (error) {
            message.error('Failed to delete announcement');
        }
    };

    return (
        <div>
            <h1>Announcements</h1>
            <List
                loading={loading}
                dataSource={announcements}
                pagination={{
                    ...pagination,
                    onChange: handlePageChange,
                    showSizeChanger: false,
                    showQuickJumper: true
                }}
                renderItem={(item) => (
                    <List.Item>
                        <Card
                            style={{ width: '100%' }}
                            actions={[
                                <Link to={`/announcement/${item.id}`}>
                                    <EyeOutlined /> View
                                </Link>,
                                <Link to={`/edit/${item.id}`}>
                                    <EditOutlined /> Edit
                                </Link>,
                                <Popconfirm
                                    title="Delete this announcement?"
                                    onConfirm={() => handleDelete(item.id)}
                                >
                                    <Button type="link" danger>
                                        <DeleteOutlined /> Delete
                                    </Button>
                                </Popconfirm>
                            ]}
                        >
                            <Card.Meta
                                title={item.title}
                                description={`${item.description?.substring(0, 100)}...`}
                            />
                            <p style={{ marginTop: 10, color: '#666' }}>
                                Added: {new Date(item.dateAdded).toLocaleDateString()}
                            </p>
                        </Card>
                    </List.Item>
                )}
            />
        </div>
    );
};

export default AnnouncementList;