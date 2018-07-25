using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Collections;

namespace Assets.Script.Game.Role
{
    /// <summary>
    /// 角色朝向
    /// </summary>
    enum RoleTurnDirection
    {
        Normal,
        Left,
        Right
    }

    /// <summary>
    /// 角色状态
    /// </summary>
    enum RoleState
    {
        Normal,//通常
        Run,//跑动
        Jump,//跳跃
        Special//特殊
    }

    abstract class RoleBase : MonoBehaviour
    {
        /// <summary>
        /// 基础属性
        /// </summary>
        protected string roleName = "未命名";
        protected float speed = 1;
        protected int blood = 1;
        protected float width = 1;
        protected float height = 1;
        protected float jumpHeight = 1;
        protected RoleState roleState;
        protected RoleTurnDirection roleTurnDirection;

        static TutorialLevelScript levelScript = null;

        /// <summary>
        /// 跳跃需要的变量
        /// </summary>
        protected float jumpStartTime = 0;
        protected float haveFinishJumpHeight = 0;
        protected float jumpTime = 0;
        protected float jumpNeedTime = 0.3f;
        protected float vy = 0;

        void Start()
        {
            ResetState();
        }

        protected virtual void Update()
        {
            if (roleState == RoleState.Jump)
            {
                if (jumpTime - jumpStartTime <= jumpNeedTime)
                {
                    float tempJumpHeight = (Mathf.Sin((Time.time - jumpStartTime) / jumpNeedTime * Mathf.PI / 2) - Mathf.Sin((jumpTime - jumpStartTime) / jumpNeedTime * Mathf.PI / 2)) * jumpHeight;
                    haveFinishJumpHeight += tempJumpHeight;
                    gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, tempJumpHeight);

                    jumpTime = Time.time;
                }
                else
                {
                    vy += GameCommonValue.acceleration * GameCommonValue.gameBaseLength * Time.deltaTime;
                    gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, -vy);
                }
                if (roleTurnDirection == RoleTurnDirection.Right)
                {
                    gameObject.transform.localPosition = gameObject.transform.localPosition + (Vector3.right * speed * Time.deltaTime);
                }
                else if (roleTurnDirection == RoleTurnDirection.Left)
                {
                    gameObject.transform.localPosition = gameObject.transform.localPosition + (Vector3.left * speed * Time.deltaTime);
                }
            }
        }

        public static void SetLevelScript(TutorialLevelScript tutorialLevelScript)
        {
            levelScript = tutorialLevelScript;
        }

        public void StopJump()
        {
            if(roleState==RoleState.Jump)
            {
                roleState = RoleState.Normal;
            }
        }

        /// <summary>
        /// 爬过物体
        /// </summary>
        /// <param name="otherGameObject"></param>
        void Climb(GameObject otherGameObject)
        {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Ground":
                    StopJump();
                    break;
                case "Special":
                    Climb(other.gameObject);
                    break;
                case "Helper":
                    levelScript.ShowHelperText(other.gameObject.GetComponent<HelperScript>().helperText);
                    break;
                default:
                    break;
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Ground":
                    break;
                case "LeftBlock":
                    MoveToRight(true);
                    break;
                case "RightBlock":
                    MoveToRight(false);
                    break;
                case "Block":
                    break;
                case "SavePoint":
                    levelScript.Save();
                    break;
                case "End":
                    levelScript.End();
                    break;
                default:
                    break;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "LeftBlock":
                    Jump(RoleTurnDirection.Right);
                    break;
                case "RightBlock":
                    Jump(RoleTurnDirection.Left);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 重新设置状态
        /// </summary>
        public abstract void ResetState();

        public float GetSpeed()
        {
            return speed;
        }

        public void SetPosition(Vector3 position)
        {
            gameObject.transform.localPosition = position;
        }

        public float GetPositionX()
        {
            return gameObject.transform.localPosition.x;
        }

        public abstract Color GetThemeColor();

        public abstract string GetInfo();

        public int GetBlood()
        {
            return blood;
        }

        public void Jump(RoleTurnDirection roleTurnDirection= RoleTurnDirection.Normal)
        {
            if (roleState != RoleState.Normal)
            {
                return;
            }
            this.roleTurnDirection = roleTurnDirection;
            roleState = RoleState.Jump;
            jumpStartTime = Time.time;
            jumpTime = jumpStartTime;
            haveFinishJumpHeight = 0;
            vy = 0;
        }

        /// <summary>
        /// 控制角色指定方向移动
        /// </summary>
        /// <param name="isRight"></param>
        public abstract void MoveToRight(bool isRight);

        /// <summary>
        /// 角色进行特殊操作
        /// </summary>
        public virtual void SpecialAction()
        {

        }
    }
}
