using System.Collections;

namespace Commander.Ataques
{
    // Template Method: a ordem Carregar → Disparar → Finalizar é fixa, cada ataque só implementa as etapas
    public abstract class AtaqueBase : IAtaqueBoss
    {
        public string Nome { get; }

        protected AtaqueBase(string nome)
        {
            Nome = nome;
        }

        public virtual bool PodeExecutar(ContextoAtaque contexto) => true;

        public IEnumerator Executar(ContextoAtaque contexto)
        {
            LogDeControle.Registrar($"{Nome}: Carregar() iniciado", contexto.Executor);
            yield return Carregar(contexto);

            LogDeControle.Registrar($"{Nome}: Disparar() iniciado", contexto.Executor);
            yield return Disparar(contexto);

            LogDeControle.Registrar($"{Nome}: Finalizar() iniciado", contexto.Executor);
            yield return Finalizar(contexto);

            LogDeControle.Registrar($"{Nome}: execução concluída", contexto.Executor);
        }

        // Antecipação: aviso visual/sonoro antes do golpe
        protected abstract IEnumerator Carregar(ContextoAtaque contexto);

        // O golpe em si
        protected abstract IEnumerator Disparar(ContextoAtaque contexto);

        // Espera o que for preciso antes de liberar o cooldown (ex.: projéteis ainda no ar)
        protected virtual IEnumerator Finalizar(ContextoAtaque contexto)
        {
            yield break;
        }

        public virtual void Cancelar() { }
    }
}
