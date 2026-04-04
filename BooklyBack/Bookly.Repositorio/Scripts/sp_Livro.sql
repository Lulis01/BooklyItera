-- ============================================================
-- Stored Procedures para a entidade: Livro
-- ============================================================

-- ------------------------------------------------------------
-- Criar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CriarLivro
    @Id             UNIQUEIDENTIFIER,
    @Titulo         NVARCHAR(300),
    @Autor          NVARCHAR(200),
    @ISBN           NVARCHAR(20),
    @AnoPublicacao  INT,
    @Genero         NVARCHAR(100),
    @DataCriacao    DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Livros (Id, Titulo, Autor, ISBN, AnoPublicacao, Genero, DataCriacao)
    VALUES (@Id, @Titulo, @Autor, @ISBN, @AnoPublicacao, @Genero, @DataCriacao);
END
GO

-- ------------------------------------------------------------
-- Obter por Id
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ObterLivroPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Titulo, Autor, ISBN, AnoPublicacao, Genero, DataCriacao
    FROM   Livros
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Listar todos
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ListarLivros
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Titulo, Autor, ISBN, AnoPublicacao, Genero, DataCriacao
    FROM   Livros
    ORDER BY Titulo ASC;
END
GO

-- ------------------------------------------------------------
-- Atualizar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_AtualizarLivro
    @Id            UNIQUEIDENTIFIER,
    @Titulo        NVARCHAR(300),
    @Autor         NVARCHAR(200),
    @ISBN          NVARCHAR(20),
    @AnoPublicacao INT,
    @Genero        NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Livros
    SET    Titulo        = @Titulo,
           Autor         = @Autor,
           ISBN          = @ISBN,
           AnoPublicacao = @AnoPublicacao,
           Genero        = @Genero
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Deletar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_DeletarLivro
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Livros
    WHERE Id = @Id;
END
GO
