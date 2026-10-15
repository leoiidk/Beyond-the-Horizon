using System;
using UnityEngine;

public class MovimentacaoTerceiraPessoa : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
  
    [SerializeField] Transform trCamera;
    [SerializeField] float velocidadeDeMovimento = 10f;
    [SerializeField] float velocidadeDeRotacao = 5f;

    Vector3 frenteProjetada;
    Vector3 velFinal;
    Vector2 inputJogador;
    Vector3 direcao;

    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        LeInput();
        AplicarDirecao();
    }
    private void FixedUpdate()
    {
        AplicaMovimento();
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
            direcao = velFinal.normalized;
            direcao.y = 0;
            AplicarRotacao();
        }
    }
    private void AplicaMovimento()
    {
        velFinal = trCamera.right * inputJogador.x;
        frenteProjetada = Vector3.ProjectOnPlane(trCamera.forward, Vector3.up).normalized;
        velFinal += frenteProjetada * inputJogador.y;
        velFinal *= velocidadeDeMovimento;
        velFinal.y = rb.linearVelocity.y;
        rb.linearVelocity = velFinal;
    }
    private void AplicarRotacao()
    {
       tr.forward = Vector3.Slerp(tr.forward, direcao, velocidadeDeRotacao * Time.deltaTime); 
    }

}
