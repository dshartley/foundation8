USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsInsert]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionsInsert]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@ConnectionContractType int,
	@DateActioned datetime,
	@DateLastActive datetime,
	@ConnectionStatus int
)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [f23-data].[dbo].[RelativeConnections]
		([ApplicationID],
		[FromRelativeMemberID],
		[ToRelativeMemberID],
		[ConnectionContractType],
		[DateActioned],
		[DateLastActive],
		[ConnectionStatus])
    VALUES
        (@ApplicationID,
		@FromRelativeMemberID,
		@ToRelativeMemberID,
		@ConnectionContractType,
		@DateActioned,
		@DateLastActive,
		@ConnectionStatus)

	Set @ID=Scope_Identity()

END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionsUpdate]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@ConnectionContractType int,
	@DateActioned datetime,
	@DateLastActive datetime,
	@ConnectionStatus int
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [f23-data].[dbo].[RelativeConnections]
	SET [ApplicationID] = @ApplicationID,
		[FromRelativeMemberID] = @FromRelativeMemberID,
		[ToRelativeMemberID] = @ToRelativeMemberID,
		[ConnectionContractType] = @ConnectionContractType,
		[DateActioned] = @DateActioned,
		[DateLastActive] = @DateLastActive,
		[ConnectionStatus] = @ConnectionStatus
	WHERE ID = @ID

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsDelete]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionsDelete]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM [f23-data].[dbo].[RelativeConnections]
	WHERE ID = @ID

END


GO


USE [f23-data]
GO


DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelect]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelect]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelectByID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelectByID]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	rc.[ID] = @ID
					
END



GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelectByForRelativeMemberIDWithRelativeMemberID]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelectByForRelativeMemberIDWithRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@ForRelativeMemberID nvarchar(30),
	@WithRelativeMemberID nvarchar(30),
	@ConnectionContractType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		ConnectionContractType int,
		DateActioned datetime,
		DateLastActive datetime,
		ConnectionStatus int)

	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	((rc.[FromRelativeMemberID] = @ForRelativeMemberID
				AND rc.[ToRelativeMemberID] = @WithRelativeMemberID)
				OR (rc.[FromRelativeMemberID] = @WithRelativeMemberID
				AND rc.[ToRelativeMemberID] = @ForRelativeMemberID))
			AND rc.[ConnectionContractType] = @ConnectionContractType
			AND rc.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnections
	SELECT * FROM @RC
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rc.[FromRelativeMemberID] FROM @RC rc
						UNION
						SELECT rc.[ToRelativeMemberID] FROM @RC rc)
				
END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelectByFromRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelectByFromRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ConnectionContractType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;

	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		ConnectionContractType int,
		DateActioned datetime,
		DateLastActive datetime,
		ConnectionStatus int)

	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	rc.[FromRelativeMemberID] = @FromRelativeMemberID
			AND rc.[ConnectionContractType] = @ConnectionContractType
			AND rc.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnections
	SELECT * FROM @RC
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rc.[FromRelativeMemberID] FROM @RC rc
						UNION
						SELECT rc.[ToRelativeMemberID] FROM @RC rc)

			
END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelectByFromRelativeMemberIDToRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelectByFromRelativeMemberIDToRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@ConnectionContractType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;		
				
	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		ConnectionContractType int,
		DateActioned datetime,
		DateLastActive datetime,
		ConnectionStatus int)

	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	rc.[FromRelativeMemberID] = @FromRelativeMemberID
			AND rc.[ToRelativeMemberID] = @ToRelativeMemberID
			AND rc.[ConnectionContractType] = @ConnectionContractType
			AND rc.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnections
	SELECT * FROM @RC
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rc.[FromRelativeMemberID] FROM @RC rc
						UNION
						SELECT rc.[ToRelativeMemberID] FROM @RC rc)			
				
				
END



GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelectByToRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelectByToRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@ConnectionContractType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;

	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		ConnectionContractType int,
		DateActioned datetime,
		DateLastActive datetime,
		ConnectionStatus int)

	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	rc.[ToRelativeMemberID] = @ToRelativeMemberID
			AND rc.[ConnectionContractType] = @ConnectionContractType
			AND rc.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnections
	SELECT * FROM @RC
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rc.[FromRelativeMemberID] FROM @RC rc
						UNION
						SELECT rc.[ToRelativeMemberID] FROM @RC rc)

				
END



GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSelectByWithRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSelectByWithRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@WithRelativeMemberID nvarchar(30),
	@ConnectionContractType int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;	
			
	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE(
		ID int,
		ApplicationID nvarchar(30),
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30),
		ConnectionContractType int,
		DateActioned datetime,
		DateLastActive datetime,
		ConnectionStatus int)

	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[ApplicationID],
			rc.[FromRelativeMemberID],
			rc.[ToRelativeMemberID],
			rc.[ConnectionContractType],
			rc.[DateActioned],
			rc.[DateLastActive],
			rc.[ConnectionStatus]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	(rc.[FromRelativeMemberID] = @WithRelativeMemberID
				OR rc.[ToRelativeMemberID] = @WithRelativeMemberID)
			AND rc.[ConnectionContractType] = @ConnectionContractType
			AND rc.[ApplicationID] = @ApplicationID
	
	-- Select RelativeConnections
	SELECT * FROM @RC
		
	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT rc.[FromRelativeMemberID] FROM @RC rc
						UNION
						SELECT rc.[ToRelativeMemberID] FROM @RC rc)
									
END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeConnectionsSetTransientContract]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeConnectionsSetTransientContract]
(
	@ID int output,
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30)
)
AS
BEGIN

	SET NOCOUNT ON;


	DECLARE @RelativeConnectionID int = -1
	DECLARE @ConnectionContractType int = 5		-- Transient
	DECLARE @ApplicationID nvarchar(30)

	-- Get RelativeConnection for transient contract
	SELECT	TOP(1) 
			@RelativeConnectionID = rc.[ID], 
			@ApplicationID = rc.[ApplicationID]
	FROM	[f23-data].[dbo].[RelativeConnections] rc
	WHERE	rc.[FromRelativeMemberID] = @FromRelativeMemberID
			AND rc.[ToRelativeMemberID] = @ToRelativeMemberID
			AND rc.[ConnectionContractType] = @ConnectionContractType

	SET @RelativeConnectionID = ISNULL(@RelativeConnectionID, -1)

	IF (@RelativeConnectionID > -1)
	BEGIN

		-- Update RelativeConnection
		UPDATE [f23-data].[dbo].[RelativeConnections]
		SET [DateLastActive] = GETDATE()
		WHERE ID = @RelativeConnectionID

	END
	ELSE
	BEGIN
		
		DECLARE @DateActioned datetime = GETDATE()
		DECLARE @DateLastActive datetime = @DateActioned
		DECLARE @ConnectionStatus int = 1

		-- Get ApplicationID for FromRelativeMember
		SELECT	TOP(1) 
				@ApplicationID = rm.[ApplicationID]
		FROM	[f23-data].[dbo].[RelativeMembers] rm
		WHERE	rm.[ID] = @FromRelativeMemberID
		SET @ApplicationID = ISNULL(@ApplicationID, '')

		-- Insert RelativeConnection
		EXECUTE [f23-data].[dbo].[sp_RelativeConnectionsInsert] @RelativeConnectionID OUTPUT,
			@ApplicationID,
			@FromRelativeMemberID,
			@ToRelativeMemberID,
			@ConnectionContractType,
			@DateActioned,
			@DateLastActive,
			@ConnectionStatus

		SET @ID = @RelativeConnectionID

	END



END



GO
