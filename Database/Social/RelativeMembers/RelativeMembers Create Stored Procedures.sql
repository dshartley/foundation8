USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersInsert]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeMembersInsert]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@UserProfileID nvarchar(30),
	@Email nvarchar(256),
	@FullName nvarchar(30),
	@AvatarImageFileName nvarchar(50)
)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [f23-data].[dbo].[RelativeMembers]
		([ApplicationID],
		[UserProfileID],
		[Email],
		[FullName],
		[AvatarImageFileName])
    VALUES
        (@ApplicationID,
		@UserProfileID,
		@Email,
		@FullName,
		@AvatarImageFileName)

	Set @ID=Scope_Identity()

END


GO


USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeMembersUpdate]
(
	@ID int output,
	@ApplicationID nvarchar(30),
	@UserProfileID nvarchar(30),
	@Email nvarchar(256),
	@FullName nvarchar(30),
	@AvatarImageFileName nvarchar(50)
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [f23-data].[dbo].[RelativeMembers]
	SET [ApplicationID] = @ApplicationID,
		[UserProfileID] = @UserProfileID,
		[Email] = @Email,
		[FullName] = @FullName,
		[AvatarImageFileName] = @AvatarImageFileName
	WHERE ID = @ID

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersDelete]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeMembersDelete]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM [f23-data].[dbo].[RelativeMembers]
	WHERE ID = @ID

END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersSelect]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeMembersSelect]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm

END


GO
USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByID]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[ID] = @ID
					
END



