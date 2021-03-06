USE [master]
GO
/****** Object:  Database [NewsPortal]    Script Date: 10/27/2019 1:18:45 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_GetAllNews]    Script Date: 10/27/2019 1:18:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[usp_GetAllNews]
(
@NewsId numeric(18,0)=0,
@CreatedUserId int=0,
@ReviewdBy int=0
)
as
begin
SET NOCOUNT ON;
if(@NewsId=0 and @CreatedUserId=0)
begin
	       select np.Id,
		   np.EnglishTitle,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
           np.ModifiedOn,
           np.IsReviewed,
           np.CategoryId,
           np.CreatedBy,
           np.ReviewedBy,
		   tblnews.OdiaName as CategoryOdia,
           tblnews.NewsType as Category,
		   usr.FullName as Created,
		   uses.FullName as Reviewed
           from [dbo].[NewsPost] np join
		   [dbo].[tblNewsType] tblnews on np.CategoryId=tblnews.Id
		   join [dbo].[tblUser] usr on np.CreatedBy=usr.UserId
		   left join [dbo].[tblUser] uses on np.ReviewedBy=uses.UserId
		   where np.IsDeleted=0 and tblnews.IsDeleted=0
		   order by np.PostedOn desc
end
else if(@NewsId<>0 and @CreatedUserId=0)
begin
	       select np.Id,
		   np.EnglishTitle,
           np.OdiaTitle,
		   np.HeaderImageName,
		   np.EngShortDesc,
		   np.ODShortDesc,
           np.ODContent,
           np.SeoMeta,
           np.CategoryId,
           np.Tags,
           np.PostedOn,
           np.ModifiedOn,
           np.IsReviewed,
           np.CreatedBy,
           np.ReviewedBy,
		   tblnews.OdiaName as CategoryOdia,
           tblnews.NewsType as Category,
		   usr.FullName as Created,
		   uses.FullName as Reviewed
           from [dbo].[NewsPost] np join
		   [dbo].[tblNewsType] tblnews on np.CategoryId=tblnews.Id
		   join [dbo].[tblUser] usr on np.CreatedBy=usr.UserId
		   left join [dbo].[tblUser] uses on np.ReviewedBy=uses.UserId
		   where np.Id=@NewsId and np.IsDeleted=0 and tblnews.IsDeleted=0
		    order by np.PostedOn desc
end
else if(@NewsId=0 and @CreatedUserId<>0)
begin
	       select np.Id,
		   np.EnglishTitle,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
           np.ModifiedOn,
           np.IsReviewed,
           np.CategoryId,
           np.CreatedBy,
           np.ReviewedBy,
		   tblnews.OdiaName as CategoryOdia,
           tblnews.NewsType as Category,
		   usr.FullName as Created,
		   uses.FullName as Reviewed
           from [dbo].[NewsPost] np join
		   [dbo].[tblNewsType] tblnews on np.CategoryId=tblnews.Id
		   join [dbo].[tblUser] usr on np.CreatedBy=usr.UserId
		   left join [dbo].[tblUser] uses on np.ReviewedBy=uses.UserId
		   where np.CreatedBy=@CreatedUserId and np.IsDeleted=0 and tblnews.IsDeleted=0
		    order by np.PostedOn desc
end
else if(@NewsId=0 and @ReviewdBy<>0)
begin
	       select np.Id,
		   np.EnglishTitle,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
           np.ModifiedOn,
           np.IsReviewed,
           np.CategoryId,
           np.CreatedBy,
           np.ReviewedBy,
		   tblnews.OdiaName as CategoryOdia,
           tblnews.NewsType as Category,
		   usr.FullName as Created,
		   uses.FullName as Reviewed
           from [dbo].[NewsPost] np join
		   [dbo].[tblNewsType] tblnews on np.CategoryId=tblnews.Id
		   join [dbo].[tblUser] usr on np.CreatedBy=usr.UserId
		   left join [dbo].[tblUser] uses on np.ReviewedBy=uses.UserId
		   where np.ReviewedBy=@ReviewdBy and np.IsDeleted=0 and tblnews.IsDeleted=0
		    order by np.PostedOn desc
end

end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAnalytics]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetAnalytics]
(
@UserId int
)
AS
BEGIN
   SET NOCOUNT ON
   IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
   DROP TABLE #AllCategory
   CREATE TABLE #AllCategory(Id INT null,OdiaName NVARCHAR(50) null,NewsType NVARCHAR(50) null)
   INSERT INTO #AllCategory SELECT Id,OdiaName,NewsType FROM [dbo].[tblNewsType] WHERE IsDeleted=0

   --Total Count
   IF @UserId=0
   BEGIN
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1
		GROUP BY tblnews.Id,tblnews.OdiaName
   END
   ELSE
   BEGIN
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   END
    --Total View
    IF @UserId=0
   BEGIN
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1
		GROUP BY tblnews.Id,tblnews.OdiaName
   END
   ELSE
   BEGIN
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   END
   --Today vs Yesterday Post
   	IF @UserId=0
	BEGIN
        SELECT * INTO #TdayYday FROM(
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=CONVERT(date,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=DATEADD(DAY,-1,CONVERT(date,GETDATE()))
		GROUP BY tblnews.Id,tblnews.OdiaName
      ) AS ty 
   END
   ELSE
   BEGIN
        SELECT * INTO #TdayYdaye FROM(
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=CONVERT(date,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=DATEADD(DAY,-1,CONVERT(date,GETDATE()))
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
      ) AS ty
   END
  
   --Week vs Last Week Post
   IF @UserId=0
	BEGIN
    SELECT * INTO #Wkly FROM(
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,DATEADD(WEEK,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
   ELSE
   BEGIN
     SELECT * INTO #Wklye FROM(
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,DATEADD(WEEK,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
    
      --Month vs Last Month Post
    IF @UserId=0
	BEGIN
    SELECT * INTO #Monthly FROM(
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,DATEADD(MONTH,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
   ELSE
   BEGIN
    --Month vs Last Month Post
    SELECT * INTO #Monthlye FROM(
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT COUNT_BIG(np.Id) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,DATEADD(MONTH,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
  
    --Today vs Yesterday View
    IF @UserId=0
	BEGIN
    SELECT * INTO #DayView FROM(
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=CONVERT(date,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=DATEADD(DAY,-1,CONVERT(date,GETDATE()))
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
   ELSE
   BEGIN
    SELECT * INTO #DayViewe FROM(
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=CONVERT(date,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and PostedDate=DATEADD(DAY,-1,CONVERT(date,GETDATE()))
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END

   --Week vs Last Week View
    IF @UserId=0
	BEGIN
    SELECT * INTO #WkView FROM(
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,DATEADD(WEEK,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
   ELSE
   BEGIN
    SELECT * INTO #WkViewe FROM(
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(WEEK,PostedDate)=DATEPART(WEEK,DATEADD(WEEK,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
    --Month vs Last Month View
	 IF @UserId=0
	BEGIN
    SELECT * INTO #MonthView FROM(
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,DATEADD(MONTH,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END
   ELSE
   BEGIN
    SELECT * INTO #MonthViewe FROM(
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'cur' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,GETDATE()) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
	  UNION
		SELECT SUM(np.Frequency) AS TotView,tblnews.OdiaName,tblnews.Id as CatID,'prev' as staus FROM [dbo].[NewsPost] np join
		#AllCategory tblnews ON np.CategoryId=tblnews.Id
		WHERE IsDeleted=0 and IsActive=1 and DATEPART(MONTH,PostedDate)=DATEPART(MONTH,DATEADD(MONTH,-1,GETDATE())) and DATEPART(YEAR,PostedDate)=DATEPART(YEAR,GETDATE())
		and np.CreatedBy=@UserId
		GROUP BY tblnews.Id,tblnews.OdiaName
   ) AS ty
   END

    IF @UserId=0
	BEGIN
		SELECT * FROM #TdayYday order by staus,CatID
		SELECT * FROM #Wkly order by staus,CatID
		SELECT * FROM #Monthly order by staus,CatID
		SELECT * FROM #DayView order by staus,CatID
		SELECT * FROM #WkView order by staus,CatID
		SELECT * FROM #MonthView order by staus,CatID
	END
	ELSE
	BEGIN
		SELECT * FROM #TdayYdaye order by staus,CatID
		SELECT * FROM #Wklye order by staus,CatID
		SELECT * FROM #Monthlye order by staus,CatID
		SELECT * FROM #DayViewe order by staus,CatID
		SELECT * FROM #WkViewe order by staus,CatID
		SELECT * FROM #MonthViewe order by staus,CatID
	END

   IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
   DROP TABLE #AllCategory
   
   IF OBJECT_ID('tempdb..#TdayYday') IS NOT NULL
   DROP TABLE #TdayYday
   IF OBJECT_ID('tempdb..#Wkly') IS NOT NULL
   DROP TABLE #Wkly
   IF OBJECT_ID('tempdb..#Monthly') IS NOT NULL
   DROP TABLE #Monthly
   IF OBJECT_ID('tempdb..#DayView') IS NOT NULL
   DROP TABLE #DayView
   IF OBJECT_ID('tempdb..#WkView') IS NOT NULL
   DROP TABLE #WkView
   IF OBJECT_ID('tempdb..#MonthView') IS NOT NULL
   DROP TABLE #MonthView

   IF OBJECT_ID('tempdb..#TdayYdaye') IS NOT NULL
   DROP TABLE #TdayYdaye
   IF OBJECT_ID('tempdb..#Wklye') IS NOT NULL
   DROP TABLE #Wklye
   IF OBJECT_ID('tempdb..#Monthlye') IS NOT NULL
   DROP TABLE #Monthlye
   IF OBJECT_ID('tempdb..#DayViewe') IS NOT NULL
   DROP TABLE #DayViewe
   IF OBJECT_ID('tempdb..#WkViewe') IS NOT NULL
   DROP TABLE #WkViewe
   IF OBJECT_ID('tempdb..#MonthViewe') IS NOT NULL
   DROP TABLE #MonthViewe

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCatagoryData]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[usp_GetCatagoryData]
(
@CategoryId int
)
AS
BEGIN
IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
DROP TABLE #AllCategory
CREATE TABLE #AllCategory(Id int null,OdiaName nvarchar(50) null,NewsType nvarchar(50) null)
INSERT INTO #AllCategory SELECT Id,OdiaName,NewsType FROM [dbo].[tblNewsType] WHERE IsDeleted=0

SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,Thumbnail86,Thumbnail279,Thumbnail210,HeaderImageName,SlugUrl,b.OdiaName, b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Latest News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id WHERE PostedDate<=getDate() ORDER BY PostedDate DESC
SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,Thumbnail86,Thumbnail279,Thumbnail210,HeaderImageName,SlugUrl,b.OdiaName,b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Popular News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id ORDER BY Frequency,PostedDate DESC
SELECT TOP(10) np.Id,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
		   np.PostedYear,
		   np.PostedMonth,
		   np.SlugUrl,
		   np.ODContent,
		   np.HeaderImageName,
		   np.Thumbnail86,
		   np.Thumbnail279,
		   np.Thumbnail210,
		   np.ODShortDesc,
		   tblnews.NewsType,
		   tblnews.Id as CategoryId,
           tblnews.OdiaName AS Category
           FROM [dbo].[NewsPost] np join
		   #AllCategory tblnews ON np.CategoryId=tblnews.Id
		   WHERE np.IsDeleted=0 and np.CategoryId=@CategoryId
		   ORDER BY np.PostedOn DESC
SELECT COUNT(id) as TotalCount FROM NewsPost WHERE IsDeleted=0 and CategoryId=@CategoryId
IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
DROP TABLE #AllCategory
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetClientSideNews]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[usp_GetClientSideNews]
(
 @PageIndex INT,
 @pageSize INT,
 @CategoryId INT 
) 
As
 Begin
 SET NOCOUNT ON;
    select np.Id,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
		   np.PostedYear,
		   np.PostedMonth,
		   np.SlugUrl,
		   np.ODContent,
		   np.HeaderImageName,
		   np.Thumbnail86,
		   np.Thumbnail279,
		   np.Thumbnail210,
           tblnews.OdiaName as Category
           from [dbo].[NewsPost] np join
		   [dbo].[tblNewsType] tblnews on np.CategoryId=tblnews.Id
		   where np.IsDeleted=0 and tblnews.IsDeleted=0 and np.CategoryId=@CategoryId
		   order by np.PostedOn desc OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY

 SELECT count(Id) as totalCount FROM NewsPost where IsDeleted=0 and CategoryId=@CategoryId
 End
GO
/****** Object:  StoredProcedure [dbo].[usp_GetIndexPage]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[usp_GetIndexPage]
AS
BEGIN
	IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
	DROP TABLE #AllCategory
	
	CREATE TABLE #AllCategory(Id int null,OdiaName nvarchar(50) null,NewsType nvarchar(50) null)
	
	INSERT INTO #AllCategory SELECT Id,OdiaName,NewsType FROM [dbo].[tblNewsType] WHERE IsDeleted=0 order by Id asc
	
	SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,a.Thumbnail210,Thumbnail279,a.HeaderImageName,SlugUrl,b.OdiaName,b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Latest News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id WHERE PostedDate<=getDate() and b.Id<>10 ORDER BY PostedDate DESC
	
	SELECT news.*
    FROM ( SELECT DISTINCT Id FROM #AllCategory ) category
    CROSS APPLY 
	( 
	       SELECT TOP(8) np.Id,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
		   np.PostedYear,
		   np.PostedMonth,
		   np.SlugUrl,
		   np.ODContent,
		   np.HeaderImageName,
		   np.Thumbnail86,
		   np.Thumbnail279,
		   np.Thumbnail210,
		   newstype.NewsType,
		   np.CategoryId AS CategoryId,
           newstype.OdiaName AS Category
           FROM [dbo].[NewsPost] np
		   join #AllCategory newstype on np.CategoryId=newstype.Id
		   WHERE np.IsDeleted=0 and np.CategoryId=category.Id and np.CategoryId<>10
		   ORDER BY np.CategoryId,np.PostedOn DESC
	) news
   
    SELECT Id,OdiaName,NewsType from #AllCategory where Id<>10

    SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,Thumbnail86,Thumbnail279,Thumbnail210,HeaderImageName,SlugUrl,b.OdiaName,b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Popular News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id where b.Id<>10 ORDER BY Frequency,PostedDate DESC
    
    SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,Thumbnail86,Thumbnail279,Thumbnail210,HeaderImageName,SlugUrl,b.OdiaName,b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Photo News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id
	where a.CategoryId=10
    ORDER BY Frequency,PostedDate DESC

	IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
	DROP TABLE #AllCategory
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetNewPost]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[usp_GetNewPost]
(
@Id numeric(18,0)
)
as
begin
IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
DROP TABLE #AllCategory
CREATE TABLE #AllCategory(Id int null,OdiaName nvarchar(50) null,NewsType nvarchar(50) null)
INSERT INTO #AllCategory SELECT Id,OdiaName,NewsType FROM [dbo].[tblNewsType] WHERE IsDeleted=0

select top(6) a.Id,OdiaTitle,PostedMonth,PostedYear,a.Thumbnail86,a.Thumbnail279,a.Thumbnail210,SlugUrl,b.OdiaName,'Latest News',a.PostedOn,b.OdiaName as Category from NewsPost a 
join #AllCategory b on a.CategoryId=b.Id  where PostedDate<=getDate() order by PostedDate desc

select top(6) a.Id,OdiaTitle,PostedMonth,PostedYear,a.Thumbnail86,a.Thumbnail279,a.Thumbnail210,SlugUrl,b.OdiaName,'Popular News',a.PostedOn,b.OdiaName as Category from NewsPost a 
join #AllCategory b on a.CategoryId=b.Id order by Frequency,PostedDate desc

select top(6) a.Id,OdiaTitle,PostedMonth,PostedYear,a.Thumbnail86,a.Thumbnail279,a.Thumbnail210,SlugUrl,b.OdiaName,'Related News',a.PostedOn,b.OdiaName as Category  from NewsPost a
join #AllCategory b on a.CategoryId=b.Id
where a.CategoryId=(select top(1) CategoryId from NewsPost where Id=@Id)
order by a.PostedDate,a.Frequency desc

select np.Id,
       np.EnglishTitle,
	   np.SeoMeta,
	   np.EngShortDesc,
	   np.ODShortDesc,
       np.OdiaTitle,
       np.Tags,
       np.PostedOn,
	   np.ODContent,
	   np.HeaderImageName,
	   np.CategoryId,
	   tblnews.NewsType,
       tblnews.OdiaName as Category
       from [dbo].[NewsPost] np join
	   #AllCategory tblnews on np.CategoryId=tblnews.Id
	   where np.IsDeleted=0 and np.Id=@Id
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetSearch]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetSearch]
(
 @PageIndex INT,
 @pageSize INT,
 @SearchStr NVARCHAR(50)
) 
AS
BEGIN
IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
DROP TABLE #AllCategory
CREATE TABLE #AllCategory(Id int null,OdiaName nvarchar(50) null,NewsType nvarchar(50) null)
INSERT INTO #AllCategory SELECT Id,OdiaName,NewsType FROM [dbo].[tblNewsType] WHERE IsDeleted=0

SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,Thumbnail86,Thumbnail279,Thumbnail210,HeaderImageName,SlugUrl,b.OdiaName, b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Latest News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id WHERE PostedDate<=getDate() ORDER BY PostedDate DESC
SELECT TOP(6) a.Id,OdiaTitle,PostedMonth,PostedYear,Thumbnail86,Thumbnail279,Thumbnail210,HeaderImageName,SlugUrl,b.OdiaName,b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Popular News' as Header FROM NewsPost a join #AllCategory b ON a.CategoryId=b.Id ORDER BY Frequency,PostedDate DESC

 IF(@SearchStr<>'Latest')
 BEGIN
          SELECT np.Id,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
		   np.PostedYear,
		   np.PostedMonth,
		   np.SlugUrl,
		   np.ODContent,
		   np.HeaderImageName,
		   np.Thumbnail86,
		   np.Thumbnail279,
		   np.Thumbnail210,
		   np.ODShortDesc,
		   tblnews.NewsType,
		   tblnews.Id as CategoryId,
           tblnews.OdiaName AS Category
           FROM [dbo].[NewsPost] np join
		   #AllCategory tblnews ON np.CategoryId=tblnews.Id
		   WHERE  np.IsDeleted=0 and (np.OdiaTitle like N'%'+@SearchStr+'%')
		   ORDER by np.PostedOn DESC OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY

		   SELECT COUNT(np.Id) as TotalCount FROM [dbo].[NewsPost] np join
		   #AllCategory tblnews ON np.CategoryId=tblnews.Id
		   WHERE  np.IsDeleted=0 and (np.OdiaTitle like N'%'+@SearchStr+'%')
 END
 ELSE
 BEGIN
           SELECT np.Id,
           np.OdiaTitle,
           np.Tags,
           np.PostedOn,
		   np.PostedYear,
		   np.PostedMonth,
		   np.SlugUrl,
		   np.ODContent,
		   np.HeaderImageName,
		   np.Thumbnail86,
		   np.Thumbnail279,
		   np.Thumbnail210,
           tblnews.OdiaName AS Category
           FROM [dbo].[NewsPost] np join
		   #AllCategory tblnews ON np.CategoryId=tblnews.Id
		   WHERE  np.IsDeleted=0
		   ORDER BY np.PostedOn DESC OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY

		   SELECT COUNT(np.Id) as TotalCount FROM [dbo].[NewsPost] np join
		   #AllCategory tblnews ON np.CategoryId=tblnews.Id
		   WHERE  np.IsDeleted=0
		   ORDER by np.PostedOn DESC
 END
 IF OBJECT_ID('tempdb..#AllCategory') IS NOT NULL
 DROP TABLE #AllCategory
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetSetionData]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetSetionData]
(
@PageIndex INT,
@SectionName NVARCHAR(50),
@CategoryId INT=0
)
AS
BEGIN
DECLARE @PageSize INT=6
	IF(@SectionName='Latest')
		BEGIN
		    SELECT a.Id,OdiaTitle,a.PostedOn,a.Thumbnail86,a.Thumbnail279,a.Thumbnail210,SlugUrl,b.OdiaName, b.NewsType,b.Id as CategoryId,b.OdiaName AS Category,'Latest News' as Header FROM NewsPost a 
			join [dbo].[tblNewsType] b ON a.CategoryId=b.Id WHERE PostedDate<=getDate() and a.CategoryId<>10 ORDER BY PostedDate DESC 
			OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE IF(@SectionName='Popular')
		BEGIN
			SELECT a.Id,OdiaTitle,a.PostedOn,a.Thumbnail86,a.Thumbnail279,a.Thumbnail210,SlugUrl,b.OdiaName, b.NewsType,b.Id as CategoryId,b.OdiaName AS Category,'Popular News' as Header FROM NewsPost a 
			join [dbo].[tblNewsType] b ON a.CategoryId=b.Id where a.CategoryId<>10 ORDER BY Frequency,PostedDate DESC
			OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE IF(@SectionName='Related')
		BEGIN
		    SELECT a.Id,OdiaTitle,a.PostedOn,a.Thumbnail86,a.Thumbnail279,a.Thumbnail210,SlugUrl,b.OdiaName, b.NewsType,b.Id as CategoryId,b.OdiaName AS Category,'Related News' as Header FROM NewsPost a 
			join [dbo].[tblNewsType] b ON a.CategoryId=b.Id
			where a.CategoryId<>10 and a.CategoryId=(select top(1) CategoryId from NewsPost where Id=@CategoryId)
			ORDER BY Frequency DESC
			OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE IF(@SectionName='Photo')
		BEGIN
		    SELECT a.Id,OdiaTitle,a.PostedOn,Thumbnail86,Thumbnail279,Thumbnail210,SlugUrl,b.OdiaName,b.NewsType,b.Id as CategoryId,a.PostedOn,b.OdiaName AS Category,'Photo News' as Header FROM NewsPost a join [dbo].[tblNewsType] b ON a.CategoryId=b.Id
			where a.CategoryId=10
			ORDER BY Frequency,PostedDate DESC
			OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateNewsCount]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_UpdateNewsCount]
	@NewsId numeric(18,0)
AS
BEGIN
    declare @CurrentView numeric(18,0)
	select @CurrentView=Frequency from NewsPost where Id=@NewsId
	update NewsPost set Frequency=@CurrentView+1 where Id=@NewsId
	select 1
END

GO
/****** Object:  Table [dbo].[NewsPost]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsPost](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[EnglishTitle] [nvarchar](150) NULL,
	[OdiaTitle] [nvarchar](150) NULL,
	[Thumbnail210] [nvarchar](50) NULL,
	[Thumbnail279] [nvarchar](50) NULL,
	[Thumbnail86] [nvarchar](50) NULL,
	[HeaderImageName] [nvarchar](50) NULL,
	[EngShortDesc] [nvarchar](500) NULL,
	[ODShortDesc] [nvarchar](500) NULL,
	[ODContent] [nvarchar](max) NULL,
	[SeoMeta] [nvarchar](500) NULL,
	[CategoryId] [int] NULL,
	[PostedDate] [date] NULL,
	[PostedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[Tags] [nvarchar](200) NULL,
	[PostedMonth] [int] NULL,
	[PostedYear] [int] NULL,
	[Frequency] [int] NULL,
	[SlugUrl] [nvarchar](200) NULL,
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
/****** Object:  Table [dbo].[NewsTagMap]    Script Date: 10/27/2019 1:18:48 AM ******/
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
/****** Object:  Table [dbo].[Tags]    Script Date: 10/27/2019 1:18:48 AM ******/
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
/****** Object:  Table [dbo].[tblContact]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblContact](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Message] [nvarchar](350) NULL,
	[PostedOn] [datetime] NULL,
 CONSTRAINT [PK_E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblNewsType]    Script Date: 10/27/2019 1:18:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblNewsType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OdiaName] [nvarchar](50) NULL,
	[NewsType] [nvarchar](50) NULL,
	[UrlSlug] [nvarchar](50) NULL,
	[IsMenu] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tblNewsType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRights]    Script Date: 10/27/2019 1:18:48 AM ******/
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
/****** Object:  Table [dbo].[tblRole]    Script Date: 10/27/2019 1:18:48 AM ******/
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
/****** Object:  Table [dbo].[tblUser]    Script Date: 10/27/2019 1:18:48 AM ******/
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
USE [master]
GO
ALTER DATABASE [NewsPortal] SET  READ_WRITE 
GO
