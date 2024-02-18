using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D playerCol;

    [SerializeField] GameObject currentPlatform;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("1");
            if (currentPlatform != null)
            {
                Debug.Log("2");
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            currentPlatform = null;
        }
    }


    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCol, platformCollider);

        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreCollision(playerCol, platformCollider,false);
    }
}
