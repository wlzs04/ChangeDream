using Assets.Script.Game.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Weather
{
    class Wind
    {
        float startTime = 0;//开始时间
        float durationTime = 0;//持续时间
        float stopTime = 0;//停止时间
        bool isWinding = false;//风正在刮
        float intensity = 1;//风的强度
        float speed = 1;//风的速度


        RoleTurnDirection windDirection;//暂时用角色转向表示风向
        System.Random random = new System.Random();

        /// <summary>
        /// 随机产生风
        /// </summary>
        public void StartByRandom()
        {
            windDirection = (RoleTurnDirection)(random.Next(2)+1);
            durationTime =(float)( random.Next(GameCommonValue.windMaxDurationTime)*random.NextDouble()+ GameCommonValue.windMinDurationTime);
            intensity = (float)(0.5+0.5 *random.NextDouble());
            speed = intensity * GameCommonValue.windMaxSpeed* GameCommonValue.gameBaseLength;
            startTime = Time.time;
            isWinding = true;
        }

        /// <summary>
        /// 按传入值产生风
        /// </summary>
        /// <param name="windDirection"></param>
        /// <param name="durationTime"></param>
        /// <param name="intensity"></param>
        public void StartByValue(RoleTurnDirection windDirection,float durationTime,float intensity)
        {
            this.windDirection = windDirection;
            this.durationTime = durationTime;
            this.intensity = intensity;
            speed = intensity * GameCommonValue.windMaxSpeed;
            startTime = Time.time;
            isWinding = true;
        }

        public void Update()
        {
            if(isWinding&&Time.time-startTime>= durationTime)
            {
                isWinding = false;
                stopTime = Time.time;
            }
        }

        public bool IsWinding()
        {
            return isWinding;
        }

        public float GetIntensity()
        {
            return intensity;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public RoleTurnDirection GetWindDirection()
        {
            return windDirection;
        }

        public float GetStopTime()
        {
            return stopTime;
        }
    }
}
