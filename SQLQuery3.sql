CREATE PROCEDURE sp_GetPreparate
AS
BEGIN
    SELECT * FROM Preparate;
END
GO

CREATE PROCEDURE sp_UpdateStoc
    @Id INT,
    @CantitateComandata INT
AS
BEGIN
    UPDATE Preparate 
    SET CantitateTotala = CantitateTotala - @CantitateComandata
    WHERE Id = @Id;
END
GO