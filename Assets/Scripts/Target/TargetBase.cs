using UnityEngine;
public abstract class TargetBase : MonoBehaviour, ITarget
{
    [Header("Target Settings")]
    public int scoreValue = 1;

    [Header("Effects")]
    public ParticleSystem hitEffectPrefab; // assign in inspector

    protected bool isActive;

    public virtual void OnSpawn()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public virtual void OnHit()
    {
        if (!isActive || !GameManager.Instance.IsGameActive()) return;

        // Play hit effect
        if (hitEffectPrefab != null)
        {
            ParticleSystem hitFx = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            hitFx.Play();
            Destroy(hitFx.gameObject, hitFx.main.duration);
        }

        GameManager.Instance.AddKill();
        OnDespawn();
    }

    public virtual void OnDespawn()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}

