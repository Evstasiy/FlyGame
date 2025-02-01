using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Скорость вращения
    public float rotationLimit = 20f; // Ограничение вращения
    private float currentRotationY;
    private float direction = 1f; // Направление вращения

    private void Start()
    {
        currentRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // Обновляем текущее значение угла вращения
        currentRotationY += direction * rotationSpeed * Time.deltaTime;

        // Проверяем, достигли ли мы ограничений
        if (currentRotationY >= rotationLimit)
        {
            currentRotationY = rotationLimit; // Устанавливаем на максимальное значение
            direction = -1f; // Меняем направление
        } else if (currentRotationY <= -rotationLimit)
        {
            currentRotationY = -rotationLimit; // Устанавливаем на минимальное значение
            direction = 1f; // Меняем направление
        }

        // Применяем угловое вращение к объекту
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }
}
