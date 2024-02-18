using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float lengthX,lengthY, startPosX,startPosY;
    Transform cam;

    [SerializeField] float parallaxEffect;
    [SerializeField] bool vertical;


    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
        cam = Camera.main.transform;
    }

    private void Update()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffect));
        float distX = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosX + distX, transform.position.y, transform.position.z);

        if (tempX > startPosX + lengthX)
        {
            startPosX += lengthX;
        }
        else if (tempX < startPosX - lengthX)
        {
            startPosX -= lengthX;
        }
        
        if(vertical && cam.transform.position.y > -5)
        {
            float tempY = (cam.transform.position.y * (1 - parallaxEffect));
            float distY = (cam.transform.position.y * parallaxEffect);

            transform.position = new Vector3(transform.position.x, startPosY + distY, transform.position.z);

            if (tempY > startPosY + lengthY)
            {
                startPosY += lengthY;
            }
            else if (tempY < startPosY - lengthY)
            {
                startPosY -= lengthY;
            }
        }
    }
}
