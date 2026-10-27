using System.Collections;
using Commander.Ataques;
using UnityEngine;

namespace Commander.Estados
{
    // Roda um ataque do começo ao fim e volta para o cooldown
    public class EstadoAtacando : IEstadoBoss
    {
        private readonly Boss _boss;
        private readonly IAtaqueBoss _ataque;
        private Coroutine _corotina;
        private bool _terminou;

        public EstadoAtacando(Boss boss, IAtaqueBoss ataque)
        {
            _boss = boss;
            _ataque = ataque;
        }

        public void Entrar()
        {
            LogDeControle.Registrar($"StartCoroutine: {_ataque.Nome}.Executar()", _boss);
            _corotina = _boss.StartCoroutine(RodarAtaque());
        }

        public void Atualizar() { }

        public void Sair()
        {
            // Saiu antes do ataque acabar (ex.: boss morreu): interrompe e deixa o ataque limpar o que criou
            if (!_terminou && _corotina != null)
            {
                LogDeControle.Registrar($"Ataque {_ataque.Nome} interrompido: StopCoroutine + {_ataque.Nome}.Cancelar()", _boss);
                _boss.StopCoroutine(_corotina);
                _ataque.Cancelar();
                return;
            }

            LogDeControle.Registrar($"Saindo após {_ataque.Nome} terminar normalmente", _boss);
        }

        IEnumerator RodarAtaque()
        {
            yield return _ataque.Executar(_boss.Contexto);

            _terminou = true;
            LogDeControle.Registrar($"{_ataque.Nome} terminou: voltando para o cooldown", _boss);
            _boss.Maquina.TrocarEstado(new EstadoCooldown(_boss));
        }
    }
}
