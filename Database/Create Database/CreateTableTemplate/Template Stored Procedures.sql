USE [f30-data]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TemplatesDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_TemplatesDelete]
GO

USE [f30-data]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_TemplatesDelete]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM [f30-data].[dbo].[Templates]
	WHERE ID = @ID

END



GO


USE [f30-data]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TemplatesInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_TemplatesInsert]
GO

USE [f30-data]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_TemplatesInsert]
(
	@ID int output,
    @Key nvarchar(100)
)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [f30-data].[dbo].[Templates]
           ([Key])
     VALUES
           (@Key
			)

	Set @ID=Scope_Identity()

END



GO
USE [f30-data]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TemplatesSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_TemplatesSelect]
GO

USE [f30-data]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_TemplatesSelect]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	a.[ID],
			a.[Key]
	FROM	[f30-data].[dbo].[Templates] a
			ORDER BY a.[ID] DESC

END




GO

USE [f30-data]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TemplatesSelectByID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_TemplatesSelectByID]
GO

USE [f30-data]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_TemplatesSelectByID]
(
	@ID int
)
AS
BEGIN

	SET NOCOUNT ON;

	SET DATEFORMAT dmy;

	SELECT	a.[ID],
			a.[Key]
	FROM	[f30-data].[dbo].[Templates] a
	WHERE	a.[ID] = @ID
	ORDER BY a.[ID] DESC
	
							
END


GO
USE [f30-data]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_TemplatesUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_TemplatesUpdate]
GO

USE [f30-data]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_TemplatesUpdate]
(
	@ID int output,
    @Key nvarchar(100)
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [f30-data].[dbo].[Templates]
	SET		[Key] = @Key
	WHERE ID = @ID

END

GO
