using UnityEngine;

public class FlutuacaoBarco : MonoBehaviour
{
    Transform tr;

    [Header("Água (fonte dos parâmetros da onda)")]
    [SerializeField] Renderer rendererDaAgua; // renderer cujo material usa o shader Ayo_ToonWater_Ondas

    [Header("Flutuação")]
    [SerializeField] float alturaDeFlutuacao = 0.1f; // offset entre o casco e a superfície da onda
    [SerializeField] float raioDeAmostragem = 1.5f;  // "meio-comprimento" do casco p/ estimar inclinação
    [SerializeField] float velocidadeDeInclinacao = 4f;

    const float DOIS_PI = Mathf.PI * 2f;

    static readonly int idAmplitude = Shader.PropertyToID("_Vertex_Wave_Amplitude");
    static readonly int idFrequencia = Shader.PropertyToID("_Vertex_Wave_Frequency");
    static readonly int idVelocidadeAngular = Shader.PropertyToID("_Vertex_Wave_Angular_Speed");
    static readonly int idFase = Shader.PropertyToID("_Vertex_Wave_Phase");
    static readonly int idDirecao = Shader.PropertyToID("_Vertex_Wave_Direction");

    Transform trAgua;
    Material materialDaAgua;

    void Start()
    {
        tr = transform;
        trAgua = rendererDaAgua.transform;
        materialDaAgua = rendererDaAgua.sharedMaterial;
    }

    // LateUpdate: o _Time do shader avança por frame renderizado, então amostramos no mesmo ritmo
    void LateUpdate()
    {
        Vector3 pos = tr.position;

        pos.y = AlturaDaOnda(pos.x, pos.z) + alturaDeFlutuacao;
        tr.position = pos;

        AplicarInclinacao(pos);
    }

    // Retorna a altura em espaço de mundo da superfície da água sob (x, z) de mundo.
    // Espelha a função OndaVertical do shader, que opera em espaço de objeto.
    public float AlturaDaOnda(float x, float z)
    {
        float amplitude = materialDaAgua.GetFloat(idAmplitude);
        float frequencia = materialDaAgua.GetFloat(idFrequencia);
        float velocidadeAngular = materialDaAgua.GetFloat(idVelocidadeAngular);
        float faseInicial = materialDaAgua.GetFloat(idFase);
        Vector4 direcaoOnda = materialDaAgua.GetVector(idDirecao);

        Vector3 local = trAgua.InverseTransformPoint(new Vector3(x, trAgua.position.y, z));

        float periodo = DOIS_PI / Mathf.Max(velocidadeAngular, 0.0001f);
        float tempoCiclico = Mathf.Repeat(Time.timeSinceLevelLoad / periodo, 1f) * DOIS_PI; // equivale a _Time.y
        Vector2 dir = new Vector2(direcaoOnda.x + 1e-5f, direcaoOnda.y + 1e-5f).normalized;
        float fase = frequencia * (local.x * dir.x + local.z * dir.y) + faseInicial + tempoCiclico;

        Vector3 pontoLocal = new Vector3(local.x, amplitude * Mathf.Sin(fase), local.z);
        return trAgua.TransformPoint(pontoLocal).y;
    }

    void AplicarInclinacao(Vector3 posAtual)
    {
        Vector3 frenteXZ = Vector3.ProjectOnPlane(tr.forward, Vector3.up).normalized * raioDeAmostragem;
        Vector3 direitaXZ = Vector3.ProjectOnPlane(tr.right, Vector3.up).normalized * raioDeAmostragem;

        float hFrente = AlturaDaOnda(posAtual.x + frenteXZ.x, posAtual.z + frenteXZ.z);
        float hTras = AlturaDaOnda(posAtual.x - frenteXZ.x, posAtual.z - frenteXZ.z);
        float hDireita = AlturaDaOnda(posAtual.x + direitaXZ.x, posAtual.z + direitaXZ.z);
        float hEsquerda = AlturaDaOnda(posAtual.x - direitaXZ.x, posAtual.z - direitaXZ.z);

        Vector3 tangenteFrente = new Vector3(frenteXZ.x * 2f, hFrente - hTras, frenteXZ.z * 2f);
        Vector3 tangenteDireita = new Vector3(direitaXZ.x * 2f, hDireita - hEsquerda, direitaXZ.z * 2f);

        Vector3 normalDaOnda = Vector3.Cross(tangenteDireita, tangenteFrente).normalized;
        if (normalDaOnda.y < 0f)
            normalDaOnda = -normalDaOnda; // garante "pra cima"

        Vector3 frenteAchatada = Vector3.ProjectOnPlane(tr.forward, Vector3.up).normalized;
        if (frenteAchatada.sqrMagnitude < 0.0001f)
            frenteAchatada = Vector3.forward;

        Quaternion rotacaoAlvo = Quaternion.LookRotation(frenteAchatada, normalDaOnda);
        tr.rotation = Quaternion.Slerp(tr.rotation, rotacaoAlvo, velocidadeDeInclinacao * Time.deltaTime);
    }
}
