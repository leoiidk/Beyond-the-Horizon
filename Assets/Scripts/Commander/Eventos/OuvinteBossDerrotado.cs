using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Commander.Eventos
{
    // Liga o canal "boss derrotado" a qualquer ação configurada no Inspector (igual ao OuvinteMudancaDeFase),
    // com uma espera antes, para dar tempo do boss sumir. Na Blocagem ele chama a cena Final.
    public class OuvinteBossDerrotado : MonoBehaviour
    {
        [SerializeField] CanalEventoBossDerrotado canal;
        [Tooltip("Segundos entre o boss ser derrotado e a ação acontecer")]
        [SerializeField] float espera = 4f;
        [SerializeField] UnityEvent aoDerrotarOBoss;

        public UnityEvent AoDerrotarOBoss => aoDerrotarOBoss;

        void OnEnable()
        {
            if (canal != null) canal.AoDisparar += Responder;
        }

        void OnDisable()
        {
            if (canal != null) canal.AoDisparar -= Responder;
        }

        void Responder()
        {
            StartCoroutine(ResponderDepois());
        }

        IEnumerator ResponderDepois()
        {
            yield return new WaitForSeconds(espera);
            aoDerrotarOBoss?.Invoke();
        }
    }
}
