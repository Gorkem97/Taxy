using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class COLL : MonoBehaviour
{
    bool first = true;
    public GameObject won;
    Collider2D emmy;
    int a = -1;
    public List<GameObject> platforms = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player1" || collision.gameObject.tag == "player2")
        {
            emmy = collision;
            if (this.gameObject.tag == "WinnerBoy")
            {
                Winner();
            }
            if (this.gameObject.tag == "grapples")
            {
                GrappleCount();
            }
            if (this.gameObject.tag == "DrillRefill")
            {
                DrillFiller();
            }
            if (this.gameObject.tag == "jump")
            {
                DrillFiller();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DrilPlatform" && !platforms.Contains(collision.gameObject) && this.gameObject.tag == "drillEnd")
        {
            platforms.Add(collision.gameObject);
            if (first)
            {
                first = false;
                GameObject parent = transform.parent.parent.gameObject;
                parent.transform.position = transform.position;
            }
            a = 4;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        emmy = collision;
        if (collision.gameObject.tag == "DrilPlatform" && platforms.Contains(collision.gameObject) && this.gameObject.tag == "drillEnd" && a<0)
        {
            platforms.Remove(collision.gameObject);
            if (platforms.Count == 0)
            {
                MineEnd();
            }
        }
    }
    private void Update()
    {
        if (a>-3)
        {
            a -= 1;
        }
    }

    void Winner()
    {
        TextMeshProUGUI yazi = GameObject.Find("Brovo").GetComponent<TextMeshProUGUI>();
        yazi.text = emmy.gameObject.tag + "  won!";
    }
    void GrappleCount()
    {
        emmy.gameObject.GetComponent<Grapple>().Grapples += 2;
        Destroy(this.gameObject);
    }
    void MineEnd()
    {
        GameObject parent = transform.parent.parent.gameObject;
        parent.GetComponent<Drill>().isDrill = 3;
        parent.transform.position = transform.position;
        first = true;
    }
    void DrillFiller()
    {
        emmy.GetComponent<Drill>().DrillRemain += 5;
        Destroy(this.gameObject);
    }
    void Jump()
    {

        emmy.gameObject.GetComponent<Move>().extraJump = 2;
        Destroy(this.gameObject);
    }

}
