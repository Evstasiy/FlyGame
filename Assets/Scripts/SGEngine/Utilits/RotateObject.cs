using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // �������� ��������
    public float rotationLimit = 20f; // ����������� ��������
    private float currentRotationY;
    private float direction = 1f; // ����������� ��������

    private void Start()
    {
        currentRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // ��������� ������� �������� ���� ��������
        currentRotationY += direction * rotationSpeed * Time.deltaTime;

        // ���������, �������� �� �� �����������
        if (currentRotationY >= rotationLimit)
        {
            currentRotationY = rotationLimit; // ������������� �� ������������ ��������
            direction = -1f; // ������ �����������
        } else if (currentRotationY <= -rotationLimit)
        {
            currentRotationY = -rotationLimit; // ������������� �� ����������� ��������
            direction = 1f; // ������ �����������
        }

        // ��������� ������� �������� � �������
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }
}
