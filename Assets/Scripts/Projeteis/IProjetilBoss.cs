using System;
using UnityEngine;

namespace Projeteis
{
    // Contrato dos projéteis lançados pelo boss
    public interface IProjetilBoss
    {
        // Disparado quando o projétil termina (pousou, acertou algo ou foi destruído)
        event Action AoFinalizar;

        void Lancar(Vector3 origem, Vector3 destino, float tempoDeVoo, float alturaDoArco, float dano);
    }
}
