using System;
using UnityEngine;

public class Movementlock : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    void Start()
    {
        transform.position = player.position;
    }

    void Update()
    {
        if (transform.position != player.position)
        {
            transform.position = player.position;
        }
    }
}
