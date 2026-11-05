using UnityEngine;

public class NavegacaoBarco : MonoBehaviour
{
    Transform tr;

    [SerializeField] float velocidadeDeAvanco = 6f;
    [SerializeField] float velocidadeDeGiro = 60f; // graus por segundo
    
    [SerializeField] GameObject particula;
    [SerializeField] Transform localParticula;

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
        if (inputAvanco > 0)
        {
            SoundManager.instance.SfxMovingPlayer();
            Instantiate(particula, localParticula.position, localParticula.rotation);            
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
}
