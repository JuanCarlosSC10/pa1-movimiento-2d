using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    [SerializeField] private Transform objetivo; 

    [Header("Ajustes de seguimiento")]
    [SerializeField] private float suavizado = 5f; // Velocidad de seguimiento suave
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f); // Distancia/posicion respecto al jugador

    [Header("Límites del Mapa (Opcional)")]
    [SerializeField] private bool usarLimites = false;
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;

    private void LateUpdate()
    {
        if (objetivo == null) return;

        // Posición deseada sumando la distancia offset
        Vector3 posicionDeseada = objetivo.position + offset;

        // Aplicar límites si están activados para que la cámara no muestre fuera del mapa
        if (usarLimites)
        {
            posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, minX, maxX);
            posicionDeseada.y = Mathf.Clamp(posicionDeseada.y, minY, maxY);
        }

        // Movimiento suave utilizando Lerp
        Vector3 posicionSuave = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        transform.position = posicionSuave;
    }
}
