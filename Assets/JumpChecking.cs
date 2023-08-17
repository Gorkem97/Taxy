using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JumpChecking : MonoBehaviour
{
    public Move movenator;
    public string[] NoTags;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (NoTags.Contains(collision.gameObject.tag))
        {
            movenator.CayoteeFall = 0.2f;
            movenator.isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (NoTags.Contains(collision.gameObject.tag))
        {
            movenator.isGround = false;
        }
    }

}
