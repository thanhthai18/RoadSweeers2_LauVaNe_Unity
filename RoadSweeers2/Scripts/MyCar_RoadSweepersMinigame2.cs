using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCar_RoadSweepersMinigame2 : AbstractMyCar_Minigame
{
    public Tween tweenScale;
    public int score;
    public Vector3 startScale;
    public GameObject cleanFXPrefab;

    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_DonDep, anim_PhunNuoc, anim_Idle, anim_Thua;

    private void Start()
    {
        currentLane = 2;
        startScale = transform.localScale;
        score = 0;
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == anim_DonDep)
        {
            PlayAnim(anim, anim_Idle, true);
        }
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    private void Update()
    {
        if(!GameController_RoadSweepersMinigame2.instance.isLose && !GameController_RoadSweepersMinigame2.instance.isWin && !GameController_RoadSweepersMinigame2.instance.isOutro && GameController_RoadSweepersMinigame2.instance.isBegin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.transform.position;
            }
            if (Input.GetMouseButtonUp(0))
            {
                lastPos = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.transform.position;

                MoveCar_UpDown(mousePos, lastPos, 3, 3);
                if (GameController_RoadSweepersMinigame2.instance.tutorial.activeSelf)
                {
                    GameController_RoadSweepersMinigame2.instance.tutorial.SetActive(false);
                    GameController_RoadSweepersMinigame2.instance.tutorial.transform.DOKill();
                }
            }
        }    
    }

    public override void UpDownAction(int updownBinary, float distance)
    {
        if (GameController_RoadSweepersMinigame2.instance.currentWallObj != null)
        {
            var tmpWall = GameController_RoadSweepersMinigame2.instance.currentWallObj;


            if (Mathf.Abs(currentLane - tmpWall.currentLane) == 0 || Mathf.Abs(currentLane - tmpWall.currentLane) == 2)
            {
                base.UpDownAction(updownBinary, distance);
            }
            else if (Mathf.Abs(currentLane - tmpWall.currentLane) == 1)
            {
                var distanceCheck = transform.position.x - tmpWall.transform.position.x;
                if (distanceCheck <= -2.32f || distanceCheck >= 1.8f)
                {
                    base.UpDownAction(updownBinary, distance);
                }
                else
                {
                    if (transform.position.y > tmpWall.transform.position.y && updownBinary == 1)
                    {
                        base.UpDownAction(updownBinary, distance);
                    }
                    else if (transform.position.y < tmpWall.transform.position.y && updownBinary == 0)
                    {
                        base.UpDownAction(updownBinary, distance);
                    }
                }
            }

        }
        else
        {
            base.UpDownAction(updownBinary, distance);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Tree"))
        {
            var tmpCleanFX = Instantiate(cleanFXPrefab, collision.transform.position, Quaternion.identity);
            tmpCleanFX.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
             {
                 Destroy(tmpCleanFX);
             });
            Destroy(collision.gameObject);
            if (score < 30)
            {
                score++;
                GameController_RoadSweepersMinigame2.instance.SetScore(score);
                if (score == 30)
                {
                    GameController_RoadSweepersMinigame2.instance.Outro();
                }
            }
            tweenScale.Kill();
            transform.localScale = startScale;
            tweenScale = transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.Linear);
            if (!GameController_RoadSweepersMinigame2.instance.isOutro)
            {
                GameController_RoadSweepersMinigame2.instance.SpawnDirty();
            }
        }

        if (collision.gameObject.CompareTag("Trash"))
        {
            PlayAnim(anim, anim_Thua, false);
            GetComponent<Collider2D>().enabled = false;
            GameController_RoadSweepersMinigame2.instance.isLose = true;
            transform.DOKill();
        }
    }
}
