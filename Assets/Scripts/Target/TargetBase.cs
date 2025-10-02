using UnityEngine;

public abstract class TargetBase : MonoBehaviour, ITarget
{
    [Header("Target Settings")]
    public int scoreValue = 1;

    protected bool isActive;

    public virtual void OnSpawn()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public virtual void OnHit()
    {
        if (!isActive) return;

        GameManager.Instance.AddKill();
        OnDespawn();
    }

    public virtual void OnDespawn()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
