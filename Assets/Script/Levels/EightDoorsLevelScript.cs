using Assets.Script.Game.Role;
using Assets.Script.Game.Weather;
using Assets.Script.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Levels
{
    class EightDoorsLevelScript : LevelScriptBase
    {
        AnimationHelper animationHelper;

        public GameObject doorThreeShengMenPre = null;

        protected override void Start()
        {
            base.Start();

            muTalkContentList.Add("不同的门有不同的通过方式。");
            muTalkContentList.Add("会不会有从天而降的门呢？");

            levelIndex = 2;

            animationHelper = AnimationHelper.GetSingleInstance();
        }

        protected override void Update()
        {
            base.Update();
            animationHelper.Update();
        }
        
    }
}
