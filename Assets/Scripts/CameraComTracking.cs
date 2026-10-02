using System;
using UnityEngine;

public class CameraComTracking : MonoBehaviour
{
    Transform meuTr;
    [SerializeField] Transform trObjetoParaOlhar;
    [SerializeField] float velRotacao = 12f;
    [SerializeField] Transform trObjetoParaSeguir;
    [SerializeField] float velMovimento = 8f;
    [SerializeField] Vector3 distanciaDoObjeto;
    [SerializeField] Vector3 enquadramento;

    Vector3 posicaoFinalDaCamera;
    Vector3 direcaoObjetoParaOlhar;
    Vector3 direcaoFinaldaCamera;

    void Start()
    {
        meuTr = transform;    
    }

    // Update is called once per frame
    void Update()
    {
        PosicionaCamera(); //primeiro eu posiciono a camera
        RotacionaCameraParaAlvo(); //depois rotaciono ela
    }
    private void PosicionaCamera()
    {
        direcaoObjetoParaOlhar = (trObjetoParaOlhar.position - trObjetoParaSeguir.position).normalized; //Calculo do destino menos origem.
                                                                                                        //Destino para onde que eu quero olhar -  Origem da onde eu estou olhando.
                                                                                                        //Esse menos esse vai gerar um vetor que aponta para onde eu quero olhar.
        //Deslocamento da Camera nos eixos tridimencionais X, Y e Z
        posicaoFinalDaCamera = trObjetoParaSeguir.position; // primeiro coloco a camera na mesma posicao de quem ela vai seguir, nesse caso barquinho;
        posicaoFinalDaCamera += Vector3.ProjectOnPlane(direcaoObjetoParaOlhar, Vector3.up).normalized * distanciaDoObjeto.z; // o deslocamento em Z acontece
                                                                                                                             // em relacao ao que eu estou olhando.
                                                                                                                             // Então, se minha câmera está olhando
                                                                                                                             // para esse objeto aqui,eu quero me distanciar
                                                                                                                             //Ignoro o Y por meio da projecao e "achato" ela em relacao ao mundo
                                                                                                                             //Vector3.up normal universal do mundo
                                                   
        posicaoFinalDaCamera += meuTr.right * distanciaDoObjeto.x; //deslocamento em x, meuTr.right = o quanto eu quero deslocar para direita, vezes a distancia em X que quero do objeto 
        posicaoFinalDaCamera += Vector3.up * distanciaDoObjeto.y; // deslocamento em y , o quanto eu quero deslocar para cima, vezes a distancia em Y que quero do objeto

        meuTr.position = Vector3.Lerp(meuTr.position, posicaoFinalDaCamera, Time.deltaTime * velMovimento); //suavização desse deslocamento. Do contrário, a câmera fica dura.
    }
    private void RotacionaCameraParaAlvo()
    {
        direcaoFinaldaCamera = trObjetoParaOlhar.position - meuTr.position; //Destino menos origem.
        direcaoFinaldaCamera += direcaoObjetoParaOlhar * enquadramento.z; //somo vetor de direcao e multiplico pelo valor do enquadramento da camera em Z para ter a direcao final da camera em Z 
        direcaoFinaldaCamera += meuTr.right * enquadramento.x; //somo meu vetor da direita e multiplico pelo valor do enquadramento em X para ter a direcao final da camera em X 
        direcaoFinaldaCamera += Vector3.up * enquadramento.y; //somo a normal universal do mundo e multplico pelo enquadramento em Y para ter a direcao final da camera em Y
        meuTr.forward = Vector3.Slerp(meuTr.forward, direcaoFinaldaCamera, velRotacao * Time.deltaTime); //suavização dessa rotacao. Do contrário, a câmera fica dura.
    }
}

