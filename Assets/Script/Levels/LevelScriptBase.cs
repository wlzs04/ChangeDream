using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Game;
using Assets.Script.Game.Role;

namespace Assets.Script.Levels
{
    public class LevelScriptBase : MonoBehaviour
    {
        GameObject ground = null;
        Image back = null;
        Text help = null;
        Text life = null;
        protected int levelIndex = 0;

        string savePointNameEx = "SavePointImage";

        int currentRoleIndex = 0;
        protected List<RoleBase> roleList = new List<RoleBase>();
        protected MuRole muRole = null;
        protected MingRole mingRole = null;
        protected QiRole qiRole = null;

        List<SavePoint> savePointList = new List<SavePoint>();

        protected RoleBase currentControlRole = null;//当前控制角色

        SavePoint lastSavePoint = null;

        RoleCamera roleCamera = null;

        bool bindAllRoles = false;//绑定所有角色

        protected List<string> muTalkContentList = new List<string>();
        protected System.Random random = new System.Random();

        // Use this for initialization
        protected virtual void Start()
        {
            ground = GameObject.Find("GroundRawImage");

            back = GameObject.Find("BackImage").GetComponent<Image>();
            help = GameObject.Find("HelpText").GetComponent<Text>();
            life = GameObject.Find("LifeText").GetComponent<Text>();
            InitLevel();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            float thisTickTime = Time.deltaTime;
            bool turnLeft = false;
            bool turnRight = false;
            if (Input.GetKey(KeyCode.A))
            {
                if (!bindAllRoles)
                {
                    currentControlRole.MoveToRight(false);
                }
                else
                {
                    foreach (var item in roleList)
                    {
                        item.MoveToRight(false);
                    }
                }
                turnLeft = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                turnRight = true;
                if (!bindAllRoles)
                {
                    currentControlRole.MoveToRight(true);
                }
                else
                {
                    foreach (var item in roleList)
                    {
                        item.MoveToRight(true);
                    }
                }
            }
            roleCamera.ResetViewport();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (turnLeft && !turnRight)
                {
                    currentControlRole.Jump(RoleTurnDirection.Left);
                }
                else if (!turnLeft && turnRight)
                {
                    currentControlRole.Jump(RoleTurnDirection.Right);
                }
                else
                {
                    currentControlRole.Jump();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeRole();
            }
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                bindAllRoles = !bindAllRoles;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.GetSingleInstance().EnterMainScene();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ReviveAtSavePoint();
            }
        }

        /// <summary>
        /// 随机返回场景说话集合的一句
        /// </summary>
        /// <returns></returns>
        public string GetRandomTalkContent()
        {
            return muTalkContentList[random.Next(muTalkContentList.Count)];
        }

        /// <summary>
        /// 获得关卡排名
        /// </summary>
        /// <returns></returns>
        public int GetLevelIndex()
        {
            return levelIndex;
        }

        /// <summary>
        /// 在存档点复活，通常是最后一个存档点
        /// </summary>
        void ReviveAtSavePoint()
        {
            Vector3 basePosition = lastSavePoint.GetPosition();
            basePosition.x -= GameCommonValue.saveRoleSpaceRateLength * GameCommonValue.gameBaseLength;
            foreach (var item in roleList)
            {
                item.ResetState();
                item.SetPosition(basePosition);
                item.SetRotation(Quaternion.Euler(0, 0, 0));
                basePosition.x += GameCommonValue.saveRoleSpaceRateLength * GameCommonValue.gameBaseLength;
            }

            SetRole();
            life.text = "剩余生命：" + GameManager.GetSingleInstance().GetUserdata().residueLife;
        }

        /// <summary>
        /// 设置控制角色
        /// </summary>
        void SetRole()
        {
            currentControlRole = roleList[currentRoleIndex];
            roleCamera.SetFollowRole(currentControlRole);
            back.color = currentControlRole.GetThemeColor();
        }

        /// <summary>
        /// 获得当前控制角色
        /// </summary>
        /// <returns></returns>
        public RoleBase GetCurrentRole()
        {
            return currentControlRole;
        }

