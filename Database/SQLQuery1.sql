Create database StackIt
use StackIt 

-- 1. Users
CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) UNIQUE,
    email VARCHAR(255) UNIQUE,
    password VARCHAR(255),
    fullName VARCHAR(100),
    profilePictureUrl VARCHAR(500),
    bio TEXT,
    reputation INT DEFAULT 0,
    role VARCHAR(20) DEFAULT 'user',
    isActive BIT DEFAULT 1,
    isBanned BIT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE()
);

-- 2. Questions
CREATE TABLE Questions (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    title VARCHAR(300),
    description TEXT,
    viewCount INT DEFAULT 0,
    voteScore INT DEFAULT 0,
    answerCount INT DEFAULT 0,
    acceptedAnswerId INT,
    isClosed BIT DEFAULT 0,
    closedReason TEXT,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Questions_User FOREIGN KEY (userId) REFERENCES Users(id)
);

-- 3. Answers
CREATE TABLE Answers (
    id INT PRIMARY KEY IDENTITY(1,1),
    questionId INT,
    userId INT,
    content TEXT,
    voteScore INT DEFAULT 0,
    isAccepted BIT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Answers_Question FOREIGN KEY (questionId) REFERENCES Questions(id),
    CONSTRAINT FK_Answers_User FOREIGN KEY (userId) REFERENCES Users(id)
);

-- 4. Tags
CREATE TABLE Tags (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(50) UNIQUE,
    description TEXT,
    color VARCHAR(7) DEFAULT '#007bff',
    usageCount INT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE()
);

-- 5. QuestionTags
CREATE TABLE QuestionTags (
    id INT PRIMARY KEY IDENTITY(1,1),
    questionId INT,
    tagId INT,
    createdAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT uq_QuestionTag UNIQUE (questionId, tagId),
    CONSTRAINT FK_QuestionTags_Question FOREIGN KEY (questionId) REFERENCES Questions(id),
    CONSTRAINT FK_QuestionTags_Tag FOREIGN KEY (tagId) REFERENCES Tags(id)
);

-- 6. Votes
CREATE TABLE Votes (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    votableType VARCHAR(20),
    votableId INT,
    voteType VARCHAR(10),
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT uq_Vote UNIQUE (userId, votableType, votableId),
    CONSTRAINT FK_Votes_User FOREIGN KEY (userId) REFERENCES Users(id)
);

-- 7. Comments
CREATE TABLE Comments (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    commentableType VARCHAR(20),
    commentableId INT,
    content TEXT,
    voteScore INT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Comments_User FOREIGN KEY (userId) REFERENCES Users(id)
);

-- 8. Notifications
CREATE TABLE Notifications (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    type VARCHAR(50),
    title VARCHAR(200),
    message TEXT,
    relatedType VARCHAR(20),
    relatedId INT,
    isRead BIT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Notifications_User FOREIGN KEY (userId) REFERENCES Users(id)
);

INSERT INTO Users (username, email, password, fullName, bio, reputation, role, isActive, isBanned, createdAt, updatedAt)
VALUES 
('user1', 'user1@example.com', 'hashedpass1', 'User 1', 'Bio of user 1', 10, 'user', 1, 0, GETDATE(), GETDATE()),
('user2', 'user2@example.com', 'hashedpass2', 'User 2', 'Bio of user 2', 20, 'user', 1, 0, GETDATE(), GETDATE()),
('user3', 'user3@example.com', 'hashedpass3', 'User 3', 'Bio of user 3', 30, 'user', 1, 0, GETDATE(), GETDATE()),
('user4', 'user4@example.com', 'hashedpass4', 'User 4', 'Bio of user 4', 40, 'user', 1, 0, GETDATE(), GETDATE()),
('user5', 'user5@example.com', 'hashedpass5', 'User 5', 'Bio of user 5', 50, 'user', 1, 0, GETDATE(), GETDATE());

