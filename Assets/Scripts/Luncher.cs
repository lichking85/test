using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Luncher : MonoBehaviour
{
    public List<Ball> balls = new List<Ball>();
    public Ball ballPrefab;
    Vector2 startPos;
    Vector2 currentPos;
    Vector2 shootDirection;
    Vector2 ballPosition;
    RaycastHit2D hit;
    bool canLunch = true;
    [SerializeField] bool canDraw = true ;
    [SerializeField] Image lunchArrow;
    [SerializeField] Image ballShadow;
    [SerializeField] BallCollector ballCollector;
    private void Start()
    {
        foreach (Ball ball in balls)
        {
            ball.OnBallHitBonus += InstantiateBall;
        }
        ballCollector.OnAllBallsArrived += CanDraw;
        ballCollector.OnAllBallsArrived += CanLunch;
        lunchArrow.gameObject.SetActive(false);
        ballShadow.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            ballPosition = balls[0].transform.position;
        }
        if (Input.GetMouseButton(0))
        {
            currentPos = Input.mousePosition;
            shootDirection = (currentPos - startPos).normalized;
            if (canDraw)
            {
                DrawLunchArrow();
                FindBallShadowPlace();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            lunchArrow.gameObject.SetActive(false);
            ballShadow.gameObject.SetActive(false);
            if (shootDirection.magnitude > Mathf.Epsilon && canLunch)
            {
                canDraw = false;
                StartCoroutine(LunchBalls(shootDirection));
            }
        }
    }
    IEnumerator LunchBalls(Vector3 lunchDirection)
    {
        var timeBetweenLunches = new WaitForSeconds(0.1f);
        var i = 0;
        var j = balls.Count;
        while (i < j)
        {
            balls[i].Lunch(lunchDirection);
            i++;
            yield return timeBetweenLunches;
        }
        canLunch = false;
    }
    void DrawLunchArrow()
    {
        lunchArrow.gameObject.SetActive(true);
        lunchArrow.transform.position = ballPosition;
        lunchArrow.transform.up = shootDirection;
    }
    void FindBallShadowPlace()
    {
        ballShadow.gameObject.SetActive(true);
        hit = Physics2D.Raycast(ballPosition, shootDirection );
        var shadowPosition = hit.point;
        shadowPosition -= shootDirection * 0.2f;
        ballShadow.transform.position = shadowPosition;
    }
    void CanDraw()
    {
        canDraw = true;
    }
    void CanLunch()
    {
        canLunch = true;
    }
    void InstantiateBall(Vector3 bonusPosition)
    {
        var ball = Instantiate(ballPrefab, bonusPosition, Quaternion.identity);
        ball.Lunch(-Vector3.up);
        balls.Add(ball);
    }
}
