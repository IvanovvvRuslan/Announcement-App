import React from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import { Layout, Menu } from 'antd';
import AnnouncementList from './components/AnnouncementList';
import AnnouncementDetail from './components/AnnouncementDetail';
import AnnouncementForm from './components/AnnouncementForm';
const { Header, Content } = Layout;


function App() {
    return (
        <Layout>
            <Header>
                <Menu
                    theme="dark"
                    mode="horizontal"
                    items={[
                        {
                            key: '1',
                            label: <Link to="/">Announcements</Link>
                        },
                        {
                            key: '2',
                            label: <Link to="/create">Add Announcement</Link>
                        }
                    ]}
                />
            </Header>
            <Content style={{ padding: '20px', maxWidth: '1200px', margin: '0 auto' }}>
                <Routes>
                    <Route path="/" element={<AnnouncementList />} />
                    <Route path="/announcement/:id" element={<AnnouncementDetail />} />
                    <Route path="/create" element={<AnnouncementForm />} />
                    <Route path="/edit/:id" element={<AnnouncementForm />} />
                </Routes>
            </Content>
        </Layout>
    );
}

export default App;