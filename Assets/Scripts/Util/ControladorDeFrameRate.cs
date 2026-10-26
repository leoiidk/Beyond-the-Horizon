using UnityEngine;

public class ControladorDeFrameRate : MonoBehaviour
{
    [SerializeField] int frameRateAlvo = 60; //FPS m�ximo do jogo
    [SerializeField] KeyCode teclaAlternar = KeyCode.P; //Liga/desliga o limite de FPS
    [SerializeField] bool limiteAtivo = true;
    [SerializeField] float intervaloDoLog = 1f; //Segundos entre cada log de FPS

    int vSyncOriginal;
    int frameRateOriginal;
    int framesContados;
    float tempoAcumulado;

    void Awake()
    {
        vSyncOriginal = QualitySettings.vSyncCount;
        frameRateOriginal = Application.targetFrameRate;
    }

    void Start()
    {
        AplicarLimite();
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaAlternar))
        {
            limiteAtivo = !limiteAtivo;
            AplicarLimite();
            // Debug.Log(limiteAtivo
            //     ? $"[FrameRate] Limite LIGADO ({frameRateAlvo} FPS)"
            //     : "[FrameRate] Limite DESLIGADO");
        }

        //M�dia de FPS no intervalo (unscaled para n�o depender do timeScale)
        framesContados++;
        tempoAcumulado += Time.unscaledDeltaTime;
        if (tempoAcumulado >= intervaloDoLog)
        {
            // float fps = framesContados / tempoAcumulado;
            //Debug.Log($"[FrameRate] FPS atual: {fps:F1} | Limite: {(limiteAtivo ? "ligado" : "desligado")}");
            framesContados = 0;
            tempoAcumulado = 0f;
        }
    }

    void AplicarLimite()
    {
        if (limiteAtivo)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRateAlvo;
        }
        else
        {
            QualitySettings.vSyncCount = vSyncOriginal;
            Application.targetFrameRate = frameRateOriginal;
        }
    }
}
