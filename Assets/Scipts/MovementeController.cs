using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.zero;
    public float speed = 5f;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft = KeyCode.A;

    private void Awake()
    {

        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Reset direction at the beginning of each frame
        Vector2 newDirection = Vector2.zero;

        // Check each key independently
        if (Input.GetKey(inputUp))
        {
            Debug.Log("arriba");
            newDirection += Vector2.up;
        }
        
        if (Input.GetKey(inputDown))
        {
            Debug.Log("abajo");
            newDirection += Vector2.down;
        }
        
        if (Input.GetKey(inputLeft))
        {
            Debug.Log("Left");
            newDirection += Vector2.left;
        }
        
        if (Input.GetKey(inputRight))
        {
            Debug.Log("derecha");
            newDirection += Vector2.right;
        }

        // Normalize to prevent diagonal movement from being faster
        if (newDirection != Vector2.zero)
        {
            newDirection.Normalize();
        }

        SetDirection(newDirection);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
        if (direction != Vector2.zero)
        {
            Debug.Log("Nueva dirección: " + direction);
        }
    }
}