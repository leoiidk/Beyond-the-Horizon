using UnityEngine;

namespace Projeteis
{
    // Factory Method: cada subclasse decide como o projétil é criado (Instantiate, pool...)
    // É um ScriptableObject para poder ser trocada no Inspector sem mexer no código do ataque
    public abstract class FabricaDeProjeteisConfig : ScriptableObject, IFabricaDeProjeteis
    {
        public abstract IProjetilBoss Criar(Vector3 posicao, Quaternion rotacao);
    }
}