INSERT INTO Questions (userId, title, description, viewCount, voteScore, answerCount, acceptedAnswerId, isClosed, closedReason, createdAt, updatedAt)
VALUES 
(1, 'Question Title 1', 'How do I solve problem 1?', 5, 2, 0, NULL, 0, NULL, GETDATE(), GETDATE()),
(2, 'Question Title 2', 'How do I solve problem 2?', 10, 4, 0, NULL, 0, NULL, GETDATE(), GETDATE()),
(3, 'Question Title 3', 'How do I solve problem 3?', 15, 6, 0, NULL, 0, NULL, GETDATE(), GETDATE()),
(4, 'Question Title 4', 'How do I solve problem 4?', 20, 8, 0, NULL, 0, NULL, GETDATE(), GETDATE()),
(5, 'Question Title 5', 'How do I solve problem 5?', 25, 10, 0, NULL, 0, NULL, GETDATE(), GETDATE());

INSERT INTO Answers (questionId, userId, content, voteScore, isAccepted, createdAt, updatedAt)
VALUES 
(1, 2, 'This is answer 1', 1, 0, GETDATE(), GETDATE()),
(2, 3, 'This is answer 2', 2, 0, GETDATE(), GETDATE()),
(3, 4, 'This is answer 3', 3, 0, GETDATE(), GETDATE()),
(4, 5, 'This is answer 4', 4, 0, GETDATE(), GETDATE()),
(5, 1, 'This is answer 5', 5, 0, GETDATE(), GETDATE());

INSERT INTO Comments (userId, commentableType, commentableId, content, voteScore, createdAt, updatedAt)
VALUES 
(1, 'answer', 1, 'This is comment 1', 1, GETDATE(), GETDATE()),
(2, 'answer', 2, 'This is comment 2', 2, GETDATE(), GETDATE()),
(3, 'answer', 3, 'This is comment 3', 3, GETDATE(), GETDATE()),
(4, 'answer', 4, 'This is comment 4', 4, GETDATE(), GETDATE()),
(5, 'answer', 5, 'This is comment 5', 5, GETDATE(), GETDATE());

INSERT INTO Tags (name, description, color, usageCount, createdAt)
VALUES 
('Tag1', 'Description for tag 1', '#001010', 3, GETDATE()),
('Tag2', 'Description for tag 2', '#002020', 6, GETDATE()),
('Tag3', 'Description for tag 3', '#003030', 9, GETDATE()),
('Tag4', 'Description for tag 4', '#004040', 12, GETDATE()),
('Tag5', 'Description for tag 5', '#005050', 15, GETDATE());

INSERT INTO QuestionTags (questionId, tagId, createdAt)
VALUES 
(2, 1, GETDATE()),
(3, 2, GETDATE()),
(4, 3, GETDATE()),
(5, 4, GETDATE()),
(1, 5, GETDATE());

INSERT INTO Votes (userId, votableType, votableId, voteType, createdAt, updatedAt)
VALUES 
(1, 'question', 1, 'upvote', GETDATE(), GETDATE()),
(2, 'question', 2, 'upvote', GETDATE(), GETDATE()),
(3, 'answer',   3, 'upvote', GETDATE(), GETDATE()),
(4, 'answer',   1, 'upvote', GETDATE(), GETDATE()),
(5, 'question', 5, 'downvote', GETDATE(), GETDATE());

INSERT INTO Notifications (userId, type, title, message, relatedType, relatedId, isRead, createdAt)
VALUES 
(1, 'answer_posted', 'New Answer', 'Your question got a new answer.', 'answer', 1, 0, GETDATE()),
(2, 'comment_added', 'New Comment', 'Someone commented on your answer.', 'comment', 2, 0, GETDATE()),
(3, 'mention', 'You were mentioned', 'Bob mentioned you in a comment.', 'answer', 3, 0, GETDATE()),
(4, 'answer_posted', 'New Answer', 'Your question about SQL got a response.', 'answer', 4, 0, GETDATE()),
(5, 'vote_received', 'You got an upvote!', 'Your answer received an upvote.', 'answer', 5, 0, GETDATE());


-- 1. Users
SELECT * FROM Users;

-- 2. Questions
SELECT * FROM Questions;

-- 3. Answers
SELECT * FROM Answers;

-- 4. Tags
SELECT * FROM Tags;

-- 5. QuestionTags
SELECT * FROM QuestionTags;

-- 6. Votes
SELECT * FROM Votes;

-- 7. Comments
SELECT * FROM Comments;

-- 8. Notifications
SELECT * FROM Notifications;
