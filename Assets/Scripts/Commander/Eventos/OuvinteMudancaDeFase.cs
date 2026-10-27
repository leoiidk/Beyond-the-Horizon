using UnityEngine;
using UnityEngine.Events;

namespace Commander.Eventos
{
    // Liga o canal de mudança de fase a qualquer ação configurada no Inspector, sem escrever código
    public class OuvinteMudancaDeFase : MonoBehaviour
    {
        [SerializeField] CanalEventoFaseBoss canal;
        [Tooltip("Recebe o número da nova fase (2, 3...)")]
        [SerializeField] UnityEvent<int> aoMudarDeFase;

        void OnEnable()
        {
            if (canal != null) canal.AoDisparar += Responder;
        }

        void OnDisable()
        {
            if (canal != null) canal.AoDisparar -= Responder;
        }

        void Responder(DadosMudancaDeFase dados)
        {
            aoMudarDeFase?.Invoke(dados.FaseNova);
        }
    }
}
