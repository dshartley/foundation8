USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsInsert]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsInsert]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@ForRelativeMemberID nvarchar(30),
    @RelativeInteractionID nvarchar(30),
    @EventType int,
    @DateActioned datetime,
    @EventStatus int
)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [f23-data].[dbo].[RelativeTimelineEvents]
		([ApplicationID],
		[ForRelativeMemberID],
        [RelativeInteractionID],
        [EventType],
        [DateActioned],
        [EventStatus])
    VALUES
		(@ApplicationID,
		@ForRelativeMemberID,
        @RelativeInteractionID,
        @EventType,
        @DateActioned,
        @EventStatus)

	Set @ID=Scope_Identity()

END



GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsUpdate]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@ForRelativeMemberID nvarchar(30),
    @RelativeInteractionID nvarchar(30),
    @EventType int,
    @DateActioned datetime,
    @EventStatus int
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [f23-data].[dbo].[RelativeTimelineEvents]
	SET [ApplicationID] = @ApplicationID,
		[ForRelativeMemberID] = @ForRelativeMemberID,
        [RelativeInteractionID] = @RelativeInteractionID,
        [EventType] = @EventType,
        [DateActioned] = @DateActioned,
        [EventStatus] = @EventStatus
	WHERE ID = @ID

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsDelete]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsDelete]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM [f23-data].[dbo].[RelativeTimelineEvents]
	WHERE ID = @ID

END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsSelect]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsSelect]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	rtle.[ID],
			rtle.[ApplicationID],
	        rtle.[ForRelativeMemberID],
			rtle.[RelativeInteractionID],
			rtle.[EventType],
			rtle.[DateActioned],
			rtle.[EventStatus]
	FROM	[f23-data].[dbo].[RelativeTimelineEvents] rtle

END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsSelectByID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsSelectByID]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	rtle.[ID],
			rtle.[ApplicationID],
		    rtle.[ForRelativeMemberID],
			rtle.[RelativeInteractionID],
			rtle.[EventType],
			rtle.[DateActioned],
			rtle.[EventStatus]
	FROM	[f23-data].[dbo].[RelativeTimelineEvents] rtle
	WHERE	rtle.[ID] = @ID
					
END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsSelectByForRelativeMemberIDPreviousRelativeTimelineEventID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsSelectByForRelativeMemberIDPreviousRelativeTimelineEventID]
(
	@ApplicationID nvarchar(30),
	@ForRelativeMemberID nvarchar(30),
	@CurrentRelativeMemberID nvarchar(30),
	@ScopeType int,
	@RelativeTimelineEventTypes nvarchar(30),
	@PreviousRelativeTimelineEventID nvarchar(30),
	@NumberOfItemsToLoad int,
	@SelectItemsAfterPreviousYN bit
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;		
		

	-- Create a table variable for the RelativeMembers
	DECLARE @RM TABLE(ID int)


	-- Create a table variable for the RelativeTimelineEventTypes
	DECLARE @RTLET TABLE(EventType int)
		

	-- Insert RelativeTimelineEventTypes into @RTLET from @RelativeTimelineEventTypes
	IF (LEN(@RelativeTimelineEventTypes) > 0)
	BEGIN

		INSERT INTO @RTLET
		SELECT EventType FROM fnSplitRelativeTimelineEventTypes(@RelativeTimelineEventTypes)
	
	END

	-- Create a table variable for the RelativeTimelineEvents
	DECLARE @RTLE TABLE(
		ID int,
		ApplicationID nvarchar(30),
		ForRelativeMemberID nvarchar(30),
		RelativeInteractionID nvarchar(30),
		EventType int,
		DateActioned datetime,
		EventStatus int,
		RowNumber int)


	-- Insert RelativeTimelineEvents into @RTLE

	INSERT INTO @RTLE
	SELECT	rtle.[ID],
			rtle.[ApplicationID],
			rtle.[ForRelativeMemberID],
			rtle.[RelativeInteractionID],
			rtle.[EventType],
			rtle.[DateActioned],
			rtle.[EventStatus],
			0
	FROM	[f23-data].[dbo].[RelativeTimelineEvents] rtle
	WHERE	rtle.[ForRelativeMemberID] = @ForRelativeMemberID
			AND rtle.[ApplicationID] = @ApplicationID
	ORDER BY
			CASE WHEN @SelectItemsAfterPreviousYN = 1 THEN [DateActioned] ELSE '' END DESC,
			CASE WHEN @SelectItemsAfterPreviousYN = 0 THEN [DateActioned] ELSE '' END ASC,
			rtle.[ID] DESC


	-- Delete RelativeTimelineEvents in @RTLE that are not within the specified RelativeTimelineEventTypes
	IF ((SELECT COUNT(*) FROM @RTLET) > 0)
	BEGIN

		DELETE FROM @RTLE
		WHERE [EventType] NOT IN (SELECT [EventType] FROM @RTLET)

	END


	-- TODO:

	-- Delete RelativeTimelineEvents in @RTLE that are not within the specified scope
	--IF (@ScopeType = 2)		-- Friends
	--BEGIN

	--	DELETE FROM @RTLE
	--	WHERE [IsFriendYN] = 0

	--END


	DECLARE @StartRowNumber int = 1
	DECLARE @RowNumber int = 0

	-- Use a cursor to iterate through RelativeTimelineEvents in @RTLE
	DECLARE @RTLEID int
			
	DECLARE rtleCursor CURSOR FOR 
		SELECT	ID
		FROM	@RTLE

	OPEN rtleCursor

	FETCH NEXT FROM rtleCursor 
	INTO @RTLEID
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Increment @RowNumber
		SET @RowNumber = @RowNumber + 1

		-- Check if @RTLEID is previous ID then the query should start at next row
		IF (@RTLEID = @PreviousRelativeTimelineEventID)
		BEGIN
			SET @StartRowNumber = @RowNumber + 1
		END

		-- Update values in RelativeTimelineEvent
		UPDATE @RTLE 
		SET [RowNumber] = @RowNumber
		WHERE [ID] = @RTLEID
		
		FETCH NEXT FROM rtleCursor 
		INTO @RTLEID
	END 

	CLOSE rtleCursor
	DEALLOCATE rtleCursor


	-- Delete RelativeTimelineEvents in @RTLE that are not within the specified row numbers
	DELETE FROM @RTLE
	WHERE	[RowNumber] < @StartRowNumber
			OR [RowNumber] >= (@StartRowNumber + @NumberOfItemsToLoad)


	-- Create a table variable for the RelativeInteractions
	DECLARE @RI TABLE(
		ID int,
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30))


	-- Insert RelativeInteractions into @RI
	INSERT INTO @RI
	SELECT	ri.[ID],
			ri.[FromRelativeMemberID],
			ri.[ToRelativeMemberID]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri
	WHERE	ri.[ID] IN (SELECT rtle.[RelativeInteractionID] FROM @RTLE rtle)


	-- Insert RelativeMembers into @RM
	INSERT INTO @RM
	SELECT	ri.[FromRelativeMemberID] as 'ID'
	FROM	@RI ri

	INSERT INTO @RM
	SELECT	ri.[ToRelativeMemberID] as 'ID'
	FROM	@RI ri


	-- Select RelativeTimelineEvents
	SELECT * FROM @RTLE


	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT DISTINCT rm1.[ID] FROM @RM rm1)		


	-- Select RelativeInteractions
	SELECT	ri.[ID],
			ri.[ApplicationID],
			ri.[FromRelativeMemberID],
			ri.[ToRelativeMemberID],
			ri.[InteractionType],
			ri.[DateActioned],
			ri.[InteractionStatus],
			ri.[Text]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri
	WHERE	ri.[ID] IN (SELECT rtle.[RelativeInteractionID] FROM @RTLE rtle)	
		
		
	-- Check ForRelativeMemberID is not CurrentRelativeMemberID
	IF (@ForRelativeMemberID <> @CurrentRelativeMemberID
		AND LEN(@ForRelativeMemberID) > 0)
	BEGIN

		DECLARE @RelativeConnectionID int

		-- Set transient contract between CurrentRelativeMember and ForRelativeMember 
		EXECUTE [f23-data].[dbo].[sp_RelativeConnectionsSetTransientContract] @RelativeConnectionID OUTPUT,
			@CurrentRelativeMemberID,
			@ForRelativeMemberID	
		
	END
						
