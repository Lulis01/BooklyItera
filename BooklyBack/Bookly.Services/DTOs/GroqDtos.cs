namespace Bookly.Services.DTOs;

public class LivroAvaliadoDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Autor  { get; set; } = string.Empty;
    public int    Nota   { get; set; }
}

public class LivroRecomendadoDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Autor  { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
}

public class ChatbotRequest
{
    public string Mensagem { get; set; } = string.Empty;
}

public class ChatbotResponse
{
    public string                    Mensagem      { get; set; } = string.Empty;
    public List<LivroRecomendadoDto> Recomendacoes { get; set; } = [];
}

internal class GroqApiResponse
{
    public List<GroqChoice> Choices { get; set; } = [];
}

internal class GroqChoice
{
    public GroqMessage Message { get; set; } = new();
}

internal class GroqMessage
{
    public string Content { get; set; } = string.Empty;
}
