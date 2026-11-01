using UnityEngine;

namespace Commander.Ataques
{
    // Tudo o que um ataque precisa saber sobre o boss e o alvo, sem depender da classe Boss
    public class ContextoAtaque
    {
        private readonly Rigidbody _rbAlvo;

        public MonoBehaviour Executor { get; }
        public Transform PontoDeDisparo { get; }
        public Transform Alvo { get; }
        public Vector3 VelocidadeDoAlvo => _rbAlvo != null ? _rbAlvo.linearVelocity : Vector3.zero;
        public int FaseAtual { get; set; }

        public ContextoAtaque(MonoBehaviour executor, Transform pontoDeDisparo, Transform alvo)
        {
            Executor = executor;
            PontoDeDisparo = pontoDeDisparo;
            Alvo = alvo;
            _rbAlvo = alvo != null ? alvo.GetComponent<Rigidbody>() : null;
        }
    }
}
