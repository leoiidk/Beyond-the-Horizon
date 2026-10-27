namespace Commander.Estados
{
    // Estado final: o boss não volta mais
    public class EstadoDerrotado : IEstadoBoss
    {
        private readonly Boss _boss;

        public EstadoDerrotado(Boss boss)
        {
            _boss = boss;
        }

        public void Entrar()
        {
            LogDeControle.Registrar("Chamando FinalizarLuta()", _boss);
            _boss.FinalizarLuta();
        }

        public void Atualizar() { }

        public void Sair()
        {
            LogDeControle.Registrar("Saindo do estado derrotado", _boss);
        }
    }
}
