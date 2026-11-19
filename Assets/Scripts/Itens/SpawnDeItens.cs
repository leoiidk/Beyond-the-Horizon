using Player;
using UnityEngine;

namespace Itens
{
    public class SpawnDeItens : MonoBehaviour
    {
        [Header("Item")]
        [Tooltip("Objeto 3D do item que vai aparecer no mapa (precisa do ItemDeMunicao)")]
        [SerializeField] GameObject prefabItem;
        [Tooltip("Tempo em segundos que o item fica visível e consumível")]
        [SerializeField, Min(0)] float tempoVisivel = 3f;

        [Header("Referências")]
        [Tooltip("Balas e posição do player: o item aparece quando as balas acabam")]
        [SerializeField] PlayerData playerData;
        [Tooltip("Centro da zona negativa e da zona segura (usa a posição atual do boss)")]
        [SerializeField] Transform trBoss;
        [Tooltip("Limite do cenário (hoje é o \"invisible wall\")")]
        [SerializeField] Collider limiteDoCenario;

        [Header("Zonas (diâmetro)")]
        [Tooltip("Zona em volta do boss onde o item NÃO pode aparecer")]
        [SerializeField] float diametroZonaNegativa = 20f;
        [Tooltip("Zona em volta do boss que serve de base para o spawn")]
        [SerializeField] float diametroZonaSegura = 60f;
        [Tooltip("Raio em volta do player onde o item NÃO pode aparecer")]
        [SerializeField, Min(0)] float raioLongeDoPlayer = 15f;
        [Tooltip("Altura (Y) em que o item aparece, ex.: nível da água")]
        [SerializeField] float alturaDoSpawn;
        [SerializeField] int maxTentativas = 30;

        GameObject itemAtual;

        void Awake()
        {
            if (playerData == null)
            {
                Debug.LogError("[SpawnDeItens] playerData não configurado no Inspector", this);
            }
        }

        void Update()
        {
            // um item por vez; se ele sumir sem ser pego e o player seguir sem balas, nasce outro
            if (playerData != null && !playerData.HasBullets && itemAtual == null)
            {
                itemAtual = SpawnarItem();
            }
        }

        [ContextMenu("Spawnar Item")]
        public GameObject SpawnarItem()
        {
            if (prefabItem == null || trBoss == null)
            {
                Debug.LogWarning("[SpawnDeItens] prefabItem ou trBoss não configurado", this);
                return null;
            }

            for (int i = 0; i < maxTentativas; i++)
            {
                Vector3 ponto = SortearPontoNoAnel();
                if (EstaDentroDoLimite(ponto) && EstaLongeDoPlayer(ponto))
                {
                    GameObject item = Instantiate(prefabItem, ponto, Quaternion.identity);
                    Destroy(item, tempoVisivel);
                    return item;
                }
            }

            Debug.LogWarning($"[SpawnDeItens] Nenhum ponto válido em {maxTentativas} tentativas. Confira os diâmetros das zonas e o limite do cenário.", this);
            return null;
        }

        // sorteio uniforme na área entre a zona negativa e a zona segura
        Vector3 SortearPontoNoAnel()
        {
            float raioNegativo = diametroZonaNegativa * 0.5f;
            float raioSeguro = diametroZonaSegura * 0.5f;

            float raio = Mathf.Sqrt(Random.Range(raioNegativo * raioNegativo, raioSeguro * raioSeguro));
            float angulo = Random.Range(0f, Mathf.PI * 2f);

            Vector3 centro = CentroDasZonas();
            return new Vector3(centro.x + Mathf.Cos(angulo) * raio, alturaDoSpawn, centro.z + Mathf.Sin(angulo) * raio);
        }

        bool EstaDentroDoLimite(Vector3 ponto)
        {
            if (limiteDoCenario == null) return true;

            Bounds limites = limiteDoCenario.bounds;
            if (ponto.x < limites.min.x || ponto.x > limites.max.x || ponto.z < limites.min.z || ponto.z > limites.max.z)
            {
                return false;
            }

            // se o caminho do boss até o ponto atravessa a parede, o ponto está fora do cenário
            // testa nos dois sentidos porque o raycast só acerta a face da frente do MeshCollider
            Vector3 centro = CentroDasZonas();
            return !AtravessaLimite(centro, ponto) && !AtravessaLimite(ponto, centro);
        }

        bool AtravessaLimite(Vector3 origem, Vector3 destino)
        {
            Vector3 direcao = destino - origem;
            RaycastHit[] hits = Physics.RaycastAll(origem, direcao.normalized, direcao.magnitude, ~0, QueryTriggerInteraction.Collide);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == limiteDoCenario) return true;
            }
            return false;
        }

        // distância só no plano XZ, a altura do barco não conta
        bool EstaLongeDoPlayer(Vector3 ponto)
        {
            if (playerData == null) return true;

            Vector3 posPlayer = playerData.transform.position;
            Vector2 diferenca = new Vector2(ponto.x - posPlayer.x, ponto.z - posPlayer.z);
            return diferenca.sqrMagnitude >= raioLongeDoPlayer * raioLongeDoPlayer;
        }

        Vector3 CentroDasZonas()
        {
            return new Vector3(trBoss.position.x, alturaDoSpawn, trBoss.position.z);
        }

        void OnValidate()
        {
            if (diametroZonaSegura < diametroZonaNegativa)
            {
                diametroZonaSegura = diametroZonaNegativa;
            }
        }

        void OnDrawGizmos()
        {
            if (trBoss != null)
            {
                // círculos achatados no plano XZ
                Gizmos.matrix = Matrix4x4.TRS(CentroDasZonas(), Quaternion.identity, new Vector3(1f, 0.001f, 1f));

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Vector3.zero, diametroZonaNegativa * 0.5f);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(Vector3.zero, diametroZonaSegura * 0.5f);

                Gizmos.matrix = Matrix4x4.identity;
            }

            if (playerData != null)
            {
                Vector3 posPlayer = playerData.transform.position;
                Gizmos.matrix = Matrix4x4.TRS(new Vector3(posPlayer.x, alturaDoSpawn, posPlayer.z), Quaternion.identity, new Vector3(1f, 0.001f, 1f));
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(Vector3.zero, raioLongeDoPlayer);
                Gizmos.matrix = Matrix4x4.identity;
            }

            if (limiteDoCenario != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(limiteDoCenario.bounds.center, limiteDoCenario.bounds.size);
            }
        }
    }
}
