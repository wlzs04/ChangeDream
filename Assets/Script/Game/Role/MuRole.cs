using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Role
{
    class MuRole : RoleBase
    {
        void Start()
        {
            roleName = "木";
            speed = GameCommonValue.muMoveRateSpeed * GameCommonValue.gameBaseLength;
            width = GameCommonValue.muRateWidth * GameCommonValue.gameBaseLength;
            height = GameCommonValue.muRateHeight * GameCommonValue.gameBaseLength;
            jumpHeight = GameCommonValue.muJumpRateHeight * GameCommonValue.gameBaseLength;
            ResetState();
        }

        protected override void Update()
        {
            base.Update();
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
    }
}
