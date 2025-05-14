using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BomboController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.LeftShift;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public GameObject destructibleBlockPrefab;


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
        // Obtenc��n de la posici�n del jugador en los ejes x,y
        Vector2 position = transform.position;

        // Normalizamos la posici�n a enteros para evitar problemas con la colisi�n
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //Instacear un objeto bomba en la escena
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        //audioManager.PlaySFX(audioManager.placeBomb);

        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.10f);
            bomb.transform.Rotate(0f, 0f, 60f);
        }

        // Esperar unos segundos a que la bomba estalle
        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

                Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        //audioManager.PlaySFX(audioManager.bombExplosion);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructibleBlockPrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    const int MAXIMUM_ITEM_VALUE = 15;

    public void AddExtraBomb()
    {
        bombAmount     = Mathf.Min(MAXIMUM_ITEM_VALUE, bombAmount + 1);
        bombsRemaining = Mathf.Min(MAXIMUM_ITEM_VALUE, bombsRemaining + 1);
    }

    public void IncreaseExplosionRadius()
    {
        explosionRadius = Mathf.Min(MAXIMUM_ITEM_VALUE, explosionRadius + 1);
    }

    public void AddBossItems()
    {
        for (int i = 0; i < MAXIMUM_ITEM_VALUE + 5; i++)
        {
            AddExtraBomb();
            IncreaseExplosionRadius();
        }
        bombFuseTime = 1f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
