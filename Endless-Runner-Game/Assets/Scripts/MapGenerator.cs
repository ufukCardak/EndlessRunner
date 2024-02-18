using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const int POOL_SIZE = 3;

    [SerializeField] List<GameObject> grounds;
    [SerializeField] Vector2 checkerVector;
    [SerializeField] LayerMask terrainMask;

    public Queue<GameObject> groundPool;

    [SerializeField] Transform grid;

    [SerializeField] Vector3 latestCheckpoint;

    private void Start()
    {
        latestCheckpoint = transform.position;

        groundPool = new Queue<GameObject>();

        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject ground = Instantiate(grounds[Random.Range(0, grounds.Count)], Vector3.zero, Quaternion.identity);

            ground.transform.parent = grid;
            ground.SetActive(false);

            groundPool.Enqueue(ground);
        }
    }

    void CheckGround(Transform currentGround)
    {
        if (!Physics2D.OverlapBox(currentGround.GetChild(0).position, checkerVector, terrainMask))
        {
            GameObject ground = GetGroundFromPool();

            if (ground == null) return;

            ground.transform.position = new Vector2(currentGround.position.x + 64, currentGround.position.y);
            ground.SetActive(true);
            ground.transform.parent = grid;

            Collider2D latestGround = Physics2D.OverlapBox(new Vector2(currentGround.transform.position.x - 125, currentGround.position.y), checkerVector, terrainMask);

            if (latestGround != null)
            {
                latestGround.gameObject.SetActive(false);
                groundPool.Enqueue(latestGround.gameObject);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CheckGround(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Debug.LogWarning("1");
            transform.position = latestCheckpoint;
        }
        else if (collision.gameObject.CompareTag("CheckPoint"))
        {
            latestCheckpoint = collision.transform.position;
        }
    }

    private GameObject GetGroundFromPool()
    {
        if (groundPool.Count > 0)
        {
            GameObject ground = groundPool.Dequeue();
            ground.SetActive(true);
            return ground;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.GetChild(0).position, checkerVector);
    }
}
