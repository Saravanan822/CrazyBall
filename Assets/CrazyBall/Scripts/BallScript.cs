using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using System.Diagnostics;

public class BallScript : MonoBehaviour
{
    Vector3 changePos;
    public float ballMoveSpeed;
    public float ballJumpSpeed;
    public float extents;
    public Rigidbody2D rb;
    public GameObject bottomBase;
    float upMovementTimer;
    public bool isFlyingBall;
    public Animator animator;
    public AudioSource audios;


    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void Start()
    {
        changePos = new Vector3();
        extents = GetComponent<Collider2D>().bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameHandler.instance.GameEnd)
        {
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }
        if (upMovementTimer > 0)
        {
            rb.gravityScale = 0;
            transform.position += Vector3.up * Time.deltaTime * ballJumpSpeed;
            upMovementTimer -= Time.deltaTime;
            ballJumpSpeed -= 5;
            if(upMovementTimer <= 0)
            {
                upMovementTimer = 0;
                rb.gravityScale = 75;
                ballJumpSpeed = 500;
            }
            
        }
        if (Input.touchCount > 0 && !ObjectMover.objectMoving && Input.GetTouch(Input.touchCount - 1).position.y > bottomBase.transform.position.y)
        {
            var delta = Input.GetTouch(Input.touchCount - 1).deltaPosition;

            changePos.x = delta.x;
            if(isFlyingBall) changePos.y = delta.y;
            else changePos.y = 0;

            transform.position += (changePos *ballMoveSpeed);
        }
        if((transform.position.x + extents) >= Screen.width || (transform.position.x + extents) <= 0 || (transform.position.y + extents) >= Screen.height || (transform.position.y + extents) <= bottomBase.transform.position.y
            )
        {
            GameHandler.instance.GameEnd = true;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
    }
    public void MoveBallUpWard()
    {
        upMovementTimer = 1;
    }
   
}
