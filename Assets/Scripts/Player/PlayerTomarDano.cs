using System.Collections;
using Projeteis;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerTomarDano : MonoBehaviour,IDanificavel
    {
        [SerializeField] SpawnProjectiles tiroDoPlayer;
        [SerializeField] PlayerData playerData;
        [Tooltip("Script de movimento desligado ao morrer (NavegacaoBarco ou MovimentacaoTerceiraPessoa). Se vazio, procura NavegacaoBarco no player")]
        [SerializeField] Behaviour movimentacao;
        [SerializeField] float tempoDeRespawn = 2f;

        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (movimentacao == null) movimentacao = GetComponent<NavegacaoBarco>();

            if (playerData == null)
            {
                Debug.LogError("[PlayerTomarDano] playerData não configurado no Inspector", this);
            }
        }
        public void ReceberDano(float dano)
        {
            if (!playerData.IsAlive) return;
            
            playerData.TakeHit();
            Debug.Log($"VIDA: {playerData.Life}");

            if (!playerData.IsAlive)
            {
                Morrer();
            }
        }
        private void Morrer()
        {
            // nada aqui pode quebrar antes do StartCoroutine, senão a luta nunca recomeça
            if (movimentacao != null) movimentacao.enabled = false;
            if (rb != null) rb.linearVelocity = Vector3.zero;
            if (tiroDoPlayer != null) tiroDoPlayer.enabled = false;

            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }

            StartCoroutine(RecomecarLuta());
        }
        
        IEnumerator RecomecarLuta()
        {
            yield return new WaitForSeconds(tempoDeRespawn);

            // só restaura depois da espera: durante o respawn o player tem que continuar morto
            playerData.Resurrect();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
