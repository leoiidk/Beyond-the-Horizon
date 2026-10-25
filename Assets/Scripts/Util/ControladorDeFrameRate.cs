using UnityEngine;

public class ControladorDeFrameRate : MonoBehaviour
{
    [SerializeField] int frameRateAlvo = 60; //FPS máximo do jogo
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRateAlvo;
    }
}
