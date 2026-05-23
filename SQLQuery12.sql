CREATE OR ALTER PROCEDURE sp_InsertPreparat
    @Nume NVARCHAR(100),
    @Descriere NVARCHAR(500),
    @Pret DECIMAL(18, 2),
    @CategorieId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- AICI era problema: scria 'Preparat' în loc de 'Preparate'
    INSERT INTO [dbo].[Preparate] (Nume, Descriere, Pret, CategorieId)
    VALUES (@Nume, @Descriere, @Pret, @CategorieId);
    
    SELECT SCOPE_IDENTITY() AS NewID;
END