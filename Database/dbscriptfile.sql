USE [master]
GO
/****** Object:  Database [StackIt]    Script Date: 12-07-2025 09:52:16 ******/
CREATE DATABASE [StackIt]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StackIt', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\StackIt.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StackIt_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\StackIt_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [StackIt] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StackIt].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StackIt] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StackIt] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StackIt] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StackIt] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StackIt] SET ARITHABORT OFF 
GO
ALTER DATABASE [StackIt] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [StackIt] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StackIt] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StackIt] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StackIt] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StackIt] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StackIt] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StackIt] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StackIt] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StackIt] SET  ENABLE_BROKER 
GO
ALTER DATABASE [StackIt] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StackIt] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StackIt] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StackIt] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StackIt] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StackIt] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StackIt] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StackIt] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StackIt] SET  MULTI_USER 
GO
ALTER DATABASE [StackIt] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StackIt] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StackIt] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StackIt] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StackIt] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [StackIt] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [StackIt] SET QUERY_STORE = ON
GO
ALTER DATABASE [StackIt] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [StackIt]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionId] [int] NULL,
	[userId] [int] NULL,
	[content] [text] NULL,
	[voteScore] [int] NULL,
	[isAccepted] [bit] NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[commentableType] [varchar](20) NULL,
	[commentableId] [int] NULL,
	[content] [text] NULL,
	[voteScore] [int] NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[type] [varchar](50) NULL,
	[title] [varchar](200) NULL,
	[message] [text] NULL,
	[relatedType] [varchar](20) NULL,
	[relatedId] [int] NULL,
	[isRead] [bit] NULL,
	[createdAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[title] [varchar](300) NULL,
	[description] [text] NULL,
	[viewCount] [int] NULL,
	[voteScore] [int] NULL,
	[answerCount] [int] NULL,
	[acceptedAnswerId] [int] NULL,
	[isClosed] [bit] NULL,
	[closedReason] [text] NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuestionTags]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionTags](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionId] [int] NULL,
	[tagId] [int] NULL,
	[createdAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uq_QuestionTag] UNIQUE NONCLUSTERED 
(
	[questionId] ASC,
	[tagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[description] [text] NULL,
	[color] [varchar](7) NULL,
	[usageCount] [int] NULL,
	[createdAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[email] [varchar](255) NULL,
	[password] [varchar](255) NULL,
	[fullName] [varchar](100) NULL,
	[profilePictureUrl] [varchar](500) NULL,
	[bio] [text] NULL,
	[reputation] [int] NULL,
	[role] [varchar](20) NULL,
	[isActive] [bit] NULL,
	[isBanned] [bit] NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Votes]    Script Date: 12-07-2025 09:52:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[votableType] [varchar](20) NULL,
	[votableId] [int] NULL,
	[voteType] [varchar](10) NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uq_Vote] UNIQUE NONCLUSTERED 
(
	[userId] ASC,
	[votableType] ASC,
	[votableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answers] ADD  DEFAULT ((0)) FOR [voteScore]
GO
ALTER TABLE [dbo].[Answers] ADD  DEFAULT ((0)) FOR [isAccepted]
GO
ALTER TABLE [dbo].[Answers] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Answers] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT ((0)) FOR [voteScore]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ((0)) FOR [isRead]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT ((0)) FOR [viewCount]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT ((0)) FOR [voteScore]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT ((0)) FOR [answerCount]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT ((0)) FOR [isClosed]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[QuestionTags] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Tags] ADD  DEFAULT ('#007bff') FOR [color]
GO
ALTER TABLE [dbo].[Tags] ADD  DEFAULT ((0)) FOR [usageCount]
GO
ALTER TABLE [dbo].[Tags] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [reputation]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('user') FOR [role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [isActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [isBanned]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[Votes] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Votes] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Answers_Question] FOREIGN KEY([questionId])
REFERENCES [dbo].[Questions] ([id])
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Answers_Question]
GO
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Answers_User] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Answers_User]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_User] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_User]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_User] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_User]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_User] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_User]
GO
ALTER TABLE [dbo].[QuestionTags]  WITH CHECK ADD  CONSTRAINT [FK_QuestionTags_Question] FOREIGN KEY([questionId])
REFERENCES [dbo].[Questions] ([id])
GO
ALTER TABLE [dbo].[QuestionTags] CHECK CONSTRAINT [FK_QuestionTags_Question]
GO
ALTER TABLE [dbo].[QuestionTags]  WITH CHECK ADD  CONSTRAINT [FK_QuestionTags_Tag] FOREIGN KEY([tagId])
REFERENCES [dbo].[Tags] ([id])
GO
ALTER TABLE [dbo].[QuestionTags] CHECK CONSTRAINT [FK_QuestionTags_Tag]
GO
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_User] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_User]
GO
USE [master]
GO
ALTER DATABASE [StackIt] SET  READ_WRITE 
GO
