using Assets.Script.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script
{
    class GameManager
    {
        static GameManager gameManager = new GameManager();
        AudioScript audioScript = null;
        string bgmName = "54湘江静";
        string gameSavePath = "/UserDate.txt";

        Userdata userdata = null;
        
        List<string> levelNameList = new List<string>();
        
        private GameManager()
        {
            LoadUserdata();
            if(GameObject.Find("BGMPlayer") != null)
            {
                audioScript = GameObject.Find("BGMPlayer").GetComponent<AudioScript>();
                audioScript.SetAudioByName(bgmName);
                audioScript.SetVolume(userdata.audioValue);
            }

            SceneManager.sceneLoaded += CheckContinueGameButton;
            CheckContinueGameButton(SceneManager.GetActiveScene(),LoadSceneMode.Single);

            levelNameList.Add("TutorialLevelScene");
            levelNameList.Add("WeatherLevelScene");
        }

        public static GameManager GetSingleInstance()
        {
            return gameManager;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                if(SceneManager.GetActiveScene().name=="SettingScene")
                {
                    EnterMainScene();
                }
            }
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene(levelNameList[0], LoadSceneMode.Single);
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void ContinueGame()
        {
            if(userdata.alreadyClearLevelNumber>= levelNameList.Count)
            {
                StartGame();
            }
            else
            {
                SceneManager.LoadScene(levelNameList[userdata.alreadyClearLevelNumber], LoadSceneMode.Single);
            }
        }

        /// <summary>
        /// 检测继续游戏按钮是否显示
        /// </summary>
        void CheckContinueGameButton(Scene scene, LoadSceneMode mode)
        {
            if(scene.name== "MainScene")
            {
                GameObject.Find("ContinueGameButton").SetActive(userdata.haveGameSave);
            }
        }

        /// <summary>
        /// 进入主场景
        /// </summary>
        public void EnterMainScene()
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }

        /// <summary>
        /// 进入设置场景
        /// </summary>
        public void EnterSettingScene()
        {
            SceneManager.LoadScene("SettingScene",LoadSceneMode.Single);
        }

        /// <summary>
        /// 返回上一场景
        /// </summary>
        public void ReturnLastScene()
        {
            if (SceneManager.GetActiveScene().name == "SettingScene")
            {
                EnterMainScene();
            }
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void Exit()
        {
            SaveUserdata();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        /// <summary>
        /// 设置游戏音量
        /// </summary>
        /// <param name="value"></param>
        public void SetAudioVolume(float value)
        {
            audioScript.SetVolume(value);
            userdata.audioValue = value;
        }

        /// <summary>
        /// 保存用户数据
        /// </summary>
        public void SaveUserdata()
        {
            XMLHelper.SaveDataToXML(Application.dataPath + gameSavePath, userdata);
        }

        /// <summary>
        /// 由关卡调用
        /// </summary>
        public void SaveByLevel(int levelIndex,int savePointIndex)
        {
            userdata.haveGameSave = true;
            userdata.alreadyClearLevelNumber = levelIndex;
            userdata.alreadyClearMiddleLevelNumber = savePointIndex;
            SaveUserdata();
        }

        /// <summary>
        /// 获得上次进行的关卡
        /// </summary>
        public int GetLastLevelIndex()
        {
            return userdata.alreadyClearLevelNumber;
        }

        /// <summary>
        /// 获得上次进行的关卡的最后通过的保存点
        /// </summary>
        public int GetLastSavePointIndex()
        {
            return userdata.alreadyClearMiddleLevelNumber;
        }

        /// <summary>
        /// 读取用户数据
        /// </summary>
        public void LoadUserdata()
        {
            if (File.Exists(Application.dataPath + gameSavePath))
            {
                userdata = XMLHelper.LoadDataFromXML<Userdata>(Application.dataPath + gameSavePath);
            }
            else
            {
                Debug.LogError("LoadUserData,存档缺失！");
                userdata = new Userdata();
            }
        }

        /// <summary>
        /// 获得用户数据
        /// </summary>
        /// <returns></returns>
        public Userdata GetUserdata()
        {
            return userdata;
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        public void RoleDeath()
        {
            userdata.residueLife--;

            if (userdata.residueLife<=0)
            {
                //提示生命值已经为零。
                userdata.residueLife = 1024;
            }
            SaveUserdata();
        }

        /// <summary>
        /// 进入下一关卡
        /// </summary>
        /// <param name="currentLevelIndex"></param>
        public void EnterNextLevel(int currentLevelIndex)
        {
            if (currentLevelIndex + 1 >= levelNameList.Count)
            {
                EnterMainScene();
            }
            else
            {
                ContinueGame();
            }
        }
    }
}
