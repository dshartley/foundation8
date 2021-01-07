USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsInsert]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsInsert]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@RequestType int,
	@DateActioned datetime,
	@RequestStatus int
)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [f23-data].[dbo].[RelativeConnectionRequests]
		([ApplicationID],
		[FromRelativeMemberID],
		[ToRelativeMemberID],
		[RequestType],
		[DateActioned],
		[RequestStatus])
    VALUES
		(@ApplicationID,
		@FromRelativeMemberID,
		@ToRelativeMemberID,
		@RequestType,
		@DateActioned,
		@RequestStatus)

	Set @ID=Scope_Identity()

END



GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsUpdate]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@RequestType int,
	@DateActioned datetime,
	@RequestStatus int
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [f23-data].[dbo].[RelativeConnectionRequests]
	SET [ApplicationID] = @ApplicationID,
		[FromRelativeMemberID] = @FromRelativeMemberID,
		[ToRelativeMemberID] = @ToRelativeMemberID,
		[RequestType] = @RequestType,
		[DateActioned] = @DateActioned,
		[RequestStatus] = @RequestStatus
	WHERE ID = @ID

END

GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsDelete]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsDelete]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM [f23-data].[dbo].[RelativeConnectionRequests]
	WHERE ID = @ID

END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelect]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelect]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	rcr.[ID],
			rcr.[ApplicationID],
			rcr.[FromRelativeMemberID],
			rcr.[ToRelativeMemberID],
			rcr.[RequestType],
			rcr.[DateActioned],
			rcr.[RequestStatus]
	FROM	[f23-data].[dbo].[RelativeConnectionRequests] rcr

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByID]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByID]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	rcr.[ID],
			rcr.[ApplicationID],
			rcr.[FromRelativeMemberID],
			rcr.[ToRelativeMemberID],
			rcr.[RequestType],
			rcr.[DateActioned],
			rcr.[RequestStatus]
	FROM	[f23-data].[dbo].[RelativeConnectionRequests] rcr
	WHERE	rcr.[ID] = @ID
					
END



GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByFromRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByFromRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@RequestType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
				
	-- Create a table variable for the RelativeConnectionRequests
	DECLARE @RCR TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		RequestType int,
		DateActioned datetime,
		RequestStatus int)

	INSERT INTO @RCR
	SELECT	rcr.[ID],
			rcr.[ApplicationID],
			rcr.[FromRelativeMemberID],
			rcr.[ToRelativeMemberID],
			rcr.[RequestType],
			rcr.[DateActioned],
			rcr.[RequestStatus]
	FROM	[f23-data].[dbo].[RelativeConnectionRequests] rcr
	WHERE	rcr.[FromRelativeMemberID] = @FromRelativeMemberID
			AND rcr.[RequestType] = @RequestType
			AND rcr.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnectionRequests
	SELECT * FROM @RCR
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rcr.[FromRelativeMemberID] FROM @RCR rcr
						UNION
						SELECT rcr.[ToRelativeMemberID] FROM @RCR rcr)				
				
	
END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByFromRelativeMemberIDToRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByFromRelativeMemberIDToRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@RequestType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	-- Create a table variable for the RelativeConnectionRequests
	DECLARE @RCR TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		RequestType int,
		DateActioned datetime,
		RequestStatus int)

	INSERT INTO @RCR
	SELECT	rcr.[ID],
			rcr.[ApplicationID],
			rcr.[FromRelativeMemberID],
			rcr.[ToRelativeMemberID],
			rcr.[RequestType],
			rcr.[DateActioned],
			rcr.[RequestStatus]
	FROM	[f23-data].[dbo].[RelativeConnectionRequests] rcr
	WHERE	rcr.[FromRelativeMemberID] = @FromRelativeMemberID
			AND rcr.[ToRelativeMemberID] = @ToRelativeMemberID
			AND rcr.[RequestType] = @RequestType
			AND rcr.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnectionRequests
	SELECT * FROM @RCR
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rcr.[FromRelativeMemberID] FROM @RCR rcr
						UNION
						SELECT rcr.[ToRelativeMemberID] FROM @RCR rcr)
								
END


GO

USE [f23-data]
GO


DROP PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByToRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionRequestsSelectByToRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@RequestType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	-- Create a table variable for the RelativeConnectionRequests
	DECLARE @RCR TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		RequestType int,
		DateActioned datetime,
		RequestStatus int)

	INSERT INTO @RCR
	SELECT	rcr.[ID],
			rcr.[ApplicationID],
			rcr.[FromRelativeMemberID],
			rcr.[ToRelativeMemberID],
			rcr.[RequestType],
			rcr.[DateActioned],
			rcr.[RequestStatus]
	FROM	[f23-data].[dbo].[RelativeConnectionRequests] rcr
	WHERE	rcr.[ToRelativeMemberID] = @ToRelativeMemberID
			AND rcr.[RequestType] = @RequestType
			AND rcr.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnectionRequests
	SELECT * FROM @RCR
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rcr.[FromRelativeMemberID] FROM @RCR rcr
						UNION
						SELECT rcr.[ToRelativeMemberID] FROM @RCR rcr)

		
END


GO


