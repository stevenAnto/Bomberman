using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }

    private Vector2 direction = Vector2.zero;
    public float speed        = 5f;
    public GameObject deathScreenUI;

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
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
       

       

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);


    }

    private void OnDeathSequenceEnded()
    {
        Debug.Log("Mensaje");
       // gameObject.SetActive(false);
        if (deathScreenUI != null)
        {
            TextMeshProUGUI textComponent = deathScreenUI.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                if (gameObject.name == "Player")
                {
                    textComponent.text = "Jugador 1 ha muerto\n¡Jugador 2 gana!";
                }
                else if (gameObject.name == "Player_2")
                {
                    textComponent.text = "Jugador 2 ha muerto\n¡Jugador 1 gana!";
                }
            }

            
        }
        deathScreenUI.SetActive(true);
        StartCoroutine(WaitForEnterAndLoadScene());
    }



    private IEnumerator WaitForEnterAndLoadScene()
    {
       
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        
        SceneManager.LoadScene("Level 3");
    }

}