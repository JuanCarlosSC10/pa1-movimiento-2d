using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            // Si no existe un AudioManager en la escena actual, lo crea automáticamente
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    _instance = obj.AddComponent<AudioManager>();
                    obj.AddComponent<AudioSource>(); 
                    obj.AddComponent<AudioSource>(); 
                }
            }
            return _instance;
        }
    }

    [HideInInspector] public AudioSource musicaSource;
    [HideInInspector] public AudioSource sfxSource;

    [Header("Control de Volumen (0.0 a 1.0)")]
    [Range(0f, 1f)] public float volumenMusica = 0.3f;
    [Range(0f, 1f)] public float volumenSFX = 1f;

    [HideInInspector] public bool estaMuteado = false; 

    [Header("Clips de Sonido")]
    public AudioClip musicaMenu;
    public AudioClip musicaNivel;
    public AudioClip sfxColeccionable;
    public AudioClip sfxDano;
    public AudioClip sfxSalto;
    public AudioClip sfxVictoria;
    public AudioClip sfxDerrota;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            ConfigurarFuentes();
        }
        else if (_instance != this)
        {
            Destroy(gameObject); 
        }
    }

    private void ConfigurarFuentes()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        while (sources.Length < 2)
        {
            gameObject.AddComponent<AudioSource>();
            sources = GetComponents<AudioSource>();
        }

        musicaSource = sources[0];
        sfxSource = sources[1];
    }

    private void Update()
    {
        if (musicaSource != null) musicaSource.volume = volumenMusica;
        if (sfxSource != null) sfxSource.volume = volumenSFX;
    }

    public void AlternarMute()
    {
        estaMuteado = !estaMuteado;

        if (musicaSource != null) musicaSource.mute = estaMuteado;
        if (sfxSource != null) sfxSource.mute = estaMuteado;
    }

    public void ReproducirMusica(AudioClip nuevaMusica)
    {
        if (musicaSource != null && nuevaMusica != null)
        {
           
            estaMuteado = false;
            if (musicaSource != null) musicaSource.mute = false;
            if (sfxSource != null) sfxSource.mute = false;

        
            musicaSource.Stop();
            musicaSource.clip = nuevaMusica;
            musicaSource.loop = true;
            musicaSource.volume = volumenMusica;
            musicaSource.Play();
        }
    }

    public void ReproducirSFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.mute = estaMuteado; 
            sfxSource.PlayOneShot(clip, volumenSFX);
        }
    }
}