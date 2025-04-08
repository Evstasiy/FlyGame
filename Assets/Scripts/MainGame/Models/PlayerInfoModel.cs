using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainGame.Models
{
    public class PlayerInfoModel
    {
        /*EXP RESULT*/
        public const float EXP_COEFF_DISTANCE = 0.7f;
        public const float EXP_PERCENT_COEFF_DISTANCE = 0.6f;

        public const float EXP_COEFF_COINS = 0.1f;
        public const float EXP_PERCENT_COEFF_COINS = 0.2f;
        /*END EXP RESULT*/

        public const float MIN_GRAVITY = 0.02f;
        public const float MAX_GRAVITY = 130f;

        public const float MIN_SPEED = 5f;
        public const float MAX_SPEED = 1100f;
        public const float BASE_SPEED = 100f;

        //private float baseSpeed = 10;
        private float maxSpeedWithUpdates = BASE_SPEED;

        private float speed = 0;
        private float baseDebuffSpeed = -0.15f;

        /// <summary>
        /// Дистанция игрока
        /// </summary>
        private float playerDistance = 0;

        private int playerDistanceRecord = 0;

        /// <summary>
        /// Перемещение по Y
        /// </summary>
        private float baseMobility = 16;
        /*private*/
        public float mobility = 16;
        private float minMobility = 10;
        private float maxMobility = 38;

        #region Effects
        public HashSet<EffectEnum> ActivePlayerEffects = new HashSet<EffectEnum>();

        private int countToDisabledShield = 3;
        private float magnetRadius = 8.6f;
        #endregion Effects

        public event CoinsAdd UserCoinsChange;
        public delegate void CoinsAdd(int playerCoins);

        public event SpecialCoinsAdd UserSpecialCoinsChange;
        public delegate void SpecialCoinsAdd(int playerSpecialCoins);

        public int PlayerCoins { get; private set; } = 0;
        public int PlayerSpecialCoins { get; private set; } = 0;

        public float PlayerDistance { get { return playerDistance; } }

        public float FinalSpeed { get { return GetFinalSpeed(); } }
        public float FinalMobility { get { return mobility; } }
        public int CountToDisabledShield { get { return countToDisabledShield; } }

        private float GetFinalSpeed() 
        {
            return speed;
        }
        
        public void SetPlayerDistance(float newDistance)
        {
            playerDistance = newDistance;
        }
        
        public void AddPlayerDistanceBySpeed()
        {
            playerDistance += (speed / 50f) * Time.deltaTime;
        }

        public void AddPlayerSpeed(float speedAdd) 
        {
            var finalSpeed = speedAdd + speed;
            if (finalSpeed < MIN_SPEED)
            {
                finalSpeed = MIN_SPEED;
            } 
            else if (finalSpeed > maxSpeedWithUpdates)
            {
                finalSpeed = maxSpeedWithUpdates;
            }
            speed = finalSpeed;
        }

        public float GetMagnetRadius() => magnetRadius;
        public float GetMaxPlayerSpeedWithUpdates() => maxSpeedWithUpdates;

        public int GetFinalResultExp()
        {
            var result = EXP_PERCENT_COEFF_DISTANCE * (EXP_COEFF_DISTANCE * PlayerDistance) + EXP_PERCENT_COEFF_COINS * (EXP_COEFF_COINS * PlayerCoins);
            int finalResult = Mathf.RoundToInt(result);

            return finalResult;
        }

        public int SetPlayerDistanceRecord(int newPlayerDistanceRecord) => playerDistanceRecord = newPlayerDistanceRecord;

        public int GetPlayerDistanceRecord() => playerDistanceRecord;
        public float GetPlayerBaseDebuffSpeed() => baseDebuffSpeed;

        public void AddPlayerBaseDebuffSpeed(float debuffSpeed)
        {
            baseDebuffSpeed += debuffSpeed;
        }
        
        public void AddForBasePlayerMobility(int addModility)
        {
            AddPlayerMobility(addModility);
            baseMobility = mobility;
        }
        
        public void SetDefaultPlayerMobility()
        {
            mobility = baseMobility;
        }
        
        public void AddMaxPlayerSpeed(int addedMaxSpeed)
        {
            var newMaxSpeed = BASE_SPEED + addedMaxSpeed;
            maxSpeedWithUpdates = (newMaxSpeed > MAX_SPEED) ? MAX_SPEED : newMaxSpeed;
        }

        public void AddPlayerMobility(float mobilityAdd)
        {
            var finalMobility = mobilityAdd + mobility;
            if (finalMobility < minMobility)
            {
                finalMobility = minMobility;
            } else if (finalMobility > maxMobility)
            {
                finalMobility = maxMobility;
            }
            mobility = finalMobility;
        }

        public void AddPlayerCoins(int couinsAdd)
        {
            PlayerCoins += couinsAdd;
            UserCoinsChange?.Invoke(PlayerCoins);
        }
        
        public void AddPlayerSpecialCoins(int couinsAdd)
        {
            PlayerSpecialCoins += couinsAdd;
            UserSpecialCoinsChange?.Invoke(PlayerSpecialCoins);
        }
    }
}

