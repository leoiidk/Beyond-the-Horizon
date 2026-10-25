using UnityEngine;

namespace Commander
{
    public class LookAt : MonoBehaviour
    {
        [SerializeField] Transform tr;
        [SerializeField] Transform trAlvo;
        [SerializeField] float velRotacao = 3f;
        void Start()
        {
            tr = transform;
        }

        // Update is called once per frame
        void Update()
        {
            tr.forward = Vector3.Slerp(tr.forward, trAlvo.position - tr.position, Time.deltaTime * velRotacao);
        }
    }
}
