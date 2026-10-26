using System;
using System.Collections.Generic;
using Commander.Ataques;
using UnityEngine;

namespace Commander
{
    [Serializable]
    public class AtaquePonderado
    {
        [SerializeField] AtaqueConfig ataque;
        [Tooltip("Quanto maior, mais vezes o ataque é sorteado em relação aos outros da fase")]
        [Range(0f, 100f)] [SerializeField] float peso = 1f;

        public AtaqueConfig Ataque => ataque;
        public float Peso => peso;
    }

    [Serializable]
    public class FaseBoss
    {
        [Tooltip("Tempo de espera entre o fim de um ataque e o começo do próximo")]
        [SerializeField] float cooldownEntreAtaques = 3.5f;
        [SerializeField] List<AtaquePonderado> ataques = new();

        public float CooldownEntreAtaques => cooldownEntreAtaques;
        public IReadOnlyList<AtaquePonderado> Ataques => ataques;

        public FaseBoss() { }

        public FaseBoss(float cooldownEntreAtaques)
        {
            this.cooldownEntreAtaques = cooldownEntreAtaques;
        }
    }
}
