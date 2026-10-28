using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commander.Ataques.NovaImpact
{
    // Boss emerge e causa dano em área ao seu redor. Instantâneo: nunca passa pelo sorteio/cooldown normal,
    // é acionado diretamente pelo SensorDeAproximacaoBoss quando o player entra no raio
    public class AtaqueNovaImpact : AtaqueBase
    {
        private readonly NovaImpactConfig _config;

        public AtaqueNovaImpact(NovaImpactConfig config) : base(config.Nome)
        {
            _config = config;
        }

        protected override IEnumerator Carregar(ContextoAtaque contexto)
        {
            // Sem antecipação real: o aviso é a animação do boss, tocada no mesmo instante do dano
            if (!string.IsNullOrEmpty(_config.TriggerDeAnimacao))
            {
                Animator animador = contexto.Executor.GetComponent<Animator>();
                if (animador != null)
                {
                    animador.SetTrigger(_config.TriggerDeAnimacao);
                }
            }
            yield break;
        }

        protected override IEnumerator Disparar(ContextoAtaque contexto)
        {
            Vector3 centro = contexto.PontoDeDisparo.position;
            var atingidos = new HashSet<IDanificavel>();

            foreach (Collider colisor in Physics.OverlapSphere(centro, _config.Raio))
            {
                IDanificavel alvo = colisor.GetComponentInParent<IDanificavel>();
                if (alvo == null || !atingidos.Add(alvo)) continue;

                // Dano em aberto: o design pede HIT KILL, o valor real depende do sistema de vida do player
                LogDeControle.Registrar($"Impacto atingiu {colisor.name} ({_config.Dano} de dano)", contexto.Executor);
                alvo.ReceberDano(_config.Dano);
            }

            yield break;
        }
    }
}
