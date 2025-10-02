using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float baseInterval = 2f;        // initial spawn delay
    public float minInterval = 0.75f;      // max difficulty cap
    public Transform[] spawnPoints;

    private float timer;
    private float difficultyTimer;
    private float currentInterval;

    void Start()
    {
        currentInterval = baseInterval;
        timer = currentInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnControlledTarget();
            timer = currentInterval;
        }

        // Gradually reduce interval for higher difficulty
        if (difficultyTimer >= 20f) // every 20 sec
        {
            currentInterval = Mathf.Max(minInterval, currentInterval - 0.1f);
            difficultyTimer = 0f;
        }
    }

    void SpawnControlledTarget()
    {
        int ammo = GameManager.Instance.GetAmmo();
        int kills = GameManager.Instance.GetKillCount();

        string targetType;

        //Ammo-aware spawning
        if (ammo <= 3)
        {
            // Low ammo → spawn a SpecialTarget opportunity
            targetType = "Special";
        }
        else
        {
            //Mix of Static and Moving, with occasional Special
            float roll = Random.value;
            if (roll < 0.6f)
                targetType = "Static";
            else if (roll < 0.9f)
                targetType = "Moving";
            else
                targetType = "Special";
        }

        // Pick spawn point
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Get from pool
        GameObject targetObj = TargetPool.Instance.GetTarget(targetType);
        if (targetObj == null) return;

        targetObj.transform.position = point.position;

        // Increase difficulty for moving targets
        if (targetType == "Moving")
        {
            var moving = targetObj.GetComponent<MovingTarget>();
            moving.speed += kills * 0.05f; // faster over time
        }

        // Activate target
        var target = targetObj.GetComponent<ITarget>();
        target?.OnSpawn();
    }
}
