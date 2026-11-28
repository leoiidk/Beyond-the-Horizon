using UnityEngine;

// Créditos: SPACE pausa/continua a rolagem e ESC volta para o menu
public class PausarCreditos : MonoBehaviour
{
    [SerializeField] Animator animacaoDosCreditos;
    [SerializeField] GameObject avisoPausado;
    [SerializeField] ScreenTransition transicao;

    void Start()
    {
        if (avisoPausado != null) avisoPausado.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool pausado = animacaoDosCreditos.speed == 0f;
            animacaoDosCreditos.speed = pausado ? 1f : 0f;
            if (avisoPausado != null) avisoPausado.SetActive(!pausado);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && transicao != null)
        {
            animacaoDosCreditos.speed = 1f;
            transicao.LoadScene("MainMenu");
        }
    }
}
