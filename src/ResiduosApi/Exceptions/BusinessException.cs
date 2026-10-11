namespace ResiduosApi.Exceptions;

/// <summary>Violação de regra de negócio → traduzido para HTTP 422 pelo middleware.</summary>
public class BusinessException : Exception
{
    public BusinessException(string mensagem) : base(mensagem) { }
}
