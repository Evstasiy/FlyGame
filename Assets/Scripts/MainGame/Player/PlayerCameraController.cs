using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform player; // ������ �� ������������� ������
    public float smoothSpeed = 0.3f; // �������� �������� ����������

    [SerializeField]
    private Vector3 startVector;
    [SerializeField]
    private Vector3 endVector;
    [SerializeField]
    private float offsetY;

    public float heightOffset = 1f; // �������� ������ ����� ��� ���� ��� ������� W ��� S
    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    void FixedUpdate()
    {
        if (isPaused)
        {
            return;
        }

        // �������� ������� ������
        if (Input.GetKey(KeyCode.W))
        {
            startVector.y = player.position.y + offsetY + heightOffset;
            endVector.y = player.position.y + offsetY + heightOffset;
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            startVector.y = player.position.y + offsetY - heightOffset;
            endVector.y = player.position.y + offsetY - heightOffset;
        } 
        else
        {
            startVector.y = player.position.y + offsetY;
            endVector.y = player.position.y + offsetY;
        }
        // ������������ �������� ������ � ��������� �� 0 �� 1
        float normalizedSpeed = Mathf.InverseLerp(PlayerInfoModel.MIN_SPEED, PlayerInfoModel.MAX_SPEED, GlobalPlayerInfo.playerInfoModel.FinalSpeed);

        // ����������� �������� ������� ������ ����� ����� ������� �� ������ ��������������� ��������
        Vector3 desiredPosition = Vector3.Lerp(startVector, endVector, normalizedSpeed);

        // ������� ���������������� ����� ������� �������� ������ � �������� ��������
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // ���������� ������� ������
        transform.position = smoothedPosition;
    }
}

