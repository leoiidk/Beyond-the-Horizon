using UnityEngine;

// Pilar do tutorial: leva 3 tiros e quebra.
// Quando os 3 pilares quebram, toca a animação "Despertar" do Tutorial.
public class AlvoTutorial : MonoBehaviour
{
    public Transform barco;
    public Animator animacaoDoPilar;     // animações do pilar (Acerto e Quebrar)
    public Animator animacaoDoTutorial;  // animação do final (Despertar)
    public Transform centroDaMira;       // para onde o tiro vai
    public int tirosParaQuebrar = 3;

    int tirosRecebidos = 0;
    static int pilaresQuebrados = 0;

    void Start()
    {
        pilaresQuebrados = 0;
    }

    void Update()
    {
        // se o pilar já quebrou, não faz mais nada
        if (tirosRecebidos >= tirosParaQuebrar)
        {
            return;
        }

        // o barco está de frente para este pilar?
        Vector3 frenteDoBarco = barco.forward;
        frenteDoBarco.y = 0;
        Vector3 doBarcoAteOPilar = transform.position - barco.position;
        doBarcoAteOPilar.y = 0;

        float angulo = Vector3.Angle(frenteDoBarco, doBarcoAteOPilar);
        float distancia = doBarcoAteOPilar.magnitude;

        // se estiver, todos os tiros do jogador vão na direção do pilar
        if (angulo < 30 && distancia < 70)
        {
            GameObject[] tiros = GameObject.FindGameObjectsWithTag("PlayerProjectile");
            foreach (GameObject tiro in tiros)
            {
                tiro.transform.LookAt(centroDaMira);
            }
        }
    }

    void OnCollisionEnter(Collision colisao)
    {
        // só conta o tiro do jogador
        if (colisao.gameObject.tag != "PlayerProjectile")
        {
            return;
        }

        tirosRecebidos = tirosRecebidos + 1;

        if (tirosRecebidos < tirosParaQuebrar)
        {
            animacaoDoPilar.SetTrigger("Acerto");
        }
        else if (tirosRecebidos == tirosParaQuebrar)
        {
            animacaoDoPilar.SetTrigger("Quebrar");
            GetComponent<Collider>().enabled = false;

            pilaresQuebrados = pilaresQuebrados + 1;
            if (pilaresQuebrados == 3)
            {
                animacaoDoTutorial.SetTrigger("Despertar");
            }
        }
    }
}
