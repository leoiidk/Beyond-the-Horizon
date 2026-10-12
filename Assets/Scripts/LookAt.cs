using UnityEngine;

public class LookAt : MonoBehaviour
{
    Transform tr;
    [SerializeField] Transform trAlvo;
    [SerializeField] float velRotacao = 3;


    void Start()
    {
        tr = transform;
    }

    void Update()
    {
        tr.forward = Vector3.Slerp(tr.forward, trAlvo.position - tr.position, Time.deltaTime * velRotacao);
    }
}
