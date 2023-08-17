using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    GameObject aquaTarkus;
    public GameObject target;
    public TextMeshProUGUI TimeShow;
    public GameObject[] players;
    float Timer;
    public float StartTime = 3;
    public float GameTime = 5;
    public int StartPeople = 1;
    public int Turn = 1;
    void Start()
    {
        StartCoroutine(TimeManagement(StartTime));
    }

    void Update()
    {
        target.transform.position = aquaTarkus.transform.position;
        TimeShow.text = Mathf.Ceil(Timer).ToString();
    }
    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
    }
    IEnumerator TimeManagement(float HowMuchWait)
    {
        aquaTarkus = players[Turn - 1];
        aquaTarkus.GetComponent<Move>().isActive = true;
        aquaTarkus.GetComponent<Move>().myTurn = true;
        aquaTarkus.GetComponent<Drill>().DrillRemain +=1;
        Timer = HowMuchWait;
        yield return new WaitForSeconds(HowMuchWait);
        foreach (var item in players)
        {
            item.GetComponent<Move>().isActive = false;
            item.GetComponent<Move>().myTurn = false;
        }
        Timer = 2;
        yield return new WaitForSeconds(2);
        Turn += 1;
        if (Turn>StartPeople)
        {
            Turn = 1;
        }
        StartCoroutine(TimeManagement(GameTime));
    }

}
