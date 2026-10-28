using UnityEngine;

namespace Commander.Ataques
{
    // Tudo o que um ataque precisa saber sobre o boss e o alvo, sem depender da classe Boss
    public class ContextoAtaque
    {
        public MonoBehaviour Executor { get; }
        public Transform PontoDeDisparo { get; }
        public Transform Alvo { get; }
        public int FaseAtual { get; set; }

        public ContextoAtaque(MonoBehaviour executor, Transform pontoDeDisparo, Transform alvo)
        {
            Executor = executor;
            PontoDeDisparo = pontoDeDisparo;
            Alvo = alvo;
        }
    }
}
