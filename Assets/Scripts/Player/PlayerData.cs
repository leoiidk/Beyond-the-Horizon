using UnityEngine;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField, Min(0)] int vidaInicial = 3;
        [SerializeField, Min(0)] int balasIniciais = 10;

        public int Life { get; private set; }
        public int Bullets { get; private set; }
        public bool IsAlive => Life > 0;
        public bool HasBullets => Bullets > 0;

        void Awake()
        {
            Life = vidaInicial;
            Bullets = balasIniciais;
        }

        // gasta uma bala, retorna false se não tiver
        public bool TryUseBullet()
        {
            if (Bullets <= 0) return false;

            Bullets--;
            return true;
        }
        
        public void Resurrect()
        {
            Life = vidaInicial;
            Bullets = balasIniciais;
        }

        // cada hit tira 1 vida, independente do dano
        public void TakeHit()
        {
            Life = Mathf.Max(0, Life - 1);
        }

        public void AddBullets(int quantidade)
        {
            Bullets += Mathf.Max(0, quantidade);
        }
    }
}
