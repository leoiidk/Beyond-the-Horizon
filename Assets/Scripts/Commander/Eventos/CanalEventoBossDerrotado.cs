using System;
using UnityEngine;

namespace Commander.Eventos
{
    [CreateAssetMenu(menuName = "Boss/Eventos/Canal Boss Derrotado", fileName = "CanalBossDerrotado")]
    public class CanalEventoBossDerrotado : ScriptableObject
    {
        public event Action AoDisparar;

        public void Disparar()
        {
            AoDisparar?.Invoke();
        }
    }
}
