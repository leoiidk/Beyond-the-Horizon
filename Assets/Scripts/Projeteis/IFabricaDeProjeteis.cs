using UnityEngine;

namespace Projeteis
{
    // Contrato da Factory de projéteis: quem pede um projétil não sabe como ele é criado
    public interface IFabricaDeProjeteis
    {
        IProjetilBoss Criar(Vector3 posicao, Quaternion rotacao);
    }
}
