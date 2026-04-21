using Bookly.Aplicacao;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Interfaces;
using Bookly.Repositorio.Repositorios;
using Bookly.Services;
using Bookly.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Repositorio
builder.Services.AddScoped<IUsuarioRepositorio>(provider => new UsuarioRepositorio(connectionString));
builder.Services.AddScoped<ILivroRepositorio>(provider => new LivroRepositorio(connectionString));
builder.Services.AddScoped<IAvaliacaoRepositorio>(provider => new AvaliacaoRepositorio(connectionString));
builder.Services.AddScoped<IComentarioRepositorio>(provider => new ComentarioRepositorio(connectionString));
builder.Services.AddScoped<ICurtidaRepositorio>(provider => new CurtidaRepositorio(connectionString));

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
