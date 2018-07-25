using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Role
{
    class QiRole : RoleBase
    {
        void Start()
        {
            roleName = "七";
            speed = GameCommonValue.qiMoveRateSpeed * GameCommonValue.gameBaseLength;
            width = GameCommonValue.qiRateWidth * GameCommonValue.gameBaseLength;
            height = GameCommonValue.qiRateHeight * GameCommonValue.gameBaseLength;
            jumpHeight = GameCommonValue.qiJumpRateHeight * GameCommonValue.gameBaseLength;
            ResetState();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override string GetInfo()
        {
            return "七，速度慢跳跃低，无法抵挡攻击，控制此角色时可以通过狭小的地方。";
        }

        public override Color GetThemeColor()
        {
            return new Color(0.75f, 0.75f, 0.25f);
        }

        public override void ResetState()
        {
            blood = GameCommonValue.qiBloodVolume;
            ((RectTransform)this.gameObject.transform).sizeDelta = new Vector2(width, height);
        }

        public override void MoveToRight(bool isRight)
        {
            if (isRight)
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition + (Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition + (Vector3.left * speed * Time.deltaTime);
            }
        }
    }
}
