using System.Collections;
using Commander.Ataques;
using Commander.Estados;
using Commander.Eventos;
using UnityEngine;
using UnityEngine.UI;

namespace Commander
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] Slider barraDeVida;
        [SerializeField] private float vidaMaxima = 100f;
        [SerializeField] private float vidaAtual;
        [SerializeField] private float danoProjetil = 10f;
        [SerializeField] GameObject bossMesh;
        [SerializeField] bool estaMorto;

        [Header("Ataques")]
        [SerializeField] BossConfig config;
        [Tooltip("De onde saem os ataques (GameObject no centro do boss). Se vazio, usa o próprio boss")]
        [SerializeField] Transform pontoDeDisparo;
        [SerializeField] Transform trPlayer;

        [Header("Eventos")]
        [SerializeField] CanalEventoFaseBoss canalMudancaDeFase;
        [SerializeField] CanalEventoBossDerrotado canalBossDerrotado;

        [Header("Debug")]
        [Tooltip("Liga/desliga os logs de controle (LogDeControle) no Console")]
        [SerializeField] bool logsDeControle = true;
        [SerializeField] int faseAtual;
        [SerializeField] string estadoAtual;

        [Header("Damage Flash")]
        [SerializeField] private Material materialFlash;
        [SerializeField] float  tempoFlash = 0.1f;  
        private Material materialOriginalBoss;
        private Material materialOriginalBall;
        [SerializeField] Renderer rendBoss;
        [SerializeField] Renderer rendBall;

        private Collider _colisor;
        private Renderer[] _renderers;
        private SeletorDeAtaquePonderado _seletor;

        internal MaquinaDeEstadosBoss Maquina { get; private set; }
        internal ContextoAtaque Contexto { get; private set; }
        internal BossConfig Config => config;
        internal FaseBoss FaseAtualConfig => config.Fases[faseAtual];
        internal bool EstaMorto => estaMorto;

        void Start()
        {
            LogDeControle.Ativo = logsDeControle;

            _colisor = GetComponent<Collider>();
            _renderers = bossMesh != null ? bossMesh.GetComponentsInChildren<Renderer>(true) : new Renderer[0];
            materialOriginalBoss = rendBoss.material;
            materialOriginalBall = rendBall.material;
            estaMorto = false;
            vidaAtual = vidaMaxima;
            AtualizarBarraDeVida();

            if (config == null || config.Fases.Count == 0)
            {
                Debug.LogError("Boss sem BossConfig (ou sem fases): os ataques não vão rodar.", this);
                return;
            }

            faseAtual = 0;
            Contexto = new ContextoAtaque(this, pontoDeDisparo != null ? pontoDeDisparo : transform, trPlayer);
            _seletor = new SeletorDeAtaquePonderado(config);
            Maquina = new MaquinaDeEstadosBoss(this);

            LogDeControle.Registrar($"{config.name} carregado ({config.Fases.Count} fases), iniciando em EstadoCooldown", this);
            Maquina.TrocarEstado(new EstadoCooldown(this));
        }

        void Update()
        {
            if (Maquina == null) return;

            Maquina.Atualizar();
            estadoAtual = Maquina.EstadoAtual?.GetType().Name;
        }

        void TomarDano()
        {
            if (estaMorto) return;

            vidaAtual -= danoProjetil;
            AtualizarBarraDeVida();
            LogDeControle.Registrar($"Recebeu {danoProjetil} de dano, vida {vidaAtual}/{vidaMaxima}", this);
            Flash();

            if (vidaAtual <= 0)
            {
                Morrer();
            }
        }

        void Morrer()
        {
            estaMorto = true;

            if (Maquina == null)
            {
                LogDeControle.Registrar("Morreu sem máquina de estados: só DefinirVisivel(false)", this);
                DefinirVisivel(false);
                return;
            }

            bool ehUltimaFase = faseAtual >= config.Fases.Count - 1;
            LogDeControle.Registrar($"Vida zerou na fase {faseAtual + 1}: indo para {(ehUltimaFase ? nameof(EstadoDerrotado) : nameof(EstadoTrocandoFase))}", this);
            Maquina.TrocarEstado(ehUltimaFase ? new EstadoDerrotado(this) : new EstadoTrocandoFase(this));
        }

        private void OnCollisionEnter(Collision objetoQueColidiu)
        {
            if (objetoQueColidiu.gameObject.CompareTag("PlayerProjectile"))
            {
                TomarDano();
            }
        }

        internal IAtaqueBoss SortearAtaque()
        {
            return _seletor.Sortear(faseAtual, Contexto);
        }

        internal void DefinirVisivel(bool visivel)
        {
            // Não desativa o GameObject: bossMesh pode ser o próprio objeto do Boss, e isso pararia o Update que traz ele de volta
            LogDeControle.Registrar($"{_renderers.Length} renderers.enabled = {visivel}, collider.enabled = {visivel}", this);
            foreach (Renderer rend in _renderers) rend.enabled = visivel;
            if (_colisor != null) _colisor.enabled = visivel;
        }

        internal void AvancarFase()
        {
            int faseAnterior = faseAtual;
            faseAtual++;
            Contexto.FaseAtual = faseAtual;

            vidaAtual = vidaMaxima;
            AtualizarBarraDeVida();
            estaMorto = false;
            DefinirVisivel(true);

            LogDeControle.Registrar($"Fase {faseAnterior + 1} → {faseAtual + 1}, vida restaurada ({vidaMaxima})", this);
            if (canalMudancaDeFase != null)
            {
                LogDeControle.Registrar($"Disparando {canalMudancaDeFase.name}", this);
                canalMudancaDeFase.Disparar(new DadosMudancaDeFase(faseAnterior + 1, faseAtual + 1));
            }
        }

        internal void FinalizarLuta()
        {
            DefinirVisivel(false);
            if (barraDeVida != null) barraDeVida.gameObject.SetActive(false);

            LogDeControle.Registrar("Boss derrotado definitivamente!", this);
            if (canalBossDerrotado != null)
            {
                LogDeControle.Registrar($"Disparando {canalBossDerrotado.name}", this);
                canalBossDerrotado.Disparar();
            }
        }

        void AtualizarBarraDeVida()
        {
            if (barraDeVida == null) return;

            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;
        }

        private IEnumerator DanoFlash()
        {
            rendBoss.material = materialFlash;  
            rendBall.material = materialFlash; 
            yield return new WaitForSeconds(tempoFlash);
            rendBoss.material = materialOriginalBoss;
            rendBall.material = materialOriginalBall;
        }

        void Flash()
        {
            StartCoroutine(DanoFlash());
        }
    }
}
