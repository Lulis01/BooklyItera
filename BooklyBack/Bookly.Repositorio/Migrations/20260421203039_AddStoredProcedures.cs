using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookly.Repositorio.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // During 'dotnet ef database update', base dir is usually the Startup project bin directory.
            // We can resolve relative from AppContext.BaseDirectory: Bookly.API\bin\Debug\net9.0
            var basePath = AppContext.BaseDirectory;
            var scriptsPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Bookly.Repositorio", "Scripts"));
            
            if (Directory.Exists(scriptsPath))
            {
                foreach (var file in Directory.GetFiles(scriptsPath, "*.sql"))
                {
                    var sql = File.ReadAllText(file);
                    var batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (var batch in batches)
                    {
                        if (string.IsNullOrWhiteSpace(batch)) continue;
                        
                        var safeBatch = batch;
                        // Replace CREATE OR ALTER with simply CREATE to avoid any SQL parser ambiguity
                        if (safeBatch.Contains("CREATE OR ALTER PROCEDURE")) {
                            var procNameMatch = Regex.Match(safeBatch, @"CREATE OR ALTER PROCEDURE\s+([^\s\(]+)", RegexOptions.IgnoreCase);
                            if (procNameMatch.Success) {
                                var procName = procNameMatch.Groups[1].Value;
                                migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS {procName};");
                            }
                            safeBatch = Regex.Replace(safeBatch, @"CREATE OR ALTER PROCEDURE", "CREATE PROCEDURE", RegexOptions.IgnoreCase);
                        }
                        
                        migrationBuilder.Sql(safeBatch);
                    }
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Simplified down migration: remove all the added procedures
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CriarUsuario; DROP PROCEDURE IF EXISTS sp_ObterUsuarioPorId; DROP PROCEDURE IF EXISTS sp_ListarUsuarios; DROP PROCEDURE IF EXISTS sp_AtualizarUsuario; DROP PROCEDURE IF EXISTS sp_DeletarUsuario;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CriarLivro; DROP PROCEDURE IF EXISTS sp_ObterLivroPorId; DROP PROCEDURE IF EXISTS sp_ListarLivros; DROP PROCEDURE IF EXISTS sp_AtualizarLivro; DROP PROCEDURE IF EXISTS sp_DeletarLivro;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CriarAvaliacao; DROP PROCEDURE IF EXISTS sp_ObterAvaliacaoPorId; DROP PROCEDURE IF EXISTS sp_ListarAvaliacoes; DROP PROCEDURE IF EXISTS sp_AtualizarAvaliacao; DROP PROCEDURE IF EXISTS sp_DeletarAvaliacao;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CriarComentario; DROP PROCEDURE IF EXISTS sp_ObterComentarioPorId; DROP PROCEDURE IF EXISTS sp_ListarComentariosPorAvaliacao; DROP PROCEDURE IF EXISTS sp_AtualizarComentario; DROP PROCEDURE IF EXISTS sp_DeletarComentario;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CriarCurtida; DROP PROCEDURE IF EXISTS sp_ListarCurtidasPorAvaliacao; DROP PROCEDURE IF EXISTS sp_DeletarCurtida;");
        }
    }
}
