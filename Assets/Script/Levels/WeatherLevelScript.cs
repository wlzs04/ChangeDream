using Assets.Script.Game.Role;
using Assets.Script.Game.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Levels
{
    class WeatherLevelScript : LevelScriptBase
    {
        bool startWind = true;
        Wind wind = new Wind();

        protected override void Start()
        {
            base.Start();

            muTalkContentList.Add("突然感觉有点冷！");
            muTalkContentList.Add("冰雹会从地面发射吗？");
            muTalkContentList.Add("鸣，有你在很安心。");

            levelIndex = 1;
        }

        protected override void Update()
        {
            base.Update();

            if(startWind)
            {
                wind.Update();

                if (wind.IsWinding())
                {
                    foreach (var item in roleList)
                    {
                        if(!RoleStandBehindMing(item))
                        {
                            item.MoveByOther(null, Time.deltaTime * (1 / item.GetWeight()) * wind.GetSpeed(), wind.GetWindDirection());
                        }
                    }
                }
                else
                {
                    if (Time.time - wind.GetStopTime() > 3)
                    {
                        wind.StartByRandom();
                    }
                }
            }
        }

        /// <summary>
        /// 判断角色是否站在鸣的身后
        /// </summary>
        /// <returns></returns>
        bool RoleStandBehindMing(RoleBase role)
        {
            if(role==mingRole)
            {
                return false;
            }
            RoleTurnDirection windDirection = wind.GetWindDirection();
            float flag = mingRole.GetPositionX() - role.GetPositionX();
            switch (windDirection)
            {
                case RoleTurnDirection.Normal:
                    Debug.LogError("风向有误！Normal");
                    break;
                case RoleTurnDirection.Left:
                    return flag > 0;
                case RoleTurnDirection.Right:
                    return flag < 0;
                default:
                    break;
            }
            return false;
        }
    }
}
