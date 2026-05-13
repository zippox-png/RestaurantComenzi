-- =============================================
-- 1. CREARE TABELE (Forma Normală 3)
-- =============================================

CREATE TABLE Categorii (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Denumire NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Utilizatori (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nume NVARCHAR(50) NOT NULL,
    Prenume NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Parola NVARCHAR(255) NOT NULL,
    Telefon NVARCHAR(15) NOT NULL,
    AdresaLivrare NVARCHAR(200) NOT NULL,
    Rol NVARCHAR(20) DEFAULT 'Client' 
);
GO

CREATE TABLE Preparate (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Denumire NVARCHAR(100) NOT NULL,
    Pret DECIMAL(18,2) NOT NULL,
    CantitatePortie NVARCHAR(50) NOT NULL,
    CantitateTotala DECIMAL(18,2) NOT NULL,
    CategorieId INT NOT NULL,
    Disponibil BIT DEFAULT 1,
    FOREIGN KEY (CategorieId) REFERENCES Categorii(Id)
);
GO

-- Tabel separat pentru lista de fotografii (1:N)
CREATE TABLE FotografiiPreparate (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PreparatId INT NOT NULL,
    ImaginePath NVARCHAR(255) NOT NULL,
    FOREIGN KEY (PreparatId) REFERENCES Preparate(Id) ON DELETE CASCADE
);
GO

-- Tabela pentru Alergeni
CREATE TABLE Alergeni (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Denumire NVARCHAR(100) NOT NULL
);
GO

-- Tabel de legătură Preparate - Alergeni (M:N)
CREATE TABLE PreparateAlergeni (
    PreparatId INT NOT NULL,
    AlergenId INT NOT NULL,
    PRIMARY KEY (PreparatId, AlergenId),
    FOREIGN KEY (PreparatId) REFERENCES Preparate(Id) ON DELETE CASCADE,
    FOREIGN KEY (AlergenId) REFERENCES Alergeni(Id) ON DELETE CASCADE
);
GO

-- Tabela Meniuri (grupuri de preparate)
CREATE TABLE Meniuri (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Denumire NVARCHAR(100) NOT NULL,
    CategorieId INT NOT NULL,
    Disponibil BIT DEFAULT 1,
    FOREIGN KEY (CategorieId) REFERENCES Categorii(Id)
);
GO

-- Tabel de legătură Meniuri - Preparate (M:N)
CREATE TABLE MeniuriPreparate (
    MeniuId INT NOT NULL,
    PreparatId INT NOT NULL,
    PRIMARY KEY (MeniuId, PreparatId),
    FOREIGN KEY (MeniuId) REFERENCES Meniuri(Id) ON DELETE CASCADE,
    FOREIGN KEY (PreparatId) REFERENCES Preparate(Id) ON DELETE CASCADE
);
GO

-- Tabela Comenzi
CREATE TABLE Comenzi (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodComanda UNIQUEIDENTIFIER DEFAULT NEWID(), -- Cod unic automat
    UtilizatorId INT NOT NULL,
    DataComanda DATETIME DEFAULT GETDATE(),
    Stare NVARCHAR(50) DEFAULT 'inregistrata', -- inregistrata, se pregateste, livrata, anulata
    PretTotal DECIMAL(18,2) NOT NULL,
    CostTransport DECIMAL(18,2) DEFAULT 0,
    OraEstimativaLivrare DATETIME,
    FOREIGN KEY (UtilizatorId) REFERENCES Utilizatori(Id)
);
GO

-- Detaliile comenzii (ce produse și câte bucăți)
CREATE TABLE DetaliiComanda (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ComandaId INT NOT NULL,
    PreparatId INT NULL, -- Poate fi NULL dacă e meniu
    MeniuId INT NULL,    -- Poate fi NULL dacă e preparat simplu
    Cantitate INT NOT NULL,
    FOREIGN KEY (ComandaId) REFERENCES Comenzi(Id) ON DELETE CASCADE,
    FOREIGN KEY (PreparatId) REFERENCES Preparate(Id),
    FOREIGN KEY (MeniuId) REFERENCES Meniuri(Id)
);
GO


-- =============================================
-- 2. PROCEDURI STOCATE (Minim 10)
-- =============================================

-- 1. SELECT Categorii
CREATE PROCEDURE sp_GetCategorii
AS
BEGIN
    SELECT Id, Denumire FROM Categorii;
END
GO

-- 2. INSERT Categorie
CREATE PROCEDURE sp_InsertCategorie
    @Denumire NVARCHAR(100)
AS
BEGIN
    INSERT INTO Categorii (Denumire) VALUES (@Denumire);
END
GO

-- 3. SELECT (Login Utilizator)
CREATE PROCEDURE sp_LoginUtilizator
    @Email NVARCHAR(100),
    @Parola NVARCHAR(255)
AS
BEGIN
    SELECT Id, Nume, Prenume, Email, Telefon, AdresaLivrare, Rol 
    FROM Utilizatori 
    WHERE Email = @Email AND Parola = @Parola;
END
GO

-- 4. INSERT Utilizator (Register)
CREATE PROCEDURE sp_InsertUtilizator
    @Nume NVARCHAR(50),
    @Prenume NVARCHAR(50),
    @Email NVARCHAR(100),
    @Parola NVARCHAR(255),
    @Telefon NVARCHAR(15),
    @Adresa NVARCHAR(200)
AS
BEGIN
    INSERT INTO Utilizatori (Nume, Prenume, Email, Parola, Telefon, AdresaLivrare, Rol)
    VALUES (@Nume, @Prenume, @Email, @Parola, @Telefon, @Adresa, 'Client');
END
GO

-- 5. SELECT Toate Preparatele
CREATE PROCEDURE sp_GetPreparate
AS
BEGIN
    SELECT Id, Denumire, Pret, CantitatePortie, CantitateTotala, CategorieId, Disponibil
    FROM Preparate;
END
GO

-- 6. UPDATE Stoc Preparat (folosit la livrarea unei comenzi)
CREATE PROCEDURE sp_UpdateStocPreparat
    @PreparatId INT,
    @CantitateScazuta DECIMAL(18,2)
AS
BEGIN
    UPDATE Preparate
    SET CantitateTotala = CantitateTotala - @CantitateScazuta
    WHERE Id = @PreparatId;
END
GO

-- 7. UPDATE Disponibilitate Preparat (indisponibil cand stocul e epuizat)
CREATE PROCEDURE sp_SetPreparatIndisponibil
    @PreparatId INT
AS
BEGIN
    UPDATE Preparate
    SET Disponibil = 0
    WHERE Id = @PreparatId;
END
GO

-- 8. SELECT Comenzi ale unui Client
CREATE PROCEDURE sp_GetComenziClient
    @UtilizatorId INT
AS
BEGIN
    SELECT Id, CodComanda, DataComanda, Stare, PretTotal, CostTransport, OraEstimativaLivrare
    FROM Comenzi
    WHERE UtilizatorId = @UtilizatorId
    ORDER BY DataComanda DESC;
END
GO

-- 9. UPDATE Stare Comanda (angajatul o muta din "inregistrata" in "livrata" etc)
CREATE PROCEDURE sp_UpdateStareComanda
    @ComandaId INT,
    @StareNoua NVARCHAR(50)
AS
BEGIN
    UPDATE Comenzi
    SET Stare = @StareNoua
    WHERE Id = @ComandaId;
END
GO

-- 10. INSERT Comanda Noua
CREATE PROCEDURE sp_InsertComanda
    @UtilizatorId INT,
    @PretTotal DECIMAL(18,2),
    @CostTransport DECIMAL(18,2),
    @OraEstimativa DATETIME
AS
BEGIN
    INSERT INTO Comenzi (UtilizatorId, PretTotal, CostTransport, OraEstimativaLivrare)
    VALUES (@UtilizatorId, @PretTotal, @CostTransport, @OraEstimativa);
    
    -- Returnează ID-ul noii comenzi pentru a putea introduce apoi Detaliile (produsele)
    SELECT SCOPE_IDENTITY() AS NewComandaId;
END
GO