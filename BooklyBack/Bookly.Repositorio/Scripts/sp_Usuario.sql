-- Stored Procedures para a entidade: Usuario


-- Criar

CREATE OR ALTER PROCEDURE sp_CriarUsuario
    @Id          UNIQUEIDENTIFIER,
    @Nome        NVARCHAR(150),
    @Email       NVARCHAR(200),
    @SenhaHash   NVARCHAR(500),
    @DataCriacao DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuarios (Id, Nome, Email, SenhaHash, DataCriacao)
    VALUES (@Id, @Nome, @Email, @SenhaHash, @DataCriacao);
END
GO


-- Obter por Id

CREATE OR ALTER PROCEDURE sp_ObterUsuarioPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, Email, SenhaHash, DataCriacao
    FROM   Usuarios
    WHERE  Id = @Id;
END
GO


-- Listar todos

CREATE OR ALTER PROCEDURE sp_ListarUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, Email, SenhaHash, DataCriacao
    FROM   Usuarios
    ORDER BY Nome ASC;
END
GO


-- Atualizar

CREATE OR ALTER PROCEDURE sp_AtualizarUsuario
    @Id        UNIQUEIDENTIFIER,
    @Nome      NVARCHAR(150),
    @Email     NVARCHAR(200),
    @SenhaHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET    Nome      = @Nome,
           Email     = @Email,
           SenhaHash = @SenhaHash
    WHERE  Id = @Id;
END
GO


-- Deletar

CREATE OR ALTER PROCEDURE sp_DeletarUsuario
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Usuarios
    WHERE Id = @Id;
END
GO
