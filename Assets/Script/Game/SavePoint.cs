using Assets.Script.Game.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game
{
    public class SavePoint : MonoBehaviour
    {
        bool haveArrive = false;
        int savePointIndex = 0;
        public bool canCarry = false;// 是否可以携带
        bool isCarrying = false;
        RoleBase role = null;

        public void SetSavePointIndex(int index)
        {
            savePointIndex = index;
        }

        public int GetSavePointIndex()
        {
            return savePointIndex;
        }

        /// <summary>
        /// 到达存档点
        /// </summary>
        public void Arrive()
        {
            haveArrive = true;
        }

        /// <summary>
        /// 携带存档点
        /// </summary>
        public void CarryBy(RoleBase role)
        {
            this.role = role;
            isCarrying = true;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 放下存档点
        /// </summary>
        public void Drop()
        {
            float distanceX = role.GetRoleTurnDirection() == RoleTurnDirection.Right ? 70 : -70;
            transform.localPosition = role.transform.localPosition + new Vector3(distanceX, 0, 0);
            role = null;
            isCarrying = false;
            gameObject.SetActive(true);
        }

        public Vector3 GetPosition()
        {
            return gameObject.transform.localPosition;
        }

        /// <summary>
        /// 判断此保存点是否已经到达
        /// </summary>
        /// <returns></returns>
        public bool IsArrived()
        {
            return haveArrive;
        }
    }
}
