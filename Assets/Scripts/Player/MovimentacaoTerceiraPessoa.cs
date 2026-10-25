using System;
using UnityEngine;

public class MovimentacaoTerceiraPessoa : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float velocidadeDeMovimento = 10f;
    [SerializeField] float velocidadeDeCurva = 120f; //quantos graus por segundo o barco gira segurando A ou D 120 = volta completa em 3 segundos

    Vector2 inputJogador;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        LeInput();
    }
    private void FixedUpdate()
    {
        AplicarCurva();
        AplicaMovimento();
    }

    void LeInput()
    {
        inputJogador.x = Input.GetAxis("Horizontal");
        inputJogador.y = Input.GetAxis("Vertical");

    }
    private void AplicarCurva()
    {
        //giro só em volta do eixo Y (o "cima" do mundo), então o barco nunca empina nem inclina
        Quaternion giro = Quaternion.Euler(0, inputJogador.x * velocidadeDeCurva * Time.fixedDeltaTime, 0);
        rb.MoveRotation(giro * rb.rotation);
    }
    private void AplicaMovimento()
    {
        Vector3 frenteDoBarco = rb.rotation * Vector3.forward; //para onde o barco está apontando
        Vector3 velFinal = frenteDoBarco * (inputJogador.y * velocidadeDeMovimento); 
        velFinal.y = rb.linearVelocity.y; 
        rb.linearVelocity = velFinal;
    }

}
