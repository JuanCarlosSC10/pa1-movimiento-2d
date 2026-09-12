using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Variable estática que recuerda si venimos de una derrota o victoria en el nivel
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
        // Prioridad 1: Si ganó el partido
        if (irAVictoriaDirecto)
        {
            MostrarVictoria();
            irAVictoriaDirecto = false; // Reseteamos la variable

            // Reproduce sonido/música de victoria si está asignado
            if (AudioManager.Instance != null && AudioManager.Instance.sfxVictoria != null)
            {
                AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxVictoria);
            }
        }
        else if (irAGameOverDirecto)
        {
            MostrarGameOver();
            irAGameOverDirecto = false; // Reseteamos la variable para futuras cargas

            // Reproduce sonido/música de derrota si está asignado
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

    // Método auxiliar para detener la música del menú o cambiar a la del nivel
    private void ManejarMusicaAlEntrarAlJuego()
    {
        if (AudioManager.Instance != null)
        {
            // Si asignaste una canción para el nivel en el AudioManager, la reproduce
            if (AudioManager.Instance.musicaNivel != null)
            {
                AudioManager.Instance.ReproducirMusica(AudioManager.Instance.musicaNivel);
            }
            // Si no hay música de nivel, simplemente detiene la música del menú
            else if (AudioManager.Instance.musicaSource != null)
            {
                AudioManager.Instance.musicaSource.Stop();
            }
        }
    }
}