USE [mydatabase]
GO

/****** Object:  Table [dbo].[Department]    Script Date: 13-02-2023 23:52:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](500) NULL
) ON [PRIMARY]
GO