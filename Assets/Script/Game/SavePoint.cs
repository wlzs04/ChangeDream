using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game
{
    class SavePoint
    {
        GameObject gameObject = null;
        bool haveArrive = false;

        public void SetSaveObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
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
    }
}
