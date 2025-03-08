using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recognizer
{
    public Vector2 GetCentroid(DollarPoint[] points)
    {
        float centerX = points.Sum(point => point.Point.x) / points.Length;
        float centerY = points.Sum(point => point.Point.y) / points.Length;
        return new Vector2(centerX, centerY);
    }

    public DollarPoint[] ResamplePoints(DollarPoint[] points, int n)
    {
        if (points.Length == 0) return points;

        float incrementValue = PathLength(points) / (n - 1);
        if (incrementValue <= 0) return points; // ������ �� ������� �� 0

        float proceedDistance = 0;
        List<DollarPoint> newPoints = new List<DollarPoint> { points[0] };

        for (int i = 1; i < points.Length; i++)
        {
            DollarPoint previousDollarPoint = points[i - 1];
            DollarPoint currentDollarPoint = points[i];

            if (previousDollarPoint.StrokeIndex == currentDollarPoint.StrokeIndex)
            {
                float distance = Vector2.Distance(previousDollarPoint.Point, currentDollarPoint.Point);

                // ���� ��� ����� ���������, ���������� ��
                if (distance == 0) continue;

                while (proceedDistance + distance >= incrementValue)
                {
                    float t = (incrementValue - proceedDistance) / distance;

                    // ���������� �������� �� NaN � �������������
                    if (float.IsNaN(t) || float.IsInfinity(t))
                    {
                        break;
                    }

                    float approximatedX = previousDollarPoint.Point.x + t * (currentDollarPoint.Point.x - previousDollarPoint.Point.x);
                    float approximatedY = previousDollarPoint.Point.y + t * (currentDollarPoint.Point.y - previousDollarPoint.Point.y);

                    DollarPoint approximatedDollarPoint = new DollarPoint()
                    {
                        Point = new Vector2(approximatedX, approximatedY),
                        StrokeIndex = previousDollarPoint.StrokeIndex
                    };
                    newPoints.Add(approximatedDollarPoint);

                    proceedDistance = 0;
                    previousDollarPoint = approximatedDollarPoint;
                    distance = Vector2.Distance(previousDollarPoint.Point, currentDollarPoint.Point);

                    // ���� ����� ���������� ����� 0, �� �������, ����� �� ��������
                    if (distance == 0) break;
                }

                proceedDistance += distance;
            }
        }

        if (proceedDistance > 0)
        {
            newPoints.Add(points[^1]);
        }

        return newPoints.ToArray();
    }


    public float PathLength(DollarPoint[] points)
    {
        float length = 0;
        for (int i = 1; i < points.Length; i++)
        {
            DollarPoint previous = points[i - 1];
            DollarPoint current = points[i];
            float distance = Vector2.Distance(previous.Point, current.Point);
            if (!float.IsNaN(distance) && previous.StrokeIndex == current.StrokeIndex)
            {
                length += distance;
            }
        }

        return length;
    }
}