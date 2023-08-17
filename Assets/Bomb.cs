using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    int a = 0;
    public AudioSource puff;
    public GameObject bum;
    public int BombCount;
    public List<GameObject> Bombs = new List<GameObject>();
    Rigidbody2D rb;
    public float BombPower;
    public float distance;
    public TextMeshProUGUI Count;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Move>().isActive)
        {
            return;
        }
        Count.text = BombCount.ToString();
        if (a>-5)
        {
            a -= 1;
        }
        GameObject.Find("gruple").GetComponent<TextMeshProUGUI>().text = GetComponent<Grapple>().Grapples.ToString();
        if (Input.GetKeyDown(KeyCode.LeftControl) && BombCount>0)
        {
            a = 6;
            GameObject yeah = Instantiate(bum, transform.position, transform.rotation);
            Bombs.Add(yeah);
            BombCount -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Q) && Bombs.Count>0)
        {
            List<GameObject> fword = new List<GameObject>();
            StartCoroutine(BombWait());
            rb.velocity = Vector2.zero;
            foreach (GameObject item in Bombs)
            {
                GameObject at = item;
                if (Vector2.Distance(at.transform.position, transform.position) <= distance)
                {
                    rb.AddForce((transform.position - at.transform.position).normalized * BombPower, ForceMode2D.Impulse);
                }
                puff.Play();
                fword.Add(at);
            }
            Bombs.Clear();
            foreach (var item in fword)
            {
                Destroy(item);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Bomb" && !Bombs.Contains(collision.transform.parent.gameObject) && a<0)
        {
            rb.AddForce((transform.position - collision.transform.position).normalized*BombPower,ForceMode2D.Impulse);
            puff.Play();
            Destroy(collision.transform.parent.gameObject);
        }
        
        if (collision.gameObject.tag == "BombFill")
        {
            BombCount += 2;
            Destroy(collision.gameObject);
        }
    }
    IEnumerator BombWait()
    {
        GetComponent<Move>().isActive = false;
        yield return new WaitForSeconds(1f);
        GetComponent<Move>().isActive = true;
    }
}
