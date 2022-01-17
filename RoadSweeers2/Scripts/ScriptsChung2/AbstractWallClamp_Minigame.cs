using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWallClamp_Minigame : MonoBehaviour
{
    public float speedWall, startSpeed;
    public bool isVaCham;
    public Vector3 direction;
    public Vector2 startPosObj;


    public virtual void SetDirection(MyDirection enumDirection)
    {
        if (enumDirection == MyDirection.down)
        {
            direction = Vector3.down;
        }
        else if (enumDirection == MyDirection.up)
        {
            direction = Vector3.up;
        }
        else if (enumDirection == MyDirection.right)
        {
            direction = Vector3.right;
        }
        else if (enumDirection == MyDirection.left)
        {
            direction = Vector3.left;
        }
    }

    public virtual void DoSomeThing1()
    {

    }

    public virtual void DoSomeThing2()
    {

    }

    //Update/FixedUpdate
    public virtual void Move()
    {
        if (!isVaCham)
        {
            transform.Translate(direction * speedWall * Time.deltaTime);
        }
    }

    //OnTriggerEnter
    public virtual void ClampObject_VaCham(GameObject collisionObj, MyDirection enumDirection, float distance)
    {
        isVaCham = true;
        float duration = distance / speedWall;
        startSpeed = speedWall;
        startPosObj = collisionObj.transform.position;
        speedWall = 0;
        DoSomeThing1();
        if (enumDirection == MyDirection.down)
        {
            collisionObj.transform.DOMoveY(collisionObj.transform.position.y - distance, duration).SetEase(Ease.Linear);
            transform.DOMoveX(transform.position.y - distance, duration).SetEase(Ease.Linear);
        }
        else if (enumDirection == MyDirection.up)
        {
            collisionObj.transform.DOMoveY(collisionObj.transform.position.y + distance, duration).SetEase(Ease.Linear);
            transform.DOMoveX(transform.position.y + distance, duration).SetEase(Ease.Linear);
        }
        else if (enumDirection == MyDirection.right)
        {
            collisionObj.transform.DOMoveX(collisionObj.transform.position.x + distance, duration).SetEase(Ease.Linear);
            transform.DOMoveX(transform.position.x + distance, duration).SetEase(Ease.Linear);
        }
        else if (enumDirection == MyDirection.left)
        {
            collisionObj.transform.DOMoveX(collisionObj.transform.position.x - distance, duration).SetEase(Ease.Linear);
            transform.DOMoveX(transform.position.x - distance, duration).SetEase(Ease.Linear);
        }
    }
    //OnTriggerExit
    public virtual void GiaiPhongObj(GameObject collisionObj, MyDirection enumDirection)
    {
        isVaCham = false;
        speedWall = startSpeed;
        collisionObj.transform.DOKill();

        if (enumDirection == MyDirection.right || enumDirection == MyDirection.left)
        {
           collisionObj.transform.DOMoveX(startPosObj.x, 1).SetEase(Ease.Linear).OnComplete(() =>
           {
               DoSomeThing2();
           });
        }
        else if (enumDirection == MyDirection.down || enumDirection == MyDirection.up)
        {
            collisionObj.transform.DOMoveY(startPosObj.y, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                DoSomeThing2();
            });
        }
    }
}
