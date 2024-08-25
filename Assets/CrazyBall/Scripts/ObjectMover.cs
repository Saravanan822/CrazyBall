using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject bottomBase;
    public SpecialPowers.Type type;
    public static bool objectMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(Transform trans)
    {
        if (!gameObject.activeSelf) return;
        objectMoving = true;
        var changePos = Input.GetTouch(0).position;
        trans.position = new Vector3(changePos.x, changePos.y, 0);
    }
    public void OnDrag(Transform trans)
    {
        if (!gameObject.activeSelf) return;
        objectMoving = true;
        var changePos = Input.GetTouch(0).position;
        trans.position = new Vector3(changePos.x, changePos.y, 0);
    }
    public void OnReleased(Transform trans)
    {
        if (!gameObject.activeSelf) return;
        objectMoving = false;
        trans.gameObject.SetActive(false);
        if(trans.position.y > bottomBase.transform.position.y)
        {
            if (type == SpecialPowers.Type.FlyingBall) {
                GameHandler.instance.UseFlyingBall();
            }
            else if (type == SpecialPowers.Type.SpecialPads)
            {
                GameHandler.instance.UseSpecialPads(trans.position);
            }
        }
    }
}
