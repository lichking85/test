using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    Vector3 ballDirection;
    [SerializeField] float ballSpeed;
    [SerializeField] Rigidbody2D rb;
    public Action<Vector3> OnBallHitBonus;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bonus")
        {
            OnBallHitBonus?.Invoke(collision.transform.position);
            collision.gameObject.SetActive(false);
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (BallCollector.Instance.resetPos != Vector2.zero)
            {
                StartCoroutine(MoveToOnePosition());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ballDirection = Vector2.Reflect(ballDirection , collision.contacts[0].normal);
        MoveRigidBody();
    }
    public void Lunch(Vector2 lunchDir)
    {
        ballDirection = lunchDir;
        MoveRigidBody();   
    }
    void MoveRigidBody()
    {
        rb.velocity = ballDirection * ballSpeed;
    }
    IEnumerator MoveToOnePosition()
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        var startPos = transform.position;
        var targetPos = BallCollector.Instance.resetPos;
        var duration = 1f;
        float dt = 0;
        while (dt < duration)
        {
            dt += Time.deltaTime;
            float t = dt * 3;
            transform.position = Vector3.Lerp(startPos , targetPos, t);
            yield return waitForEndOfFrame; 
        }
    }
}
