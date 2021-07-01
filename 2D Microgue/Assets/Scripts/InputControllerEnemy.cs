using UnityEngine;

public class InputControllerEnemy : EntityInputBase
{
    public Transform target;

    Vector2 inputDirection = Vector2.zero;
    bool alignedWithTarget;

    public enum Orientation { horizontal, vertical };
    public Orientation orientation = Orientation.horizontal;

    public override Vector2 DirectionOfTarget()
    {
        Vector2 startPos = transform.position;
        Vector2 targetPos = target.position;
        Vector2 distanceToTarget = targetPos - startPos;
        Vector2 directionToTarget = Vector2.zero;

        // Used for special actions which require alignment (ibroglio-style projectiles)
        alignedWithTarget = (startPos.x == targetPos.x || startPos.y == targetPos.y);

        // Direction will bias towards whichever axis is further, or randomly choose if they are equal
        orientation = Mathf.Abs(distanceToTarget.x) == Mathf.Abs(distanceToTarget.y) ? (Orientation) Random.Range(0, 2) : 
                      Mathf.Abs(distanceToTarget.x) > Mathf.Abs(distanceToTarget.y) ? Orientation.horizontal : Orientation.vertical;

        // Horizontal orientation
        if (orientation == Orientation.horizontal) { directionToTarget.x = startPos.x == targetPos.x ? 0 : targetPos.x > startPos.x ? 1 : -1; }

        // Vertical orientation
        else { directionToTarget.y = startPos.y == targetPos.y ? 0 : targetPos.y > startPos.y ? 1 : -1; }

        return directionToTarget;
    }
}
