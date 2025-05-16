using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpIcon : MonoBehaviour
{
    [SerializeField] private GameObject fullHeart;
    [SerializeField] private GameObject emptyHeart;

    public void SetFull(bool isFull)
    {
        fullHeart.SetActive(isFull);
        emptyHeart.SetActive(!isFull);
    }
}