        /// <summary>
        /// 初始化关卡
        /// </summary>
        void InitLevel()
        {
            int index = 0;
            while (true)
            {
                GameObject savePointObject = GameObject.Find(savePointNameEx + index);
                if (savePointObject != null)
                {
                    SavePoint savePoint = savePointObject.GetComponent<SavePoint>();
                    savePoint.SetSavePointIndex(index);
                    savePointList.Add(savePoint);
                    index++;
                }
                else
                {
                    break;
                }
            }

            int haveArriveSavePointNumber = GameManager.GetSingleInstance().GetLastSavePointIndex();

            for (int i = 0; i < haveArriveSavePointNumber + 1; i++)
            {
                savePointList[i].Arrive();
                lastSavePoint = savePointList[i];
            }

            roleCamera = new RoleCamera(ground);

            InitRole();

            ReviveAtSavePoint();
        }

        /// <summary>
        /// 初始化角色
        /// </summary>
        void InitRole()
        {
            muRole = GameObject.Find("MuRoleImage").GetComponent<MuRole>();
            mingRole = GameObject.Find("MingRoleImage").GetComponent<MingRole>();
            qiRole = GameObject.Find("QiRoleImage").GetComponent<QiRole>();

            RoleBase.SetLevelScript(this);

            roleList.Add(muRole);
            roleList.Add(mingRole);
            roleList.Add(qiRole);
        }

        /// <summary>
        /// 切换控制的角色
        /// </summary>
        void ChangeRole()
        {
            currentRoleIndex = currentRoleIndex >= 2 ? 0 : currentRoleIndex + 1;
            SetRole();
        }

        /// <summary>
        /// 绑定所有角色，绑定时玩家的操作可以作用到所有角色中
        /// </summary>
        public void BindAllRoles()
        {
            bindAllRoles = true;
        }

        /// <summary>
        /// 解除绑定所有角色
        /// </summary>
        public void UnbindAllRoles()
        {
            bindAllRoles = false;
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="text"></param>
        public void ShowHelperText(string text)
        {
            help.text = text;
        }

        /// <summary>
        /// 当角色到达保存点调用，所有角色都接触到保存点，并且此保存点未到达的话保存记录，并改为已到达。
        /// </summary>
        /// <param name="savePoint"></param>
        public void CheckSaveBySavePoint(SavePoint savePoint)
        {
            if (savePoint.IsArrived())
            {
                return;
            }
            foreach (var item in roleList)
            {
                if (item.GetOtherGameObject() != savePoint.gameObject)
                {
                    return;
                }
            }
            savePoint.Arrive();
            lastSavePoint = savePoint;
            GameManager.GetSingleInstance().SaveByLevel(levelIndex, savePoint.GetSavePointIndex());
            muRole.Talk("保存成功！");
        }

        /// <summary>
        /// 当角色到达终点调用，所有角色都接触到终点，保存记录。
        /// </summary>
        /// <param name="endObject"></param>
        public void CheckEnd(GameObject endObject)
        {
            foreach (var item in roleList)
            {
                if (item.GetOtherGameObject() != endObject)
                {
                    return;
                }
            }
            GameManager.GetSingleInstance().SaveByLevel(levelIndex + 1, 0);
            GameManager.GetSingleInstance().EnterNextLevel(levelIndex);
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        public void RoleDeath(RoleBase role, RoleDeathReason deathReason,string deathReasonText="")
        {
            GameManager.GetSingleInstance().RoleDeath();
            ReviveAtSavePoint();
            life.text = "剩余生命：" + GameManager.GetSingleInstance().GetUserdata().residueLife;

            switch (deathReason)
            {
                case RoleDeathReason.Unknown:
                    muRole.Talk("上次死亡原因：" + deathReasonText + "不明不白。");
                    break;
                case RoleDeathReason.FallDown:
                    muRole.Talk("上次死亡原因：" + deathReasonText + "跳得越高，摔得越狠。");
                    break;
                case RoleDeathReason.OutScene:
                    muRole.Talk("上次死亡原因：" + deathReasonText + "这是开发者的问题。");
                    break;
                default:
                    break;
            }
        }
    }
}
