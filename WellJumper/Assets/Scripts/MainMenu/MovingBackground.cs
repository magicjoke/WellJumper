using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public float speed = 1f;

    public float clamppos;

    public Vector3 StartPosition;

    private void Start()
    {
        StartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        float NewPosition = Mathf.Repeat(Time.time * speed, clamppos);
        transform.position = StartPosition = Vector3.up * NewPosition;
    }
}
