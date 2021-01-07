USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeInteractionsInsert]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeInteractionsInsert]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@InteractionType int,
	@DateActioned datetime,
	@InteractionStatus int,
	@Text nvarchar(max)
)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [f23-data].[dbo].[RelativeInteractions]
		([ApplicationID],
		[FromRelativeMemberID],
		[ToRelativeMemberID],
		[InteractionType],
		[DateActioned],
		[InteractionStatus],
		[Text])
    VALUES
        (@ApplicationID,
		@FromRelativeMemberID,
		@ToRelativeMemberID,
		@InteractionType,
		@DateActioned,
		@InteractionStatus,
		@Text)

	Set @ID=Scope_Identity()


	DECLARE @RelativeInteractionID int = @ID
	DECLARE @RelativeTimelineEventID int
	DECLARE @EventType int = @InteractionType
	DECLARE @EventStatus int = 1

	-- Insert RelativeTimelineEvent for FromRelativeMember
	EXECUTE [f23-data].[dbo].[sp_RelativeTimelineEventsInsert] @RelativeTimelineEventID OUTPUT,
		@ApplicationID,
		@FromRelativeMemberID,
		@RelativeInteractionID,
		@EventType,
		@DateActioned,
		@EventStatus


	-- Note: Expect the sp_RelativeInteractionsBroadcast stored procedure to be called to broadcast the
	-- RelativeInteraction for the ToRelativeMember and other members


	-- Check FromRelativeMemberID is not ToRelativeMemberID
	IF (@FromRelativeMemberID <> @ToRelativeMemberID
		AND LEN(@ToRelativeMemberID) > 0)
	BEGIN

		DECLARE @RelativeConnectionID int

		-- Set transient contract between FromRelativeMember and ToRelativeMember
		EXECUTE [f23-data].[dbo].[sp_RelativeConnectionsSetTransientContract] @RelativeConnectionID OUTPUT,
			@FromRelativeMemberID,
			@ToRelativeMemberID	
		
	END


END



GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeInteractionsUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeInteractionsUpdate]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@FromRelativeMemberID nvarchar(30),
	@ToRelativeMemberID nvarchar(30),
	@InteractionType int,
	@DateActioned datetime,
	@InteractionStatus int,
	@Text nvarchar(max)
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [f23-data].[dbo].[RelativeInteractions]
	SET [ApplicationID] = @ApplicationID,
		[FromRelativeMemberID] = @FromRelativeMemberID,
		[ToRelativeMemberID] = @ToRelativeMemberID,
		[InteractionType] = @InteractionType,
		[DateActioned] = @DateActioned,
		[InteractionStatus] = @InteractionStatus,
		[Text] = @Text
	WHERE ID = @ID

END



GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeInteractionsDelete]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeInteractionsDelete]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM [f23-data].[dbo].[RelativeInteractions]
	WHERE ID = @ID

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeInteractionsSelect]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeInteractionsSelect]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	ri.[ID],
			ri.[ApplicationID],
			ri.[FromRelativeMemberID],
			ri.[ToRelativeMemberID],
			ri.[InteractionType],
			ri.[DateActioned],
			ri.[InteractionStatus],
			ri.[Text]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeInteractionsSelectByID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeInteractionsSelectByID]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	ri.[ID],
			ri.[ApplicationID],
			ri.[FromRelativeMemberID],
			ri.[ToRelativeMemberID],
			ri.[InteractionType],
			ri.[DateActioned],
			ri.[InteractionStatus],
			ri.[Text]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri
	WHERE	ri.[ID] = @ID
					