GO
USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByAspectsPreviousRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByAspectsPreviousRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@AspectTypes nvarchar(30),
	@MaxResults int,
	@CurrentRelativeMemberID nvarchar(30),
	@ScopeType int,
	@PreviousRelativeMemberID nvarchar(30),
	@NumberOfItemsToLoad int,
	@SelectItemsAfterPreviousYN bit
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;


	-- Create a table variable for the RelativeMemberQueryAspectTypes
	DECLARE @RMQAT TABLE(AspectType int)
		

	-- Insert RelativeMemberQueryAspectTypes into @RMQAT from @AspectTypes
	IF (LEN(@AspectTypes) > 0)
	BEGIN

		INSERT INTO @RMQAT
		SELECT AspectType FROM fnSplitRelativeMemberQueryAspectTypes(@AspectTypes)
	
	END


	-- Create a table variable for the RelativeMembers
	DECLARE @RM TABLE	(RowID INT IDENTITY,
						ID int, 
						ConnectionContractTypes nvarchar(250),
						IsFriendYN bit,
						IsFollowerYN bit,
						IsFollowedYN bit,
						IsHandshakeGiverYN bit,
						IsHandshakeReceiverYN bit,
						IsTransientYN bit,
						DateLastActive datetime,
						RowNumber int)


	-- Insert RelativeMembers into @RM by AspectTypes

	DECLARE @ConnectionContractType int

	IF (@ScopeType = 8)	-- Transient
	BEGIN

		SET @ConnectionContractType = 5	-- Transient

		INSERT INTO @RM
		SELECT [RelativeMemberID] AS 'ID',
				'',			-- ConnectionContractTypes
				0,			-- IsFriendYN
				0,			-- IsFollowerYN
				0,			-- IsFollowedYN
				0,			-- IsHandshakeGiverYN
				0,			-- IsHandshakeReceiverYN
				0,			-- IsTransientYN
				'1/1/1900',	-- DateLastActive
				0			-- RowNumber
		FROM [dbo].[fnRelativeMembersSelectByRelationTo] (@ApplicationID, @CurrentRelativeMemberID, @ConnectionContractType)

	END
	ELSE
	BEGIN

		INSERT INTO @RM
		SELECT	rm.[ID],
				'',			-- ConnectionContractTypes
				0,			-- IsFriendYN
				0,			-- IsFollowerYN
				0,			-- IsFollowedYN
				0,			-- IsHandshakeGiverYN
				0,			-- IsHandshakeReceiverYN
				0,			-- IsTransientYN
				'1/1/1900',	-- DateLastActive
				0			-- RowNumber
		FROM	RelativeMembers rm
		WHERE	rm.[ApplicationID] = @ApplicationID

	END


	-- Delete duplicates from @RM
	DELETE	rm
	FROM	@RM rm
	WHERE	[RowID] IN (SELECT a.[RowID] FROM @RM a, 
						(SELECT	[RowID], 
								(SELECT MAX([ID]) FROM @RM i WHERE o.[ID] = i.[ID] GROUP BY [ID] HAVING o.[RowID] = MAX(i.[RowID])) AS MaxValue 
						FROM	@RM o) b
    WHERE	a.[RowID] = b.[RowID] AND b.MaxValue IS NULL)


	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE	(ID int, 
						RelativeMemberID int, 
						ConnectionContractType int,
						DateLastActive datetime,
						FromCurrentRelativeMemberYN bit)
	

	-- Insert RelativeConnections into @RC by ToRelativeMemberID and FromRelativeMemberID
	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[FromRelativeMemberID] as 'RelativeMemberID', 
			rc.[ConnectionContractType],
			rc.[DateLastActive],
			0	-- FromCurrentRelativeMemberYN = 0
	FROM	RelativeConnections rc
	WHERE	rc.[ToRelativeMemberID] = @CurrentRelativeMemberID
			AND rc.[FromRelativeMemberID] IN (SELECT rm_temp.[ID] FROM @RM rm_temp)
			AND rc.[ApplicationID] = @ApplicationID
	UNION
	SELECT	rc.[ID], 
			rc.[ToRelativeMemberID] as 'RelativeMemberID', 
			rc.[ConnectionContractType],
			rc.[DateLastActive],
			1	-- FromCurrentRelativeMemberYN = 1
	FROM	RelativeConnections rc
	WHERE	rc.[FromRelativeMemberID] = @CurrentRelativeMemberID
			AND rc.[ToRelativeMemberID] IN (SELECT rm_temp.[ID] FROM @RM rm_temp)
			AND rc.[ApplicationID] = @ApplicationID


	DECLARE @IsFriendYN bit = 0
	DECLARE @IsFollowerYN bit = 0
	DECLARE @IsFollowedYN bit = 0
	DECLARE @IsHandshakeGiverYN bit = 0
	DECLARE @IsHandshakeReceiverYN bit = 0
	DECLARE @IsTransientYN bit = 0



	-- Use a cursor to iterate through RelativeConnections in @RC
	DECLARE @RCID int
	DECLARE @RelativeMemberID int
	DECLARE @FromCurrentRelativeMemberYN bit
	DECLARE @DateLastActive datetime	
			
	DECLARE rcCursor CURSOR FOR 
		SELECT	ID,
				RelativeMemberID,
				ConnectionContractType,
				DateLastActive,
				FromCurrentRelativeMemberYN
		FROM	@RC

	OPEN rcCursor

	FETCH NEXT FROM rcCursor 
	INTO @RCID, @RelativeMemberID, @ConnectionContractType, @DateLastActive, @FromCurrentRelativeMemberYN
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @IsFriendYN = 0
		SET @IsFollowerYN = 0
		SET @IsFollowedYN = 0
		SET @IsHandshakeGiverYN = 0
		SET @IsHandshakeReceiverYN = 0
		SET @IsTransientYN = 0

		IF (@ConnectionContractType = 1)				-- Friend
		BEGIN
			SET @IsFriendYN = 1
		END
		ELSE IF (@ConnectionContractType = 2)			-- Follow	
		BEGIN

			IF (@FromCurrentRelativeMemberYN = 0)
			BEGIN
				SET @IsFollowerYN = 1					-- Follower
			END
			ELSE
			BEGIN
				SET @IsFollowedYN = 1					-- Followed
			END
			
		END
		ELSE IF (@ConnectionContractType = 3)			-- Handshake
		BEGIN

			IF (@FromCurrentRelativeMemberYN = 0)
			BEGIN
				SET @IsHandshakeGiverYN = 1				-- IsHandshakeGiverYN
			END
			ELSE
			BEGIN
				SET @IsHandshakeReceiverYN = 1			-- IsHandshakeReceiverYN
			END

		END
		ELSE IF (@ConnectionContractType = 5)			-- Transient
		BEGIN
			SET @IsTransientYN = 1
		END


		-- Update values in RelativeMember
		UPDATE	@RM 
		SET		[IsFriendYN] = @IsFriendYN,
				[IsFollowerYN] = @IsFollowerYN,
				[IsFollowedYN] = @IsFollowedYN,
				[IsHandshakeGiverYN] = @IsHandshakeGiverYN,
				[IsHandshakeReceiverYN] = @IsHandshakeReceiverYN,
				[IsTransientYN] = @IsTransientYN,
				[DateLastActive] = @DateLastActive
		WHERE	[ID] = @RelativeMemberID

		
		FETCH NEXT FROM rcCursor 
		INTO @RCID, @RelativeMemberID, @ConnectionContractType, @DateLastActive, @FromCurrentRelativeMemberYN
	END 

	CLOSE rcCursor
	DEALLOCATE rcCursor


	-- Delete RelativeMembers in @RM that are not within the specified scope
	IF (@ScopeType = 2)	-- Friends
	BEGIN

		DELETE FROM @RM
		WHERE [IsFriendYN] = 0

	END
	ELSE IF (@ScopeType = 3)	-- Followers
	BEGIN

		DELETE FROM @RM
		WHERE [IsFollowerYN] = 0

	END
	ELSE IF (@ScopeType = 4)	-- Followed
	BEGIN

		DELETE FROM @RM
		WHERE [IsFollowedYN] = 0

	END
	-- ELSE IF (@ScopeType = 5)	-- Contractless		// Not required
	ELSE IF (@ScopeType = 6)	-- HandshakeGiver
	BEGIN

		DELETE FROM @RM
		WHERE [IsHandshakeGiverYN] = 0

	END
	ELSE IF (@ScopeType = 7)	-- HandshakeReceiver
	BEGIN

		DELETE FROM @RM
		WHERE [IsHandshakeReceiverYN] = 0

	END
	ELSE IF (@ScopeType = 8)	-- Transient
	BEGIN

		DELETE FROM @RM
		WHERE [IsTransientYN] = 0

	END


	DECLARE @StartRowNumber int = 1
	DECLARE @RowNumber int = 0

	-- Use a cursor to iterate through RelativeMembers in @RM
	DECLARE @RMID int
			
	DECLARE rmCursor CURSOR FOR 
		SELECT	ID
		FROM	@RM rm
		ORDER BY rm.[DateLastActive] DESC
	OPEN rmCursor

	FETCH NEXT FROM rmCursor 
	INTO @RMID
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Increment @RowNumber
		SET @RowNumber = @RowNumber + 1

		-- Check if @RMID is previous ID then the query should start at next row
		IF (@RMID = @PreviousRelativeMemberID)
		BEGIN
			SET @StartRowNumber = @RowNumber + 1
		END

		-- Update values in RelativeMember
		UPDATE @RM 
		SET [ConnectionContractTypes] = 
			ISNULL(SUBSTRING(
				(SELECT ',' + CONVERT(NVARCHAR(1), rc_temp.[ConnectionContractType])
				FROM @RC rc_temp
				WHERE rc_temp.[RelativeMemberID] = @RMID
				FOR XML PATH ('')),
			2, 250), ''),
			[RowNumber] = @RowNumber
		WHERE [ID] = @RMID
		
		FETCH NEXT FROM rmCursor 
		INTO @RMID
	END 

	CLOSE rmCursor
	DEALLOCATE rmCursor


	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName],
			rm_temp.[ConnectionContractTypes],
			rm_temp.[RowNumber]
	FROM	@RM rm_temp
			JOIN [f23-data].[dbo].[RelativeMembers] rm ON rm_temp.[ID] = rm.[ID]
	WHERE	rm_temp.[RowNumber] >= @StartRowNumber
			AND rm_temp.[RowNumber] < (@StartRowNumber + @NumberOfItemsToLoad)
	ORDER BY rm_temp.[RowNumber] ASC
			

