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
