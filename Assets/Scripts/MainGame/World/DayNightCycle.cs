using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight; 
    public float dayDuration = 120f; 
    public Gradient lightColor; // Цвет освещения в зависимости от времени суток

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
            timeOfDay = 0f; // Начинаем новый день
        }

        // Вращаем Directional Light
        float rotationAngle = timeOfDay * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(rotationAngle - 90f, 170f, 0));

        // Меняем цвет Directional Light в зависимости от времени суток
        directionalLight.color = lightColor.Evaluate(timeOfDay);
    }
}
