using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [Header("Projectile")]
    public float projectileSpeed = 50f;
    public float spawnDistance = 4.5f;

    [Header("Effects")]
    public ParticleSystem muzzleFlashPrefab;

    [Header("UI")]
    public Image crosshair;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.visible = false;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameActive())
            return;
            
        var input = InputHandler.Instance;

        // Move crosshair if valid input position
        if (crosshair != null && input.HasValidPosition())
            crosshair.transform.position = input.ScreenPosition;

        // Shooting
        if (input.IsPressedDown && GameManager.Instance.HasAmmo())
        {
            Shoot(input.ScreenPosition);
        }
    }

    void Shoot(Vector3 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        Vector3 spawnPoint = cam.ScreenToWorldPoint(
            new Vector3(screenPos.x,
                        screenPos.y,
                        cam.nearClipPlane + spawnDistance));

        // Get bullet from pool
        GameObject projectileGO = BulletPool.Instance.GetBullet();
        projectileGO.transform.position = spawnPoint;
        projectileGO.transform.rotation = Quaternion.identity;
        projectileGO.SetActive(true);

        // Launch projectile
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Launch(ray.direction, projectileSpeed);

        // Play muzzle flash
        if (muzzleFlashPrefab != null)
        {
            ParticleSystem flash = Instantiate(muzzleFlashPrefab, spawnPoint, Quaternion.identity);
            flash.transform.forward = ray.direction;
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }

        // Decrease ammo
        GameManager.Instance.UseAmmo();
    }
}
