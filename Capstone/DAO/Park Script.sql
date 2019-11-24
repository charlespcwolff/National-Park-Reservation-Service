-- Switch to the system (aka master) database
USE master;
GO

-- Delete the [npcampground] Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='npcampground')
DROP DATABASE [npcampground];
GO

/****** Object:  Database [npcampground]    Script Date: 10/23/2019 4:05:34 PM ******/
CREATE DATABASE [npcampground]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'npcampground', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\npcampground.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'npcampground_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\npcampground_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [npcampground] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [npcampground].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [npcampground] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [npcampground] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [npcampground] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [npcampground] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [npcampground] SET ARITHABORT OFF 
GO
ALTER DATABASE [npcampground] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [npcampground] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [npcampground] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [npcampground] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [npcampground] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [npcampground] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [npcampground] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [npcampground] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [npcampground] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [npcampground] SET  ENABLE_BROKER 
GO
ALTER DATABASE [npcampground] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [npcampground] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [npcampground] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [npcampground] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [npcampground] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [npcampground] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [npcampground] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [npcampground] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [npcampground] SET  MULTI_USER 
GO
ALTER DATABASE [npcampground] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [npcampground] SET DB_CHAINING OFF 
GO
ALTER DATABASE [npcampground] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [npcampground] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [npcampground] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [npcampground] SET QUERY_STORE = OFF
GO
USE [npcampground]
GO
/****** Object:  Table [dbo].[campground]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[campground](
	[campground_id] [int] IDENTITY(1,1) NOT NULL,
	[park_id] [int] NOT NULL,
	[name] [varchar](80) NOT NULL,
	[open_from_mm] [int] NOT NULL,
	[open_to_mm] [int] NOT NULL,
	[daily_fee] [money] NOT NULL,
 CONSTRAINT [pk_campground_campground_id] PRIMARY KEY CLUSTERED 
(
	[campground_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[park]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[park](
	[park_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](80) NOT NULL,
	[location] [varchar](50) NOT NULL,
	[establish_date] [date] NOT NULL,
	[area] [int] NOT NULL,
	[visitors] [int] NOT NULL,
	[description] [varchar](500) NOT NULL,
 CONSTRAINT [pk_park_park_id] PRIMARY KEY CLUSTERED 
(
	[park_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[reservation]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reservation](
	[reservation_id] [int] IDENTITY(1,1) NOT NULL,
	[site_id] [int] NOT NULL,
	[name] [varchar](80) NOT NULL,
	[from_date] [date] NOT NULL,
	[to_date] [date] NOT NULL,
	[create_date] [datetime] NULL,
 CONSTRAINT [pk_reservation_reservation_id] PRIMARY KEY CLUSTERED 
(
	[reservation_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleItem]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleItem](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[site]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site](
	[site_id] [int] IDENTITY(1,1) NOT NULL,
	[campground_id] [int] NOT NULL,
	[site_number] [int] NOT NULL,
	[max_occupancy] [int] NOT NULL,
	[accessible] [bit] NOT NULL,
	[max_rv_length] [int] NOT NULL,
	[utilities] [bit] NOT NULL,
 CONSTRAINT [pk_site_site_number_campground_id] PRIMARY KEY CLUSTERED 
(
	[site_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Salt] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VendUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserReservation]    Script Date: 10/23/2019 4:05:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserReservation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ReservationId] [int] NOT NULL,
 CONSTRAINT [PK_UserReservation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[campground] ON 
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (1, 1, N'Blackwoods', 1, 12, 35.0000)
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (2, 1, N'Seawall', 5, 9, 30.0000)
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (3, 1, N'Schoodic Woods', 5, 10, 30.0000)
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (4, 2, N'Devil''s Garden', 1, 12, 25.0000)
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (5, 2, N'Canyon Wren Group Site', 1, 12, 160.0000)
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (6, 2, N'Juniper Group Site', 1, 12, 250.0000)
GO
INSERT [dbo].[campground] ([campground_id], [park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) VALUES (7, 3, N'The Unnamed Primitive Campsites', 5, 11, 20.0000)
GO
SET IDENTITY_INSERT [dbo].[campground] OFF
GO
SET IDENTITY_INSERT [dbo].[park] ON 
GO
INSERT [dbo].[park] ([park_id], [name], [location], [establish_date], [area], [visitors], [description]) VALUES (1, N'Acadia', N'Maine', CAST(N'1919-02-26' AS Date), 47389, 2563129, N'Covering most of Mount Desert Island and other coastal islands, Acadia features the tallest mountain on the Atlantic coast of the United States, granite peaks, ocean shoreline, woodlands, and lakes. There are freshwater, estuary, forest, and intertidal habitats.')
GO
INSERT [dbo].[park] ([park_id], [name], [location], [establish_date], [area], [visitors], [description]) VALUES (2, N'Arches', N'Utah', CAST(N'1929-04-12' AS Date), 76518, 1284767, N'This site features more than 2,000 natural sandstone arches, including the famous Delicate Arch. In a desert climate, millions of years of erosion have led to these structures, and the arid ground has life-sustaining soil crust and potholes, which serve as natural water-collecting basins. Other geologic formations are stone columns, spires, fins, and towers.')
GO
INSERT [dbo].[park] ([park_id], [name], [location], [establish_date], [area], [visitors], [description]) VALUES (3, N'Cuyahoga Valley', N'Ohio', CAST(N'2000-10-11' AS Date), 32860, 2189849, N'This park along the Cuyahoga River has waterfalls, hills, trails, and exhibits on early rural living. The Ohio and Erie Canal Towpath Trail follows the Ohio and Erie Canal, where mules towed canal boats. The park has numerous historic homes, bridges, and structures, and also offers a scenic train ride.')
GO
SET IDENTITY_INSERT [dbo].[park] OFF
GO
SET IDENTITY_INSERT [dbo].[reservation] ON 
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (1, 1, N'Smith Family Reservation', CAST(N'2019-10-21' AS Date), CAST(N'2019-10-25' AS Date), CAST(N'2019-10-23T11:02:43.357' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (2, 1, N'Lockhart Family Reservation', CAST(N'2019-10-17' AS Date), CAST(N'2019-10-20' AS Date), CAST(N'2019-10-23T11:02:43.357' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (3, 2, N'Jones Reservation', CAST(N'2019-10-21' AS Date), CAST(N'2019-10-25' AS Date), CAST(N'2019-10-23T11:02:43.357' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (4, 3, N'Bauer Family Reservation', CAST(N'2019-10-23' AS Date), CAST(N'2019-10-25' AS Date), CAST(N'2019-10-23T11:02:43.357' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (5, 4, N'Eagles Family Reservation', CAST(N'2019-10-28' AS Date), CAST(N'2019-11-02' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (6, 7, N'Aersomith Reservation', CAST(N'2019-10-20' AS Date), CAST(N'2019-10-25' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (7, 9, N'Claus Family Reservation', CAST(N'2019-10-23' AS Date), CAST(N'2019-10-24' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (8, 9, N'Taylor Family Reservation', CAST(N'2019-10-16' AS Date), CAST(N'2019-10-18' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (9, 10, N'Astley Family Reservation', CAST(N'2019-11-06' AS Date), CAST(N'2019-11-13' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (10, 13, N'Jobs Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (11, 14, N'Cook Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (12, 15, N'Gates Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (13, 16, N'Cue Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (14, 17, N'Ive Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (15, 18, N'Federighi Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (16, 19, N'Schiller Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (17, 20, N'Williams Family Reservation', CAST(N'2019-10-24' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (18, 20, N'Kawasaki Family Reservation', CAST(N'2019-11-02' AS Date), CAST(N'2019-11-13' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (19, 20, N'Branson Family Reservation', CAST(N'2019-11-14' AS Date), CAST(N'2019-11-20' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (20, 20, N'Zukerberg Family Reservation', CAST(N'2019-11-22' AS Date), CAST(N'2019-11-25' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (21, 24, N'Musk Family Reservation', CAST(N'2019-10-27' AS Date), CAST(N'2019-11-02' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (22, 24, N'Buffett Family Reservation', CAST(N'2019-10-19' AS Date), CAST(N'2019-10-23' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (23, 25, N'Satya Nedella', CAST(N'2019-10-26' AS Date), CAST(N'2019-11-02' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (24, 25, N'Scott Gutherie', CAST(N'2019-11-02' AS Date), CAST(N'2019-11-06' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (25, 28, N'Amy Hood', CAST(N'2019-10-28' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (26, 29, N'Peggy Johnson', CAST(N'2019-10-28' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (27, 31, N'Terry Myerson', CAST(N'2019-11-01' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (28, 32, N'Steve Ballmer', CAST(N'2019-11-05' AS Date), CAST(N'2019-11-07' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (29, 32, N'Gates Family Reservation', CAST(N'2019-11-08' AS Date), CAST(N'2019-11-11' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (30, 37, N'Marisa Mayer', CAST(N'2019-10-08' AS Date), CAST(N'2019-10-13' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (31, 37, N'Beth Mooney', CAST(N'2019-10-23' AS Date), CAST(N'2019-10-27' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (32, 40, N'William Priemer', CAST(N'2019-10-25' AS Date), CAST(N'2019-10-29' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (33, 42, N'Tricia Griffith', CAST(N'2019-10-26' AS Date), CAST(N'2019-10-31' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (34, 43, N'Toby Cosgrove', CAST(N'2019-10-28' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (35, 43, N'Akram Boutros', CAST(N'2019-11-04' AS Date), CAST(N'2019-11-10' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (36, 44, N'Barbara Snyder', CAST(N'2019-11-01' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (37, 45, N'Bill Board', CAST(N'2019-10-14' AS Date), CAST(N'2019-10-24' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (38, 45, N'Bill Loney', CAST(N'2019-10-24' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (39, 45, N'Cara Van', CAST(N'2019-11-09' AS Date), CAST(N'2019-11-13' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (40, 45, N'Forrest Gump', CAST(N'2019-11-23' AS Date), CAST(N'2019-11-29' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (41, 46, N'Simpson Family', CAST(N'2019-10-17' AS Date), CAST(N'2019-10-24' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (42, 46, N'Smith Family', CAST(N'2019-10-25' AS Date), CAST(N'2019-11-03' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (43, 46, N'Leela Family', CAST(N'2019-11-06' AS Date), CAST(N'2019-11-07' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (44, 46, N'Scott Family', CAST(N'2019-11-27' AS Date), CAST(N'2019-12-02' AS Date), CAST(N'2019-10-23T11:02:43.360' AS DateTime))
GO
INSERT [dbo].[reservation] ([reservation_id], [site_id], [name], [from_date], [to_date], [create_date]) VALUES (48, 7, N'Christopher', CAST(N'2020-09-23' AS Date), CAST(N'2021-09-23' AS Date), CAST(N'2019-10-23T20:04:13.220' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[reservation] OFF
GO
INSERT [dbo].[RoleItem] ([Id], [Name]) VALUES (1, N'Administrator')
GO
INSERT [dbo].[RoleItem] ([Id], [Name]) VALUES (2, N'StandardUser')
GO
SET IDENTITY_INSERT [dbo].[site] ON 
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (1, 1, 1, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (2, 1, 2, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (3, 1, 3, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (4, 1, 4, 6, 0, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (5, 1, 5, 6, 1, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (6, 1, 6, 6, 1, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (7, 1, 7, 6, 0, 20, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (8, 1, 8, 6, 0, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (9, 1, 9, 6, 1, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (10, 1, 10, 6, 0, 35, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (11, 1, 11, 6, 0, 35, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (12, 1, 12, 6, 1, 35, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (13, 2, 1, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (14, 2, 2, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (15, 2, 3, 6, 0, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (16, 2, 4, 6, 1, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (17, 2, 5, 6, 1, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (18, 2, 6, 6, 1, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (19, 2, 7, 6, 0, 20, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (20, 2, 8, 6, 0, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (21, 2, 9, 6, 1, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (22, 2, 10, 6, 1, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (23, 2, 11, 6, 0, 35, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (24, 2, 12, 6, 1, 35, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (25, 3, 1, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (26, 3, 2, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (27, 3, 3, 6, 0, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (28, 3, 4, 6, 1, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (29, 3, 5, 6, 0, 20, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (30, 3, 6, 6, 0, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (31, 3, 7, 6, 0, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (32, 3, 8, 6, 1, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (33, 3, 9, 6, 0, 35, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (34, 3, 10, 6, 0, 35, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (35, 3, 11, 6, 1, 35, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (36, 3, 12, 6, 1, 35, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (37, 4, 1, 10, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (38, 4, 2, 10, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (39, 4, 3, 10, 0, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (40, 4, 4, 10, 0, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (41, 4, 5, 10, 1, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (42, 4, 6, 10, 1, 0, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (43, 4, 7, 7, 0, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (44, 4, 8, 7, 0, 20, 1)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (45, 5, 1, 35, 1, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (46, 6, 1, 55, 1, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (47, 7, 1, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (48, 7, 2, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (49, 7, 3, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (50, 7, 4, 6, 0, 0, 0)
GO
INSERT [dbo].[site] ([site_id], [campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) VALUES (51, 7, 5, 6, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[site] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Username], [Email], [Hash], [RoleId], [Salt]) VALUES (1, N'Christopher', N'Rupp', N'cjr', N'chris@techelevator.com', N'/mn0B+0TG+lKEu/6b/ZKvoJlVFk=', 2, N'h2vIMfAwmgJpgu9q6IqCJw==')
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserReservation] ON 
GO
INSERT [dbo].[UserReservation] ([Id], [UserId], [ReservationId]) VALUES (1, 1, 48)
GO
SET IDENTITY_INSERT [dbo].[UserReservation] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_User_Username]    Script Date: 10/23/2019 4:05:35 PM ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [UC_User_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[reservation] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((6)) FOR [max_occupancy]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((0)) FOR [accessible]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((0)) FOR [max_rv_length]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((0)) FOR [utilities]
GO
ALTER TABLE [dbo].[campground]  WITH CHECK ADD FOREIGN KEY([park_id])
REFERENCES [dbo].[park] ([park_id])
GO
ALTER TABLE [dbo].[reservation]  WITH CHECK ADD  CONSTRAINT [FK__site_reservation] FOREIGN KEY([site_id])
REFERENCES [dbo].[site] ([site_id])
GO
ALTER TABLE [dbo].[reservation] CHECK CONSTRAINT [FK__site_reservation]
GO
ALTER TABLE [dbo].[site]  WITH CHECK ADD FOREIGN KEY([campground_id])
REFERENCES [dbo].[campground] ([campground_id])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleItem] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserReservation]  WITH CHECK ADD  CONSTRAINT [FK_UserReservation_Reservation] FOREIGN KEY([ReservationId])
REFERENCES [dbo].[reservation] ([reservation_id])
GO
ALTER TABLE [dbo].[UserReservation] CHECK CONSTRAINT [FK_UserReservation_Reservation]
GO
ALTER TABLE [dbo].[UserReservation]  WITH CHECK ADD  CONSTRAINT [FK_UserReservation_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserReservation] CHECK CONSTRAINT [FK_UserReservation_User]
GO
USE [master]
GO
ALTER DATABASE [npcampground] SET  READ_WRITE 
GO
USE [npcampground]
GO