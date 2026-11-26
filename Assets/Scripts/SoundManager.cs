using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager instance;
    private void Awake()
    {
       if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] AudioSource sfxManager;

    [Header("Mixer (volumes salvos nas Opções)")]
    [SerializeField] AudioMixer mixer;

    [Header("Player")]
    [SerializeField] AudioClip movingSoundPlayer;
    [SerializeField] AudioClip attackSoundPlayer;
    [SerializeField] AudioClip damageSoundPlayer;
    [SerializeField] AudioClip dieSoundPlayer;

    [Header("Boss")]
    [SerializeField] AudioClip movingSoundBoss;
    [SerializeField] AudioClip attackSoundBoss;
    [SerializeField] AudioClip damageSoundBoss;
    [SerializeField] AudioClip dieSoundBoss;
    [SerializeField] AudioClip missilSoundBoss;
    [SerializeField] AudioClip trocaDeFaseSoundBoss;

    // som do motor e do boss parado tocam em loop, cada um no seu AudioSource
    AudioSource motor;
    AudioSource bossLoop;
    float tempoDoUltimoMovimento = -10f;
    float tempoDoUltimoDanoBoss = -10f;

    void Start()
    {
        ControleDeVolume.AplicarVolumesSalvos(mixer);

        if (movingSoundPlayer != null)
        {
            motor = CriarLoop(movingSoundPlayer);
            motor.volume = 0f;
            motor.Play();
        }

        if (movingSoundBoss != null)
        {
            bossLoop = CriarLoop(movingSoundBoss);
            bossLoop.volume = 0.5f;
            bossLoop.Play();
        }
    }

    AudioSource CriarLoop(AudioClip clip)
    {
        AudioSource fonte = gameObject.AddComponent<AudioSource>();
        fonte.clip = clip;
        fonte.loop = true;
        fonte.playOnAwake = false;
        fonte.outputAudioMixerGroup = sfxManager.outputAudioMixerGroup;
        return fonte;
    }

    void Update()
    {
        // o motor sobe enquanto o barco anda e desce quando ele para
        if (motor != null)
        {
            bool andando = Time.time - tempoDoUltimoMovimento < 0.15f;
            float alvo = andando ? 0.6f : 0f;
            motor.volume = Mathf.MoveTowards(motor.volume, alvo, Time.unscaledDeltaTime * 1.5f);
        }
    }

    public void PararSomDoBoss()
    {
        if (bossLoop != null) bossLoop.Stop();
    }

    #region Player
    public void SfxMovingPlayer()
    {
        // chamado todo frame em que o barco anda
        tempoDoUltimoMovimento = Time.time;
    }
    public void SfxAttackPlayer()
    {
        Tocar(attackSoundPlayer);
    }

    public void SfxDamagePlayer()
    {
        Tocar(damageSoundPlayer);
    }

    public void SfxDiePlayer()
    {
        Tocar(dieSoundPlayer);
    }
    #endregion

    #region Boss
    public void SfxMovingBoss()
    {
        Tocar(movingSoundBoss);
    }
    public void SfxAttackBoss()
    {
        Tocar(attackSoundBoss);
    }

    public void SfxMissilBoss()
    {
        Tocar(missilSoundBoss);
    }

    public void SfxTrocaDeFaseBoss()
    {
        Tocar(trocaDeFaseSoundBoss);
    }

    public void SfxDamageBoss()
    {
        // vários tiros seguidos não empilham o mesmo som
        if (Time.time - tempoDoUltimoDanoBoss < 0.35f) return;
        tempoDoUltimoDanoBoss = Time.time;
        Tocar(damageSoundBoss, 0.7f);
    }

    public void SfxDieBoss()
    {
        PararSomDoBoss();
        Tocar(dieSoundBoss);
    }
    #endregion

    void Tocar(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxManager == null) return;
        sfxManager.PlayOneShot(clip, volume);
    }
}
