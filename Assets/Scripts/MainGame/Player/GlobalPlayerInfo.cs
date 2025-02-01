using Assets.Scripts.MainGame.Models;

namespace Assets.Scripts.MainGame.Player
{
    public static class GlobalPlayerInfo
    {
        public static PlayerInfoModel playerInfoModel { get; private set; }
        public static void SetPlayerInfoModel(PlayerInfoModel playerInfoModel) 
        {
            GlobalPlayerInfo.playerInfoModel = playerInfoModel;
        }
    }
}
