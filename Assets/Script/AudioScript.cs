using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Script
{
    public class AudioScript : MonoBehaviour
    {
        AudioSource bgmPlayer;
        string audioPath = "Audio/";
        string bgmName = "54湘江静";

        Dictionary<string, AudioClip> audioMap = new Dictionary<string, AudioClip>();

        // Use this for initialization
        void Start()
        {
            bgmPlayer = gameObject.GetComponent<AudioSource>();

            audioMap[bgmName] = Resources.Load<AudioClip>(audioPath + bgmName);

            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            GameManager.GetSingleInstance().Update();
        }

        public AudioClip GetAudioClipByName(string name)
        {
            return audioMap[name];
        }

        public void SetAudioByName(string name)
        {
            bgmPlayer.clip = audioMap[name];
            bgmPlayer.Play();
        }

        public void SetVolume(float value)
        {
            bgmPlayer.volume = value;
        }
    }
}
