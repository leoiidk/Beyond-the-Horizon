using UnityEngine;

public class RastroBarco : MonoBehaviour
{
    [SerializeField] FlutuacaoBarco flutuacao;     // fonte da altura da onda
    [SerializeField] ParticleSystem particulas;    // Simulation Space = World
    [SerializeField] Transform pontoDaPopa;        // onde o rastro nasce
    [SerializeField] float alturaSobreAgua = 0.05f;

    [Header("Modulação pela velocidade")]
    [SerializeField] float velocidadeMaxima = 6f;
    [SerializeField] float taxaPorDistanciaMaxima = 4f;

    Vector3 posicaoAnterior;

    void Start()
    {
        posicaoAnterior = transform.position;
    }

    // LateUpdate, como o FlutuacaoBarco (a ordem entre os dois não é garantida; a diferença é de 1 frame)
    void LateUpdate()
    {
        Vector3 posPopa = pontoDaPopa.position;
        posPopa.y = flutuacao.AlturaDaOnda(posPopa.x, posPopa.z) + alturaSobreAgua;
        particulas.transform.position = posPopa;

        float velocidade = Vector3.ProjectOnPlane(transform.position - posicaoAnterior, Vector3.up).magnitude
                           / Mathf.Max(Time.deltaTime, 0.0001f);
        posicaoAnterior = transform.position;

        var emissao = particulas.emission;
        emissao.rateOverDistanceMultiplier = taxaPorDistanciaMaxima * Mathf.Clamp01(velocidade / velocidadeMaxima);
    }
}
