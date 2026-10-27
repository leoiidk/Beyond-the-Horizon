using UnityEngine;

public class NavegacaoBarco : MonoBehaviour
{
    Transform tr;

    [SerializeField] float velocidadeDeAvanco = 6f;
    [SerializeField] float velocidadeDeGiro = 60f; // graus por segundo

    float inputAvanco;
    float inputGiro;

    void Start()
    {
        tr = transform;
    }

    void Update()
    {
        LeInput();
    }

    void FixedUpdate()
    {
        AplicarGiro();
        AplicarAvanco();
    }

    void LeInput()
    {
        inputAvanco = Input.GetAxis("Vertical");
        inputGiro = Input.GetAxis("Horizontal");
    }

    void AplicarGiro()
    {
        // Gira em torno do eixo mundial Y, preservando a inclinação (pitch/roll)
        // que o FlutuacaoBarco já tenha aplicado neste frame.
        tr.Rotate(Vector3.up, inputGiro * velocidadeDeGiro * Time.fixedDeltaTime, Space.World);
    }

    void AplicarAvanco()
    {
        Vector3 frenteAchatada = Vector3.ProjectOnPlane(tr.forward, Vector3.up).normalized;
        tr.position += frenteAchatada * inputAvanco * velocidadeDeAvanco * Time.fixedDeltaTime;
    }
}
