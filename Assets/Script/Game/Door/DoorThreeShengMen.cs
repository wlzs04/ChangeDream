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
    /// 第三关：生门
    /// </summary>
    class DoorThreeShengMen : DoorBase
    {
        bool isOpening = false;
        int roleInsideNumber = 0;
        float raiseHeight = 400;


        public override void EnterDoor(RoleBase role)
        {
            if(roleInsideNumber<=0)
            {
                roleInsideNumber = 0;
                Close();
            }
            roleInsideNumber++;
        }

        public override void ExitDoor(RoleBase role)
        {
            roleInsideNumber--;
            if(roleInsideNumber<=0)
            {
                roleInsideNumber = 0;
                Open();
            }
        }

        public override void Open()
        {
            isOpening = true;
            AnimationHelper.GetSingleInstance().AddAnimation("开门上升", gameObject, transform.localPosition, transform.localPosition + new Vector3(transform.localPosition.x, transform.localPosition.y + raiseHeight, 0), 1);
        }

        /// <summary>
        /// 关门
        /// </summary>
        public void Close()
        {
            isOpening = false;
            AnimationHelper.GetSingleInstance().AddAnimation("开门上升", gameObject, transform.localPosition, transform.localPosition + new Vector3(transform.localPosition.x, 0, 0), 1);
        }

        public override void StayDoor(RoleBase role)
        {
            throw new NotImplementedException();
        }
    }
}
