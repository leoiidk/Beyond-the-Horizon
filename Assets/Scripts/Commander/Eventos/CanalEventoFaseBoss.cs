using System;
using UnityEngine;

namespace Commander.Eventos
{
    public readonly struct DadosMudancaDeFase
    {
        // Números das fases começando em 1 (fase 1, 2, 3)
        public readonly int FaseAnterior;
        public readonly int FaseNova;

        public DadosMudancaDeFase(int faseAnterior, int faseNova)
        {
            FaseAnterior = faseAnterior;
            FaseNova = faseNova;
        }
    }

    // Observer: o boss dispara, quem precisar (UI, música, câmera...) escuta, sem um conhecer o outro
    [CreateAssetMenu(menuName = "Boss/Eventos/Canal Mudança de Fase", fileName = "CanalMudancaDeFase")]
    public class CanalEventoFaseBoss : ScriptableObject
    {
        public event Action<DadosMudancaDeFase> AoDisparar;

        public void Disparar(DadosMudancaDeFase dados)
        {
            AoDisparar?.Invoke(dados);
        }
    }
}
