using UnityEngine;

namespace Projeteis
{
    [CreateAssetMenu(menuName = "Boss/Fábricas/Fábrica por Instanciação", fileName = "FabricaPorInstanciacao")]
    public class FabricaPorInstanciacao : FabricaDeProjeteisConfig
    {
        [Tooltip("Prefab com um componente que implemente IProjetilBoss (ex.: MisselEmArco)")]
        [SerializeField] GameObject prefab;

        public override IProjetilBoss Criar(Vector3 posicao, Quaternion rotacao)
        {
            if (prefab == null)
            {
                Debug.LogError($"{name}: nenhum prefab definido na fábrica.", this);
                return null;
            }

            GameObject instancia = Instantiate(prefab, posicao, rotacao);
            IProjetilBoss projetil = instancia.GetComponent<IProjetilBoss>();

            if (projetil == null)
            {
                Debug.LogError($"{name}: o prefab {prefab.name} não tem um componente IProjetilBoss.", this);
                Destroy(instancia);
            }

            return projetil;
        }
    }
}
