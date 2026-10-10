using System;
using UnityEngine;

public class MovimentacaoTerceiraPessoa : MonoBehaviour
{
    Rigidbody rb;
    Vector3 inputDirecao;
    [SerializeField] float velocidade = 10f;
    float dirX, dirZ;
    [SerializeField] Transform trCamera;
    [SerializeField] float velRotacao = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimentacao();
        AplicarDirecao();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = inputDirecao * velocidade;
    }

    void Movimentacao()
    {
        dirX = Input.GetAxis("Horizontal");
        dirZ = Input.GetAxis("Vertical");

    }
    private void AplicarDirecao()
    {
        AplicarRotacao();
    }

    private void AplicarRotacao()
    {
        if (inputDirecao.magnitude < 0.01f) return;

        Quaternion rotacaoAlvo = Quaternion.LookRotation(inputDirecao);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velRotacao * Time.deltaTime);
    }

}
