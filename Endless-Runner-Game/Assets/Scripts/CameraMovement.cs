using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float positionZ;
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, positionZ);
    }
}
