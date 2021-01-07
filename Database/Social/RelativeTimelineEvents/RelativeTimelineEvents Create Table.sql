USE [f23-data]
GO

DROP TABLE [dbo].[RelativeTimelineEvents]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RelativeTimelineEvents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ForRelativeMemberID] [nvarchar](30) NOT NULL,
	[RelativeInteractionID] [nvarchar](30) NOT NULL,
	[EventType] [int] NOT NULL,
	[DateActioned] [datetime] NOT NULL,
	[EventStatus] [int] NOT NULL,
	[ApplicationID] [nvarchar](30) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_RelativeTimelineEvents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


