using Commander.Estados;
using UnityEngine;

namespace Commander
{
    public class SincronizadorAnimacaoBoss : MonoBehaviour
    {
        public Boss boss;
        public Animator animador;
        public Collider colisor;

        void Awake()
        {
            // Se nada foi arrastado no Inspector, pega os componentes do próprio objeto
            if (boss == null) boss = GetComponent<Boss>();
            if (animador == null) animador = GetComponent<Animator>();
            if (colisor == null) colisor = GetComponent<Collider>();
        }

        void Update()
        {
            if (boss.Maquina == null) return;

            // Pergunta ao Boss em qual estado ele está agora
            IEstadoBoss estadoAtual = boss.Maquina.EstadoAtual;

            // true se o estado atual for o de ataque, false em qualquer outro
            bool estaAtacando = estadoAtual is EstadoAtacando;

            // Liga/desliga o parâmetro IsAttack do BossController
            // IsAttack = true  -> transição para BossOn  (sobe)
            // IsAttack = false -> transição para BossOff (desce)
            animador.SetBool("IsAttack", estaAtacando);
        }

        // LateUpdate roda depois de todos os Update.
        void LateUpdate()
        {
            if (boss.Maquina == null) return;

            IEstadoBoss estadoAtual = boss.Maquina.EstadoAtual;

            // Durante a troca de fase e depois de derrotado, o boss fica invisível
            bool estaVisivel = !(estadoAtual is EstadoTrocandoFase) && !(estadoAtual is EstadoDerrotado);

            // O boss está "embaixo" quando o Animator está no estado BossOff
            // e não está no meio de uma transição (subindo ou descendo)
            bool estaNoBossOff = animador.GetCurrentAnimatorStateInfo(0).IsName("BossOff");
            bool estaEmTransicao = animador.IsInTransition(0);
            bool estaEmbaixo = estaNoBossOff && !estaEmTransicao;

            // Só pode levar dano se estiver embaixo E visível
            colisor.enabled = estaEmbaixo && estaVisivel;
        }
    }
}
