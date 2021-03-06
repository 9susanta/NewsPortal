USE [master]
GO
/****** Object:  Database [NewsPortal]    Script Date: 6/3/2019 11:05:14 PM ******/
CREATE DATABASE [NewsPortal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NewsPortal', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\NewsPortal.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'NewsPortal_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\NewsPortal_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [NewsPortal] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NewsPortal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NewsPortal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NewsPortal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NewsPortal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NewsPortal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NewsPortal] SET ARITHABORT OFF 
GO
ALTER DATABASE [NewsPortal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NewsPortal] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [NewsPortal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NewsPortal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NewsPortal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NewsPortal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NewsPortal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NewsPortal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NewsPortal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NewsPortal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NewsPortal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NewsPortal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NewsPortal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NewsPortal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NewsPortal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NewsPortal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NewsPortal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NewsPortal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NewsPortal] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [NewsPortal] SET  MULTI_USER 
GO
ALTER DATABASE [NewsPortal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NewsPortal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NewsPortal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NewsPortal] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [NewsPortal]
GO
/****** Object:  Table [dbo].[NewsPost]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsPost](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[EnglishTitle] [nvarchar](50) NULL,
	[OdiaTitle] [nvarchar](50) NULL,
	[HeaderImageName] [nvarchar](50) NULL,
	[EngShortDesc] [nvarchar](500) NULL,
	[ODShortDesc] [nvarchar](500) NULL,
	[ODContent] [nvarchar](max) NULL,
	[SeoMeta] [nvarchar](500) NULL,
	[CategoryId] [int] NULL,
	[PostedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[Tags] [nvarchar](200) NULL,
	[IsReviewed] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[ReviewedBy] [int] NULL,
 CONSTRAINT [PK_NewsPost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NewsTagMap]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsTagMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Post_Id] [int] NULL,
	[Tag_Id] [int] NULL,
 CONSTRAINT [PK_NewsTagMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tags]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[UrlSlug] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblNewsType]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblNewsType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NewsType] [nvarchar](50) NULL,
	[UrlSlug] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tblNewsType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRights]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRights](
	[RightsId] [int] IDENTITY(1,1) NOT NULL,
	[RightsName] [nvarchar](150) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tblRights] PRIMARY KEY CLUSTERED 
(
	[RightsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRole]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRole](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 6/3/2019 11:05:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[RoleId] [int] NULL,
	[ImageName] [nvarchar](150) NULL,
	[DateCreate] [datetime] NULL,
	[DateUpdate] [datetime] NULL,
	[NoofFailurAttempt] [int] NULL,
	[IsBlocked] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[tblRole] ON 

INSERT [dbo].[tblRole] ([RoleId], [RoleName], [IsDeleted]) VALUES (1, N'SuperAdmin', 0)
INSERT [dbo].[tblRole] ([RoleId], [RoleName], [IsDeleted]) VALUES (2, N'Admin', 0)
INSERT [dbo].[tblRole] ([RoleId], [RoleName], [IsDeleted]) VALUES (3, N'Chief Editor', 0)
INSERT [dbo].[tblRole] ([RoleId], [RoleName], [IsDeleted]) VALUES (4, N'Editor', 0)
INSERT [dbo].[tblRole] ([RoleId], [RoleName], [IsDeleted]) VALUES (5, N'Moderator', 0)
SET IDENTITY_INSERT [dbo].[tblRole] OFF
SET IDENTITY_INSERT [dbo].[tblUser] ON 

INSERT [dbo].[tblUser] ([UserId], [FullName], [Email], [Phone], [UserName], [Password], [RoleId], [ImageName], [DateCreate], [DateUpdate], [NoofFailurAttempt], [IsBlocked], [IsDeleted]) VALUES (1, N'Susanta Rout', N'rsusanta@gmail.com', N'8908654345', N'Susanta', N'123456', 1, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[tblUser] ([UserId], [FullName], [Email], [Phone], [UserName], [Password], [RoleId], [ImageName], [DateCreate], [DateUpdate], [NoofFailurAttempt], [IsBlocked], [IsDeleted]) VALUES (2, N'susanta', N'rsusanta@gmail.com', N'8908654345', N'routs', N'1234', NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[tblUser] ([UserId], [FullName], [Email], [Phone], [UserName], [Password], [RoleId], [ImageName], [DateCreate], [DateUpdate], [NoofFailurAttempt], [IsBlocked], [IsDeleted]) VALUES (3, N'susanta', N'rsusanta@gmail.co', N'e', N'fegr', N'fefe', NULL, NULL, NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[tblUser] OFF
USE [master]
GO
ALTER DATABASE [NewsPortal] SET  READ_WRITE 
GO
