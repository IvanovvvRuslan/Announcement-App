import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { Form, Input, Button, Card, message, Spin } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { announcementAPI } from '../services/api.js';

const { TextArea } = Input;

const AnnouncementForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [initialLoading, setInitialLoading] = useState(!!id);

    const isEdit = !!id;

    useEffect(() => {
        if (isEdit) {
            fetchAnnouncement();
        }
    }, [id]);

    const fetchAnnouncement = async () => {
        try {
            const response = await announcementAPI.getById(id);
            form.setFieldsValue({
                title: response.data.title,
                description: response.data.description
            });
        } catch (error) {
            message.error('Failed to load announcement');
        } finally {
            setInitialLoading(false);
        }
    };

    const onFinish = async (values) => {
        setLoading(true);
        try {
            if (isEdit) {
                await announcementAPI.update(id, values);
                message.success('Announcement updated successfully');
            } else {
                await announcementAPI.create(values);
                message.success('Announcement created successfully');
            }
            navigate('/');
        } catch (error) {
            message.error(`Failed to ${isEdit ? 'update' : 'create'} announcement`);
        } finally {
            setLoading(false);
        }
    };

    if (initialLoading) {
        return <Spin size="large" style={{ display: 'block', margin: '50px auto' }} />;
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

            <Card title={isEdit ? 'Edit Announcement' : 'Create New Announcement'}>
                <Form
                    form={form}
                    layout="vertical"
                    onFinish={onFinish}
                    style={{ maxWidth: 600 }}
                >
                    <Form.Item
                        name="title"
                        label="Title"
                        rules={[
                            { required: true, message: 'Please enter title' },
                            { min: 3, message: 'Title must be at least 3 characters' },
                            { max: 100, message: 'Title must be less than 100 characters' }
                        ]}
                    >
                        <Input placeholder="Enter announcement title" />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label="Description"
                        rules={[
                            { required: true, message: 'Please enter description' },
                            { min: 10, message: 'Description must be at least 10 characters' },
                            { max: 1000, message: 'Description must be less than 1000 characters' }
                        ]}
                    >
                        <TextArea
                            rows={6}
                            placeholder="Enter announcement description"
                        />
                    </Form.Item>

                    <Form.Item>
                        <Button
                            type="primary"
                            htmlType="submit"
                            loading={loading}
                            style={{ marginRight: 8 }}
                        >
                            {isEdit ? 'Update' : 'Create'} Announcement
                        </Button>
                        <Button onClick={() => navigate('/')}>
                            Cancel
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </div>
    );
};

export default AnnouncementForm;