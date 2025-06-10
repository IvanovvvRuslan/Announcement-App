import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { Card, Spin, message, Button, List, Popconfirm } from 'antd';
import { ArrowLeftOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { announcementAPI } from '../services/api.js';

const AnnouncementDetail = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [announcement, setAnnouncement] = useState(null);
    const [similarAnnouncements, setSimilarAnnouncements] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchAnnouncementDetails();
    }, [id]);

    const fetchAnnouncementDetails = async () => {
        try {
            const response = await announcementAPI.getById(id);
            setAnnouncement(response.data);
            setSimilarAnnouncements(response.data.similarAnnouncements || []);
        } catch (error) {
            message.error('Failed to load announcement details');
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        try {
            await announcementAPI.delete(id);
            message.success('Announcement deleted successfully');
            navigate('/');
        } catch (error) {
            message.error('Failed to delete announcement');
        }
    };

    if (loading) {
        return <Spin size="large" style={{ display: 'block', margin: '50px auto' }} />;
    }

    if (!announcement) {
        return <div>Announcement not found</div>;
    }

    return (
        <div>
            <Button
                type="link"
                icon={<ArrowLeftOutlined />}
                style={{ marginBottom: 16 }}
            >
                <Link to="/">Back to Announcements</Link>
            </Button>

            <Card title={announcement.title} style={{ marginBottom: 24 }}>
                <p style={{ fontSize: '16px', lineHeight: '1.6' }}>
                    {announcement.description}
                </p>
                <p style={{ color: '#666', marginTop: 16 }}>
                    <strong>Date Added:</strong> {new Date(announcement.dateAdded).toLocaleDateString()}
                </p>

                <div style={{ marginTop: 20, paddingTop: 16, borderTop: '1px solid #f0f0f0' }}>
                    <Link to={`/edit/${id}`}>
                        <Button type="primary" icon={<EditOutlined />} style={{ marginRight: 8 }}>
                            Edit
                        </Button>
                    </Link>
                    <Popconfirm
                        title="Delete this announcement?"
                        description="This action cannot be undone."
                        onConfirm={handleDelete}
                        okText="Yes"
                        cancelText="No"
                    >
                        <Button danger icon={<DeleteOutlined />}>
                            Delete
                        </Button>
                    </Popconfirm>
                </div>
            </Card>

            {similarAnnouncements.length > 0 && (
                <div>
                    <h2>Similar Announcements</h2>
                    <List
                        dataSource={similarAnnouncements}
                        renderItem={(item) => (
                            <List.Item>
                                <Card
                                    style={{ width: '100%' }}
                                    title={
                                        <Link to={`/announcement/${item.id}`}>
                                            {item.title}
                                        </Link>
                                    }
                                >
                                    <p>{item.description?.substring(0, 150)}...</p>
                                    <p style={{ color: '#666', marginTop: 8 }}>
                                        Added: {new Date(item.dateAdded).toLocaleDateString()}
                                    </p>
                                </Card>
                            </List.Item>
                        )}
                    />
                </div>
            )}
        </div>
    );
};

export default AnnouncementDetail;