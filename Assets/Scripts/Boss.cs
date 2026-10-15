using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [SerializeField] Slider barraDeVida;
    [SerializeField] private float vidaMaxima = 100f;
    [SerializeField] private float vidaAtual;
    [SerializeField] private float danoProjetil = 10f;
    [SerializeField] GameObject playerProcjectile;
    [SerializeField] GameObject bossMesh;
    bool foiMorto = false;
    
    void Start()
    {
        vidaAtual = vidaMaxima;
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;
        }
    }

    void Update()
    {
        Morrer();
    }

    void TomarDano()
    {
        vidaAtual -= danoProjetil;

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaAtual;
        }
    }

    void Morrer()
    {
        if (vidaAtual <= 0 && !foiMorto)
        {
            foiMorto = true;
            bossMesh.SetActive(false);
            Collider colisorBoss = GetComponent<Collider>();
            colisorBoss.enabled = false;
            
            StartCoroutine(ResetarBoss());
        }
    }

    private void OnCollisionEnter(Collision objetoQueColidiu)
    {
        if (objetoQueColidiu.gameObject.CompareTag("PlayerProjectile"))
        {
            TomarDano();
        }
    }
    private IEnumerator ResetarBoss()
    {
        yield return new WaitForSeconds(5f);

        vidaAtual = vidaMaxima;

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaAtual;
        }

        bossMesh.SetActive(true);
        Collider colisorBoss = GetComponent<Collider>();
        colisorBoss.enabled = true;
    }
}
