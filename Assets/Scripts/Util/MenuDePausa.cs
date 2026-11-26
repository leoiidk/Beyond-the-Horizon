using UnityEngine;

// Esc abre e fecha o menu de pausa; o jogo congela enquanto ele está aberto
public class MenuDePausa : MonoBehaviour
{
    [SerializeField] GameObject painelDePausa;
    [SerializeField] GameObject painelDeOpcoes;
    [SerializeField] ScreenTransition transicao;

    bool pausado;

    void Start()
    {
        painelDePausa.SetActive(false);
        if (painelDeOpcoes != null) painelDeOpcoes.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (painelDeOpcoes != null && painelDeOpcoes.activeSelf) FecharOpcoes();
            else if (pausado) Continuar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        if (transicao != null && transicao.IsTransitioning) return;
        pausado = true;
        painelDePausa.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void Continuar()
    {
        pausado = false;
        painelDePausa.SetActive(false);
        if (painelDeOpcoes != null) painelDeOpcoes.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void AbrirOpcoes()
    {
        painelDeOpcoes.SetActive(true);
        painelDePausa.SetActive(false);
    }

    public void FecharOpcoes()
    {
        painelDeOpcoes.SetActive(false);
        painelDePausa.SetActive(true);
    }

    public void VoltarAoMenu()
    {
        Continuar();
        transicao.LoadScene("MainMenu");
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    void OnDestroy()
    {
        // nunca deixa o jogo congelado ao trocar de cena
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
