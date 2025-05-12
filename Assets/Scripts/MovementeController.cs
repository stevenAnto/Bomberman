using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }

    private Vector2 direction = Vector2.zero;
    public float speed        = 5f;

    [Header("Input")]
    public KeyCode inputUp    = KeyCode.W;
    public KeyCode inputDown  = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft  = KeyCode.A;

    [Header("Sprites")]
    public AnimatedSpritRendered spriteRendererUp;
    public AnimatedSpritRendered spriteRendererDown;
    public AnimatedSpritRendered spriteRendererLeft;
    public AnimatedSpritRendered spriteRendererRight;
    public AnimatedSpritRendered spriteRendererDeath;
    private AnimatedSpritRendered activeSpriteRenderer;

    AudioManager audioManager;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        
        // Inicializa con el sprite hacia abajo como activo por defecto
        activeSpriteRenderer = spriteRendererDown;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Reset direction at the beginning of each frame
        Vector2 newDirection = Vector2.zero;
        AnimatedSpritRendered newSpriteRenderer = null;

        // Check each key independently and determine the appropriate sprite renderer
        if (Input.GetKey(inputUp))
        {
            //* Debug.Log("arriba");
            newDirection += Vector2.up;
            newSpriteRenderer = spriteRendererUp;
        }
        else if (Input.GetKey(inputDown))
        {
            //* Debug.Log("abajo");
            newDirection += Vector2.down;
            newSpriteRenderer = spriteRendererDown;
        }
        else if (Input.GetKey(inputLeft))
        {
            //* Debug.Log("Left");
            newDirection += Vector2.left;
            newSpriteRenderer = spriteRendererLeft;
        }
        else if (Input.GetKey(inputRight))
        {
            //* Debug.Log("derecha");
            newDirection += Vector2.right;
            newSpriteRenderer = spriteRendererRight;
        }

        // Normalize to prevent diagonal movement from being faster
        if (newDirection != Vector2.zero)
        {
            newDirection.Normalize();
            // Solo actualizamos el sprite cuando hay movimiento
            SetDirection(newDirection, newSpriteRenderer);
        }
        else
        {
            // Si no hay movimiento, mantenemos el sprite actual pero lo ponemos en idle
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpritRendered spriteRenderer)
    {
        direction = newDirection;
        
        // Solo actualizamos los sprites si tenemos un sprite renderer válido
        if (spriteRenderer != null)
        {
            // Desactivamos todos los sprites
            spriteRendererUp.enabled = false;
            spriteRendererDown.enabled = false;
            spriteRendererLeft.enabled = false;
            spriteRendererRight.enabled = false;
            
            // Activamos solo el sprite correspondiente
            spriteRenderer.enabled = true;
            
            // Actualizamos el sprite renderer activo
            activeSpriteRenderer = spriteRenderer;
        }
        
        // Actualizamos el estado idle del sprite renderer activo
        if (activeSpriteRenderer != null)
        {
            activeSpriteRenderer.idle = direction == Vector2.zero;
        }
        
        if (direction != Vector2.zero)
        {
            //* Debug.Log("Nueva dirección: " + direction);
        }
    }

    public void IncreaseSpeed()
    {
        speed = Mathf.Min(10, speed + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BomboController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        audioManager.PlaySFX(audioManager.death);

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        //* GameManager.Instance.CheckWinState();
    }
}