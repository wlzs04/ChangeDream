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
        public bool inShengMen = false;

        public GameObject transferPointObject = null;

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

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                CheckVisibleInShengMen();
            }
        }

        /// <summary>
        /// 在生门中检测可见性
        /// </summary>
        public void CheckVisibleInShengMen()
        {
            currentControlRole.SetVisible(true);
            foreach (var item in roleList)
            {
                if (currentControlRole != item && item.GetContent("startLoop") != null
                    && currentControlRole.GetContent("loopNumber")!=null
                    && currentControlRole.GetContent("loopNumber").ToString() != item.GetContent("loopNumber").ToString())
                {
                    item.SetVisible(false);
                }
                else
                {
                    item.SetVisible(true);
                }
            }
        }

        public void EnterShengMen()
        {
            inShengMen = true;
        }

        public void EndShengMen()
        {
            inShengMen = false;
        }

        /// <summary>
        /// 将所有角色移动到指定位置
        /// </summary>
        public void TransferRoleToTransferPoint()
        {
            float transferPointX = transferPointObject.transform.localPosition.x;
            float distanceX = transferPointObject.transform.localPosition.x - currentControlRole.transform.localPosition.x;

            foreach (var item in roleList)
            {
                item.transform.localPosition = new Vector3(item.transform.localPosition.x+distanceX, item.transform.localPosition.y, 0);
            }
            muRole.Talk("应该出来了吧！");
        }
    }
}
