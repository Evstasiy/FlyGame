using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight; 
    public float dayDuration = 120f; 
    public Gradient lightColor; // ���� ��������� � ����������� �� ������� �����

    private float timeOfDay = 200f;
    private void Start()
    {
        directionalLight = GetComponent<Light>();
    }

    void Update()
    {
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay >= 1f)
        {
            timeOfDay = 0f; // �������� ����� ����
        }

        // ������� Directional Light
        float rotationAngle = timeOfDay * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(rotationAngle - 90f, 170f, 0));

        // ������ ���� Directional Light � ����������� �� ������� �����
        directionalLight.color = lightColor.Evaluate(timeOfDay);
    }
}
