using System.Collections;
using Commander.Ataques;
using Commander.Ataques.NovaImpact;
using UnityEngine;

namespace Commander
{
    // Fica fora do ciclo normal de sorteio/cooldown: detecta a aproximação do player e aciona a
    // Nova Impact diretamente, em paralelo a qualquer EstadoAtacando de outro ataque em andamento
    public class SensorDeAproximacaoBoss : MonoBehaviour
    {
        [SerializeField] Boss boss;
        [SerializeField] NovaImpactConfig config;

        private IAtaqueBoss _ataque;
        private bool _dentroDoRaio;
        private bool _emExecucao;

        void Awake()
        {
            if (boss == null) boss = GetComponent<Boss>();
        }

        void Start()
        {
            if (config == null)
            {
                Debug.LogError($"{name}: nenhum NovaImpactConfig definido.", this);
                return;
            }
            _ataque = config.CriarAtaque();
        }

        void Update()
        {
            if (boss == null || _ataque == null || boss.EstaMorto) return;

            ContextoAtaque contexto = boss.Contexto;
            if (contexto?.PontoDeDisparo == null || contexto.Alvo == null) return;

            float distancia = Vector3.Distance(contexto.PontoDeDisparo.position, contexto.Alvo.position);
            bool dentroAgora = distancia <= config.Raio;

            if (dentroAgora && !_dentroDoRaio && !_emExecucao)
            {
                LogDeControle.Registrar($"Player a {distancia:F1}m, dentro do raio ({config.Raio}m): acionando {_ataque.Nome}", boss);
                StartCoroutine(RodarAtaque(contexto));
            }

            _dentroDoRaio = dentroAgora;
        }

        IEnumerator RodarAtaque(ContextoAtaque contexto)
        {
            _emExecucao = true;
            yield return _ataque.Executar(contexto);
            _emExecucao = false;
        }
    }
}
