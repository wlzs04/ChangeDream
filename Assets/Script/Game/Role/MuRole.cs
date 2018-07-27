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
        string thinkText = "";

        void Start()
        {
            roleName = "木";
            speed = GameCommonValue.muMoveRateSpeed * GameCommonValue.gameBaseLength;
            width = GameCommonValue.muRateWidth * GameCommonValue.gameBaseLength;
            height = GameCommonValue.muRateHeight * GameCommonValue.gameBaseLength;
            jumpHeight = GameCommonValue.muJumpRateHeight * GameCommonValue.gameBaseLength;
            fallDownHeight = GameCommonValue.muFallDownRateHeight * GameCommonValue.gameBaseLength;
            weight = GameCommonValue.muRateWeight;
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
                    Talk(thinkText);
                }
                else if (Time.time - lastStopTalkTime >= GameCommonValue.muPrepareTalkNeedTime)
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
        public void Think(string thinkText)
        {
            isThinking = true;
            this.thinkText = thinkText;
        }

        /// <summary>
        /// 停止思考
        /// </summary>
        public void StopThink()
        {
            isThinking = false;
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

        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);

            if (other.gameObject.tag == "Helper")
            {
                HelperScript helperScript = other.gameObject.GetComponent<HelperScript>();
                if (helperScript.needThink&& levelScript.GetCurrentRole() == this)
                {
                    Think(helperScript.thinkText);
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            if (other.gameObject.tag == "Helper")
            {
                HelperScript helperScript = other.gameObject.GetComponent<HelperScript>();
                if (helperScript.needThink && levelScript.GetCurrentRole() == this)
                {
                    StopThink();
                }
            }
        }
    }
}
