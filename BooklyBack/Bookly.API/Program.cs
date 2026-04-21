using Bookly.Aplicacao;
using Bookly.Aplicacao.Interfaces;
using Bookly.Services;
using Bookly.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Aplicacao
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<ILivroAplicacao, LivroAplicacao>();
builder.Services.AddScoped<IAvaliacaoAplicacao, AvaliacaoAplicacao>();
builder.Services.AddScoped<IComentarioAplicacao, ComentarioAplicacao>();
builder.Services.AddScoped<ICurtidaAplicacao, CurtidaAplicacao>();

// Servicos externos
builder.Services.AddHttpClient<IOpenLibraryService, OpenLibraryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
