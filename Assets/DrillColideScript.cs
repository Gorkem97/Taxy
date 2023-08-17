using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillColideScript : MonoBehaviour
{
    public List<GameObject> platformlar = new List<GameObject>();
    bool CanStart = true;
    GameObject parent;
    int wait = -1;
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    void Update()
    {
        if (parent.GetComponent<Drill>().isDrill == 0)
        {
            CanStart = true;
        }
        if (wait>-3)
        {
            wait -= 1;
        }
        if (platformlar.Count >0 && Input.GetKeyDown(KeyCode.Mouse1) && CanStart)
        {
            parent.GetComponent<Drill>().isDrill = 1;
            CanStart = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DrilPlatform" && !platformlar.Contains(collision.gameObject))
        {
            platformlar.Add(collision.gameObject);
            wait = 4;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DrilPlatform" && platformlar.Contains(collision.gameObject) && wait <=0)
        {
            platformlar.Remove(collision.gameObject);
        }
    }
}
