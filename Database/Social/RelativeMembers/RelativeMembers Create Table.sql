USE [f23-data]
GO

DROP TABLE [dbo].[RelativeMembers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RelativeMembers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserProfileID] [nvarchar](30) NOT NULL,
	[FullName] [nvarchar](30) NOT NULL,
	[Email] [nvarchar](256) NOT NULL DEFAULT (''),
	[AvatarImageFileName] [nvarchar](50) NOT NULL DEFAULT (''),
	[ApplicationID] [nvarchar](30) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_RelativeMembers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


