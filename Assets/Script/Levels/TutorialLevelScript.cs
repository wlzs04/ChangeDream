using Assets.Script.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelScript : MonoBehaviour {
    GameObject ground = null;

    float groundY = 0;
    string savePointNameEx = "SavePointImage";

    List<Role> roleList = new List<Role>();
    List<SavePoint> savePointList = new List<SavePoint>();

    // Use this for initialization
    void Start () {
        ground = GameObject.Find("GroundRawImage");
        groundY = ground.transform.localPosition.x;


    }

    /// <summary>
    /// 在存档点复活，通常是最后一个存档点
    /// </summary>
    void ReviveAtSavePoint()
    {
        
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
            }
            else
            {
                break;
            }
        }
        savePointList[0].Arrive();
        ReviveAtSavePoint();
    }
	
	// Update is called once per frame
	void Update () {
        float thisTickTime = Time.deltaTime;
		if(Input.GetKey(KeyCode.A))
        {
            float lastPositionX = ground.transform.localPosition.x;
            ground.transform.localPosition = new Vector3(lastPositionX - 10, groundY, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
