using System;
using System.Collections.Generic;
using UnityEngine;

partial class MainData
{
    partial class SaveData
    {
        [CreateAssetMenu(fileName = "playerData", menuName = "Custom/PlayerData", order = 0)]
        [Serializable]
        private class PlayerData : ScriptableObject, IPlayerData
        {

            //main attributes
            private string characterName;
            private int xp;
            private int level;
            private int rankPoints;
            private SkillID[] skills;

            #region interface implementation
            #region properties
            public string CharacterName
            {
                get
                {
                    return characterName;
                }
            }

            public int Xp
            {
                get
                {
                    return xp;
                }
                set
                {
                    if(value > xp)
                    {
                        AddXp(value - xp);
                    }
                }
            }

            public int Level
            {
                get
                {
                    return level;
                }
            }

            public int RankPoints
            {
                get
                {
                    return rankPoints;
                }
            }
            #endregion

            #region methods
            /// <summary>
            /// While adding XP this method handles leveling up and updates rank points.
            /// </summary>
            /// <param name="xpToAdd"></param>
            public void AddXp(int xpToAdd)
            {
                UpdateRankPoints(xpToAdd);
                //TODO - level ups
            }
            #endregion
            #endregion

            private void LevelUp()
            {
                level++;
            }

            private void UpdateRankPoints(int score)
            {
                rankPoints = RankPointUpdater.UpdateRankPoints(rankPoints, score);
            }
        }
    }
}
