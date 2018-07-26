using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Game.Role
{
    class MuRole : RoleBase
    {
        Text talkText = null;

        /// <summary>
        /// 说话相关变量
        /// </summary>
        float lastTalkTime = 0;
        float lastStopTalkTime = 0;
        bool isTalking = false;
        bool isThinking = false;

        void Start()
        {
            roleName = "木";
            speed = GameCommonValue.muMoveRateSpeed * GameCommonValue.gameBaseLength;
            width = GameCommonValue.muRateWidth * GameCommonValue.gameBaseLength;
            height = GameCommonValue.muRateHeight * GameCommonValue.gameBaseLength;
            jumpHeight = GameCommonValue.muJumpRateHeight * GameCommonValue.gameBaseLength;
            ResetState();
            talkText = GameObject.Find("MuTalkText").GetComponent<Text>();
            talkText.text = "";
        }

        protected override void Update()
        {
            base.Update();

            if(isTalking)
            {
                if (Time.time - lastTalkTime >= GameCommonValue.muTalkShowTime)
                {
                    StopTalk();
                }
            }
            else
            {
                if(isThinking)
                {
                    return;
                }
                if (Time.time - lastStopTalkTime >= GameCommonValue.muPrepareTalkNeedTime)
                {
                    Talk(levelScript.GetRandomTalkContent());
                }
            }
        }

        public override string GetInfo()
        {
            return "木，速度中跳跃中，无法抵挡攻击，控制此角色时可以获得过关提示。";
        }

        public override Color GetThemeColor()
        {
            return new Color(1,0.5f,0.5f);
        }

        public override void ResetState()
        {
            roleState = RoleState.Normal;
            blood = GameCommonValue.muBloodVolume;
            ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// 进行思考
        /// </summary>
        public void Think()
        {

        }

        /// <summary>
        /// 说话内容
        /// </summary>
        /// <param name="content"></param>
        public void Talk(string content)
        {
            talkText.text = content;
            lastTalkTime = Time.time;
            isTalking = true;
        }

        /// <summary>
        /// 停止说话
        /// </summary>
        public void StopTalk()
        {
            talkText.text = "";
            lastStopTalkTime = Time.time;
            isTalking = false;
        }
    }
}
