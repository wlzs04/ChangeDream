using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Role
{
    public class MingRole : RoleBase
    {
        SavePoint carrySavePoint = null;
        bool isSavePointArea = false;

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
            if(Input.GetKeyDown(KeyCode.J))
            {
                if(isSavePointArea&& carrySavePoint!=null)
                {
                    carrySavePoint.Drop();
                }
            }
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
            if(other.gameObject.tag == "SavePoint")
            {
                SavePoint sp = other.gameObject.GetComponent<SavePoint>();
                if (sp.canCarry)
                {
                    if(Input.GetKey(KeyCode.J))
                    {
                        sp.CarryBy(this);
                        carrySavePoint = sp;
                    }
                }
            }

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

            if(other.gameObject.tag == "SavePointArea")
            {
                isSavePointArea = true;
            }
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            if (other.gameObject.tag == "SavePointArea")
            {
                isSavePointArea = false;
            }
        }
    }
}
