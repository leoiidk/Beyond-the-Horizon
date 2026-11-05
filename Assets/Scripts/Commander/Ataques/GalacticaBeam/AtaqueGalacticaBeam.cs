using System.Collections;
using Projeteis;
using UnityEngine;

namespace Commander.Ataques.GalacticaBeam
{
    // Carrega por um instante e lança mísseis em arco, um de cada vez, mirando onde o player vai estar quando cada um pousar
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
            // Calculado uma vez por míssil, no momento do disparo: depois de lançado, o míssil não persegue
            Vector3 destino = CalcularDestino(contexto);
            // Ímpar curva à esquerda, par à direita
            float aberturaLateral = numero % 2 == 1 ? -_config.AberturaLateral : _config.AberturaLateral;

            IProjetilBoss missil = _config.FabricaDeMisseis.Criar(origem, Quaternion.identity);
            if (missil == null) return;

            _misseisEmVoo++;
            missil.AoFinalizar += () => _misseisEmVoo--;
            LogDeControle.Registrar($"{_config.FabricaDeMisseis.name}.Criar() → míssil {numero}/{total} lançado de {origem} para {destino} (player em {contexto.Alvo.position}, velocidade {contexto.VelocidadeDoAlvo})", contexto.Executor);
            missil.Lancar(origem, destino, _config.TempoDeVoo, _config.AlturaDoArco, aberturaLateral, _config.Dano);
        }

        // Onde o player vai estar quando o míssil pousar, com um desvio sorteado a cada míssil
        Vector3 CalcularDestino(ContextoAtaque contexto)
        {
            Vector3 velocidade = contexto.VelocidadeDoAlvo;
            velocidade.y = 0f; // só o plano horizontal: evita mirar acima/abaixo do player por oscilação vertical
            Vector3 destino = contexto.Alvo.position + velocidade * _config.TempoDeVoo;

            // Parado não tem desvio: os mísseis caem em cima do player
            if (velocidade.sqrMagnitude < 0.01f) return destino;

            Vector2 desvio = Random.insideUnitCircle * _config.RaioDeDispersao;
            destino.x += desvio.x;
            destino.z += desvio.y;
            return destino;
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
