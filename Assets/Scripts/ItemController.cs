using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int itemID;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CharacterRadius")
        {
            if (collision.transform.parent.GetComponent<CharacterContoller>().selected)
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CharacterRadius")
        {
            if (collision.transform.parent.GetComponent<CharacterContoller>().selected)
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            else
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CharacterRadius")
        {
            if (collision.transform.parent.GetComponent<CharacterContoller>().selected)
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
