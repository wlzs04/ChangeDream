using Assets.Script.Game.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Door
{
    abstract class DoorBase : MonoBehaviour
    {
        protected bool isOpening = false;

        protected string doorName = "door";

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {

        }

        /// <summary>
        /// 角色进入门，与门发生接触
        /// </summary>
        /// <param name="role"></param>
        public abstract void EnterDoor(RoleBase role);

        /// <summary>
        /// 角色在门的范围内
        /// </summary>
        /// <param name="role"></param>
        public abstract void StayDoor(RoleBase role);

        /// <summary>
        /// 角色离开门
        /// </summary>
        /// <param name="role"></param>
        public abstract void ExitDoor(RoleBase role);

        /// <summary>
        /// 打开门
        /// </summary>
        public abstract void Open();

        public bool IsOpening()
        {
            return isOpening;
        }
    }
}
