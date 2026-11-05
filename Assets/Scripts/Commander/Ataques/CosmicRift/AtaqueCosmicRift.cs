using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commander.Ataques.CosmicRift
{
    // Avisa com uma listra que segue o player e lança uma onda retangular em linha reta, que atravessa tudo até o fim do alcance
    public class AtaqueCosmicRift : AtaqueBase
    {
        private readonly CosmicRiftConfig _config;
        private readonly HashSet<IDanificavel> _atingidos = new();
        private GameObject _vfxDeAvisoAtivo;
        private GameObject _vfxDaOndaAtivo;
        private Vector3 _origem;
        private Vector3 _direcao;

        ContextoAtaque _contexto;
        Quaternion _rotacao;
        float _percorrido;
        bool _ondaEmAndamento;

        public AtaqueCosmicRift(CosmicRiftConfig config) : base(config.Nome)
        {
            _config = config;
        }

        protected override IEnumerator Carregar(ContextoAtaque contexto)
        {
            if (!string.IsNullOrEmpty(_config.TriggerDeAnimacao))
            {
                Animator animador = contexto.Executor.GetComponent<Animator>();
                if (animador != null)
                {
                    animador.SetTrigger(_config.TriggerDeAnimacao);
                }
            }

            // Direção inicial caso o player esteja exatamente em cima do ponto de disparo
            _direcao = Vector3.ProjectOnPlane(contexto.PontoDeDisparo.forward, Vector3.up).normalized;
            if (_direcao == Vector3.zero) _direcao = Vector3.forward;
            AtualizarMira(contexto);

            if (_config.VfxDeAviso != null)
            {
                _vfxDeAvisoAtivo = Object.Instantiate(_config.VfxDeAviso, _origem, Quaternion.LookRotation(_direcao));
                _vfxDeAvisoAtivo.transform.localScale = new Vector3(_config.Largura, 1f, _config.Alcance);
            }

            LogDeControle.Registrar($"Avisando por {_config.TempoDeAviso}s (VFX: {(_vfxDeAvisoAtivo != null ? _config.VfxDeAviso.name : "nenhum")})", contexto.Executor);

            float tempoRestante = _config.TempoDeAviso;
            while (tempoRestante > 0f)
            {
                // A mira não trava durante o aviso: a listra acompanha o player até a onda sair
                AtualizarMira(contexto);
                Quaternion rotacao = Quaternion.LookRotation(_direcao);
                Vector3 centro = _origem + _direcao * (_config.Alcance * 0.5f);

                if (_vfxDeAvisoAtivo != null)
                {
                    _vfxDeAvisoAtivo.transform.SetPositionAndRotation(centro, rotacao);
                }
                DesenharRetangulo(centro, rotacao, _config.Alcance, Color.yellow);

                tempoRestante -= Time.deltaTime;
                yield return null;
            }

            DestruirVfx(ref _vfxDeAvisoAtivo);
        }

        protected override IEnumerator Disparar(ContextoAtaque contexto)
        {
            if (contexto.Alvo == null)
            {
                Debug.LogWarning($"{Nome}: boss sem alvo (trPlayer) definido.");
                yield break;
            }

            // Daqui em diante origem e direção ficam fixas: a onda não persegue o player
            AtualizarMira(contexto);
            _contexto = contexto;
            _rotacao = Quaternion.LookRotation(_direcao);
            _percorrido = 0f;
            _atingidos.Clear();

            if (_config.VfxDaOnda != null)
            {
                _vfxDaOndaAtivo = Object.Instantiate(_config.VfxDaOnda, _origem, _rotacao);
                _vfxDaOndaAtivo.transform.localScale = new Vector3(_config.Largura, _config.Altura, _config.Comprimento);
            }

            LogDeControle.Registrar($"Onda lançada de {_origem} na direção {_direcao} ({_config.Alcance}m em {_config.Duracao:F2}s, player em {contexto.Alvo.position})", contexto.Executor);

            _ondaEmAndamento = true;
            yield return MoverOnda();
        }
        IEnumerator MoverOnda()
        {
            float percorridoAnterior = _percorrido;
            while (true)
            {
                AplicarDano(_contexto, percorridoAnterior, _percorrido, _rotacao);

                if (_vfxDaOndaAtivo != null)
                {
                    _vfxDaOndaAtivo.transform.position = _origem + _direcao * _percorrido;
                }

                if (_percorrido >= _config.Alcance) break;

                yield return null;

                percorridoAnterior = _percorrido;
                _percorrido = Mathf.Min(_percorrido + _config.Velocidade * Time.deltaTime, _config.Alcance);
            }

            _ondaEmAndamento = false;
            DestruirVfx(ref _vfxDaOndaAtivo);
        }

        public override void Cancelar()
        {
            DestruirVfx(ref _vfxDaOndaAtivo);

            if (_ondaEmAndamento && _contexto != null && _contexto.Executor != null && _contexto.Executor.isActiveAndEnabled)
            {
                // A onda é movida por esta corotina: sem ela não há mais dano, então o visual some junto
                LogDeControle.Registrar($"Cancelado com a onda em {_percorrido:F1}/{_config.Alcance}m: ela continua até o fim do alcance", _contexto.Executor);
                _contexto.Executor.StartCoroutine(MoverOnda());
                return;
            }
            LogDeControle.Registrar("Cancelado: destruindo VFX de aviso e da onda");
            _ondaEmAndamento = false;
            DestruirVfx(ref _vfxDaOndaAtivo);
        }

        // O retângulo fica na altura do player: o ponto de disparo está no centro do boss e a área poderia passar por cima dele
        void AtualizarMira(ContextoAtaque contexto)
        {
            _origem = contexto.PontoDeDisparo.position;
            if (contexto.Alvo == null) return;

            Vector3 posicaoDoAlvo = contexto.Alvo.position;
            _origem.y = posicaoDoAlvo.y;

            Vector3 paraOAlvo = posicaoDoAlvo - _origem;
            if (paraOAlvo.sqrMagnitude > 0.0001f)
            {
                _direcao = paraOAlvo.normalized;
            }
        }

        void AplicarDano(ContextoAtaque contexto, float percorridoAnterior, float percorrido, Quaternion rotacao)
        {
            // A área cobre todo o trecho andado neste frame, para a onda não atravessar o player quando o FPS cai
            float passo = percorrido - percorridoAnterior;
            float comprimentoDaArea = _config.Comprimento + passo;
            Vector3 centro = _origem + _direcao * (percorridoAnterior + passo * 0.5f);
            Vector3 meiaExtensao = new Vector3(_config.Largura, _config.Altura, comprimentoDaArea) * 0.5f;

            DesenharRetangulo(centro, rotacao, comprimentoDaArea, Color.red);

            foreach (Collider colisor in Physics.OverlapBox(centro, meiaExtensao, rotacao))
            {
                IDanificavel alvo = colisor.GetComponentInParent<IDanificavel>();
                if (alvo == null || !_atingidos.Add(alvo)) continue;

                // Dano em aberto: o design pede HIT KILL, o valor real depende do sistema de vida do player
                LogDeControle.Registrar($"Onda atingiu {colisor.name} ({_config.Dano} de dano)", contexto.Executor);
                alvo.ReceberDano(_config.Dano);
            }
        }

        // Só aparece na Scene view: serve para conferir a área de dano sem depender dos VFX
        void DesenharRetangulo(Vector3 centro, Quaternion rotacao, float comprimento, Color cor)
        {
            Vector3 lado = rotacao * Vector3.right * (_config.Largura * 0.5f);
            Vector3 frente = rotacao * Vector3.forward * (comprimento * 0.5f);

            Debug.DrawLine(centro - lado - frente, centro + lado - frente, cor);
            Debug.DrawLine(centro + lado - frente, centro + lado + frente, cor);
            Debug.DrawLine(centro + lado + frente, centro - lado + frente, cor);
            Debug.DrawLine(centro - lado + frente, centro - lado - frente, cor);
        }

        void DestruirVfx(ref GameObject vfx)
        {
            if (vfx != null)
            {
                Object.Destroy(vfx);
                vfx = null;
            }
        }
    }
}
