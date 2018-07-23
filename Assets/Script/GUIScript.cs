using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script
{
    public class GUIScript : MonoBehaviour
    {
        GameManager gameManager = null;
        // Use this for initialization
        void Start()
        {
            gameManager = GameManager.GetSingleInstance();
            if (SceneManager.GetActiveScene().name == "SettingScene")
            {
                float audioValue = gameManager.GetUserdata().audioValue;
                GameObject.Find("AudioSlider").GetComponent<Slider>().value = audioValue;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGameButtonClick()
        {
            gameManager.StartGame();
        }

        public void ContinueGameButtonClick()
        {
            gameManager.ContinueGame();
        }

        public void SettingButtonClick()
        {
            gameManager.EnterSettingScene();
        }

        public void ExitButtonClick()
        {
            gameManager.Exit();
        }

        public void AudioSliderChange()
        {
            gameManager.SetAudioVolume(GameObject.Find("AudioSlider").GetComponent<Slider>().value);
        }

        public void ReturnButtleClick()
        {
            gameManager.ReturnLastScene();
        }
    }
}
