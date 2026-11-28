using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Liga os sliders de volume das Opções ao Audio Mixer e guarda os valores
public class ControleDeVolume : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider sliderGeral;
    [SerializeField] Slider sliderMusica;
    [SerializeField] Slider sliderEfeitos;

    // nomes dos parâmetros expostos no mixer (também usados como chave no PlayerPrefs)
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

    // o slider vai de 0 a 1, o mixer trabalha em decibéis
    static void Aplicar(AudioMixer mixer, string nome, float valor)
    {
        if (mixer == null) return;
        mixer.SetFloat(nome, Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20f);
    }

    // chamado no começo de cada cena para usar os volumes escolhidos no menu
    public static void AplicarVolumesSalvos(AudioMixer mixer)
    {
        Aplicar(mixer, Geral, PlayerPrefs.GetFloat(Geral, 0.8f));
        Aplicar(mixer, Musica, PlayerPrefs.GetFloat(Musica, 0.8f));
        Aplicar(mixer, Efeitos, PlayerPrefs.GetFloat(Efeitos, 0.8f));
    }
}
