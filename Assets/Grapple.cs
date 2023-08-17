using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public bool isActive;
    Move oyuncu;
    public float showAbsolute;
    public Vector2 rain;
    Rigidbody2D rb;
    public float GrappleForce = 10;
    public int Grapples;
    GameObject[] krisis;
    public GameObject ChosenOne;
    public float RequiredDistance;

    void Start()
    {
        oyuncu = this.gameObject.GetComponent<Move>();
        ChosenOne = GameObject.Find("Chosen");
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        float AbsoluteDistance = 10000;
        krisis = GameObject.FindGameObjectsWithTag("GrapplePoint");
        foreach (var item in krisis)
        {
            float x = Mathf.Abs(item.transform.position.x - this.gameObject.transform.position.x);
            float y = Mathf.Abs(item.transform.position.y - this.gameObject.transform.position.y);
            float distance = (x * x) + (y * y);
            if (Vector2.Distance(item.transform.position , this.gameObject.transform.position) <= AbsoluteDistance)
            {
                AbsoluteDistance = Vector2.Distance(item.transform.position, this.gameObject.transform.position);
                ChosenOne = item;
            }
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position, (ChosenOne.transform.position - transform.position));
        //Debug.Log(hit);
        if (hit.collider == null)
        {
            return;

        }
        if (hit.collider.gameObject == ChosenOne)
        {
            
            GameObject child = ChosenOne.transform.GetChild(0).gameObject;
            if (AbsoluteDistance <= RequiredDistance && oyuncu.isActive && Grapples >0)
            {
                Color tmp = child.GetComponent<SpriteRenderer>().color;
                tmp.a = 0.5f;
                ChosenOne.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tmp;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    rb.velocity = Vector2.zero;
                    Grappling();
                }
            }
        }
        else
        {
            Color tmp = ChosenOne.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            ChosenOne.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tmp;
        }

        if (AbsoluteDistance > RequiredDistance)
        {
            Color tmp = ChosenOne.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            ChosenOne.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tmp;
        }
        showAbsolute = AbsoluteDistance;
    }
    public void Grappling()
    {
        rain = (ChosenOne.transform.position - transform.position).normalized;
        StartCoroutine(GrappleWait());
    }
    IEnumerator GrappleWait()
    {
        rb.velocity = GrappleForce * rain;
        //rb.AddForce(GrappleForce*rain.normalized, ForceMode2D.Impulse);
        Grapples -= 1;
        oyuncu.isActive = false;
        oyuncu.FallHarden = true;
        yield return new WaitForSeconds(0.5f);

        oyuncu.isActive = true;
    }
}
