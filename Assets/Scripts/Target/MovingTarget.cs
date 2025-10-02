using UnityEngine;

public class MovingTarget : TargetBase
{
    public enum MoveType { Horizontal, Vertical }
    public MoveType moveType = MoveType.Horizontal;
    public float speed = 2f;
    public float distance = 2f;

    private Vector3 startPos;

    public override void OnSpawn()
    {
        base.OnSpawn();
        startPos = transform.position;
    }

    void Update()
    {
        if (!isActive) return;

        float offset = Mathf.Sin(Time.time * speed) * distance;

        if (moveType == MoveType.Horizontal)
            transform.position = startPos + new Vector3(offset, 0, 0);
        else
            transform.position = startPos + new Vector3(0, offset, 0);
    }
}
