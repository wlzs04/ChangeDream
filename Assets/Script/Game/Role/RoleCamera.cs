using System;
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

        float lastRolePositionX;
        bool isChangingView = false;
        float changeStartTime = 0;

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
            if(this.role!=null)
            {
                lastRolePositionX = this.role.GetPositionX();
                changeStartTime = Time.time;
                isChangingView = true;
            }
            this.role = role;

            ResetViewport();
        }

        /// <summary>
        /// 根据跟随角色的位置重新设置视口
        /// </summary>
        public void ResetViewport()
        {
            float backX = 0;

            if (!lockViewport)
            {
                if (isChangingView)
                {
                    backX = lastRolePositionX - role.GetPositionX();
                    backX = lastRolePositionX - backX * (Time.time - changeStartTime) / GameCommonValue.cameraChangeRoleTime - viewportWidth / 2;
                    if (Time.time - changeStartTime >= GameCommonValue.cameraChangeRoleTime)
                    {
                        isChangingView = false;
                    }
                }
                else
                {
                    backX = role.GetPositionX() - viewportWidth / 2;
                }
                if (backX <= 0)
                {
                    backTransform.position = new Vector3(0, backTransform.position.y);
                    return;
                }
                else if (backX >= backWidth - viewportWidth)
                {
                    backTransform.position = new Vector3(viewportWidth - backWidth, backTransform.position.y);
                    return;
                }
                backTransform.position = new Vector3(-backX, backTransform.position.y);
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
