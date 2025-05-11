using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        if (type == ItemType.ExtraBomb)
        {
            player.GetComponent<BomboController>().AddExtraBomb();
        }
        else if(type == ItemType.BlastRadius)
        {
            player.GetComponent<BomboController>().IncreaseExplosionRadius();
        }
        else if(type == ItemType.SpeedIncrease)
        {
            player.GetComponent<MovementController>().IncreaseSpeed();
        }
        else
        {

        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }
}
