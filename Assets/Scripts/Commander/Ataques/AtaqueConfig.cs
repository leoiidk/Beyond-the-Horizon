using UnityEngine;

namespace Commander.Ataques
{
    // Dados de um ataque guardados em asset. Cada subclasse é o Factory Method da sua estratégia
    public abstract class AtaqueConfig : ScriptableObject
    {
        [SerializeField] string nome;

        public string Nome => string.IsNullOrEmpty(nome) ? name : nome;

        public abstract IAtaqueBoss CriarAtaque();
    }
}
