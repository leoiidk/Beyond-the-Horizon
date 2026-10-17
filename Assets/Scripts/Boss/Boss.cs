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
    private float vidasRestantes = 2f;
    bool estaMorto = false;
    
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
        if (vidaAtual <= 0 && !estaMorto)
        {
            estaMorto = true;
            bossMesh.SetActive(false);
            Collider colisorBoss = GetComponent<Collider>();
            colisorBoss.enabled = false;
            
            StartCoroutine(ResetarBoss());
        }
        
        if (vidasRestantes > 0 && estaMorto)
        {
            StartCoroutine(ResetarBoss());

        }
        else if(vidasRestantes <= 0 && estaMorto)
        {
            barraDeVida.gameObject.SetActive(false);
            Debug.Log("Boss derrotado definitivamente!");
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
        vidasRestantes -= 1;
        Debug.Log("Começou o reset. Vidas restantes: " + vidasRestantes);
        
        yield return new WaitForSeconds(5f);

        vidaAtual = vidaMaxima;
        barraDeVida.maxValue = vidaMaxima;
        barraDeVida.value = vidaAtual;
        bossMesh.SetActive(true);
        Collider colisorBoss = GetComponent<Collider>();
        colisorBoss.enabled = true;
        estaMorto = false;

    }
}
