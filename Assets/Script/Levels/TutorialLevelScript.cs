using Assets.Script.Game;
using Assets.Script.Game.Role;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Levels
{
    public class TutorialLevelScript : LevelScriptBase
    {
        protected override void Start()
        {
            base.Start();

            muTalkContentList.Add("控制的角色有点多，会不会不习惯！");
            muTalkContentList.Add("墙壁会推人吗？");
            muTalkContentList.Add("据说跳的越高，进地越深。");

            levelIndex = 0;
        }
    }
}
