USE master;
GO

IF EXISTS(select * from sys.databases where name='npcampgroundTest')
DROP DATABASE [npcampgroundTest];
GO

CREATE DATABASE [npcampgroundTest];
GO

USE [npcampgroundTest]
GO
/****** Object:  Table [dbo].[campground]    Script Date: 10/25/2019 4:39:29 PM ******/
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
/****** Object:  Table [dbo].[park]    Script Date: 10/25/2019 4:39:29 PM ******/
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
/****** Object:  Table [dbo].[reservation]    Script Date: 10/25/2019 4:39:29 PM ******/
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
/****** Object:  Table [dbo].[RoleItem]    Script Date: 10/25/2019 4:39:29 PM ******/
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
/****** Object:  Table [dbo].[site]    Script Date: 10/25/2019 4:39:29 PM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 10/25/2019 4:39:29 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_User_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserReservation]    Script Date: 10/25/2019 4:39:29 PM ******/
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
