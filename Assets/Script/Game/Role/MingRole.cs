﻿using System;
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
            fallDownHeight = GameCommonValue.mingFallDownRateHeight * GameCommonValue.gameBaseLength;
            weight = GameCommonValue.mingRateWeight;
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
            roleState = RoleState.Normal;
            blood = GameCommonValue.mingBloodVolume;
            ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if(other.gameObject.tag=="Role")
            {
                RoleBase role = other.gameObject.GetComponent<RoleBase>();
                if(role.GetRoleName() == "七"&& role.GetRoleState()==RoleState.Jump)
                {
                    if(Input.GetKey(KeyCode.J)&& levelScript.GetCurrentRole()==this)
                    {
                        role.SetInvincible(true);
                        role.JumpByOther(gameObject,jumpHeight,roleTurnDirection);
                    }
                }
            }
        }
    }
}
