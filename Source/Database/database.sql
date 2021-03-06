/*
	Ken Kohler
	Prime Video Search v 1.0
	v 1.0 Apr 2018
	Licensed: MIT license
*/

USE [master]
GO
/****** Object:  Database [AWSPrimeStreaming]    Script Date: 4/22/2018 8:24:13 PM ******/
CREATE DATABASE [AWSPrimeStreaming]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AWSPrimeStreaming', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\AWSPrimeStreaming.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AWSPrimeStreaming_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\AWSPrimeStreaming_log.ldf' , SIZE = 204800KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [AWSPrimeStreaming] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AWSPrimeStreaming].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AWSPrimeStreaming] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET ARITHABORT OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AWSPrimeStreaming] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AWSPrimeStreaming] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AWSPrimeStreaming] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AWSPrimeStreaming] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET RECOVERY FULL 
GO
ALTER DATABASE [AWSPrimeStreaming] SET  MULTI_USER 
GO
ALTER DATABASE [AWSPrimeStreaming] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AWSPrimeStreaming] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AWSPrimeStreaming] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AWSPrimeStreaming] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AWSPrimeStreaming] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'AWSPrimeStreaming', N'ON'
GO
ALTER DATABASE [AWSPrimeStreaming] SET QUERY_STORE = OFF
GO
USE [AWSPrimeStreaming]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [AWSPrimeStreaming]
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genre](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TVSeriesGenre]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TVSeriesGenre](
	[TVSeriesId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_TVSeriesGenre] PRIMARY KEY CLUSTERED 
(
	[TVSeriesId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[TVSeriesGenreCount]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create view [dbo].[TVSeriesGenreCount]
as
select g.[Name], count(g.Id) as Count
from Genre g 
join TVSeriesGenre tg on g.Id = tg.GenreId
group by [Name]
GO
/****** Object:  Table [dbo].[MovieGenre]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieGenre](
	[MovieId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_MovieGenre] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[MovieGenreCount]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create view [dbo].[MovieGenreCount]
as
select g.[Name], count(g.Id) as Count
from Genre g 
join MovieGenre mg on g.Id = mg.GenreId
group by [Name]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movie](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataSin] [varchar](20) NOT NULL,
	[Title] [nvarchar](512) NOT NULL,
	[SearchTitle] [nvarchar](512) NOT NULL,
	[URL] [varchar](512) NOT NULL,
	[Image] [varchar](512) NOT NULL,
	[Rating] [varchar](5) NULL,
	[ClosedCaptioned] [bit] NOT NULL,
	[Released] [int] NOT NULL,
	[RuntimeDisplay] [varchar](20) NULL,
	[Stars] [varchar](20) NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Metadata] [nvarchar](max) NULL,
	[MetadataRetrieved] [bit] NOT NULL,
	[Ratings] [varchar](15) NULL,
	[IMDBRating] [real] NOT NULL,
	[Plot] [nvarchar](max) NULL,
	[Genres] [nvarchar](max) NULL,
	[Director] [nvarchar](100) NULL,
	[Starring] [nvarchar](max) NULL,
	[SupportingActors] [nvarchar](max) NULL,
	[IsPrime] [bit] NOT NULL,
	[Runtime] [int] NOT NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TVSeries]    Script Date: 4/22/2018 8:24:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TVSeries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataSin] [varchar](20) NOT NULL,
	[Title] [nvarchar](512) NOT NULL,
	[SearchTitle] [nvarchar](512) NOT NULL,
	[URL] [varchar](512) NOT NULL,
	[Image] [varchar](512) NOT NULL,
	[ClosedCaptioned] [bit] NOT NULL,
	[Released] [int] NOT NULL,
	[Stars] [varchar](20) NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Ratings] [varchar](15) NULL,
	[IMDBRating] [real] NOT NULL,
	[Plot] [nvarchar](max) NULL,
	[Genres] [nvarchar](max) NULL,
	[Director] [nvarchar](100) NULL,
	[Starring] [nvarchar](max) NULL,
	[SupportingActors] [nvarchar](max) NULL,
	[IsPrime] [bit] NOT NULL,
 CONSTRAINT [PK_TVSeries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Genre] ADD  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Movie] ADD  CONSTRAINT [DF_Movies_Released]  DEFAULT ((0)) FOR [Released]
GO
ALTER TABLE [dbo].[Movie] ADD  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Movie] ADD  DEFAULT (getdate()) FOR [Updated]
GO
ALTER TABLE [dbo].[Movie] ADD  CONSTRAINT [DF_Movies_MetadataRetrieved]  DEFAULT ((0)) FOR [MetadataRetrieved]
GO
ALTER TABLE [dbo].[Movie] ADD  CONSTRAINT [DF_Movie_IMDBRating]  DEFAULT ((0)) FOR [IMDBRating]
GO
ALTER TABLE [dbo].[Movie] ADD  CONSTRAINT [DF_Movie_IsPrime]  DEFAULT ((0)) FOR [IsPrime]
GO
ALTER TABLE [dbo].[Movie] ADD  CONSTRAINT [DF_Movie_Runtime]  DEFAULT ((0)) FOR [Runtime]
GO
ALTER TABLE [dbo].[TVSeries] ADD  CONSTRAINT [DF_TVSeries_Released]  DEFAULT ((0)) FOR [Released]
GO
ALTER TABLE [dbo].[TVSeries] ADD  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[TVSeries] ADD  DEFAULT (getdate()) FOR [Updated]
GO
ALTER TABLE [dbo].[TVSeries] ADD  CONSTRAINT [DF_TVSeries_IMDBRating]  DEFAULT ((0)) FOR [IMDBRating]
GO
ALTER TABLE [dbo].[TVSeries] ADD  CONSTRAINT [DF_TVSeries_IsPrime]  DEFAULT ((0)) FOR [IsPrime]
GO
ALTER TABLE [dbo].[MovieGenre]  WITH CHECK ADD  CONSTRAINT [FK_MovieGenre_Genre] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([Id])
GO
ALTER TABLE [dbo].[MovieGenre] CHECK CONSTRAINT [FK_MovieGenre_Genre]
GO
ALTER TABLE [dbo].[MovieGenre]  WITH CHECK ADD  CONSTRAINT [FK_MovieGenre_Movie] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movie] ([Id])
GO
ALTER TABLE [dbo].[MovieGenre] CHECK CONSTRAINT [FK_MovieGenre_Movie]
GO
ALTER TABLE [dbo].[TVSeriesGenre]  WITH CHECK ADD  CONSTRAINT [FK_TVSeriesGenre_Genre] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([Id])
GO
ALTER TABLE [dbo].[TVSeriesGenre] CHECK CONSTRAINT [FK_TVSeriesGenre_Genre]
GO
ALTER TABLE [dbo].[TVSeriesGenre]  WITH CHECK ADD  CONSTRAINT [FK_TVSeriesGenre_TVSeries] FOREIGN KEY([TVSeriesId])
REFERENCES [dbo].[TVSeries] ([Id])
GO
ALTER TABLE [dbo].[TVSeriesGenre] CHECK CONSTRAINT [FK_TVSeriesGenre_TVSeries]
GO
USE [master]
GO
ALTER DATABASE [AWSPrimeStreaming] SET  READ_WRITE 
GO
