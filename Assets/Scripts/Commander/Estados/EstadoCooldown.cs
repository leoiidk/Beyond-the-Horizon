using Commander.Ataques;
using UnityEngine;

namespace Commander.Estados
{
    // Espera o cooldown da fase atual e então sorteia o próximo ataque
    public class EstadoCooldown : IEstadoBoss
    {
        private readonly Boss _boss;
        private float _tempoRestante;

        public EstadoCooldown(Boss boss)
        {
            _boss = boss;
        }

        public void Entrar()
        {
            _tempoRestante = _boss.FaseAtualConfig.CooldownEntreAtaques;
            LogDeControle.Registrar($"Fase {_boss.Contexto.FaseAtual + 1}: aguardando cooldown de {_tempoRestante}s", _boss);
        }

        public void Atualizar()
        {
            _tempoRestante -= Time.deltaTime;
            if (_tempoRestante > 0f) return;

            IAtaqueBoss ataque = _boss.SortearAtaque();
            if (ataque == null)
            {
                // Nenhum ataque disponível agora: tenta de novo no próximo ciclo
                _tempoRestante = _boss.FaseAtualConfig.CooldownEntreAtaques;
                LogDeControle.Registrar($"Cooldown terminou, mas nenhum ataque disponível: reiniciando cooldown ({_tempoRestante}s)", _boss);
                return;
            }

            LogDeControle.Registrar($"Cooldown terminou, ataque sorteado: {ataque.Nome}", _boss);
            _boss.Maquina.TrocarEstado(new EstadoAtacando(_boss, ataque));
        }

        public void Sair()
        {
            LogDeControle.Registrar("Saindo do cooldown", _boss);
        }
    }
}
