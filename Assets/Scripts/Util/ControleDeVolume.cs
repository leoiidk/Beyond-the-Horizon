using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControleDeVolume : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider sliderGeral;
    [SerializeField] Slider sliderMusica;
    [SerializeField] Slider sliderEfeitos;

    public const string Geral = "VolumeGeral";
    public const string Musica = "VolumeMusica";
    public const string Efeitos = "VolumeEfeitos";

    void Start()
    {
        AplicarVolumesSalvos(mixer);
        Ligar(sliderGeral, Geral);
        Ligar(sliderMusica, Musica);
        Ligar(sliderEfeitos, Efeitos);
    }

    void Ligar(Slider slider, string nome)
    {
        if (slider == null) return;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = PlayerPrefs.GetFloat(nome, 0.8f);
        Aplicar(mixer, nome, slider.value);
        slider.onValueChanged.AddListener(valor =>
        {
            Aplicar(mixer, nome, valor);
            PlayerPrefs.SetFloat(nome, valor);
        });
    }

    static void Aplicar(AudioMixer mixer, string nome, float valor)
    {
        if (mixer == null) return;
        mixer.SetFloat(nome, Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20f);
    }

    public static void AplicarVolumesSalvos(AudioMixer mixer)
    {
        Aplicar(mixer, Geral, PlayerPrefs.GetFloat(Geral, 0.8f));
        Aplicar(mixer, Musica, PlayerPrefs.GetFloat(Musica, 0.8f));
        Aplicar(mixer, Efeitos, PlayerPrefs.GetFloat(Efeitos, 0.8f));
    }
}
