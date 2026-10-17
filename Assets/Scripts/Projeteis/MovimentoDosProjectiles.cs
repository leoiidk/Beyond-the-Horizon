using System.Collections;
using UnityEngine;

public class MovimentoDosProjectiles : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    TrailRenderer[] rastros;
    SpawnProjectiles spawner;
    [SerializeField] float velocidade = 1f;
    [SerializeField] float cadenciaDeTiro = 1f; //tempo minimo (em segundos) entre um tiro e outro desse projetil. Usado pelo SpawnProjectiles
    [SerializeField] float vida = 3f;

    float velocidadeInicial; //guardo os valores para resetar o projetil toda vez que ele for reutilizado
    float vidaInicial;
    void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        rastros = GetComponentsInChildren<TrailRenderer>();
        velocidadeInicial = velocidade;
        vidaInicial = vida;
    }
    void Update()
    {
        if (velocidade != 0)
        {
            tr.position += tr.forward * (velocidade * Time.deltaTime);
        }
        tempoDeVida();
    }
    private void OnCollisionEnter(Collision collision)
    {
        velocidade = 0;
        vida = 0;
    }

    public float PropriedadeCadenciaDeTiro
    {
        get
        { return cadenciaDeTiro; }
        set
        {
            cadenciaDeTiro = value;
        }

    }

    public SpawnProjectiles PropriedadeSpawner
    {
        set
        {
            spawner = value;
        }
    }

    public void Disparar(Vector3 posicao, Quaternion rotacao)
    {
        //reseto tudo que mudou no uso anterior do projetil
        velocidade = velocidadeInicial;
        vida = vidaInicial;
        tr.SetPositionAndRotation(posicao, rotacao);
        rb.linearVelocity = Vector3.zero; //a colisao anterior pode ter deixado o rigidbody com velocidade/giro
        rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(true);

        for (int i = 0; i < rastros.Length; i++)
        {
            rastros[i].Clear(); //limpo o rastro, senao ele desenha uma linha da posicao antiga ate a nova
        }
    }

    void tempoDeVida()
    {
        vida -= Time.deltaTime;

        if (vida <= 0)
        {
            Desativar();
        }
    }

    void Desativar()
    {
        if (spawner != null)
        {
            spawner.DevolverAoPool(gameObject); //em vez de destruir, devolvo o projetil para o pool
        }
        else
        {
            Destroy(gameObject); //se o projetil nao veio de um pool (ex: colocado direto na cena), destruo como antes
        }
    }
}
