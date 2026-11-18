using Player;
using UnityEngine;

namespace Itens
{
    [RequireComponent(typeof(Collider))]
    public class ItemDeMunicao : MonoBehaviour
    {
        [SerializeField, Min(0)] int balasRecebidas = 10;

        void Awake()
        {
            // trigger para o barco passar por cima, sem empurrar
            GetComponent<Collider>().isTrigger = true;

            // o barco se move pelo transform, então o próprio item garante o Rigidbody que o trigger exige
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        void OnTriggerEnter(Collider outro)
        {
            PlayerData player = outro.GetComponentInParent<PlayerData>();
            if (player == null) return;

            player.AddBullets(balasRecebidas);
            Destroy(gameObject);
        }
    }
}
