using UnityEngine;

public class SpecialTarget : TargetBase
{
    public float lifeTime = 3f;
    private float timer;

    public override void OnSpawn()
    {
        base.OnSpawn();
        timer = lifeTime;
    }

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
            OnDespawn();
    }

    public override void OnHit()
    {
        if (!isActive) return;

        GameManager.Instance.AddKill();
        GameManager.Instance.AddAmmo(3); // reward ammo
        OnDespawn();
    }
}
