using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int startingPoint;
    [SerializeField] Transform[] points;

    private int index;

    private void Start()
    {
        transform.position = points[startingPoint].position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, points[index].position) < 0.02f)
        {
            index++;
            if (index == points.Length)
            {
                index = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, points[index].position,speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {


        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawWireSphere(points[i].transform.position, 0.5f);
        }
    }
}
