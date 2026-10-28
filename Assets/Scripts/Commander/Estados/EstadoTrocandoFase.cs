using UnityEngine;

namespace Commander.Estados
{
    // Boss some por alguns segundos e volta com vida cheia na próxima fase
    public class EstadoTrocandoFase : IEstadoBoss
    {
        private readonly Boss _boss;
        private float _tempoRestante;

        public EstadoTrocandoFase(Boss boss)
        {
            _boss = boss;
        }

        public void Entrar()
        {
            _tempoRestante = _boss.Config.TempoDeTrocaDeFase;
            LogDeControle.Registrar($"DefinirVisivel(false), boss volta em {_tempoRestante}s", _boss);
            _boss.DefinirVisivel(false);
        }

        public void Atualizar()
        {
            _tempoRestante -= Time.deltaTime;
            if (_tempoRestante > 0f) return;

            LogDeControle.Registrar("Tempo de troca terminou: chamando AvancarFase()", _boss);
            _boss.AvancarFase();
            _boss.Maquina.TrocarEstado(new EstadoCooldown(_boss));
        }

        public void Sair()
        {
            LogDeControle.Registrar("Saindo da troca de fase", _boss);
        }
    }
}
