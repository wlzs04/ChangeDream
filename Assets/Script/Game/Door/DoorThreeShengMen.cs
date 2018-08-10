using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Game.Role;
using Assets.Script.Helper;
using Assets.Script.Levels;
using UnityEngine;

namespace Assets.Script.Game.Door
{
    /// <summary>
    /// 第三关：生门
    /// </summary>
    class DoorThreeShengMen : DoorBase
    {
        int roleInsideNumber = 0;
        float raiseHeight = 450;
        float raiseNeedTime = 0.5f;
        float doorStartY = 0;
        float loopLength = 800;//循环的长度

        public bool loopStart = false;
        public bool loopEnd = false;
        public GameObject doorLast = null;
        public GameObject doorNext = null;

        public GameObject startDoor = null;
        public GameObject endDoor = null;

        protected override void Start()
        {
            doorName = "生门";
            doorStartY = transform.localPosition.y;
        }

        public override void EnterDoor(RoleBase role)
        {
            if (roleInsideNumber<=0)
            {
                roleInsideNumber = 0;
                Open();
            }
            roleInsideNumber++;
        }

        public override void StayDoor(RoleBase role)
        {
            if (loopStart)
            {
                if (role.GetPositionX() < transform.localPosition.x)
                {
                    if(role.GetContent("startLoop")!=null)
                    {
                        int loopNumber = (int)role.GetContent("loopNumber");
                        if (loopNumber>0)
                        {
                            role.SetPosition(new Vector3(role.GetPositionX() + 800, role.GetPosition().y, 0));
                            endDoor.GetComponent<DoorThreeShengMen>().CopyDoorStateFromAnotherDoor(this);
                            loopNumber--;
                            role.AddContent("loopNumber", loopNumber);
                            if(role.GetRoleName()== "木")
                            {
                                ((MuRole)role).Talk(loopNumber.ToString());
                            }
                            ((EightDoorsLevelScript)RoleBase.GetLevelScript()).CheckVisibleInShengMen();
                        }
                        else
                        {
                            role.RemoveContent("loopNumber");
                            role.RemoveContent("startLoop");
                        }
                    }
                    return;
                }
                else
                {
                    if(role.GetContent("startLoop")==null)
                    {
                        ((EightDoorsLevelScript)RoleBase.GetLevelScript()).EnterShengMen();
                        role.AddContent("startLoop", true);
                        role.AddContent("loopNumber", 0);
                        ((EightDoorsLevelScript)RoleBase.GetLevelScript()).CheckVisibleInShengMen();
                    }
                }
            }
            if (loopEnd)
            {
                if (role.GetPositionX() > transform.localPosition.x)
                {
                    if (role.GetContent("startLoop")!=null)
                    {
                        int loopNumber = (int)role.GetContent("loopNumber");
                        loopNumber++;
                        role.AddContent("loopNumber", loopNumber);
                        role.SetPosition(new Vector3(role.GetPositionX() - 800, role.GetPosition().y, 0));
                        startDoor.GetComponent<DoorThreeShengMen>().CopyDoorStateFromAnotherDoor(this);

                        if (role.GetRoleName() == "木")
                        {
                            ((MuRole)role).Talk(loopNumber.ToString());
                        }
                        ((EightDoorsLevelScript)RoleBase.GetLevelScript()).CheckVisibleInShengMen();
                    }
                    return;
                }
            }
        }

        public override void ExitDoor(RoleBase role)
        {
            roleInsideNumber--;
            if(roleInsideNumber<=0)
            {
                roleInsideNumber = 0;
                Close();
            }
        }

        /// <summary>
        /// 开门
        /// </summary>
        public override void Open()
        {
            AnimationHelper.GetSingleInstance().AddAnimation("开门上升", gameObject, transform.localPosition, new Vector3(transform.localPosition.x, doorStartY + raiseHeight, 0), raiseNeedTime,
                (go)=>{isOpening = true; CheckWhileBreak(); });
        }

        /// <summary>
        /// 检测循环是否被打破，当前使用方法是三人抬起中间循环的四门，控制角色在中间，效果为画面中看不见门，此关过。
        /// </summary>
        public void CheckWhileBreak()
        {
            RoleCamera roleCamera = RoleBase.GetLevelScript().GetRoleCamera();
            
            for (int i = 1; i < 8; i++)
            {
                GameObject tempDoor = GameObject.Find("DoorThreeShengMenInside" + i);
                if (roleCamera.CheckGameObjectInView(tempDoor))
                {
                    return;
                }
            }
            ((EightDoorsLevelScript)RoleBase.GetLevelScript()).TransferRoleToTransferPoint();
        }

        /// <summary>
        /// 关门
        /// </summary>
        public void Close()
        {
            AnimationHelper.GetSingleInstance().AddAnimation("开门下降", gameObject, transform.localPosition, new Vector3(transform.localPosition.x, doorStartY, 0), raiseNeedTime,
                (go) => { isOpening = false; });
        }

        /// <summary>
        /// 拷贝其它门的状态，在通过循环门移动角色时调用，防止门出现较大的位置上的变化
        /// </summary>
        public void CopyDoorStateFromAnotherDoor(DoorThreeShengMen copyFromDoor)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, copyFromDoor.transform.localPosition.y, 0);
        }
    }
}
