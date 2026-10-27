namespace Commander.Estados
{
    // State Machine: o boss está sempre em exatamente um estado, e só ele decide a próxima transição
    public class MaquinaDeEstadosBoss
    {
        private readonly UnityEngine.Object _dono;

        public IEstadoBoss EstadoAtual { get; private set; }

        public MaquinaDeEstadosBoss(UnityEngine.Object dono = null)
        {
            _dono = dono;
        }

        public void TrocarEstado(IEstadoBoss novoEstado)
        {
            string anterior = EstadoAtual?.GetType().Name ?? "null";
            string novo = novoEstado?.GetType().Name ?? "null";
            LogDeControle.Registrar($"Transição {anterior} → {novo}", _dono);

            EstadoAtual?.Sair();
            EstadoAtual = novoEstado;
            EstadoAtual?.Entrar();
        }

        public void Atualizar()
        {
            EstadoAtual?.Atualizar();
        }
    }
}
