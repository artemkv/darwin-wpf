-- Verify that stored procedure does not exist.
IF OBJECT_ID ( 'dbo.DeleteEntity', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.DeleteEntity;
GO

CREATE PROCEDURE dbo.DeleteEntity
	@Id uniqueidentifier
	, @Ts varbinary(8000)
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
		BEGIN TRAN

		DELETE FROM dbo.DiagramEntity WHERE EntityId = @Id
		DELETE FROM dbo.Entity WHERE Id = @Id AND Ts = @Ts
		
		IF (@@ROWCOUNT = 0)
		BEGIN
		    RAISERROR(N'No rows were deleted.', 16, 1) WITH NOWAIT
		END

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		EXEC RethrowError
	END CATCH
END
GO