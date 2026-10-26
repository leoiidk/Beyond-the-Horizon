using System.Collections;
using Projeteis;
using UnityEngine;

namespace Commander.Ataques.GalacticaBeam
{
    // Carrega por um instante e lança mísseis em arco, um de cada vez, na posição atual do player
    public class AtaqueGalacticaBeam : AtaqueBase
    {
        private readonly GalacticaBeamConfig _config;
        private GameObject _vfxDeCargaAtivo;
        private int _misseisEmVoo;

        public AtaqueGalacticaBeam(GalacticaBeamConfig config) : base(config.Nome)
        {
            _config = config;
        }

        protected override IEnumerator Carregar(ContextoAtaque contexto)
        {
            if (_config.VfxDeCarga != null)
            {
                Transform ponto = contexto.PontoDeDisparo;
                _vfxDeCargaAtivo = Object.Instantiate(_config.VfxDeCarga, ponto.position, ponto.rotation, ponto);
            }

            LogDeControle.Registrar($"Carregando por {_config.TempoDeCarga}s (VFX: {(_vfxDeCargaAtivo != null ? _config.VfxDeCarga.name : "nenhum")})", contexto.Executor);
            yield return new WaitForSeconds(_config.TempoDeCarga);

            DestruirVfxDeCarga();
        }

        protected override IEnumerator Disparar(ContextoAtaque contexto)
        {
            int quantidade = _config.SortearQuantidade(contexto.FaseAtual);
            LogDeControle.Registrar($"Fase {contexto.FaseAtual + 1}: {quantidade} míssil(eis) sorteado(s)", contexto.Executor);

            for (int i = 0; i < quantidade; i++)
            {
                if (i > 0)
                {
                    yield return new WaitForSeconds(_config.IntervaloEntreMisseis);
                }
                LancarMissil(contexto, i + 1, quantidade);
            }
        }

        // O cooldown do boss só começa depois que o último míssil pousar
        protected override IEnumerator Finalizar(ContextoAtaque contexto)
        {
            LogDeControle.Registrar($"Aguardando {_misseisEmVoo} míssil(eis) em voo", contexto.Executor);
            while (_misseisEmVoo > 0)
            {
                yield return null;
            }
        }

        public override void Cancelar()
        {
            // Mísseis já lançados continuam o voo; só a carga é interrompida
            LogDeControle.Registrar($"Cancelado: DestruirVfxDeCarga() ({_misseisEmVoo} míssil(eis) continuam em voo)");
            DestruirVfxDeCarga();
        }

        void LancarMissil(ContextoAtaque contexto, int numero, int total)
        {
            if (_config.FabricaDeMisseis == null)
            {
                Debug.LogError($"{_config.name}: nenhuma fábrica de mísseis definida.", _config);
                return;
            }
            if (contexto.Alvo == null)
            {
                Debug.LogWarning($"{Nome}: boss sem alvo (trPlayer) definido.");
                return;
            }

            Vector3 origem = contexto.PontoDeDisparo.position;
            // Captura a posição do player no momento do disparo: o míssil não persegue
            Vector3 destino = contexto.Alvo.position;

            IProjetilBoss missil = _config.FabricaDeMisseis.Criar(origem, Quaternion.identity);
            if (missil == null) return;

            _misseisEmVoo++;
            missil.AoFinalizar += () => _misseisEmVoo--;
            LogDeControle.Registrar($"{_config.FabricaDeMisseis.name}.Criar() → míssil {numero}/{total} lançado de {origem} para {destino}", contexto.Executor);
            missil.Lancar(origem, destino, _config.TempoDeVoo, _config.AlturaDoArco, _config.Dano);
        }

        void DestruirVfxDeCarga()
        {
            if (_vfxDeCargaAtivo != null)
            {
                Object.Destroy(_vfxDeCargaAtivo);
                _vfxDeCargaAtivo = null;
            }
        }
    }
}
