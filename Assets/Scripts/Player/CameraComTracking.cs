using UnityEngine;

namespace Player
{
    public class CameraComTracking : MonoBehaviour
    {
        [Header("Alvos")]
        [SerializeField] Transform trObjetoParaSeguir; //o barco
        [SerializeField] Transform trBoss; //o inimigo travado (lock-on). Vazio = câmera só segue a frente do barco

        [Header("Posicao da camera")]
        [SerializeField] float distanciaAtras = 20f; //quanto a câmera fica atrás do barco (na linha barco -> boss)
        [SerializeField] float altura = 8f; //quanto a câmera fica acima do barco
        [SerializeField] float deslocamentoLateral = 0f; //desloca a câmera para o lado (0 = barco no centro da tela)
        [SerializeField] float velMovimento = 8f; //quão rápido a câmera chega na posição nova (maior = mais "dura")

        [Header("Enquadramento")]
        [SerializeField, Range(0f, 1f)] float pesoDoBoss = 0.6f; //para onde a câmera mira: 0 = no barco | 1 = no boss
        [SerializeField] float alturaDoOlhar = 3f; //sobe o ponto de mira
        [SerializeField] float velRotacao = 12f; //quão rápido a câmera gira para a direção nova (maior = mais "dura")

        const float distanciaMinimaDoBoss = 1f; //abaixo disso a direção barco -> boss fica instável

        Transform meuTr;
        Vector3 direcaoNoChao = Vector3.forward;

        void Start()
        {
            meuTr = transform;

            //começo já na posição certa: sem isso a câmera "voa" pelo cenário (e atravessa o barco) até chegar
            AtualizarDirecao();
            meuTr.position = CalcularPosicao();
            meuTr.rotation = CalcularRotacao(meuTr.position);
        }

        void LateUpdate() //depois do barco se mover, para a câmera não tremer
        {
            AtualizarDirecao(); //1) pego a direção barco -> boss
            PosicionaCamera(); //2) levo a câmera para trás do barco, nessa linha
            RotacionaCameraParaAlvo(); //3) giro a câmera para enquadrar barco e boss
        }

        private void AtualizarDirecao()
        {
            Vector3 direcao;
            if (trBoss != null)
            {
                direcao = Vector3.ProjectOnPlane(trBoss.position - trObjetoParaSeguir.position, Vector3.up); //destino menos origem, sem altura
                if (direcao.magnitude < distanciaMinimaDoBoss)
                {
                    return; //barco em cima do boss: mantenho a última direção para a câmera não girar loucamente
                }
            }
            else
            {
                direcao = Vector3.ProjectOnPlane(trObjetoParaSeguir.forward, Vector3.up); //sem boss: sigo a frente do barco
            }

            if (direcao != Vector3.zero)
            {
                direcaoNoChao = direcao.normalized;
            }
        }

        private Vector3 CalcularPosicao()
        {
            Vector3 direitaNoChao = Vector3.Cross(Vector3.up, direcaoNoChao); //a direita da linha barco -> boss

            Vector3 posicao = trObjetoParaSeguir.position; //começo na posição do barco
            posicao -= direcaoNoChao * distanciaAtras; //vou para trás (lado oposto ao boss)
            posicao += Vector3.up * altura; //para cima
            posicao += direitaNoChao * deslocamentoLateral; //para o lado
            return posicao;
        }

        private Quaternion CalcularRotacao(Vector3 posicaoDaCamera)
        {
            Vector3 pontoParaOlhar = trBoss != null
                ? Vector3.Lerp(trObjetoParaSeguir.position, trBoss.position, pesoDoBoss) //um ponto entre o barco e o boss
                : trObjetoParaSeguir.position + direcaoNoChao * distanciaAtras; //sem boss: um ponto na frente do barco
            pontoParaOlhar += Vector3.up * alturaDoOlhar;

            Vector3 direcaoParaOlhar = pontoParaOlhar - posicaoDaCamera; //destino menos origem
            return Quaternion.LookRotation(direcaoParaOlhar, Vector3.up); //"cima" do mundo: a câmera nunca inclina para os lados
        }

        private void PosicionaCamera()
        {
            meuTr.position = Vector3.Lerp(meuTr.position, CalcularPosicao(), Time.deltaTime * velMovimento); //suavização: do contrário a câmera fica dura
        }

        private void RotacionaCameraParaAlvo()
        {
            meuTr.rotation = Quaternion.Slerp(meuTr.rotation, CalcularRotacao(meuTr.position), velRotacao * Time.deltaTime); //suavização da rotação
        }
    }
}
