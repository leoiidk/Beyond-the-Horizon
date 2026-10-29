using System.Collections.Generic;
using UnityEngine;

namespace Projeteis
{
    public class SpawnProjectiles : MonoBehaviour
    {
        [SerializeField] GameObject spawnPoint;
        [SerializeField] Transform trPlayer;
        [SerializeField] List<GameObject> vfx = new List<GameObject>();
    
        [SerializeField] GameObject efeitosParaSpawnar;
        [SerializeField] Vector3 direcao;

        void Start()
        {
            if (vfx.Count > 0)
            {
                efeitosParaSpawnar = vfx[0];
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                SpawnVFX();
            }
        }

        private void SpawnVFX()
        {

            if (spawnPoint != null)
            {
                direcao = (trPlayer.forward - spawnPoint.transform.position).normalized;
                efeitosParaSpawnar.GetComponent<MovimentoDosProjectiles>().DirecaoDoDisparo(spawnPoint.transform.position, Quaternion.LookRotation(direcao, Vector3.up));
                Instantiate(efeitosParaSpawnar);
            }

        }
   
    }
}
    
