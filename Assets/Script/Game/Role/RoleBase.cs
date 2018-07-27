using Assets.Script.Levels;
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
    public enum RoleTurnDirection
    {
        Normal,
        Left,
        Right
    }

    /// <summary>
    /// 角色状态
    /// </summary>
    public enum RoleState
    {
        Normal,//通常
        Run,//跑动
        Jump,//跳跃
        Climb,//攀爬
        Special,//特殊
        Controlled,//被控制
    }

    public abstract class RoleBase : MonoBehaviour
    {
        /// <summary>
        /// 基础属性
        /// </summary>
        protected string roleName = "未命名";
        protected float speed = 1;
        protected int blood = 1;
        protected float width = 1;
        protected float height = 1;
        protected float jumpHeight = 1;//角色本身跳跃高度
        protected float thisTimeJumpHeight = 1;//本次跳跃高度
        protected float fallDownHeight = 1;//角色落下高度
        protected float weight = 1;//角色体重
        protected bool isInvincible = false;//无敌
        protected RoleState roleState;
        protected RoleTurnDirection roleTurnDirection;

        protected static LevelScriptBase levelScript = null;

        /// <summary>
        /// 跳跃需要的变量
        /// </summary>
        protected float jumpStartTime = 0;
        protected float haveFinishJumpHeight = 0;
        protected float jumpTopY = 0;//本次跳跃后角色最高的位置
        protected float jumpTime = 0;
        protected float jumpNeedTime = 0.3f;
        protected float vy = 0;

        protected GameObject otherGameObject = null;

        /// <summary>
        /// 攀爬相关的变量
        /// </summary>
        protected float climbStartTime = 0;
        protected float climbTime = 0;
        protected float climbNeedTime = 0;
        protected int climbStep = 1;//只有1、2阶段
        protected float climbObjectHeight = 0;//第1阶段在y方向所需移动的距离
        protected float climbObjectWidth = 0;//第2阶段在x方向所需移动的距离
        protected float climbFromX = 0;//第2阶段移动的起始X位置

        protected bool beControlled = false;//受控制无法跳跃

        void Start()
        {
            ResetState();
        }

        protected virtual void Update()
        {
            if(levelScript==null)
            {
                return;
            }

            if (roleState == RoleState.Jump)
            {
                if (jumpTime - jumpStartTime <= jumpNeedTime)
                {
                    float tempJumpHeight = (Mathf.Sin((Time.time - jumpStartTime) / jumpNeedTime * Mathf.PI / 2) - Mathf.Sin((jumpTime - jumpStartTime) / jumpNeedTime * Mathf.PI / 2)) * thisTimeJumpHeight;
                    haveFinishJumpHeight += tempJumpHeight;
                    gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, tempJumpHeight);

                    jumpTime = Time.time;
                    jumpTopY = gameObject.transform.localPosition.y;
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
            else if (roleState == RoleState.Climb)
            {
                if (climbStep == 1)
                {
                    if (climbTime - climbStartTime <= GameCommonValue.climbStepOneNeedTime)
                    {
                        float tempClimbHeight = (Time.time- climbTime) / GameCommonValue.climbStepOneNeedTime * climbObjectHeight;
                        gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, tempClimbHeight);
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, (Time.time - climbStartTime) / GameCommonValue.climbStepOneNeedTime *(-45));
                        climbTime = Time.time;
                    }
                    else
                    {
                        climbObjectWidth = ((RectTransform)(otherGameObject.transform)).sizeDelta.x + ((RectTransform)(gameObject.transform)).sizeDelta.x;//otherGameObject.transform.localPosition.x-gameObject.transform.localPosition.x+((RectTransform)(otherGameObject.transform)).sizeDelta.x;
                        climbFromX = gameObject.transform.localPosition.x;
                        climbStep = 2;
                    }
                }
                else if (climbStep == 2)
                {
                    if (climbTime - climbStartTime <= GameCommonValue.climbStepTwoNeedTime)
                    {
                        float rate =  (Time.time - climbStartTime - GameCommonValue.climbStepOneNeedTime) / (GameCommonValue.climbStepTwoNeedTime - GameCommonValue.climbStepOneNeedTime);
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, (1-rate) * (-45));
                        gameObject.transform.localPosition =  new Vector3(climbFromX+rate * climbObjectWidth, gameObject.transform.localPosition.y);

                        climbTime = Time.time;
                    }
                    else
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        gameObject.transform.localPosition = new Vector3(climbFromX+climbObjectWidth, otherGameObject.transform.localPosition.y + ((RectTransform)otherGameObject.transform).sizeDelta.y);
                        climbStep = 0;
                        roleState = RoleState.Normal;
                    }
                }
            }
        }

        public void SetRotation(Quaternion quaternion)
        {
            transform.rotation = quaternion;
        }

        public RoleState GetRoleState()
        {
            return roleState;
        }

        /// <summary>
        /// 获得角色名称
        /// </summary>
        /// <returns></returns>
        public string GetRoleName()
        {
            return roleName;
        }

        /// <summary>
        /// 设置关卡脚本
        /// </summary>
        /// <param name="tutorialLevelScript"></param>
        public static void SetLevelScript(LevelScriptBase levelScript)
        {
            RoleBase.levelScript = levelScript;
        }

        /// <summary>
        /// 停止跳跃
        /// </summary>
        public void StopJump()
        {
            if(roleState==RoleState.Jump)
            {
                roleState = RoleState.Normal;
            }
            if(isInvincible)
            {
                isInvincible = false;
            }
            else if (jumpTopY - gameObject.transform.localPosition.y >= fallDownHeight)
            {
                levelScript.RoleDeath(this, "跳得越高，摔得越狠！");
            }
        }

        /// <summary>
        /// 爬过物体
        /// </summary>
        /// <param name="otherGameObject"></param>
        void Climb(GameObject otherGameObject)
        {
            if(roleState == RoleState.Climb)
            {
                return;
            }
            roleState = RoleState.Climb;
            this.otherGameObject = otherGameObject;
            climbStartTime = Time.time;
            climbTime = Time.time;
            climbStep = 1;
            climbObjectHeight = otherGameObject.transform.localPosition.y + ((RectTransform)otherGameObject.transform).sizeDelta.y - transform.localPosition.y;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (levelScript == null)
            {
                return;
            }
            switch (other.gameObject.tag)
            {
                case "LeftBlock":
                case "RightBlock":
                    StopJump();
                    beControlled =true;
                    break;
                case "Ground":
                    StopJump();
                    break;
                case "Climb":
                    if(Input.GetKey(KeyCode.J))
                    {
                        Climb(other.gameObject);
                    }
                    break;
                case "SavePoint":
                    otherGameObject = other.gameObject;
                    levelScript.CheckSaveBySavePoint(otherGameObject.GetComponent<SavePoint>());
                    break;
                case "End":
                    otherGameObject = other.gameObject;
                    levelScript.CheckEnd(otherGameObject);
                    break;
                case "Helper":
                    levelScript.ShowHelperText(other.gameObject.GetComponent<HelperScript>().helperText);
                    break;
                case "Death":
                    levelScript.RoleDeath(this, other.gameObject.GetComponent<DeathScript>().deathReason);
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (levelScript == null)
            {
                return;
            }
            switch (other.gameObject.tag)
            {
                case "Ground":
                    break;
                case "LeftBlock":
                case "LeftBlockCanJump":
                    MoveToRight(true);
                    break;
                case "RightBlock":
                case "RightBlockCanJump":
                    MoveToRight(false);
                    break;
                case "Block":
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (levelScript == null)
            {
                return;
            }
            switch (other.gameObject.tag)
            {
                case "LeftBlock":
                case "LeftBlockCanJump":
                    beControlled = false;
                    Jump(RoleTurnDirection.Right);
                    break;
                case "RightBlock":
                case "RightBlockCanJump":
                    beControlled = false;
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

        public float GetWeight()
        {
            return weight;
        }

        /// <summary>
        /// 控制角色向某方向跳跃，默认上方向
        /// </summary>
        /// <param name="roleTurnDirection"></param>
        public void Jump(RoleTurnDirection roleTurnDirection= RoleTurnDirection.Normal)
        {
            if(beControlled)
            {
                return;
            }
            if (roleState == RoleState.Normal|| roleState == RoleState.Run)
            {
                this.roleTurnDirection = roleTurnDirection;
                roleState = RoleState.Jump;
                jumpStartTime = Time.time;
                jumpTime = jumpStartTime;
                thisTimeJumpHeight = jumpHeight;
                haveFinishJumpHeight = 0;
                vy = 0;
            }
        }

        /// <summary>
        /// 角色受外力作用跳跃
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="jumpHeight"></param>
        /// <param name="roleTurnDirection"></param>
        public void JumpByOther(GameObject gameObject,float jumpHeight, RoleTurnDirection roleTurnDirection = RoleTurnDirection.Normal)
        {
            this.roleTurnDirection = roleTurnDirection;
            roleState = RoleState.Jump;
            jumpStartTime = Time.time;
            jumpTime = jumpStartTime;
            thisTimeJumpHeight = jumpHeight;
            haveFinishJumpHeight = 0;
            vy = 0;
        }

        /// <summary>
        /// 控制角色指定方向移动
        /// </summary>
        /// <param name="isRight"></param>
        public void MoveToRight(bool isRight)
        {
            if (roleState == RoleState.Normal || roleState == RoleState.Run)
            {
                if (isRight)
                {
                    roleTurnDirection = RoleTurnDirection.Right;
                    gameObject.transform.localPosition = gameObject.transform.localPosition + (Vector3.right * speed * Time.deltaTime);
                }
                else
                {
                    roleTurnDirection = RoleTurnDirection.Left;
                    gameObject.transform.localPosition = gameObject.transform.localPosition + (Vector3.left * speed * Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// 角色受外力作用移动
        /// </summary>
        public void MoveByOther(GameObject gameObject,float length, RoleTurnDirection roleTurnDirection)
        {
            if (roleTurnDirection== RoleTurnDirection.Right)
            {
                this.roleTurnDirection = RoleTurnDirection.Right;
                this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + (Vector3.right * length);
            }
            else
            {
                this.roleTurnDirection = RoleTurnDirection.Left;
                this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + (Vector3.left * length);
            }
        }

        /// <summary>
        /// 角色进行特殊操作
        /// </summary>
        public virtual void SpecialAction()
        {

        }

        /// <summary>
        /// 获得与角色有交互的物体。
        /// </summary>
        /// <returns></returns>
        public GameObject GetOtherGameObject()
        {
            return otherGameObject;
        }

        /// <summary>
        /// 设置无敌状态
        /// </summary>
        /// <param name="isInvincible"></param>
        public void SetInvincible(bool isInvincible)
        {
            this.isInvincible = isInvincible;
        }
    }
}
