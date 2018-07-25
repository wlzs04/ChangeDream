using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Role
{
    class MingRole : RoleBase
    {
        void Start()
        {
            roleName = "鸣";
            speed = GameCommonValue.mingMoveRateSpeed * GameCommonValue.gameBaseLength;
            width = GameCommonValue.mingRateWidth * GameCommonValue.gameBaseLength;
            height = GameCommonValue.mingRateHeight * GameCommonValue.gameBaseLength;
            jumpHeight = GameCommonValue.mingJumpRateHeight * GameCommonValue.gameBaseLength;
            ResetState();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override string GetInfo()
        {
            return "鸣，速度快跳跃高，可以抵挡攻击，控制此角色时可以辅助其他角色。";
        }

        public override Color GetThemeColor()
        {
            return new Color(1, 0.5f, 0.25f);
        }

        public override void ResetState()
        {
            blood= GameCommonValue.mingBloodVolume;
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
