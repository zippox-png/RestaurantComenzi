CREATE PROCEDURE sp_InsertComanda
    @UtilizatorId INT,
    @PretTotal DECIMAL(10,2),
    @CostTransport DECIMAL(10,2),
    @OraEstimativa DATETIME
AS
BEGIN
    INSERT INTO Comenzi (UtilizatorId, PretTotal, CostTransport, OraEstimativaLivrare)
    VALUES (@UtilizatorId, @PretTotal, @CostTransport, @OraEstimativa);
    SELECT SCOPE_IDENTITY(); -- Returneaza ID-ul pentru a-l folosi la detalii
END
GO

CREATE PROCEDURE sp_GetComenziClient
    @UtilizatorId INT
AS
BEGIN
    SELECT * FROM Comenzi WHERE UtilizatorId = @UtilizatorId ORDER BY DataComanda DESC;
END
GO

CREATE PROCEDURE sp_GetAllComenzi
AS
BEGIN
    SELECT * FROM Comenzi ORDER BY DataComanda DESC;
END
GO

CREATE PROCEDURE sp_UpdateStareComanda
    @ComandaId INT,
    @StareNoua NVARCHAR(50)
AS
BEGIN
    UPDATE Comenzi SET Stare = @StareNoua WHERE Id = @ComandaId;
END
GO