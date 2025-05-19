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
        viewHeight = Camera.main.orthographicSize * 2 * Camera.main.aspect; //  가로 길이
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (sprites[endIndex].position.x < -viewHeight)
        {
            sprites[endIndex].localPosition = sprites[startIndex].localPosition + Vector3.right * viewHeight;

            int temp = startIndex;
            startIndex = endIndex;
            endIndex = temp;
        }
    }

}
