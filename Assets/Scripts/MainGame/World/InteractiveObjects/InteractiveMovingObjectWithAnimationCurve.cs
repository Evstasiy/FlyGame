using Assets.Scripts.MainGame.Player;
using UnityEngine;

public class InteractiveMovingObjectWithAnimationCurve : MonoBehaviour
{
    private InteractiveObjectModel model;

    public AnimationCurve trajectory;
    public float scale = 10f; // Коэффициент масштабирования
    public float xSpeed = 5f; // Скорость движения по оси X
    public float xOffset = 1f; // Постоянное смещение по оси X
    public bool moveRight = true; // Если true, объект будет двигаться вправо

    private bool movingForward = true; // Направление движения
    private float timeElapsed = 0f;
    private Vector3 startPosition;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    void Start()
    {
        // Запоминаем начальную позицию объекта
        startPosition = transform.position;
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        // Увеличиваем или уменьшаем timeElapsed в зависимости от направления движения
        timeElapsed += (movingForward ? 1 : -1) * Time.deltaTime;

        // Увеличиваем или уменьшаем startPosition.x в зависимости от направления движения по X
        startPosition.x += (moveRight ? 1 : -1) * xOffset * Time.deltaTime;


        // Вычисляем новую позицию по оси X
        float x = startPosition.x + (timeElapsed * (xSpeed));

        // Определяем время последней точки кривой
        float curveTime = trajectory.keys[trajectory.length - 1].time;

        // Проверяем, достиг ли timeElapsed конца или начала кривой и меняем направление движения
        if (timeElapsed > curveTime)
        {
            timeElapsed = curveTime;
            movingForward = false;
        } else if (timeElapsed < 0)
        {
            timeElapsed = 0;
            movingForward = true;
        }

        // Вычисляем новую позицию по оси Y, следуя кривой
        float y = startPosition.y + trajectory.Evaluate(timeElapsed) * scale;

        // Применяем новую позицию к объекту
        transform.position = new Vector3(x, y, startPosition.z);


        if (transform.position.x < -30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            xSpeed = 0.1f;
            transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            GlobalPlayerInfo.playerInfoModel.AddPlayerSpeed(-5);
            Destroy(gameObject, 1f);
        }
    }

}
