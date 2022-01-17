using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirty_RoadSweepersMinigame2 : MonoBehaviour
{
    public Vector3 v;
    public float speed;

    private void Start()
    {
        v = new Vector3(-1, 0, 0);
        speed = GameController_RoadSweepersMinigame2.instance.background.speedBG;
    }

    private void Update()
    {
        transform.Translate(v * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            Destroy(gameObject);
            GameController_RoadSweepersMinigame2.instance.SpawnDirty();
        }
    }
}