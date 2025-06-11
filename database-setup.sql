USE AnnouncementsDb;
GO

-- Check if Full-Text Search is available
SELECT SERVERPROPERTY('IsFullTextInstalled') AS IsFullTextInstalled;
GO

-- Create Full-Text Catalog (if not exists)
IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'AnnouncementCatalog')
BEGIN
    CREATE FULLTEXT CATALOG AnnouncementCatalog AS DEFAULT;
END
GO

-- Create Full-Text Index (if not exists)
IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID('Announcements'))
BEGIN
    CREATE FULLTEXT INDEX ON Announcements(Title, Description) 
    KEY INDEX PK_Announcements ON AnnouncementCatalog
    WITH STOPLIST = OFF, CHANGE_TRACKING AUTO;
END
GO

INSERT INTO Announcements (Title, Description, DateAdded) VALUES
('Summer Sale Electronics', 'Huge summer discounts on all electronics and gadgets', '2025-05-01'),
('Software Update Released', 'New software update with enhanced features released today', '2025-05-09'),
('Discount Electronics Store', 'Massive discounts on summer electronics at our store', '2025-06-06'),
('Coding Bootcamp Summer', 'Intensive summer coding bootcamp for aspiring developers', '2025-06-07'),
('Job Opening Developer', 'Looking for experienced software developer to join our team', '2025-05-02'),
('Hiring Software Engineer', 'We are hiring a talented software engineer immediately', '2025-06-02'),
('Volunteer Community Project', 'Volunteer needed for local summer community project', '2025-05-10'),
('Garden Tools Sale', 'Brand new garden tools available for sale this summer', '2025-05-03'),
('Car Sale Great Condition', 'Used car in excellent condition available for immediate sale', '2025-05-04'),
('Garage Sale Weekend', 'Multi-family garage sale happening this weekend', '2025-05-08'),
('Free Books Library', 'Get completely free books at the city library event', '2025-06-03'),
('Lost Brown Dog', 'Lost brown dog in city center area, please help find', '2025-05-05'),
('Found Small Dog', 'Found small dog near central park, looking for owner', '2025-06-01'),
('Summer Camp Kids', 'Join our fun and exciting summer camp program for kids', '2025-05-06'),
('Summer Music Festival', 'Don''t miss the amazing summer music festival this year', '2025-06-04'),
('City Cleanup Event', 'Join our summer cleanup event in downtown area', '2025-06-05'),
('Apartment Rent Center', 'Spacious apartment for rent near city center location', '2025-05-07'),
('Violin Lessons Private', 'Learn to play the violin with our experienced private tutor', '2025-05-15'),
('Test Announcement', 'This is a test announcement for development purposes', '2025-05-20'),
('Development Testing', 'Testing announcement for development and quality assurance', '2025-05-21');

GO

SELECT * FROM Announcements