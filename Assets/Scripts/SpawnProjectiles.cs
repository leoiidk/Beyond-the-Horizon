using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] Transform trPlayer;
    [SerializeField] List<GameObject> vfx = new List<GameObject>();
    [SerializeField] float DelayParaAtirar = 0f;

    GameObject efeitosParaSpawnar;
    GameObject particles;
    Vector3 direcao;

    void Start()
    {
        efeitosParaSpawnar = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= DelayParaAtirar) 
        {
            DelayParaAtirar = Time.time + 1 / efeitosParaSpawnar.GetComponent<MovimentoDosProjectiles>().PropriedadeCadenciaDeTiro;
            SpawnVFX();
            
        }
    }

    private void SpawnVFX()
    {
        GameObject vfx;

        if (spawnPoint != null)
        {
            direcao = trPlayer.transform.forward;
            vfx = Instantiate(efeitosParaSpawnar, spawnPoint.transform.position, Quaternion.LookRotation(direcao, Vector3.up));
            
        }

    }
   
}
    
