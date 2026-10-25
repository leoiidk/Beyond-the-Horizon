using UnityEngine;

namespace Player
{
    public class CameraComTracking : MonoBehaviour
    {
        [Header("Alvo")]
        [SerializeField] Transform trObjetoParaSeguir; //o barco

        [Header("Posicao da camera")]
        [SerializeField] Vector3 distanciaDoObjeto = new Vector3(0, 0, 0); //X: para o lado | Y: para cima | Z: para trás (negativo)
        [SerializeField] float velMovimento = 8f; //quão rápido a câmera chega na posição nova (maior = mais "dura")

        [Header("Para onde a camera olha")]
        [SerializeField] float distanciaParaOlharAFrente = 12f; //olha para um ponto a essa distância na frente do barco
        [SerializeField] Vector3 enquadramento = new Vector3(0, 4, 0); //ajuste fino da mira: X: para o lado | Y: para cima | Z: para frente
        [SerializeField] float velRotacao = 12f; //quão rápido a câmera gira para a direção nova (maior = mais "dura")

        [SerializeField] Transform meuTr;
        [SerializeField] Vector3 direcaoNoChao = Vector3.forward;

        void Start()
        {
            meuTr = transform;
        }
        void Update()
        {
            AtualizarDirecao(); //1) pego a "frente" do barco
            PosicionaCamera(); //2) levo a câmera para trás do barco
            RotacionaCameraParaAlvo(); //3) giro a câmera para olhar na frente do barco
        }

        private void AtualizarDirecao()
        {
            Vector3 direcao = Vector3.ProjectOnPlane(trObjetoParaSeguir.forward, Vector3.up).normalized; //tiro a altura: só me importa a direção no chão
            if (direcao != Vector3.zero)
            {
                direcaoNoChao = direcao;
            }
        }
        private void PosicionaCamera()
        {
            Vector3 direitaNoChao = Vector3.Cross(Vector3.up, direcaoNoChao); //a direita do barco

            Vector3 posicaoFinalDaCamera = trObjetoParaSeguir.position; //começo na posição do barco
            posicaoFinalDaCamera += direcaoNoChao * distanciaDoObjeto.z; //Z negativo = vou para trás
            posicaoFinalDaCamera += direitaNoChao * distanciaDoObjeto.x; //para o lado
            posicaoFinalDaCamera += Vector3.up * distanciaDoObjeto.y; //para cima

            meuTr.position = Vector3.Lerp(meuTr.position, posicaoFinalDaCamera, Time.deltaTime * velMovimento); //suavização: do contrário a câmera fica dura
        }
        private void RotacionaCameraParaAlvo()
        {
            Vector3 direitaNoChao = Vector3.Cross(Vector3.up, direcaoNoChao);

            Vector3 pontoParaOlhar = trObjetoParaSeguir.position + direcaoNoChao * distanciaParaOlharAFrente; //um ponto na frente do barco
            pontoParaOlhar += direitaNoChao * enquadramento.x; //ajustes do enquadramento
            pontoParaOlhar += Vector3.up * enquadramento.y;
            pontoParaOlhar += direcaoNoChao * enquadramento.z;

            Vector3 direcaoParaOlhar = pontoParaOlhar - meuTr.position; //destino menos origem
            Quaternion rotacaoFinal = Quaternion.LookRotation(direcaoParaOlhar, Vector3.up); //"cima" do mundo: a câmera nunca inclina para os lados

            meuTr.rotation = Quaternion.Slerp(meuTr.rotation, rotacaoFinal, velRotacao * Time.deltaTime); //suavização da rotação
        }
    }
}