using UnityEngine;

namespace Commander.Ataques.NovaImpact
{
    [CreateAssetMenu(menuName = "Boss/Ataques/Nova Impact", fileName = "NovaImpact")]
    public class NovaImpactConfig : AtaqueConfig
    {
        [Header("Ativação e área")]
        [Tooltip("Distância do ponto de disparo do boss: ativa o sensor de aproximação e é o raio de dano do impacto")]
        [SerializeField] float raio = 4f;

        [Header("Aviso")]
        [Tooltip("Opcional: nome do Trigger no Animator do boss. Disparado junto com o dano, sem antecipação")]
        [SerializeField] string triggerDeAnimacao;

        [Header("Dano")]
        // Dano em aberto: o design pede HIT KILL; ajustar quando o player tiver sistema de vida
        [SerializeField] float dano = 9999f;

        public float Raio => raio;
        public string TriggerDeAnimacao => triggerDeAnimacao;
        public float Dano => dano;

        public override IAtaqueBoss CriarAtaque() => new AtaqueNovaImpact(this);
    }
}
