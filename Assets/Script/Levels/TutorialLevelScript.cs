using Assets.Script;
using Assets.Script.Game;
using Assets.Script.Game.Role;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevelScript : MonoBehaviour {
    GameObject ground = null;
    Image back = null;
    Text help = null;
    int levelIndex = 0;

    string savePointNameEx = "SavePointImage";

    int currentRoleIndex = 0;
    List<RoleBase> roleList = new List<RoleBase>();
    MuRole muRole = null;

    List<SavePoint> savePointList = new List<SavePoint>();

    RoleBase currentControlRole = null;//当前控制角色

    SavePoint lastSavePoint = null;

    RoleCamera roleCamera = null;

    bool bindAllRoles = false;//绑定所有角色

    List<string> muTalkContentList = new List<string>();
    System.Random random = new System.Random();

    // Use this for initialization
    void Start() {
        ground = GameObject.Find("GroundRawImage");

        back = GameObject.Find("BackImage").GetComponent<Image>();
        help = GameObject.Find("HelpText").GetComponent<Text>();
        InitLevel();

        muTalkContentList.Add("控制的角色有点多，会不会不习惯！");
        muTalkContentList.Add("墙壁会推人吗？");
        muTalkContentList.Add("据说跳的越高，进地越深。");
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
            item.SetRotation(Quaternion.Euler(0,0,0));
            basePosition.x += GameCommonValue.saveRoleSpaceRateLength * GameCommonValue.gameBaseLength;
        }

        SetRole();
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

        for (int i = 0; i < haveArriveSavePointNumber+1; i++)
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
        MingRole mingRole = GameObject.Find("MingRoleImage").GetComponent<MingRole>();
        QiRole qiRole = GameObject.Find("QiRoleImage").GetComponent<QiRole>();

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
        currentRoleIndex= currentRoleIndex>=2?0: currentRoleIndex+1;
        SetRole();
    }
	
	// Update is called once per frame
	void Update () {
        float thisTickTime = Time.deltaTime;
        bool turnLeft = false;
        bool turnRight = false;
        if (Input.GetKey(KeyCode.A))
        {
            if(!bindAllRoles)
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
            if(turnLeft&&!turnRight)
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
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    currentControlRole.SpecialAction();
        //}
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
    /// 保存到达的保存点
    /// </summary>
    public void Save()
    {

    }

    /// <summary>
    /// 到达的终点
    /// </summary>
    public void End()
    {

    }

    /// <summary>
    /// 当角色到达保存点调用，所有角色都接触到保存点，并且此保存点未到达的话保存记录，并改为已到达。
    /// </summary>
    /// <param name="savePoint"></param>
    public void CheckSaveBySavePoint(SavePoint savePoint)
    {
        if(savePoint.IsArrived())
        {
            return;
        }
        foreach (var item in roleList)
        {
            if(item.GetOtherGameObject()!=savePoint.gameObject)
            {
                return;
            }
        }
        savePoint.Arrive();
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
        GameManager.GetSingleInstance().SaveByLevel(levelIndex+1, 0);
        GameManager.GetSingleInstance().EnterMainScene();
    }
}
