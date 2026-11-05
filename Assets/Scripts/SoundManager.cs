using UnityEngine;

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
    
    #region Player
    public void SfxMovingPlayer()
    {
        Debug.Log("vrum");
        sfxManager.PlayOneShot(movingSoundPlayer);
    }
    public void SfxAttackPlayer()
    {
        Debug.Log("tirinho");
        sfxManager.PlayOneShot(attackSoundPlayer);
    }
     
    public void SfxDamagePlayer()
    {
        Debug.Log("ai");
        sfxManager.PlayOneShot(damageSoundPlayer);
    }

    public void SfxDiePlayer()
    {
        Debug.Log("morri");
        sfxManager.PlayOneShot(dieSoundPlayer);
    }
    #endregion

    #region Boss
        public void SfxMovingBoss()
    {
        Debug.Log("wow");
        sfxManager.PlayOneShot(movingSoundBoss);
    }
    public void SfxAttackBoss()
    {
        Debug.Log("fudeu");
        sfxManager.PlayOneShot(attackSoundBoss);
    }
     
    public void SfxDamageBoss()
    {
        Debug.Log("toma");
        sfxManager.PlayOneShot(damageSoundBoss);
    }

    public void SfxDieBoss()
    {
        Debug.Log("morreu");
        sfxManager.PlayOneShot(dieSoundBoss);
    }
    #endregion

}
