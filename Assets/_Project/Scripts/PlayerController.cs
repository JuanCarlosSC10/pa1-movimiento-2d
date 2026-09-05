using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 8f;
    public float fuerzaSalto = 10f;

    private Rigidbody2D rb;
    private float movimientoHorizontal;
    private bool enSuelo = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    
        movimientoHorizontal = 0f;

  
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movimientoHorizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movimientoHorizontal = 1f;
        }

    
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
        }
    }

    private void FixedUpdate()
    {

        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.name == "Suelo")
        {
            enSuelo = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {

        if (otro.gameObject.name == "Moneda")
        {
            Destroy(otro.gameObject);
        }
    }
}