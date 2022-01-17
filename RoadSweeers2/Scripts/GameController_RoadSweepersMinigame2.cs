using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_RoadSweepersMinigame2 : MonoBehaviour
{
    public static GameController_RoadSweepersMinigame2 instance;

    public bool isWin, isLose, isBegin, isOutro;
    public Camera mainCamera;
    public float startSizeCamera;
    public Image panelScore;
    public List<Transform> listPosSpawn = new List<Transform>();
    public GameObject dirtyPrefab;
    public MyBG_RoadSweepersMinigame2 background;
    public Wall_RoadSweepersMinigame2 currentWallObj;
    public Text txtScore;
    public Wall_RoadSweepersMinigame2 wallPrefab;
    public MyCar_RoadSweepersMinigame2 roadsweepersObj;
    public List<GameObject> listEnemy = new List<GameObject>();
    public GameObject outroObjPrefab;
    public GameObject tutorial;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isBegin = false;
        isOutro = false;
    }

    private void Start()
    {
        panelScore.gameObject.SetActive(false);
        startSizeCamera = mainCamera.orthographicSize;
        SetSizeCamera();
        mainCamera.orthographicSize *= 2.0f / 5;
        tutorial.SetActive(false);
        Intro();
    }

    void SetSizeCamera()
    {
        float f1, f2;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;
        mainCamera.orthographicSize *= f1 / f2;
    }

    void Intro()
    {
        mainCamera.DOOrthoSize(startSizeCamera, 3).SetEase(Ease.Linear).OnComplete(() =>
        {
            isBegin = true;
            panelScore.gameObject.SetActive(true);
            txtScore.text = "0/30";
            SpawnDirty();
            SpawnWall();
            tutorial.transform.position = new Vector3(0, 3, 0);
            tutorial.SetActive(true);
            Tutorial();
            isBegin = true;
        });
    }

    void Tutorial()
    {
        tutorial.transform.DOMoveY(-3, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            tutorial.transform.DOMoveY(3, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (tutorial.activeSelf)
                {
                    Tutorial();
                }
            });
        });
    }
    public void SpawnDirty()
    {
        var tmpDirty = Instantiate(dirtyPrefab, listPosSpawn[Random.Range(3, listPosSpawn.Count)].position, Quaternion.identity);
    }

    public void SpawnWall()
    {
        int ran = Random.Range(0, 3);
        currentWallObj = Instantiate(wallPrefab, listPosSpawn[ran].position, Quaternion.identity);
        currentWallObj.currentLane = ran + 1;
    }

    public void SetScore(int score)
    {
        txtScore.text = score.ToString() + "/30";
    }

    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
    }
    public void Lose()
    {
        isLose = true;
        Debug.Log("Lose");
    }

    public void Outro()
    {
        StopAllCoroutines();
        Debug.Log("outro");
        isOutro = true;
        currentWallObj.GetComponent<BoxCollider2D>().enabled = false;
        currentWallObj.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
        {
            Destroy(currentWallObj.gameObject);
        });

        roadsweepersObj.transform.DOMoveX(roadsweepersObj.transform.position.x + 20, 2);
        listEnemy.ForEach(enemy =>
        {
            enemy.transform.DOMoveX(0, 2).SetEase(Ease.Linear).OnComplete(() =>
            {
                enemy.transform.GetChild(0).gameObject.SetActive(true);
                enemy.transform.DOMoveZ(enemy.transform.position.z, 2).SetEase(Ease.Linear).OnComplete(() =>
                {
                    enemy.transform.GetChild(0).gameObject.SetActive(false);
                    enemy.transform.GetChild(1).gameObject.SetActive(true);
                    enemy.transform.DOMoveZ(enemy.transform.position.z, 1).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        var tmpOutroObj = Instantiate(outroObjPrefab, listPosSpawn[1].position, Quaternion.identity);
                        tmpOutroObj.transform.DOMoveX(-20, 3).SetEase(Ease.Linear);
                        enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1, enemy.transform.localScale.y, enemy.transform.localScale.z);
                        enemy.transform.DOMoveX(-20, 3).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            Win();
                        });
                    });
                });
            });
        });
    }

}
