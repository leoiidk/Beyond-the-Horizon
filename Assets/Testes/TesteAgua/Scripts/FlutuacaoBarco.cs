using UnityEngine;

public class FlutuacaoBarco : MonoBehaviour
{
    Transform tr;

    [SerializeField] float amplitude = 0.25f;
    [SerializeField] float frequencia = 0.6f;
    [SerializeField] float velocidadeAngular = 1.2f;
    [SerializeField] float faseInicial = 0f;
    [SerializeField] Vector2 direcaoOnda = new Vector2(1f, 0f);

    [Header("Flutuação")]
    [SerializeField] float alturaDeFlutuacao = 0.1f; // offset entre o casco e a superfície da onda
    [SerializeField] float raioDeAmostragem = 1.5f;  // "meio-comprimento" do casco p/ estimar inclinação
    [SerializeField] float velocidadeDeInclinacao = 4f;

    const float DOIS_PI = Mathf.PI * 2f;

    void Start()
    {
        tr = transform;
    }

    void FixedUpdate()
    {
        Vector3 pos = tr.position;

        float altura = AlturaDaOnda(pos.x, pos.z);
        pos.y = altura + alturaDeFlutuacao;
        tr.position = pos;

        AplicarInclinacao(pos);
    }

    float AlturaDaOnda(float x, float z)
    {
        float periodo = DOIS_PI / Mathf.Max(velocidadeAngular, 0.0001f);
        float tempoCiclico = Mathf.Repeat(Time.time / periodo, 1f) * DOIS_PI; // normalized_time(s), cap.2 "Shaders & Procedural Shapes in Unity 6"
        Vector2 dir = direcaoOnda.normalized;
        float fase = frequencia * (x * dir.x + z * dir.y) + faseInicial + tempoCiclico;
        return amplitude * Mathf.Sin(fase);
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
        tr.rotation = Quaternion.Slerp(tr.rotation, rotacaoAlvo, velocidadeDeInclinacao * Time.fixedDeltaTime);
    }
}
