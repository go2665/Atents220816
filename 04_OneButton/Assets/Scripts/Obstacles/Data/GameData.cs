using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public BackgroundScroller ground;
    public Spawner spawner;

    public float baseSpeed = 8.0f;

    float increasSpeed = 0.5f;  // 1초에 속도가 0.5만큼 증가
    float currentSpeed = 0.0f;

    private void Start()
    {
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        currentSpeed += increasSpeed * Time.deltaTime;

        ground.scrollingSpeed = currentSpeed;
        spawner.UpdateSpeed(currentSpeed);
    }
}
