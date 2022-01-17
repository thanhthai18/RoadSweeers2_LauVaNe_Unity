using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBG_RoadSweepersMinigame2 : AbstractMyLoopBG_Minigame
{
    private void Awake()
    {
        speedBG = 4;
        startPos = transform.position;
    }

    private void Update()
    {
        if (GameController_RoadSweepersMinigame2.instance.isBegin && !GameController_RoadSweepersMinigame2.instance.isOutro && !GameController_RoadSweepersMinigame2.instance.isLose && !GameController_RoadSweepersMinigame2.instance.isWin)
        {
            LoopBG(MyDirection.left, -18.87f);

        }

    }
}
