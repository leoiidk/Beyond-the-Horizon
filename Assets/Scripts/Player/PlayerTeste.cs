using UnityEngine;

namespace Player
{
    public class PlayerTeste : MonoBehaviour,IDanificavel
    {
        public void ReceberDano(float dano)
        {
            Debug.Log($"Player atingido: {dano}!");
        }
    }
}
