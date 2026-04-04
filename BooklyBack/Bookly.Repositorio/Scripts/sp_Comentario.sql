-- ============================================================
-- Stored Procedures para a entidade: Comentario
-- ============================================================

-- ------------------------------------------------------------
-- Criar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CriarComentario
    @Id          UNIQUEIDENTIFIER,
    @UsuarioId   UNIQUEIDENTIFIER,
    @AvaliacaoId UNIQUEIDENTIFIER,
    @Texto       NVARCHAR(600),
    @DataCriacao DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Comentarios (Id, UsuarioId, AvaliacaoId, Texto, DataCriacao)
    VALUES (@Id, @UsuarioId, @AvaliacaoId, @Texto, @DataCriacao);
END
GO

-- ------------------------------------------------------------
-- Obter por Id
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ObterComentarioPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UsuarioId, AvaliacaoId, Texto, DataCriacao
    FROM   Comentarios
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Listar todos
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ListarComentarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UsuarioId, AvaliacaoId, Texto, DataCriacao
    FROM   Comentarios
    ORDER BY DataCriacao ASC;
END
GO

-- ------------------------------------------------------------
-- Atualizar (apenas Texto é editável)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_AtualizarComentario
    @Id    UNIQUEIDENTIFIER,
    @Texto NVARCHAR(600)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Comentarios
    SET    Texto = @Texto
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Deletar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_DeletarComentario
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Comentarios
    WHERE Id = @Id;
END
GO
