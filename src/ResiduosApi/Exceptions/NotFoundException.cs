namespace ResiduosApi.Exceptions;

/// <summary>Recurso não encontrado → traduzido para HTTP 404 pelo middleware.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string mensagem) : base(mensagem) { }
}
