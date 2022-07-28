/****** CREATE DATABASE ******/
USE [master]
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SurveyQuestionsConfigurator')
BEGIN
CREATE DATABASE [SurveyQuestionsConfigurator]
 CONTAINMENT = NONE
END
GO


/****** CREATE TABLES ******/
USE [SurveyQuestionsConfigurator]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Questions]'))
BEGIN
CREATE TABLE [dbo].[Questions]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Order] [int] NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Questions_1] PRIMARY KEY ([ID] ASC),
 CONSTRAINT [UniqueOrder_Questions] UNIQUE ([Order] ASC)
)

ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [CK_Questions] CHECK  ((len([Text])<=(4000) AND ([Type]=(0) OR [Type]=(1) OR [Type]=(2))))

ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [CK_Questions]

END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Smiley_Questions]'))
BEGIN
CREATE TABLE [dbo].[Smiley_Questions]
(
	[ID] [int] NOT NULL,
	[NumberOfSmileyFaces] [int] NOT NULL,
 CONSTRAINT [IX_Smiley_Questions] UNIQUE NONCLUSTERED ([ID] ASC)
) 

ALTER TABLE [dbo].[Smiley_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Smiley_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[Smiley_Questions]  ADD  CONSTRAINT [CK_Smiley_Questions] CHECK  (([NumberOfSmileyFaces]>=(1) AND [NumberOfSmileyFaces]<=(5)))
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Slider_Questions]'))
BEGIN
CREATE TABLE [dbo].[Slider_Questions]
(
	[ID] [int] NOT NULL,
	[StartValue] [int] NOT NULL,
	[EndValue] [int] NOT NULL,
	[StartValueCaption] [nvarchar](100) NOT NULL,
	[EndValueCaption] [nvarchar](100) NOT NULL,
 CONSTRAINT [IX_Slider_Questions] UNIQUE NONCLUSTERED ([ID] ASC)
)

ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Slider_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[Slider_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Slider_Questions] CHECK  (([StartValue]>=(1) AND [EndValue]<=(100) AND [StartValue]<[EndValue] AND len([StartValueCaption])<=(100) AND len([EndValueCaption])<=(100)))
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Star_Questions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Star_Questions]
(
	[ID] [int] NOT NULL,
	[NumberOfStars] [int] NOT NULL,
 CONSTRAINT [IX_Star_Questions] UNIQUE NONCLUSTERED ([ID] ASC)
)

ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Star_Questions_Questions] FOREIGN KEY([ID])
REFERENCES [dbo].[Questions] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[Star_Questions]  WITH CHECK ADD  CONSTRAINT [CK_Star_Questions] CHECK  (([NumberOfStars]>=(1) AND [NumberOfStars]<=(10)))
END
GO