END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeTimelineEventsSelectByRelativeInteractionID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeTimelineEventsSelectByRelativeInteractionID]
(
	@ApplicationID nvarchar(30),
	@RelativeInteractionID nvarchar(30)
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;		
		

	-- Create a table variable for the RelativeMembers
	DECLARE @RM TABLE(ID int)


	-- Create a table variable for the RelativeTimelineEvents
	DECLARE @RTLE TABLE(
		ID int,
		ApplicationID nvarchar(30),
		ForRelativeMemberID nvarchar(30),
		RelativeInteractionID nvarchar(30),
		EventType int,
		DateActioned datetime,
		EventStatus int,
		RowNumber int)


	-- Insert RelativeTimelineEvents into @RTLE
	INSERT INTO @RTLE
	SELECT	rtle.[ID],
			rtle.[ApplicationID],
		    rtle.[ForRelativeMemberID],
			rtle.[RelativeInteractionID],
			rtle.[EventType],
			rtle.[DateActioned],
			rtle.[EventStatus],
			0
	FROM	[f23-data].[dbo].[RelativeTimelineEvents] rtle
	WHERE	rtle.[RelativeInteractionID] = @RelativeInteractionID
			AND rtle.[ApplicationID] = @ApplicationID


	-- Create a table variable for the RelativeInteractions
	DECLARE @RI TABLE(
		ID int,
		FromRelativeMemberID nvarchar(30),
		ToRelativeMemberID nvarchar(30))


	-- Insert RelativeInteractions into @RI
	INSERT INTO @RI
	SELECT	ri.[ID],
			ri.[FromRelativeMemberID],
			ri.[ToRelativeMemberID]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri
	WHERE	ri.[ID] = @RelativeInteractionID


	-- Insert RelativeMembers into @RM
	INSERT INTO @RM
	SELECT	ri.[FromRelativeMemberID] as 'ID'
	FROM	@RI ri

	INSERT INTO @RM
	SELECT	ri.[ToRelativeMemberID] as 'ID'
	FROM	@RI ri


	-- Select RelativeTimelineEvents
	SELECT * FROM @RTLE
		

	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] IN (SELECT DISTINCT rm1.[ID] FROM @RM rm1)			


	-- Select RelativeInteractions
	SELECT	ri.[ID],
			ri.[ApplicationID],
			ri.[FromRelativeMemberID],
			ri.[ToRelativeMemberID],
			ri.[InteractionType],
			ri.[DateActioned],
			ri.[InteractionStatus],
			ri.[Text]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri
	WHERE	ri.[ID] = @RelativeInteractionID	
						
END


GO


