using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronIA : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waitTime;
    [SerializeField] private float speed;
    // Start is called before the first frame update

    private int currentWaypoint;
    private bool isWaiting = false;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x != waypoints[currentWaypoint].position.x)
        {
            Debug.Log("Voltea");
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, 
                                    speed * Time.deltaTime);    
        }
        else if(!isWaiting)
        {
            StartCoroutine(Wait());
            
        }
    }

    IEnumerator Wait()
    {
       
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWaypoint++;

        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }
        isWaiting = false;

        Flip();
    }

    private void Flip()
    {
        if(transform.position.x > waypoints[currentWaypoint].position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f,0f); 
        }
    }
}
