using System.Collections;
using UnityEngine;

public class MovimentoDosProjectiles : MonoBehaviour
{
    Transform tr;
    [SerializeField] float velocidade = 1f;
    [SerializeField] float cadenciaDeTiro = 1f;
    [SerializeField] float vida = 3f;

    void Start()
    {
        tr = transform;
    }

    // Update is called once per frame
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
        Destroy(gameObject);
    }

    public float PropriedadeCadenciaDeTiro
    {
        get
        { return cadenciaDeTiro;  }
        set
        {
            cadenciaDeTiro = value;
        }
        
    }

    void tempoDeVida()
    {
        vida -= Time.deltaTime;

        if(vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
