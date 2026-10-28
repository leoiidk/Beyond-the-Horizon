using Projeteis;
using UnityEngine;

namespace Commander.Ataques.GalacticaBeam
{
    [CreateAssetMenu(menuName = "Boss/Ataques/Galactica Beam", fileName = "GalacticaBeam")]
    public class GalacticaBeamConfig : AtaqueConfig
    {
        [Header("Carga")]
        [SerializeField] float tempoDeCarga = 1f;
        [Tooltip("Opcional: efeito instanciado no ponto de disparo durante a carga")]
        [SerializeField] GameObject vfxDeCarga;

        [Header("Mísseis")]
        [SerializeField] FabricaDeProjeteisConfig fabricaDeMisseis;
        [Tooltip("Mínimo (x) e máximo (y) de mísseis por fase, inclusivo. Elemento 0 = fase 1")]
        [SerializeField] Vector2Int[] quantidadePorFase = { new(1, 2), new(1, 3), new(2, 4) };
        [Tooltip("Tempo entre um míssil e outro dentro da mesma execução")]
        [SerializeField] float intervaloEntreMisseis = 0.4f;
        [Tooltip("Tempo que o míssil leva para chegar no ponto onde o player estava")]
        [SerializeField] float tempoDeVoo = 1.5f;
        [SerializeField] float alturaDoArco = 8f;

        [Header("Dano")]
        // Dano em aberto: o design pede HIT KILL; ajustar quando o player tiver sistema de vida
        [SerializeField] float dano = 9999f;

        public float TempoDeCarga => tempoDeCarga;
        public GameObject VfxDeCarga => vfxDeCarga;
        public FabricaDeProjeteisConfig FabricaDeMisseis => fabricaDeMisseis;
        public float IntervaloEntreMisseis => intervaloEntreMisseis;
        public float TempoDeVoo => tempoDeVoo;
        public float AlturaDoArco => alturaDoArco;
        public float Dano => dano;

        public int SortearQuantidade(int fase)
        {
            if (quantidadePorFase == null || quantidadePorFase.Length == 0) return 1;

            Vector2Int faixa = quantidadePorFase[Mathf.Clamp(fase, 0, quantidadePorFase.Length - 1)];
            return Random.Range(faixa.x, faixa.y + 1);
        }

        public override IAtaqueBoss CriarAtaque() => new AtaqueGalacticaBeam(this);
    }
}
