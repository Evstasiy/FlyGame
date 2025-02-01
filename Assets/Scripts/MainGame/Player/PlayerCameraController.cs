using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform player; // Ссылка на трансформацию игрока
    public float smoothSpeed = 0.3f; // Скорость плавного следования

    [SerializeField]
    private Vector3 startVector;
    [SerializeField]
    private Vector3 endVector;
    [SerializeField]
    private float offsetY;

    public float heightOffset = 1f; // Смещение камеры вверх или вниз при нажатии W или S
    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    void FixedUpdate()
    {
        if (isPaused)
        {
            return;
        }

        // Проверка нажатых клавиш
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
        // Нормализация скорости игрока в диапазоне от 0 до 1
        float normalizedSpeed = Mathf.InverseLerp(PlayerInfoModel.MIN_SPEED, PlayerInfoModel.MAX_SPEED, GlobalPlayerInfo.playerInfoModel.FinalSpeed);

        // Определение желаемой позиции камеры между двумя точками на основе нормализованной скорости
        Vector3 desiredPosition = Vector3.Lerp(startVector, endVector, normalizedSpeed);

        // Плавное интерполирование между текущей позицией камеры и желаемой позицией
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Обновление позиции камеры
        transform.position = smoothedPosition;
    }
}

