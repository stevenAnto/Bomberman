using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BomboController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.LeftShift;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {

        // Se procede a plantar una bomba al teclear el inputKey y si tienen bombas disponibles
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        //Obtencíón de la posición del jugador en los ejes x,y
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //Instacear un objeto bomba en la escena 
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        //Esperar unos segundos a que la bomba estalle
        yield return new WaitForSeconds(bombFuseTime);
        Destroy(bomb);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