/****** CREATE STORED PROCEDURES ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_SMILEY_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[INSERT_SMILEY_QUESTION]
END
GO
CREATE PROCEDURE [dbo].[INSERT_SMILEY_QUESTION]
@ORDER INT,
@TEXT NVARCHAR(4000),
@TYPE INT,
@NumberOfSmileyFaces INT
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

    INSERT INTO Questions 
    ([Order], [Text], [Type])
    VALUES 
    (@ORDER, @TEXT, @TYPE)
	
	IF(SCOPE_IDENTITY() IS NOT NULL)
	BEGIN
	INSERT INTO Smiley_Questions(ID, NumberOfSmileyFaces)
	VALUES (SCOPE_IDENTITY(), @NumberOfSmileyFaces)
	
	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS
	END
END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_SLIDER_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[INSERT_SLIDER_QUESTION]
END
GO
CREATE PROCEDURE [dbo].[INSERT_SLIDER_QUESTION]
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@StartValue INT,
@EndValue INT,
@StartValueCaption nvarchar(100),
@EndValueCaption nvarchar(100)
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	INSERT INTO Questions 
	([Order], [Text], [Type])
	VALUES 
	(@Order, @Text, @Type)
	
	INSERT INTO Slider_Questions
	(ID, StartValue, EndValue, StartValueCaption, EndValueCaption)
	VALUES (SCOPE_IDENTITY(), @StartValue, @EndValue, @StartValueCaption, @EndValueCaption)
	
	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_STAR_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[INSERT_STAR_QUESTION]
END
GO
CREATE PROCEDURE [dbo].[INSERT_STAR_QUESTION]
@ORDER INT,
@TEXT NVARCHAR(4000),
@TYPE INT,
@NumberOfStars INT
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

    INSERT INTO Questions 
    ([Order], [Text], [Type])
    VALUES 
    (@ORDER, @TEXT, @TYPE)
	
	INSERT INTO Star_Questions(ID,  NumberOfStars)
	VALUES (SCOPE_IDENTITY(), @NumberOfStars)
	
	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS
END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_SMILEY_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[UPDATE_SMILEY_QUESTION]
END
GO
CREATE PROCEDURE [dbo].[UPDATE_SMILEY_QUESTION]
@ID INT,
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@NumberOfSmileyFaces INT
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	UPDATE Questions
	SET [Order] = @Order, [Text] = @Text
	WHERE [ID] = @ID

	UPDATE Smiley_Questions
	SET NumberOfSmileyFaces = @NumberOfSmileyFaces
	WHERE ID = @ID

	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_SLIDER_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[UPDATE_SLIDER_QUESTION]
END
GO
CREATE PROCEDURE [dbo].[UPDATE_SLIDER_QUESTION]
@ID INT,
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@StartValue INT,
@EndValue INT,
@StartValueCaption nvarchar(100),
@EndValueCaption nvarchar(100)

AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	UPDATE Questions
	SET [Order] = @Order, [Text] = @Text
	WHERE [ID] = @ID

	UPDATE Slider_Questions
    SET StartValue = @StartValue, EndValue = @EndValue, StartValueCaption = @StartValueCaption, EndValueCaption = @EndValueCaption
	WHERE ID = @ID

	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_STAR_QUESTION]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[UPDATE_STAR_QUESTION]
END
GO
CREATE PROCEDURE [dbo].[UPDATE_STAR_QUESTION]
@ID INT,
@Order INT,
@Text NVARCHAR(4000),
@Type INT,
@NumberOfStars INT
AS
SET NOCOUNT ON

BEGIN TRY
	DECLARE @SUCCESS INT, @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	UPDATE Questions
	SET [Order] = @Order, [Text] = @Text
	WHERE [ID] = @ID

	UPDATE Star_Questions
	SET NumberOfStars = @NumberOfStars
	WHERE ID = @ID

	SELECT @SUCCESS AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @SUCCESS

END TRY
BEGIN CATCH
	SELECT @ERROR AS ErrorCode, ERROR_MESSAGE() AS ErrorMessage
	RETURN @ERROR
END CATCH
GO


/****** CREATE FUNCTIONS ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckIfOrderExist]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
DROP FUNCTION [dbo].[CheckIfOrderExist]
END
GO
CREATE FUNCTION [dbo].[CheckIfOrderExist] (@ORDER INT)
RETURNS INT
AS
BEGIN
	DECLARE @SUCCESS INT, @SQL_VIOLATION INT, @ID INT
	SET @SUCCESS = 1
	SET @SQL_VIOLATION = 2


	IF((SELECT COUNT(ID) FROM Questions WHERE [Order] = @ORDER) = 0)
		RETURN @SUCCESS

	RETURN @SQL_VIOLATION
END;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckIfUpdatingSameQuestion]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
DROP FUNCTION [dbo].[CheckIfUpdatingSameQuestion]
END
GO
CREATE FUNCTION [dbo].[CheckIfUpdatingSameQuestion] (@ORDER INT, @ID INT)
RETURNS INT
AS
BEGIN
	DECLARE @SUCCESS INT
	DECLARE @ERROR INT
	SET @SUCCESS = 1
	SET @ERROR = -1

	DECLARE @ReturnedID INT
	SET @ReturnedID = (SELECT [ID] FROM Questions WHERE [Order] = @ORDER)
	IF(@ReturnedID = @ID)
		RETURN @SUCCESS

	RETURN @ERROR
END;
GO