-- ============================================================
-- Stored Procedures para a entidade: Avaliacao
-- ============================================================

-- ------------------------------------------------------------
-- Criar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CriarAvaliacao
    @Id          UNIQUEIDENTIFIER,
    @UsuarioId   UNIQUEIDENTIFIER,
    @LivroId     UNIQUEIDENTIFIER,
    @Texto       NVARCHAR(400),
    @Nota        INT,
    @DataCriacao DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Avaliacoes (Id, UsuarioId, LivroId, Texto, Nota, DataCriacao)
    VALUES (@Id, @UsuarioId, @LivroId, @Texto, @Nota, @DataCriacao);
END
GO

-- ------------------------------------------------------------
-- Obter por Id
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ObterAvaliacaoPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UsuarioId, LivroId, Texto, Nota, DataCriacao
    FROM   Avaliacoes
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Listar todas
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ListarAvaliacoes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UsuarioId, LivroId, Texto, Nota, DataCriacao
    FROM   Avaliacoes
    ORDER BY DataCriacao DESC;
END
GO

-- ------------------------------------------------------------
-- Atualizar (apenas Texto e Nota são editáveis)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_AtualizarAvaliacao
    @Id    UNIQUEIDENTIFIER,
    @Texto NVARCHAR(400),
    @Nota  INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Avaliacoes
    SET    Texto = @Texto,
           Nota  = @Nota
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Deletar (apaga também Comentarios e Curtidas via CASCADE)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_DeletarAvaliacao
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Avaliacoes
    WHERE Id = @Id;
END
GO
