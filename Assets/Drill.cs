using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
    Slider DrillSlider;
    public float drillStop = 10;
    public float StartSpeed = 5;
    public GameObject drill;
    public GameObject triget;
    public float MoveSpeed;
    Rigidbody2D rb;
    public float drillTime;
    public float DrillRemain;
    Grapple fiyuv;
    Move Berry;
    public int isDrill = 0;
    bool drillStart = false;
    public List<GameObject> platforms = new List<GameObject>();
    int a = -1;

    GameObject realTarget;

    void Start()
    {
        DrillSlider = GameObject.Find("DrillSlider").GetComponent<Slider>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        fiyuv = this.gameObject.GetComponent<Grapple>();
        Berry = this.gameObject.GetComponent<Move>();
    }

    void Update()
    {
        
        if (DrillRemain>drillTime)
        {
            DrillRemain = drillTime;
        }
        if (Berry.myTurn)
        {
            DrillSlider.value = DrillRemain / drillTime;
        }
        if (a>-3)
        {
            a -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !drillStart)
        {
            isDrill = 1;
            drillStart = true;

        }
        if (isDrill == 1)
        {
            if (DrillRemain <= 0 || !Berry.myTurn)
            {
                isDrill = 0;
                drillStart = false;
                return;
            }
            float Distance = 2;
            GameObject target;
            target = this.gameObject;
            foreach (var item in GameObject.FindGameObjectsWithTag("DrilPlatform"))
            {
                Collider2D collidier = item.GetComponent<Collider2D>();

                float allah = Vector2.Distance(transform.position, collidier.ClosestPoint(transform.position));
                if (Mathf.Abs(allah)< Distance)
                {
                    Distance = Mathf.Abs(allah);
                    target = item;
                }
            }
            if (Distance == 2)
            {
                isDrill = 0;
                drillStart = false;
                return;
            }

            this.gameObject.GetComponent<Collider2D>().isTrigger = true;
            //Physics2D.IgnoreLayerCollision(7,3,true);

            Debug.Log(Distance);
            fiyuv.isActive = false;
            Berry.isActive = false;
            Physics2D.gravity = Vector2.zero;

            realTarget = target;

            isDrill = 4;
            rb.velocity = Vector2.zero;
        }
        if (isDrill == 4)
        {
            transform.position = Vector2.MoveTowards(transform.position, realTarget.GetComponent<Collider2D>().ClosestPoint(transform.position), StartSpeed*Time.deltaTime);

            Debug.Log(realTarget);
        }
        if (isDrill == 2)
        {
            if (platforms.Count == 0)
            {
                isDrill = 3;
                return;
            }
            rb.velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.D))
            {
                drill.transform.Rotate(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                drill.transform.Rotate(0, 0, 1);
            }
            transform.position = Vector2.MoveTowards(transform.position, triget.transform.position, MoveSpeed*Time.deltaTime);
            DrillRemain -= Time.deltaTime;
            if (DrillRemain <= 0)
            {
                drillStart = false;
                isDrill = 0;
                drill.SetActive(false);
                return;
            }
        }
        if (isDrill == 3)
        {
            drillStart = false;
            //Physics2D.IgnoreLayerCollision(7, 3, false);
            this.gameObject.GetComponent<Collider2D>().isTrigger = false;
            rb.AddForce((triget.transform.position - transform.position).normalized*drillStop,ForceMode2D.Impulse);
            drill.SetActive(false);
            isDrill = 0;
            fiyuv.isActive = true;
            Berry.isActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DrilPlatform" && platforms.Contains(collision.gameObject) && a<0)
        {
            platforms.Remove(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DrilPlatform" && !platforms.Contains(collision.gameObject))
        {
            platforms.Add(collision.gameObject);
            a = -1;
        }
        if (collision.gameObject.tag == "DrilPlatform" && isDrill ==4)
        {
            drill.SetActive(true);
            Vector2 drill2 = new Vector2(drill.transform.position.x, drill.transform.position.y);
            Vector2 diff = drill2 - realTarget.GetComponent<Collider2D>().ClosestPoint(transform.position);
            //Vector3 diff = drill.transform.position - realTarget.transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            drill.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            isDrill = 2 ;
        }
    }
}
