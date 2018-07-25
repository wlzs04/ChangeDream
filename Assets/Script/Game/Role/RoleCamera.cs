﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Game.Role
{
    class RoleCamera
    {
        GameObject back = null;
        RectTransform backTransform = null;
        RoleBase role = null;

        bool lockViewport = false;
        float backWidth = 0;

        float viewportWidth = 1024;

        public RoleCamera(GameObject back)
        {
            this.back = back;
            backTransform = (RectTransform)back.transform;
            backWidth = ((RectTransform)back.transform).sizeDelta.x;
            viewportWidth = Camera.main.pixelWidth;
        }

        /// <summary>
        /// 设置跟随角色
        /// </summary>
        /// <param name="role"></param>
        public void SetFollowRole(RoleBase role)
        {
            this.role = role;
            ResetViewport();
        }

        /// <summary>
        /// 根据跟随角色的位置重新设置视口
        /// </summary>
        public void ResetViewport()
        {
            if (!lockViewport)
            {
                float backX = 0;
                backX = role.GetPositionX() - viewportWidth / 2;
                if (backX <= 0)
                {
                    backTransform.offsetMin = new Vector2(0, backTransform.offsetMin.y);
                    return;
                }
                else if(backX >= backWidth - viewportWidth)
                {
                    backTransform.offsetMin = new Vector2(viewportWidth - backWidth, backTransform.offsetMin.y);
                    return;
                }
                backTransform.offsetMin = new Vector2(-backX, backTransform.offsetMin.y);
            }
        }

        /// <summary>
        /// 锁定视角
        /// </summary>
        public void LockViewport()
        {
            lockViewport = true;
        }

        /// <summary>
        /// 解锁视角
        /// </summary>
        public void UnlockViewport()
        {
            lockViewport = false;
        }
    }
}