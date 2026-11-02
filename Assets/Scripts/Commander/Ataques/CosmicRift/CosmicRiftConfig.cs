using UnityEngine;

namespace Commander.Ataques.CosmicRift
{
    [CreateAssetMenu(menuName = "Boss/Ataques/Cosmic Rift", fileName = "CosmicRift")]
    public class CosmicRiftConfig : AtaqueConfig
    {
        [Header("Aviso")]
        [Tooltip("Tempo que a listra de aviso fica na tela, seguindo o player, antes da onda sair")]
        [SerializeField] float tempoDeAviso = 1f;
        [Tooltip("Opcional: nome do Trigger no Animator do boss. Disparado no início do aviso")]
        [SerializeField] string triggerDeAnimacao;
        [Tooltip("Opcional: listra por onde a onda vai passar. A raiz é centralizada no trajeto e escalada para (largura, 1, alcance). Modele o visual em 1x1 num filho, sem Collider")]
        [SerializeField] GameObject vfxDeAviso;

        [Header("Onda")]
        [Tooltip("Junto com o alcance define a duração da onda (alcance / velocidade)")]
        [Min(0.1f)] [SerializeField] float velocidade = 25f;
        [Tooltip("Distância que a onda percorre em linha reta a partir do boss")]
        [Min(0f)] [SerializeField] float alcance = 60f;
        [Tooltip("Tamanho da onda de um lado ao outro, perpendicular ao trajeto")]
        [Min(0f)] [SerializeField] float largura = 8f;
        [Tooltip("Espessura da onda no sentido do trajeto")]
        [Min(0f)] [SerializeField] float comprimento = 3f;
        [Tooltip("Altura da área de dano, centralizada na altura do player")]
        [Min(0f)] [SerializeField] float altura = 6f;
        [Tooltip("Opcional: visual da onda. A raiz é escalada para (largura, altura, comprimento). Modele o visual em 1x1x1 num filho, sem Collider")]
        [SerializeField] GameObject vfxDaOnda;

        [Header("Dano")]
        // Dano em aberto: o design pede HIT KILL; ajustar quando o player tiver sistema de vida
        [SerializeField] float dano = 9999f;

        public float TempoDeAviso => tempoDeAviso;
        public string TriggerDeAnimacao => triggerDeAnimacao;
        public GameObject VfxDeAviso => vfxDeAviso;
        public float Velocidade => velocidade;
        public float Alcance => alcance;
        public float Largura => largura;
        public float Comprimento => comprimento;
        public float Altura => altura;
        public GameObject VfxDaOnda => vfxDaOnda;
        public float Dano => dano;
        public float Duracao => alcance / velocidade;

        public override IAtaqueBoss CriarAtaque() => new AtaqueCosmicRift(this);
    }
}
