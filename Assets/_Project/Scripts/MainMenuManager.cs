using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
 
    public static bool irAGameOverDirecto = false;
    public static bool irAVictoriaDirecto = false;

    [Header("Paneles UI")]
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelVictoria;

    [Header("Escena del Juego")]
    [SerializeField] private string nombreEscenaJuego = "Nivel1";

    private void Start()
    {
        //  Si ganó el partido
        if (irAVictoriaDirecto)
        {
            MostrarVictoria();
            irAVictoriaDirecto = false; 

            // Reproduce sonido de victoria
            if (AudioManager.Instance != null && AudioManager.Instance.sfxVictoria != null)
            {
                AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxVictoria);
            }
        }
        else if (irAGameOverDirecto)
        {
            MostrarGameOver();
            irAGameOverDirecto = false;

            // Reproduce sonido de derrota 
            if (AudioManager.Instance != null && AudioManager.Instance.sfxDerrota != null)
            {
                AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxDerrota);
            }
        }
        else
        {
            MostrarMenuPrincipal();
        }
    }

    public void MostrarVictoria()
    {
        if (panelVictoria) panelVictoria.SetActive(true);
        if (panelGameOver) panelGameOver.SetActive(false);
        if (panelMenuPrincipal) panelMenuPrincipal.SetActive(false);
    }

    public void MostrarGameOver()
    {
        if (panelVictoria) panelVictoria.SetActive(false);
        if (panelGameOver) panelGameOver.SetActive(true);
        if (panelMenuPrincipal) panelMenuPrincipal.SetActive(false);
    }

    public void MostrarMenuPrincipal()
    {
        if (panelVictoria) panelVictoria.SetActive(false);
        if (panelGameOver) panelGameOver.SetActive(false);
        if (panelMenuPrincipal) panelMenuPrincipal.SetActive(true);

        // Vuelve a reproducir la música del menú al regresar al menú principal
        if (AudioManager.Instance != null && AudioManager.Instance.musicaMenu != null)
        {
            AudioManager.Instance.ReproducirMusica(AudioManager.Instance.musicaMenu);
        }
    }

    public void IniciarPartido()
    {
        ManejarMusicaAlEntrarAlJuego();
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void ReintentarNivel()
    {
        ManejarMusicaAlEntrarAlJuego();
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void VolverAlMenu()
    {
        MostrarMenuPrincipal();
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }

    // detener la música del menú 
    private void ManejarMusicaAlEntrarAlJuego()
    {
        if (AudioManager.Instance != null)
        {
         
            if (AudioManager.Instance.musicaNivel != null)
            {
                AudioManager.Instance.ReproducirMusica(AudioManager.Instance.musicaNivel);
            }
        
            else if (AudioManager.Instance.musicaSource != null)
            {
                AudioManager.Instance.musicaSource.Stop();
            }
        }
    }
}