using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_RoadSweepersMinigame2 : AbstractWallClamp_Minigame
{
    public GameObject breakFXPrefab;
    public int currentLane;

    private void Start()
    {
        speedWall = 4;
        SetDirection(MyDirection.left);
    }

    private void Update()
    {
        if (GameController_RoadSweepersMinigame2.instance.isBegin)
        {
            Move();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ClampObject_VaCham(collision.gameObject, MyDirection.left, 20);
        }

        if (collision.gameObject.CompareTag("Trash"))
        {
            var tmpBreakFX = Instantiate(breakFXPrefab, transform.position, Quaternion.identity);
            tmpBreakFX.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
            {
                Destroy(tmpBreakFX);
            });
            Destroy(gameObject);
            if (!GameController_RoadSweepersMinigame2.instance.isWin && !GameController_RoadSweepersMinigame2.instance.isLose && !GameController_RoadSweepersMinigame2.instance.isOutro)
            {
                GameController_RoadSweepersMinigame2.instance.SpawnWall();

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GiaiPhongObj(collision.gameObject, MyDirection.left);
        }
    }
}
