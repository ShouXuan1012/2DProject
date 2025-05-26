using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int startIndex;
    [SerializeField] int endIndex;

    public Transform[] sprites;
    private float viewHeight;

    void Start()
    {
        float spriteWidth = sprites[0].GetComponent<SpriteRenderer>().bounds.size.x;
        viewHeight = spriteWidth;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        SpriteRenderer sr = sprites[endIndex].GetComponent<SpriteRenderer>();
        float spriteRightEdge = sr.bounds.max.x;
        float cameraLeftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero).x;

        if (spriteRightEdge < cameraLeftEdge)
        {
            float spriteWidth = sr.bounds.size.x;
            Vector3 newPos = sprites[startIndex].position + Vector3.right * spriteWidth;
            sprites[endIndex].position = new Vector3(newPos.x, sprites[endIndex].position.y, sprites[endIndex].position.z);

            int temp = startIndex;
            startIndex = endIndex;
            endIndex = temp;
        }
    }


}
