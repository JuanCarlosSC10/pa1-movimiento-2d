using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Ajustes de movimiento desde el inspector
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 7f;
    [SerializeField] private float fuerzaSalto = 12f;

    [Header("UI & Marcador")]
    [SerializeField] private TMP_Text textoContador; 
    private int balonesRecogidos = 0;
    private int totalBalonesEnNivel;

    [Header("Sistema de Vidas")]
    [SerializeField] private int vidaMaxima = 3;
    private int vidaActual;
    [SerializeField] private Image[] corazonesUI;
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;

    // Componentes del player
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator; 

    // Variables de control
    private float movimientoHorizontal;
    [SerializeField] private bool enSuelo;
    [SerializeField] private bool empujando; 

    private void Start()
    {
        // Guardamos las referencias al iniciar
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (textoContador != null)
        {
            textoContador.text = "0";
        }
        vidaActual = vidaMaxima;
        ActualizarInterfazVida();
        totalBalonesEnNivel = GameObject.FindGameObjectsWithTag("Coleccionable").Length;
    }

    private void Update()
    {

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
            empujando = false; 

            // Reproducir sonido de salto
            if (AudioManager.Instance != null && AudioManager.Instance.sfxSalto != null)
            {
                AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxSalto);
            }
        }

     
        ActualizarAnimaciones();
    }

    private void ActualizarAnimaciones()
    {
        if (animator != null)
        {

            animator.SetFloat("VelocidadHorizontal", Mathf.Abs(movimientoHorizontal));

            animator.SetFloat("VelocidadVertical", rb.linearVelocity.y);

            animator.SetBool("EnSuelo", enSuelo);

            animator.SetBool("Empujando", empujando);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // 1. Chequeamos si colisiona con el suelo, plataformas o cajas desde arriba
        if (col.gameObject.CompareTag("Suelo") || col.gameObject.name.Contains("Caja") || col.gameObject.CompareTag("Caja"))
        {
            foreach (ContactPoint2D contacto in col.contacts)
            {
                if (contacto.normal.y > 0.5f)
                {
                    enSuelo = true;
                }
            }
        }

        if (col.gameObject.name.Contains("Caja") || col.gameObject.CompareTag("Caja"))
        {
            foreach (ContactPoint2D contacto in col.contacts)
            {
                // Si el contacto es lateral (horizontal) y mantienes presionada la tecla de dirección
                if (Mathf.Abs(contacto.normal.x) > 0.5f && movimientoHorizontal != 0)
                {
                    empujando = true;
                    return;
                }
            }
        }

        empujando = false;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // Cuando deja de tocar la superficie pierde el estado de suelo
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }

        // apaga la animación de empuje inmediatamente cuando no hay acercamiento a ello
        if (col.gameObject.name.Contains("Caja") || col.gameObject.CompareTag("Caja"))
        {
            empujando = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Recoleccion de balones
        if (otro.CompareTag("Coleccionable"))
        {
            balonesRecogidos++;
            if (textoContador != null)
            {
                textoContador.text = balonesRecogidos.ToString();
            }

            // Reproducir sonido de coleccionable 
            if (AudioManager.Instance != null && AudioManager.Instance.sfxColeccionable != null)
            {
                AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxColeccionable);
            }

            Destroy(otro.gameObject);

            if (balonesRecogidos >= totalBalonesEnNivel)
            {
                GanarPartido();
            }
        }
        // Impacto con obstáculos los conos, tarjeta rojas
        else if (otro.CompareTag("Peligro"))
        {
            RecibirDano(1);
            Destroy(otro.gameObject);
        }
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        // Reproducir sonido de daño
        if (AudioManager.Instance != null && AudioManager.Instance.sfxDano != null)
        {
            AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxDano);
        }

        ActualizarInterfazVida();

        if (vidaActual <= 0)
        {
            MainMenuManager.irAGameOverDirecto = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
        }
    }

    private void GanarPartido()
    {
        MainMenuManager.irAVictoriaDirecto = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }

    private void ActualizarInterfazVida()
    {
        if (corazonesUI == null) return;

        for (int i = 0; i < corazonesUI.Length; i++)
        {
            if (corazonesUI[i] != null)
            {
                if (i < vidaActual)
                {
                    corazonesUI[i].sprite = corazonLleno;
                }
                else
                {
                    corazonesUI[i].sprite = corazonVacio;
                }
            }
        }
    }
}