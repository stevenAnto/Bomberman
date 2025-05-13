using System;
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
        Boss,
    }

    public ItemType type;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnItemPickup(GameObject player)
    {
        audioManager.PlaySFX(audioManager.itemGet);

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
        else if(type == ItemType.Boss)
        {
            player.GetComponent<BomboController>().AddBossItems();

            player.GetComponent<CircleCollider2D>().radius = 0.25f;

            Vector3 localScale = player.GetComponent<Transform>().localScale;
            localScale.x = 2;
            localScale.y = 2;
            localScale.z = 2;
            player.GetComponent<Transform>().localScale = localScale;
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
