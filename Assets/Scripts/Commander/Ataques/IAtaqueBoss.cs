using System.Collections;

namespace Commander.Ataques
{
    // Strategy: cada skill do boss é uma implementação deste contrato
    public interface IAtaqueBoss
    {
        string Nome { get; }

        // Condição extra para o ataque entrar no sorteio (distância do player, vida...)
        bool PodeExecutar(ContextoAtaque contexto);

        // Roda o ataque inteiro como corotina; termina quando o ataque acabou de verdade
        IEnumerator Executar(ContextoAtaque contexto);

        // Chamado se o ataque for interrompido no meio (ex.: boss morreu)
        void Cancelar();
    }
}
