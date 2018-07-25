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
        
        private GameManager()
        {
            LoadUserdata();
            audioScript = GameObject.Find("BGMPlayer").GetComponent<AudioScript>();
            audioScript.SetAudioByName(bgmName);
            audioScript.SetVolume(userdata.audioValue);

            SceneManager.sceneLoaded += CheckContinueGameButton;
            CheckContinueGameButton(SceneManager.GetActiveScene(),LoadSceneMode.Single);
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
            SceneManager.LoadScene("TutorialLevelScene",LoadSceneMode.Single);
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void ContinueGame()
        {
            
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
    }
}
