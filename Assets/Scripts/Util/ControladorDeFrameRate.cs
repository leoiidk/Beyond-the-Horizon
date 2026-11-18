using UnityEngine;

namespace Util
{
    public class ControladorDeFrameRate : MonoBehaviour
    {
        [SerializeField] int frameRateAlvo = 60; //FPS m�ximo do jogo
        [SerializeField] KeyCode teclaAlternar = KeyCode.P; //Liga/desliga o limite de FPS
        [SerializeField] bool limiteAtivo = true;
        [SerializeField] bool usarVSync = true; //Sincroniza com o monitor (ignora o frameRateAlvo enquanto ligado)
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
            }

            framesContados++;
            tempoAcumulado += Time.unscaledDeltaTime;
            if (tempoAcumulado >= intervaloDoLog)
            {
                framesContados = 0;
                tempoAcumulado = 0f;
            }
        }

        void AplicarLimite()
        {
            if (limiteAtivo)
            {
                QualitySettings.vSyncCount = usarVSync ? 1 : 0;
                Application.targetFrameRate = frameRateAlvo;
            }
            else
            {
                QualitySettings.vSyncCount = vSyncOriginal;
                Application.targetFrameRate = frameRateOriginal;
            }
        }
    }
}
