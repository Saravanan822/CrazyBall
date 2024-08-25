using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPowers : MonoBehaviour
{

    public Type currType;
    public enum Type
    {
        Coins,
        SpecialPads,
        FlyingBall
    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.gameObject.SetActive(false);
        GameHandler.instance.specialPowerShowing = false;
        GameHandler.instance.specialPowerTimer = 0;

        if (currType == Type.Coins)
        {
            GameHandler.instance.coinsCollected++;
            GameHandler.instance.coinsText.text = GameHandler.instance.coinsCollected.ToString();
        }
        else if (currType == Type.FlyingBall)
        {
            GameHandler.instance.flyingBallCollected++;
            GameHandler.instance.flyingBallCountText.text = GameHandler.instance.flyingBallCollected.ToString();
        }
        else if (currType == Type.SpecialPads)
        {
            GameHandler.instance.specialPadCollected++;
            GameHandler.instance.specialPadCountText.text = GameHandler.instance.specialPadCollected.ToString();
        }
    }

    

}
