using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] Transform trPlayer;
    [SerializeField] List<GameObject> vfx = new List<GameObject>();
    [SerializeField] int tamanhoInicialDoPool = 20; //quantos projeteis ja ficam criados (e desligados) quando o jogo comeca

    GameObject efeitosParaSpawnar;
    Vector3 direcao;

    float cadenciaDeTiro; //tempo minimo (em segundos) entre um tiro e outro. Vem do prefab do projetil
    float tempoParaProximoTiro; //contador regressivo: so posso atirar de novo quando chegar em 0
    Queue<GameObject> poolDeProjeteis = new Queue<GameObject>(); //fila com os projeteis desligados, prontos para serem reutilizados

    void Start()
    {
        efeitosParaSpawnar = vfx[0];
        cadenciaDeTiro = efeitosParaSpawnar.GetComponent<MovimentoDosProjectiles>().PropriedadeCadenciaDeTiro; 
        CriarPool();
    }

    // Update is called once per frame
    void Update()
    {
        if (tempoParaProximoTiro > 0)
        {
            tempoParaProximoTiro -= Time.deltaTime; //vou descontando o tempo que passou desde o ultimo tiro
        }

        if (Input.GetKeyDown(KeyCode.Space) && tempoParaProximoTiro <= 0) //apertar o tiro durante a espera nao faz nada.
        {                                                                 //O jogador precisa escolher o momento certo de atirar

            SpawnVFX();
            tempoParaProximoTiro = cadenciaDeTiro; //comeca a contar a espera ate o proximo tiro

        }
    }
    private void SpawnVFX()
    {
        GameObject vfx;

        if (spawnPoint != null)
        {
            direcao = trPlayer.transform.forward;
            vfx = PegarProjetilDoPool(); //em vez de Instantiate, pego um projetil que ja existe e esta desligado
            vfx.GetComponent<MovimentoDosProjectiles>().Disparar(spawnPoint.transform.position, Quaternion.LookRotation(direcao, Vector3.up));

        }

    }

    #region Pooling

    // Pooling: em vez de criar (Instantiate) e destruir (Destroy) um projetil a cada tiro,
    // crio varios no comeco do jogo, deixo eles desligados e vou reutilizando.

    void CriarPool()
    {
        for (int i = 0; i < tamanhoInicialDoPool; i++)
        {
            poolDeProjeteis.Enqueue(CriarProjetil()); //crio o projetil e coloco ele na fila
        }
    }

    GameObject CriarProjetil()
    {
        GameObject projetil = Instantiate(efeitosParaSpawnar, transform); //crio como filho desse objeto so para organizar a Hierarchy
        projetil.GetComponent<MovimentoDosProjectiles>().PropriedadeSpawner = this; //o projetil precisa saber para qual pool ele volta
        projetil.SetActive(false); //comeca desligado, esperando ser usado
        return projetil;
    }

    GameObject PegarProjetilDoPool()
    {
        if (poolDeProjeteis.Count == 0) //se todos os projeteis estiverem em uso,
        {
            return CriarProjetil(); //crio mais um. Quando ele for devolvido, o pool fica maior
        }
        return poolDeProjeteis.Dequeue(); //tiro o primeiro projetil da fila
    }

    public void DevolverAoPool(GameObject projetil)
    {
        if (!projetil.activeSelf) //se ja esta desligado, ja foi devolvido. Evita colocar o mesmo projetil duas vezes na fila
        {
            return;
        }
        projetil.SetActive(false);
        poolDeProjeteis.Enqueue(projetil); //volta para o final da fila
    }

    #endregion

}

