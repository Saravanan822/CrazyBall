using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pad : MonoBehaviour
{
    public enum PadType
    {
        Normal,
        Blinks,
        Avoidable,
        Bouncy
    }

    public PadType type;
    public float blinkTimer;
    bool blinkStarts;
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (GameHandler.instance.GameEnd) return;
        if (type == PadType.Blinks && blinkStarts) {
            blinkTimer += Time.deltaTime;
            if(blinkTimer <= 0.5f) image.enabled = false;
            else if(blinkTimer <= 1f) image.enabled = true;
            else if(blinkTimer <= 1.5f) image.enabled = false;
            else if(blinkTimer <= 2f) image.enabled = true; 
            else if(blinkTimer <= 2.5f) image.enabled = false;
            else if(blinkTimer <= 3) image.enabled = true;
            else if(blinkTimer <= 3.5f)
            {
                blinkStarts = false;
                blinkTimer = 0;
                gameObject.SetActive(false);
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == PadType.Avoidable)
        {
            GameHandler.instance.GameEnd = true;
        }
        if (type == PadType.Blinks)
        {
            blinkStarts = true;
            blinkTimer = 0;
        }
        if(type == PadType.Bouncy)
        {
            GameHandler.instance.ball.MoveBallUpWard();
            gameObject.SetActive(false);
        }
    }
}
