-- ============================================================
-- Stored Procedures para a entidade: Curtida
-- ============================================================

-- ------------------------------------------------------------
-- Criar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CriarCurtida
    @Id          UNIQUEIDENTIFIER,
    @UsuarioId   UNIQUEIDENTIFIER,
    @AvaliacaoId UNIQUEIDENTIFIER,
    @DataCriacao DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Curtidas (Id, UsuarioId, AvaliacaoId, DataCriacao)
    VALUES (@Id, @UsuarioId, @AvaliacaoId, @DataCriacao);
END
GO

-- ------------------------------------------------------------
-- Obter por Id
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ObterCurtidaPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UsuarioId, AvaliacaoId, DataCriacao
    FROM   Curtidas
    WHERE  Id = @Id;
END
GO

-- ------------------------------------------------------------
-- Listar todas
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ListarCurtidas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UsuarioId, AvaliacaoId, DataCriacao
    FROM   Curtidas
    ORDER BY DataCriacao DESC;
END
GO

-- ------------------------------------------------------------
-- Atualizar
-- NOTA: Curtida é imutável. Esta SP existe apenas por contrato.
-- Para "atualizar" uma curtida, delete e recrie.
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_AtualizarCurtida
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    -- Curtida não possui campos editáveis.
    -- Operação ignorada intencionalmente.
END
GO

-- ------------------------------------------------------------
-- Deletar
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_DeletarCurtida
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Curtidas
    WHERE Id = @Id;
END
GO