END



GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByEmail]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByEmail]
(
	@ApplicationID nvarchar(30),
	@Email nvarchar(30)
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	LOWER(rm.[Email]) = LOWER(@Email)
			AND rm.[ApplicationID] = @ApplicationID
					
END



GO

USE [f23-data]
GO


DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByEmailConnectionContractType]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByEmailConnectionContractType]
(
	@ApplicationID nvarchar(30),
	@Email nvarchar(30),
	@ConnectionContractType int,
	@CurrentRelativeMemberID nvarchar(30)
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	/* Create temporary table to hold relative members */
	CREATE TABLE #RM(ID int)

	-- Get relative members by Email
	INSERT INTO #RM
	SELECT	rm.[ID]
	FROM	RelativeMembers rm
	WHERE	LOWER(rm.[Email]) = LOWER(@Email)
			AND rm.[ApplicationID] = @ApplicationID

	-- Get relative member ID
	DECLARE @ID int = (SELECT TOP(1) rm_temp.[ID] FROM #RM rm_temp)

	/* Create temporary table to hold relative connections */
	CREATE TABLE #RC(ID int, RelativeMemberID int, ConnectionContractType int)

	-- Get relative connections for the relative member
	INSERT INTO #RC
	SELECT	rc.[ID], 
			@ID as 'RelativeMemberID', 
			rc.[ConnectionContractType]
	FROM	RelativeConnections rc
	WHERE	(rc.[FromRelativeMemberID] = @ID AND rc.[ToRelativeMemberID] = @CurrentRelativeMemberID)
			OR (rc.[FromRelativeMemberID] = @CurrentRelativeMemberID AND rc.[ToRelativeMemberID] = @ID)
			AND rc.[ApplicationID] = @ApplicationID


	SELECT	DISTINCT(rm.[ID]),
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	#RM rm_temp
			JOIN [f23-data].[dbo].[RelativeMembers] rm ON rm_temp.[ID] = rm.[ID]
			JOIN #RC rc_temp ON rm_temp.[ID] = rc_temp.[RelativeMemberID]
	WHERE	rc_temp.[ConnectionContractType] = @ConnectionContractType
				
	
	DROP TABLE #RM
	DROP TABLE #RC		
		
		
			
END


GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByFindTextPreviousRelativeMemberID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByFindTextPreviousRelativeMemberID]
(
	@ApplicationID nvarchar(30),
	@FindText nvarchar(max),
	@CurrentRelativeMemberID nvarchar(30),
	@ScopeType int,
	@PreviousRelativeMemberID nvarchar(30),
	@NumberOfItemsToLoad int,
	@SelectItemsAfterPreviousYN bit
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;


	-- Create a table variable for the RelativeMembers
	DECLARE @RM TABLE	(ID int, 
						ConnectionContractTypes nvarchar(250),
						IsFriendYN bit,
						IsFollowerYN bit,
						IsFollowedYN bit,
						RowNumber int)


	-- Insert RelativeMembers into @RM by FindText
	INSERT INTO @RM
	SELECT	rm.[ID], 
			'',
			0,
			0,
			0,
			0
	FROM	RelativeMembers rm
	WHERE	CHARINDEX(LOWER(@FindText) collate Latin1_General_BIN, LOWER(rm.[FullName]) collate Latin1_General_BIN) > 0
			AND rm.[ID] <> @CurrentRelativeMemberID
			AND rm.[ApplicationID] = @ApplicationID
	ORDER BY rm.[FullName] ASC


	-- Create a table variable for the RelativeConnections
	DECLARE @RC TABLE	(ID int, 
						RelativeMemberID int, 
						ConnectionContractType int,
						FromCurrentRelativeMemberYN bit)
	

	-- Insert RelativeConnections into @RC by ToRelativeMemberID and FromRelativeMemberID
	INSERT INTO @RC
	SELECT	rc.[ID],
			rc.[FromRelativeMemberID] as 'RelativeMemberID', 
			rc.[ConnectionContractType],
			0	-- FromCurrentRelativeMemberYN = 0
	FROM	RelativeConnections rc
	WHERE	rc.[ToRelativeMemberID] = @CurrentRelativeMemberID
			AND rc.[FromRelativeMemberID] IN (SELECT rm_temp.[ID] FROM @RM rm_temp)
			AND rc.[ApplicationID] = @ApplicationID
	UNION
	SELECT	rc.[ID], 
			rc.[ToRelativeMemberID] as 'RelativeMemberID', 
			rc.[ConnectionContractType],
			1	-- FromCurrentRelativeMemberYN = 1
	FROM	RelativeConnections rc
	WHERE	rc.[FromRelativeMemberID] = @CurrentRelativeMemberID
			AND rc.[ToRelativeMemberID] IN (SELECT rm_temp.[ID] FROM @RM rm_temp)
			AND rc.[ApplicationID] = @ApplicationID


	DECLARE @IsFriendYN bit = 0
	DECLARE @IsFollowerYN bit = 0
	DECLARE @IsFollowedYN bit = 0
	DECLARE @IsHandshakeYN bit = 0


	-- Use a cursor to iterate through RelativeConnections in @RC
	DECLARE @RCID int
	DECLARE @RelativeMemberID int
	DECLARE @ConnectionContractType int
	DECLARE @FromCurrentRelativeMemberYN bit
			
	DECLARE rcCursor CURSOR FOR 
		SELECT	ID,
				RelativeMemberID,
				ConnectionContractType,
				FromCurrentRelativeMemberYN
		FROM	@RC

	OPEN rcCursor

	FETCH NEXT FROM rcCursor 
	INTO @RCID, @RelativeMemberID, @ConnectionContractType, @FromCurrentRelativeMemberYN
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @IsFriendYN = 0
		SET @IsFollowerYN = 0
		SET @IsFollowedYN = 0
		SET @IsHandshakeYN = 0

		IF (@ConnectionContractType = 1)				-- Friend
		BEGIN
			SET @IsFriendYN = 1
		END
		ELSE IF (@ConnectionContractType = 2)			-- Follow	
		BEGIN

			IF (@FromCurrentRelativeMemberYN = 0)
			BEGIN
				SET @IsFollowerYN = 1	-- Follower
			END
			ELSE
			BEGIN
				SET @IsFollowedYN = 1	-- Followed
			END
			
		END
		ELSE IF (@ConnectionContractType = 3)			-- Handshake
		BEGIN
			SET @IsHandshakeYN = 1
		END


		-- Update values in RelativeMember
		UPDATE	@RM 
		SET		[IsFriendYN] = @IsFriendYN,
				[IsFollowerYN] = @IsFollowerYN,
				[IsFollowedYN] = @IsFollowedYN
		WHERE	[ID] = @RelativeMemberID

		
		FETCH NEXT FROM rcCursor 
		INTO @RCID, @RelativeMemberID, @ConnectionContractType, @FromCurrentRelativeMemberYN
	END 

	CLOSE rcCursor
	DEALLOCATE rcCursor


	-- Delete RelativeMembers in @RM that are not within the specified scope
	IF (@ScopeType = 2)	-- Friends
	BEGIN

		DELETE FROM @RM
		WHERE [IsFriendYN] = 0

	END
	ELSE IF (@ScopeType = 3)	-- Followers
	BEGIN

		DELETE FROM @RM
		WHERE [IsFollowerYN] = 0

	END
	ELSE IF (@ScopeType = 4)	-- Followed
	BEGIN

		DELETE FROM @RM
		WHERE [IsFollowedYN] = 0

	END


	DECLARE @StartRowNumber int = 1
	DECLARE @RowNumber int = 0

	-- Use a cursor to iterate through RelativeMembers in @RM
	DECLARE @RMID int
			
	DECLARE rmCursor CURSOR FOR 
		SELECT	ID
		FROM	@RM

	OPEN rmCursor

	FETCH NEXT FROM rmCursor 
	INTO @RMID
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Increment @RowNumber
		SET @RowNumber = @RowNumber + 1

		-- Check if @RMID is previous ID then the query should start at next row
		IF (@RMID = @PreviousRelativeMemberID)
		BEGIN
			SET @StartRowNumber = @RowNumber + 1
		END

		-- Update values in RelativeMember
		UPDATE @RM 
		SET [ConnectionContractTypes] = 
			ISNULL(SUBSTRING(
				(SELECT ',' + CONVERT(NVARCHAR(1), rc_temp.[ConnectionContractType])
				FROM @RC rc_temp
				WHERE rc_temp.[RelativeMemberID] = @RMID
				FOR XML PATH ('')),
			2, 250), ''),
			[RowNumber] = @RowNumber
		WHERE [ID] = @RMID
		
		FETCH NEXT FROM rmCursor 
		INTO @RMID
	END 

	CLOSE rmCursor
	DEALLOCATE rmCursor


	-- Select RelativeMembers
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName],
			rm_temp.[ConnectionContractTypes],
			rm_temp.[RowNumber]
	FROM	@RM rm_temp
			JOIN [f23-data].[dbo].[RelativeMembers] rm ON rm_temp.[ID] = rm.[ID]
	WHERE	rm_temp.[RowNumber] >= @StartRowNumber
			AND rm_temp.[RowNumber] < (@StartRowNumber + @NumberOfItemsToLoad)
			
			
END



GO

USE [f23-data]
GO


DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByIDConnectionContractType]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByIDConnectionContractType]
(
	@ID int,
	@ConnectionContractType int,
	@CurrentRelativeMemberID nvarchar(30)
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	/* Create temporary table to hold relative members */
	CREATE TABLE #RM(ID int)

	-- Get relative members by ID
	INSERT INTO #RM
	SELECT	rm.[ID]
	FROM	RelativeMembers rm
	WHERE	rm.[ID] = @ID

	/* Create temporary table to hold relative connections */
	CREATE TABLE #RC(ID int, RelativeMemberID int, ConnectionContractType int)

	-- Get relative connections for the relative member
	INSERT INTO #RC
	SELECT	rc.[ID], 
			@ID as 'RelativeMemberID', 
			rc.[ConnectionContractType]
	FROM	RelativeConnections rc
	WHERE	(rc.[FromRelativeMemberID] = @ID AND rc.[ToRelativeMemberID] = @CurrentRelativeMemberID)
			OR (rc.[FromRelativeMemberID] = @CurrentRelativeMemberID AND rc.[ToRelativeMemberID] = @ID)


	SELECT	DISTINCT(rm.[ID]),
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	#RM rm_temp
			JOIN [f23-data].[dbo].[RelativeMembers] rm ON rm_temp.[ID] = rm.[ID]
			JOIN #RC rc_temp ON rm_temp.[ID] = rc_temp.[RelativeMemberID]
	WHERE	rc_temp.[ConnectionContractType] = @ConnectionContractType
				
	
	DROP TABLE #RM
	DROP TABLE #RC
		
END



GO

USE [f23-data]
GO

DROP PROCEDURE [dbo].[sp_RelativeMembersSelectByUserProfileID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_RelativeMembersSelectByUserProfileID]
(
	@ApplicationID nvarchar(30),
	@UserProfileID nvarchar(30)
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;
	
	SELECT	rm.[ID],
			rm.[ApplicationID],
			rm.[UserProfileID],
			rm.[Email],
			rm.[FullName],
			rm.[AvatarImageFileName]
	FROM	[f23-data].[dbo].[RelativeMembers] rm
	WHERE	rm.[UserProfileID] = @UserProfileID
					
END



GO



