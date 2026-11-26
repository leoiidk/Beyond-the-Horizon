using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

namespace Projeteis
{
    public class SpawnProjectiles : MonoBehaviour
    {
        [SerializeField] GameObject spawnPoint;
        [SerializeField] Transform trPlayer;
        [SerializeField] Vector3 direcao;
        [SerializeField] PlayerData playerData;
        [SerializeField] List<GameObject> vfx = new List<GameObject>();

        [SerializeField] GameObject efeitosParaSpawnar;

        [SerializeField]float cadenciaDeTiro;
        float tempoParaProximoTiro;
        
        [SerializeField] private TMP_Text text;

        void Start()
        {
            if (playerData == null)
            {
                Debug.LogError("[SpawnProjectiles] playerData não configurado no Inspector", this);
            }

            if (vfx.Count > 0)
            {
                efeitosParaSpawnar = vfx[0];
                cadenciaDeTiro = efeitosParaSpawnar.GetComponent<MovimentoDosProjectiles>().PropriedadeCadenciaDeTiro;
            }
            text.text = playerData.Bullets.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (tempoParaProximoTiro > 0)
            {
                tempoParaProximoTiro -= Time.deltaTime; //vou descontando o tempo que passou desde o ultimo tiro
            }
            // o texto acompanha a munição (inclusive quando o barco coleta balas)
            if (text.text != playerData.Bullets.ToString())
            {
                text.text = playerData.Bullets.ToString();
            }

            // jogo pausado: não atira
            if (Time.timeScale == 0f) return;

            if (Input.GetAxis("Fire1") > 0 && playerData.HasBullets)
            {
                TentarAtirar();
            }
        }
        void TentarAtirar()
        {
            if (tempoParaProximoTiro <= 0 && SpawnVFX())
            {
                playerData.TryUseBullet();
                tempoParaProximoTiro = cadenciaDeTiro;
                SoundManager.instance.SfxAttackPlayer();
                text.text = playerData.Bullets.ToString();
            }

        }
        // retorna true se o projétil foi criado
        private bool SpawnVFX()
        {
            if (spawnPoint == null) return false;
            
            direcao = (trPlayer.forward - spawnPoint.transform.position).normalized;
            efeitosParaSpawnar.GetComponent<MovimentoDosProjectiles>()
                .DirecaoDoDisparo(spawnPoint.transform.position, Quaternion.LookRotation(direcao, Vector3.up));
            Instantiate(efeitosParaSpawnar);
            return true;
        }

    }
}
