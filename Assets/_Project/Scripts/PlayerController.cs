using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 8f;
    public float fuerzaSalto = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; 
    private float movimientoHorizontal;
    private bool enSuelo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    private void Update()
    {
        // GetAxisRaw da un valor directo (-1, 0 o 1) eliminando la inercia flotante
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Giro del sprite
        if (movimientoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movimientoHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.name == "Suelo" || col.gameObject.name.Contains("Plataforma") || col.gameObject.name.Contains("Caja"))
        {
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
        enSuelo = false;
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.gameObject.name.Contains("Moneda"))
        {
            Destroy(otro.gameObject);
        }
    }
}