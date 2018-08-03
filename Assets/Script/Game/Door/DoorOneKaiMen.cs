using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Game.Role;
using Assets.Script.Helper;
using UnityEngine;

namespace Assets.Script.Game.Door
{
    /// <summary>
    /// 第一关：开门
    /// </summary>
    class DoorOneKaiMen : DoorBase
    {
        bool idOpening = false;
        float fromPositionY = 0;//起始点
        float raisingHeight = 400;//升起高度
        float raisingNeedTime = 3;//升起所需时间
        float raisingStartTime = 0;//升起所需时间

        protected override void Start()
        {
            doorName = "开门";
            fromPositionY = gameObject.transform.localPosition.y;
        }

        protected override void Update()
        {
        }

        public override void EnterDoor(RoleBase role)
        {
            if(!idOpening&&Input.GetKey(KeyCode.J))
            {
                Open();
            }
            role.StopJump();
            role.CanntJump();
        }

        public override void StayDoor(RoleBase role)
        {
            role.MoveToRight(false);
        }

        public override void ExitDoor(RoleBase role)
        {
            role.CanJump();
            role.Jump(RoleTurnDirection.Left);
        }

        public override void Open()
        {
            idOpening = true;
            AnimationHelper animationHelper = AnimationHelper.GetSingleInstance();
            animationHelper.AddAnimation(doorName, gameObject, transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y + raisingHeight,0), raisingNeedTime);
        }
    }
}