END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeInteractionsBroadcastOnInsert]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeInteractionsBroadcastOnInsert]
(
	@ID int,
	@RecipientTypes nvarchar(30),
	@DegreesofSeparation int
)
AS
BEGIN

	SET NOCOUNT ON;

	-- Create RelativeTimelineEvents table
	DECLARE @RTLE TABLE(	ID int,
							ApplicationID nvarchar(30),
							ForRelativeMemberID int,
							RelativeInteractionID int,
							EventType int,
							DateActioned datetime,
							EventStatus int)


	-- Create RelativeMembers table
	DECLARE @RM TABLE(	ID int, 
						IncludeInBroadcastYN bit DEFAULT 1)


	-- Create RelativeInteractionBroadcastRecipientTypes table
	DECLARE @RIBRT TABLE(RecipientType int)


	-- RelativeInteraction properties
	DECLARE @ApplicationID nvarchar(30)
	DECLARE @RelativeInteractionID int = -1
	DECLARE @FromRelativeMemberID int
	DECLARE @ToRelativeMemberID int
	DECLARE @InteractionType int
	DECLARE @DateActioned datetime
	DECLARE @InteractionStatus int
	DECLARE @Text nvarchar(max)


	DECLARE @IncludeFromMemberYN bit = 0
	DECLARE @IncludeToMemberYN bit = 0
	DECLARE @ConnectionContractType int


	-- Get the RelativeInteraction
	SELECT	@RelativeInteractionID = ri.[ID],
			@ApplicationID = ri.[ApplicationID],
			@FromRelativeMemberID = ri.[FromRelativeMemberID],
			@ToRelativeMemberID = ri.[ToRelativeMemberID],
			@InteractionType = ri.[InteractionType],
			@DateActioned = ri.[DateActioned],
			@InteractionStatus = ri.[InteractionStatus],
			@Text = ri.[Text]
	FROM	[f23-data].[dbo].[RelativeInteractions] ri
	WHERE	ri.[ID] = @ID

	-- Check RelativeInteraction exists
	IF (@RelativeInteractionID < 0)
	BEGIN

		-- If the RelativeInteraction doesn't exist then return
		SELECT	*
		FROM @RTLE rtle

		RETURN
	END

		

	-- Insert RelativeInteractionBroadcastRecipientTypes into @RIBRT from @RecipientTypes
	IF (LEN(@RecipientTypes) > 0)
	BEGIN

		INSERT INTO @RIBRT
		SELECT RecipientType FROM fnSplitRelativeInteractionBroadcastRecipientTypes(@RecipientTypes)
	
	END

	/*
	    RelativeInteractionBroadcastRecipientTypes:
        FromMember = 1,
        ToMember = 2,
        FriendContractMembers = 3,
        FollowContractMembers = 4,
        HandshakeContractMembers = 5,
        ContractlessContractMembers = 6
	*/

	/* RelativeInteractionTypes:
         Standard = 1,
         PostFollowContractMyFeed = 2,
         PostFriendContractMyFeed = 3,
         PostFriendContractFriendFeed = 4,
         Handshake = 5,
         PostContractlessMyFeed = 6,
         PostContractlessMemberFeed = 7
	*/

	-- FromMember
	IF EXISTS (SELECT * FROM @RIBRT WHERE [RecipientType] = 1) SET @IncludeFromMemberYN = 1
	INSERT INTO @RM VALUES (@FromRelativeMemberID, @IncludeFromMemberYN)

	-- ToMember
	IF EXISTS (SELECT * FROM @RIBRT WHERE [RecipientType] = 2) SET @IncludeToMemberYN = 1
	INSERT INTO @RM VALUES (@ToRelativeMemberID, @IncludeToMemberYN)


	-- FriendContractMembers
	IF EXISTS (SELECT * FROM @RIBRT WHERE [RecipientType] = 3)
	BEGIN

		SET @ConnectionContractType = 1	-- Friend

		/* InteractionTypes;
           -- PostFriendContractMyFeed = 3,
		   -- PostFriendContractFriendFeed = 4,
		*/

		-- Get friends of @FromRelativeMemberID
		INSERT INTO @RM
		SELECT	[RelativeMemberID], 
				1 -- IncludeInBroadcastYN
		FROM	[dbo].[fnRelativeMembersSelectByRelationToForDegreesofSeparation] (
			@ApplicationID, @FromRelativeMemberID, @ConnectionContractType, @DegreesofSeparation)

		IF (@InteractionType = 4)	-- PostFriendContractFriendFeed
		BEGIN

			-- Get friends of @ToRelativeMemberID
			INSERT INTO @RM
			SELECT	[RelativeMemberID],
					1 -- IncludeInBroadcastYN
			FROM	[dbo].[fnRelativeMembersSelectByRelationToForDegreesofSeparation] (
				@ApplicationID, @ToRelativeMemberID, @ConnectionContractType, @DegreesofSeparation)

		END
  
	END

	-- FollowContractMembers
	IF EXISTS (SELECT * FROM @RIBRT WHERE [RecipientType] = 4)
	BEGIN

		SET @ConnectionContractType = 2	-- Follow

		/* InteractionTypes;
           -- PostFollowContractMyFeed = 2
		*/

		-- Get followers of @FromRelativeMemberID
		INSERT INTO @RM
		SELECT	[RelativeMemberID],
				1 -- IncludeInBroadcastYN
		FROM	[dbo].[fnRelativeMembersSelectByRelationToForDegreesofSeparation] (
			@ApplicationID, @FromRelativeMemberID, @ConnectionContractType, @DegreesofSeparation)

	END

	-- HandshakeContractMembers
	IF EXISTS (SELECT * FROM @RIBRT WHERE [RecipientType] = 5)
	BEGIN

		-- TODO:
		PRINT('TODO')
	END

	-- ContractlessContractMembers
	IF EXISTS (SELECT * FROM @RIBRT WHERE [RecipientType] = 6)
	BEGIN

		/* RelativeInteractionTypes:
		   - PostContractlessMyFeed = 6,
		   - PostContractlessMemberFeed = 7
		*/

		-- Get contractless of @FromRelativeMemberID
		INSERT INTO @RM
		SELECT	[RelativeMemberID],
				1 -- IncludeInBroadcastYN
		FROM	[dbo].[fnRelativeMembersSelectByRelationToForDegreesofSeparation] (
			@ApplicationID, @FromRelativeMemberID, @ConnectionContractType, @DegreesofSeparation)

		IF (@InteractionType = 7)	-- PostContractlessMemberFeed
		BEGIN

			-- Get contractless of @ToRelativeMemberID
			INSERT INTO @RM
			SELECT	[RelativeMemberID],
					1 -- IncludeInBroadcastYN
			FROM	[dbo].[fnRelativeMembersSelectByRelationToForDegreesofSeparation] (
				@ApplicationID, @ToRelativeMemberID, @ConnectionContractType, @DegreesofSeparation)

		END

	END


	DECLARE @RelativeMemberID int
	DECLARE @RelativeTimelineEventID int
	DECLARE @ForRelativeMemberID int
	DECLARE @EventType int = @InteractionType
	DECLARE @EventStatus int = 1


	-- Go through each item
	DECLARE relativeMembersCursor CURSOR FOR 
		SELECT	DISTINCT(ID)
		FROM	@RM rm
		WHERE	rm.[IncludeInBroadcastYN] = 1

	OPEN relativeMembersCursor

	FETCH NEXT FROM relativeMembersCursor 
	INTO @RelativeMemberID
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Set @ForRelativeMemberID
		SET @ForRelativeMemberID = @RelativeMemberID

		-- Insert RelativeTimelineEvent
		EXECUTE [f23-data].[dbo].[sp_RelativeTimelineEventsInsert] @RelativeTimelineEventID OUTPUT,
			@ApplicationID,
			@ForRelativeMemberID,
			@RelativeInteractionID,
			@EventType,
			@DateActioned,
			@EventStatus

		-- Insert RelativeTimelineEvent
		INSERT INTO @RTLE
		VALUES (@RelativeTimelineEventID,
				@ApplicationID,
				@ForRelativeMemberID,
				@RelativeInteractionID,
				@EventType,
				@DateActioned,
				@EventStatus)

		FETCH NEXT FROM relativeMembersCursor 
		INTO @RelativeMemberID
	END 

	CLOSE relativeMembersCursor
	DEALLOCATE relativeMembersCursor


	-- Select RelativeTimelineEvents
	SELECT	*
	FROM @RTLE rtle

	-- Select RelativeInteraction
	SELECT	@RelativeInteractionID AS 'ID',
			@ApplicationID AS 'ApplicationID',
			@FromRelativeMemberID AS 'FromRelativeMemberID',
			@ToRelativeMemberID AS 'ToRelativeMemberID',
			@InteractionType AS 'InteractionType',
			@DateActioned AS 'DateActioned',
			@InteractionStatus AS 'InteractionStatus',
			@Text AS 'Text'

	-- Select RelativeMembers
	SELECT	rm1.[ID],
			rm2.[ApplicationID],
			rm2.[UserProfileID],
			rm2.[FullName],
			rm2.[Email],
			rm2.[AvatarImageFileName]
	FROM @RM rm1
	JOIN [dbo].[RelativeMembers] rm2 ON rm1.[ID] = rm2.[ID]


END



GO


