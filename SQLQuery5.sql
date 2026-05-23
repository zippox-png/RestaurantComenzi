CREATE PROCEDURE sp_GetAllAlergeni
AS
BEGIN
    SELECT * FROM Alergeni;
END
GO

CREATE PROCEDURE sp_InsertAlergen
    @Denumire NVARCHAR(100)
AS
BEGIN
    INSERT INTO Alergeni (Denumire) VALUES (@Denumire);
END
GO