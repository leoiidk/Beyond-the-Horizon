using System;
using System.Collections;
using Projeteis;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerTeste : MonoBehaviour,IDanificavel
    {
        [SerializeField] SpawnProjectiles tiroDoPlayer;
        [SerializeField] float tempoDeRespawn = 2f;

        Rigidbody rb;
        MovimentacaoTerceiraPessoa movimentacao;
        bool estaMorto;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            movimentacao = GetComponent<MovimentacaoTerceiraPessoa>();
            tiroDoPlayer = FindFirstObjectByType<SpawnProjectiles>();
        }
        public void ReceberDano(float dano)
        {
            if(estaMorto) return;
            Debug.Log($"Player atingido: {dano}!");
            Morrer();
        }
        private void Morrer()
        {
            estaMorto = true;
            movimentacao.enabled = false;
            rb.linearVelocity = Vector3.zero;
            tiroDoPlayer.enabled = false;

            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }

            StartCoroutine(RecomecarLuta());
        }
        IEnumerator RecomecarLuta()
        {
            yield return new WaitForSeconds(tempoDeRespawn);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
