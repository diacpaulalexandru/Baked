using UnityEngine;

public class LineUtils
{
    public static bool TryGetClosestPoint(Vector3[] points, int count, Vector3 targetPoint,
        out int index, out float targetDistance)
    {
        index = -1;
        targetDistance = 0;
        float dist = float.MaxValue;

        for (var i = 0; i < count - 1; i++)
        {
            var start = points[i];
            var stop = points[i + 1];

            var point = NearestPointOnFiniteLine(start, stop, targetPoint, out var distanceToPoint);

            var distance = Vector3.SqrMagnitude(point - targetPoint);

            if (distance < dist)
            {
                index = i;
                dist = distance;
                targetDistance = distanceToPoint;
            }
        }

        return index != -1;
    }

    public static Vector3 NearestPointOnFiniteLine(Vector3 start, Vector3 end, Vector3 pnt, out float distanceToPoint)
    {
        var direction = (end - start);
        var len = direction.magnitude;
        direction.Normalize();

        var v = pnt - start;
        var d = Vector3.Dot(v, direction);
        d = Mathf.Clamp(d, 0f, len);

        distanceToPoint = d;

        return start + direction * d;
    }
    
    public static Vector3 GetForward(Vector3[] allCorners, int cornersCount, Vector3 targetPoint, float forward,
        out float remainingDistance)
    {
        if (TryGetClosestPoint(allCorners, cornersCount, targetPoint, out var index,
                out var targetDistance) == false)
        {
            remainingDistance = 0;
            return targetPoint;
        }

        //remaining distance is calculated to closest point on line not on the forward
        remainingDistance = Vector3.Distance(allCorners[index], allCorners[index + 1]) - targetDistance;
        for (var i = index + 1; i < cornersCount - 1; i++)
        {
            remainingDistance += Vector3.Distance(allCorners[i], allCorners[i + 1]);
        }

        targetDistance += forward;
        retry:

        var length = Vector3.Distance(allCorners[index], allCorners[index + 1]);

        if (targetDistance > length)
        {
            if (index < cornersCount - 2)
            {
                index++;
                targetDistance -= length;
                goto retry;
            }

            targetDistance = length;
        }

        var dir = (allCorners[index + 1] - allCorners[index]).normalized;
        var endPoint = allCorners[index] + dir * targetDistance;
        return endPoint;
    }
}