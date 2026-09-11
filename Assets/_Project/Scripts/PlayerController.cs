using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Ajustes de movimiento desde el inspector
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 7f;
    [SerializeField] private float fuerzaSalto = 12f;

    // Componentes del player
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Variables de control
    private float movimientoHorizontal;
    [SerializeField] private bool enSuelo;

    private void Start()
    {
        // Guardamos las referencias al iniciar
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // GetAxisRaw para que el movimiento sea instantaneo y no resbale
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Girar el sprite segun hacia donde camina
        if (movimientoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movimientoHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Solo salta si presiona el boton y esta pisando el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
        }
    }

    private void FixedUpdate()
    {
        // Aplicamos el movimiento horizontal en las fisicas
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Chequeamos si colisiona con el suelo, plataformas o cajas
        if (col.gameObject.CompareTag("Suelo")  || col.gameObject.name.Contains("Caja"))
        {
            // Validamos que el contacto sea desde arriba y no desde los lados
            foreach (ContactPoint2D contacto in col.contacts)
            {
                if (contacto.normal.y > 0.5f)
                {
                    enSuelo = true;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // Cuando deja de tocar la superficie pierde el estado de suelo
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Recoleccion de monedas
        if (otro.CompareTag("Coleccionable"))
        {
            Destroy(otro.gameObject);
        }
    }
}