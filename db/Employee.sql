USE [mydatabase]
GO

/****** Object:  Table [dbo].[Employee]    Script Date: 13-02-2023 23:54:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeName] [nvarchar](500) NULL,
	[Department] [nvarchar](500) NULL,
	[DateOfJoining] [datetime] NULL,
	[PhotoFileName] [nvarchar](500) NULL
) ON [PRIMARY]
GO