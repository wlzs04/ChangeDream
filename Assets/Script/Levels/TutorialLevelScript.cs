using Assets.Script.Game;
using Assets.Script.Game.Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevelScript : MonoBehaviour {
    GameObject ground = null;
    Image back = null;
    Text help = null;

    string savePointNameEx = "SavePointImage";

    int currentRoleIndex = 0;
    List<RoleBase> roleList = new List<RoleBase>();
    List<SavePoint> savePointList = new List<SavePoint>();

    RoleBase currentControlRole = null;//当前控制角色

    SavePoint lastSavePoint = null;

    RoleCamera roleCamera = null;

    bool bindAllRoles = false;//绑定所有角色

    // Use this for initialization
    void Start () {
        ground = GameObject.Find("GroundRawImage");

        back = GameObject.Find("BackImage").GetComponent<Image>();
        help = GameObject.Find("HelpText").GetComponent<Text>();
        InitLevel();
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
    /// 初始化关卡
    /// </summary>
    void InitLevel()
    {
        int index = 0;
        while (true)
        {
            GameObject savePoint = GameObject.Find(savePointNameEx+ index);
            if (savePoint!=null)
            {
                SavePoint sp = new SavePoint();
                sp.SetSaveObject(savePoint);
                savePointList.Add(sp);
                index++;
            }
            else
            {
                break;
            }
        }
        savePointList[0].Arrive();
        lastSavePoint = savePointList[0];

        roleCamera = new RoleCamera(ground);

        InitRole();

        ReviveAtSavePoint();
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    void InitRole()
    {
        MuRole muRole = GameObject.Find("MuRoleImage").GetComponent<MuRole>();
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
}
