using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallCollector : MonoBehaviour
{
    int count;
    [SerializeField] float currectBallPosition = -4.7f;
    [SerializeField] bool firstBallArrived = false;
    [HideInInspector] public Vector2 resetPos;
    [SerializeField] Luncher luncher;
    public Action OnAllBallsArrived;
    public static BallCollector Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
        if (!firstBallArrived)
        {
            resetPos = collision.transform.position;
            resetPos.y = currectBallPosition;
            collision.transform.position = resetPos;
            firstBallArrived = true;
        }
        if (count >= luncher.balls.Count)
        {
            firstBallArrived = false;
            resetPos = Vector2.zero;
            count = 0;
            OnAllBallsArrived?.Invoke();
        }
    }
}
