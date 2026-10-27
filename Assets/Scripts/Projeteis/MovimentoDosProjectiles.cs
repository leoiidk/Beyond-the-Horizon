using UnityEngine;

namespace Projeteis
{
    public class MovimentoDosProjectiles : MonoBehaviour
    {
        [SerializeField] Transform tr;
        [SerializeField] float velocidade = 1f;
        [SerializeField] float cadenciaDeTiro = 1f;
        [SerializeField] float vida = 3f;

        void Start()
        {
            tr = transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (velocidade != 0)
            {
                tr.position += tr.forward * (velocidade * Time.deltaTime);
            }
            TempoDeVida();
        }
        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

        public float PropriedadeCadenciaDeTiro
        {
            get => cadenciaDeTiro;
            set => cadenciaDeTiro = value;
        }

        void TempoDeVida()
        {
            vida -= Time.deltaTime;

            if(vida <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
