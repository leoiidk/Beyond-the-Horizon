using System.Collections.Generic;
using UnityEngine;

namespace Commander
{
    [CreateAssetMenu(menuName = "Boss/Configuração do Boss", fileName = "BossConfig")]
    public class BossConfig : ScriptableObject
    {
        [Tooltip("Uma entrada por fase, na ordem. Cada morte do boss avança para a próxima")]
        [SerializeField] List<FaseBoss> fases = new() { new FaseBoss(3.5f), new FaseBoss(2.5f), new FaseBoss(1.5f) };
        [Tooltip("Tempo que o boss fica sumido entre uma fase e outra")]
        [SerializeField] float tempoDeTrocaDeFase = 5f;

        public IReadOnlyList<FaseBoss> Fases => fases;
        public float TempoDeTrocaDeFase => tempoDeTrocaDeFase;
    }
}
