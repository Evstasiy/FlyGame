using Assets.Scripts.MainGame.Models;
using Assets.Scripts.MainGame.Player;
using UnityEngine;

public class LayerMove : MonoBehaviour, IPauseHandler
{
    public event LayerInDestroyPositionX OnLayerInEndPositionX;
    public delegate void LayerInDestroyPositionX();

    public event LayerCanCreate OnLayerCanCreate;
    public delegate void LayerCanCreate(LayerEnum layerEnum);

    public LayerWorldModel model;

    private bool isLayerCanCreate = false;
    [SerializeField]
    private Animation Animation;

    private bool isPaused => ProjectContext.instance.PauseManager.IsPause;

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        float moveX = (GlobalPlayerInfo.playerInfoModel.FinalSpeed * model.SpeedBraking) * Time.deltaTime * -1;

        transform.Translate(moveX, 0, 0);

        if (transform.position.x < model.BackgrounLayerInfo.DestroyPositionX)
        {
            isLayerCanCreate = false;
            OnLayerInEndPositionX?.Invoke();
            gameObject.SetActive(false);
        }
        if (Vector2.Distance(model.SpawnPosition, transform.position) > model.SizeLayerWidth 
            && !isLayerCanCreate)
        {
            
            isLayerCanCreate = true;
            OnLayerCanCreate?.Invoke(model.LayerName);
        }
    }

    public void SetLayerModel(LayerWorldModel model)
    {
        this.model = model;
    }

    public void SetPause(bool isPause)
    {
        //this.isPause = isPause;
    }

    private void OnEnable()
    {
        Animation?.Play();
    }
}
