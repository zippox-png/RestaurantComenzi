CREATE PROCEDURE sp_LoginUtilizator
    @Email NVARCHAR(100),
    @Parola NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Utilizatori WHERE Email = @Email AND Parola = @Parola;
END
GO

CREATE PROCEDURE sp_InsertUtilizator
    @Nume NVARCHAR(100), @Prenume NVARCHAR(100), @Email NVARCHAR(100),
    @Parola NVARCHAR(100), @Telefon NVARCHAR(20), @Adresa NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Utilizatori (Nume, Prenume, Email, Parola, Telefon, Adresa)
    VALUES (@Nume, @Prenume, @Email, @Parola, @Telefon, @Adresa);
END
GO