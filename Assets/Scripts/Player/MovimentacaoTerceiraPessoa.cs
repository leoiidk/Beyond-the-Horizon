using UnityEngine;

namespace Player
{
    public class MovimentacaoTerceiraPessoa : MonoBehaviour
    {
        [SerializeField] Transform tr;
        [SerializeField] Rigidbody rb;
  
        [SerializeField] Transform trCamera;
        [SerializeField] float velocidadeDeMovimento = 10f;
        [SerializeField] float velocidadeDeRotacao = 5f;

        [SerializeField] Vector3 frenteProjetada;
        [SerializeField] Vector3 velFinal;
        [SerializeField] Vector2 inputJogador;
        [SerializeField] Vector3 direcao;

        Quaternion rotacaoAlvo; //calculada no Update (input), aplicada no FixedUpdate (fisica)

        void Start()
        {
            tr = transform;
            rb = GetComponent<Rigidbody>();
            rotacaoAlvo = rb.rotation;
        }
        void Update()
        {
            LeInput();
            AplicarDirecao();
        }
        private void FixedUpdate()
        {
            AplicaMovimento();
            AplicarRotacao();
        }

        void LeInput()
        {
            inputJogador.x = Input.GetAxis("Horizontal");
            inputJogador.y = Input.GetAxis("Vertical");

        }
        private void AplicarDirecao()
        {
            if(inputJogador.magnitude > 0)
            {
                direcao = DirecaoRelativaCamera();
                CalcularRotacao();
            }
        }
        private void AplicaMovimento()
        {
            velFinal = DirecaoRelativaCamera() * velocidadeDeMovimento;
            velFinal.y = rb.linearVelocity.y;
            rb.linearVelocity = velFinal;
        }
        private void CalcularRotacao()
        {
            rotacaoAlvo = Quaternion.LookRotation(direcao, Vector3.up);
        }
        private void AplicarRotacao()
        {
            //MoveRotation gira pelo Rigidbody: respeita a interpolacao e nao briga com a fisica
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotacaoAlvo, velocidadeDeRotacao * Time.fixedDeltaTime));
        }

        private Vector3 DirecaoRelativaCamera()
        {
            frenteProjetada = Vector3.ProjectOnPlane(trCamera.forward, Vector3.up).normalized;
            Vector3 dir = trCamera.right * inputJogador.x + frenteProjetada * inputJogador.y;
            dir.y = 0;
            return dir.normalized;
        }

    }
}
