using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [Header("Projectile")]
    public float projectileSpeed = 50f;   // bullet speed
    public float spawnDistance = 4.5f;    // spawn distance from camera

    [Header("UI")]
    public Image crosshair; // just for crosshair following mouse/touch

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.visible = false;
    }

    void Update()
    {
        // Move crosshair to input position
        if (crosshair != null)
            crosshair.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && GameManager.Instance.HasAmmo())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 spawnPoint = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y,
                        cam.nearClipPlane + spawnDistance));

        // Get bullet from pool
        GameObject projectileGO = BulletPool.Instance.GetBullet();
        projectileGO.transform.position = spawnPoint;
        projectileGO.transform.rotation = Quaternion.identity;
        projectileGO.SetActive(true);

        // Launch projectile
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Launch(ray.direction, projectileSpeed);

        // Decrease ammo
        GameManager.Instance.UseAmmo();
    }
}
