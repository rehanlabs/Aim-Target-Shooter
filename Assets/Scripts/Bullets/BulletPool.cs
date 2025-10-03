using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    public int ActiveBulletsCount { get; private set; }

    void Awake()
    {
        Instance = this;

        // Pre-spawn bullets
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform); // parent to pool GameObject
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        // Find inactive bullet
        foreach (var bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                ActiveBulletsCount++;
                return bullet;
            }
        }

        // Expand pool if needed
        GameObject newBullet = Instantiate(bulletPrefab, transform); // parent to pool GameObject
        newBullet.SetActive(false);
        pool.Add(newBullet);
        ActiveBulletsCount++;
        return newBullet;
    }

    // Called by Projectile when it deactivates
    public void NotifyBulletDeactivated()
    {
        ActiveBulletsCount = Mathf.Max(0, ActiveBulletsCount - 1);

        // Check game over condition after bullet deactivates
        GameManager.Instance.CheckGameOverCondition();
    }
}
