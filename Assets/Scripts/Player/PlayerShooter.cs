using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [Header("Projectile")]
    public float projectileSpeed = 50f;   
    public float spawnDistance = 4.5f;    

    [Header("Effects")]
    public ParticleSystem muzzleFlashPrefab; // assign in inspector

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

        // Spawn bullet
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
            flash.transform.forward = ray.direction; // orient it with shot direction
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }

        // Decrease ammo
        GameManager.Instance.UseAmmo();
    }
}
