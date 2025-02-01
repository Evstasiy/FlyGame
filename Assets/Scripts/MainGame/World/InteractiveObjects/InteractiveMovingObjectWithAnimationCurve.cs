using Assets.Scripts.MainGame.Player;
using UnityEngine;

public class InteractiveMovingObjectWithAnimationCurve : MonoBehaviour
{
    private InteractiveObjectModel model;

    public AnimationCurve trajectory;
    public float scale = 10f; // ����������� ���������������
    public float xSpeed = 5f; // �������� �������� �� ��� X
    public float xOffset = 1f; // ���������� �������� �� ��� X
    public bool moveRight = true; // ���� true, ������ ����� ��������� ������

    private bool movingForward = true; // ����������� ��������
    private float timeElapsed = 0f;
    private Vector3 startPosition;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    void Start()
    {
        // ���������� ��������� ������� �������
        startPosition = transform.position;
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        // ����������� ��� ��������� timeElapsed � ����������� �� ����������� ��������
        timeElapsed += (movingForward ? 1 : -1) * Time.deltaTime;

        // ����������� ��� ��������� startPosition.x � ����������� �� ����������� �������� �� X
        startPosition.x += (moveRight ? 1 : -1) * xOffset * Time.deltaTime;


        // ��������� ����� ������� �� ��� X
        float x = startPosition.x + (timeElapsed * (xSpeed));

        // ���������� ����� ��������� ����� ������
        float curveTime = trajectory.keys[trajectory.length - 1].time;

        // ���������, ������ �� timeElapsed ����� ��� ������ ������ � ������ ����������� ��������
        if (timeElapsed > curveTime)
        {
            timeElapsed = curveTime;
            movingForward = false;
        } else if (timeElapsed < 0)
        {
            timeElapsed = 0;
            movingForward = true;
        }

        // ��������� ����� ������� �� ��� Y, ������ ������
        float y = startPosition.y + trajectory.Evaluate(timeElapsed) * scale;

        // ��������� ����� ������� � �������
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
