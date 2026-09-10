using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles UI")]
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelGameOver;

    [Header("Escena del Juego")]
    [SerializeField] private string nombreEscenaJuego = "Nivel1";

    public void IniciarPartido()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void ReintentarNivel()
    {
        SceneManager.LoadScene(nombreEscenaJuego);

        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);
    }

    public void VolverAlMenu()
    {
        if (panelGameOver) 
        {
            panelGameOver.SetActive(false);
        }

        if (panelMenuPrincipal)
        {
            panelMenuPrincipal.SetActive(true);
        }    
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}