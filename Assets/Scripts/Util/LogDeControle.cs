using System.Diagnostics;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

// Log de controle padronizado: [Objeto] Classe.Metodo (linha N) → mensagem
// Classe, método e linha são preenchidos pelo compilador (Caller*), sem custo de StackTrace
// [Conditional] remove as chamadas de builds de release: só existe no Editor e em Development Build
public static class LogDeControle
{
    public static bool Ativo = true;

    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Registrar(string mensagem, UnityEngine.Object contexto = null,
        [CallerMemberName] string metodo = "", [CallerFilePath] string arquivo = "", [CallerLineNumber] int linha = 0)
    {
        if (!Ativo) return;

        string objeto = contexto != null ? $"[{contexto.name}] " : "";
        Debug.Log($"{objeto}{NomeDaClasse(arquivo)}.{metodo} (linha {linha}) → {mensagem}", contexto);
    }

    // No projeto cada arquivo tem o nome da sua classe; o caminho pode vir com / ou \ (Windows)
    static string NomeDaClasse(string arquivo)
    {
        int inicio = arquivo.LastIndexOfAny(new[] { '/', '\\' }) + 1;
        int fim = arquivo.LastIndexOf('.');
        return fim > inicio ? arquivo.Substring(inicio, fim - inicio) : arquivo.Substring(inicio);
    }
}
