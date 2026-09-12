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
    [SerializeField] private TMP_Text textoContador; // Referencia al texto del marcador
    private int balonesRecogidos = 0; // Contador interno
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
    private Animator animator; // Referencia al Animator

    // Variables de control
    private float movimientoHorizontal;
    [SerializeField] private bool enSuelo;
    [SerializeField] private bool empujando; // <--- NUEVA VARIABLE DE EMPUJE

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
            empujando = false; // Al saltar se cancela el empuje

            // Reproducir sonido de salto
            if (AudioManager.Instance != null && AudioManager.Instance.sfxSalto != null)
            {
                AudioManager.Instance.ReproducirSFX(AudioManager.Instance.sfxSalto);
            }
        }

        // ACTUALIZAR PARÁMETROS DEL ANIMATOR EN CADA FRAME
        ActualizarAnimaciones();
    }

    private void ActualizarAnimaciones()
    {
        if (animator != null)
        {
            // Enviamos la velocidad horizontal (Mathf.Abs para que sea siempre positiva)
            animator.SetFloat("VelocidadHorizontal", Mathf.Abs(movimientoHorizontal));

            // Enviamos la velocidad vertical del Rigidbody para detectar subida/caída
            animator.SetFloat("VelocidadVertical", rb.linearVelocity.y);

            // Enviamos el estado del suelo
            animator.SetBool("EnSuelo", enSuelo);

            // Enviamos el estado de empujar <--- ENVIAMOS ESTE VALOR AL ANIMATOR
            animator.SetBool("Empujando", empujando);
        }
    }

    private void FixedUpdate()
    {
        // Aplicamos el movimiento horizontal en las fisicas
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

        // 2. DETECTAR SI ESTÁ EMPUJANDO LA CAJA LATERALMENTE
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

        // Si toca la caja pero no se está moviendo contra ella, deja de empujar
        empujando = false;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // Cuando deja de tocar la superficie pierde el estado de suelo
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }

        // AL SEPARARSE DE LA CAJA: apaga la animación de empuje inmediatamente
        if (col.gameObject.name.Contains("Caja") || col.gameObject.CompareTag("Caja"))
        {
            empujando = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Recoleccion de balones/puntos
        if (otro.CompareTag("Coleccionable"))
        {
            balonesRecogidos++;
            if (textoContador != null)
            {
                textoContador.text = balonesRecogidos.ToString();
            }

            // Reproducir sonido de coleccionable (pelota)
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
        // Impacto con obstáculos (conos, tarjetas rojas, etc.)
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

        // Reproducir sonido de daño/pérdida de vida
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