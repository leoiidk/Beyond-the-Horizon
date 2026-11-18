using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class NavegacaoBarco : MonoBehaviour
    {
        Transform tr;

        [SerializeField] float velocidadeDeAvanco = 6f;
        [SerializeField] float velocidadeDeGiro = 60f; // graus por segundo
    
        [SerializeField] GameObject particula;
        [SerializeField] Transform localParticula;

        [SerializeField] private int poolSize;
        
        private Queue<GameObject> _pool = new();

        float inputAvanco;
        float inputGiro;

        void Start()
        {
            tr = transform;
            PoolUp();
        }

        void Update()
        {
            LeInput();
        }

        void FixedUpdate()
        {
            AplicarGiro();
            AplicarAvanco();
            if (inputAvanco > 0)
            {
                SoundManager.instance.SfxMovingPlayer();
                // Instantiate(particula, localParticula.position, localParticula.rotation);
                Spawn();
            }
        }

        void LeInput()
        {
            inputAvanco = Input.GetAxis("Vertical");
            inputGiro = Input.GetAxis("Horizontal");
        }

        void AplicarGiro()
        {
            //Serve para a flutuação.
            tr.Rotate(Vector3.up, inputGiro * velocidadeDeGiro * Time.fixedDeltaTime, Space.World);
        }

        void AplicarAvanco()
        {
            Vector3 frenteAchatada = Vector3.ProjectOnPlane(tr.forward, Vector3.up).normalized;
            tr.position += frenteAchatada * inputAvanco * velocidadeDeAvanco * Time.fixedDeltaTime;
        }

        void PoolUp()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = Instantiate(particula);
                go.SetActive(false);
                _pool.Enqueue(go);
            }
        }
        
        void Spawn()
        {
            GameObject go = _pool.Dequeue();
            go.SetActive(true);
            go.transform.position = localParticula.position;
            go.transform.rotation = localParticula.rotation;
            
            _pool.Enqueue(go);
        }
    }
}
