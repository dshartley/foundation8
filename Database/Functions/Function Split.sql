USE [f30-data]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 16/07/2020 10:53:27 ******/
DROP FUNCTION [dbo].[Split]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 16/07/2020 10:53:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE FUNCTION [dbo].[Split](

    @String nvarchar(4000),
	@Delimiter nvarchar(1)

) RETURNS @Result table (Item nvarchar(4000))

BEGIN

	DECLARE @Item nvarchar(100)

	WHILE (CHARINDEX(@Delimiter, @String, 0) <> 0)
	BEGIN

		SELECT	@Item = RTRIM(
						LTRIM(
						SUBSTRING(@String, 1, CHARINDEX(@Delimiter, @String, 0)-1))), 
				@String =	RTRIM(
							LTRIM(
							SUBSTRING(@String, CHARINDEX(@Delimiter, @String, 0) + LEN(@Delimiter), LEN(@String))))

		IF (LEN(@Item) > 0)
		BEGIN
			INSERT INTO @Result SELECT @Item
		END

	END

	-- Add the last item
	IF (LEN(@String) > 0)
	BEGIN
		INSERT INTO @Result SELECT @String
	END

	RETURN

END



GO


