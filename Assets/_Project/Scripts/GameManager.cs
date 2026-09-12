using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void ReiniciarNivel()
    {
        Debug.Log("Botón presionado");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}