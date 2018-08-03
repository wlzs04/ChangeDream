using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helper
{
    public delegate void AnimationCallBack(GameObject gameObject);

    /// <summary>
    /// 动画物体类，包括动画所需的一些参数
    /// </summary>
    class AnimationObject
    {
        public string name = "animation";
        public GameObject gameObject = null;
        public Vector3 fromPosition;
        public Vector3 toPosition;
        public float needTime = 0;
        public float startTime = 0;
        public AnimationCallBack animationCallBack;
    }

    /// <summary>
    /// 动画帮助类
    /// </summary>
    class AnimationHelper
    {
        Dictionary<string, AnimationObject> animations = new Dictionary<string, AnimationObject>();
        float currentTime = 0;
        float thisTickTime = 0;
        static AnimationHelper animationHelper = new AnimationHelper();

        private AnimationHelper()
        {

        }

        public static AnimationHelper GetSingleInstance()
        {
            return animationHelper;
        }

        /// <summary>
        /// 添加动画
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="gameObject"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="needTime"></param>
        public void AddAnimation(string animationName,GameObject gameObject,Vector3 fromPosition,Vector3 toPosition,float needTime)
        {
            AnimationObject animationObject = new AnimationObject();
            animationObject.name = animationName;
            animationObject.gameObject = gameObject;
            animationObject.fromPosition = fromPosition;
            animationObject.toPosition = toPosition;
            animationObject.needTime = needTime;
            animationObject.startTime = Time.time;
            AddAnimation(animationObject);
        }

        /// <summary>
        /// 添加动画
        /// </summary>
        /// <param name="animationObject"></param>
        public void AddAnimation(AnimationObject animationObject)
        {
            if(animations.ContainsKey(animationObject.name))
            {
                Debug.LogError("动画已存在："+ animationObject.name);
            }
            animations[animationObject.name] = animationObject;
        }

        public void Update()
        {
            currentTime = Time.time;
            thisTickTime = Time.deltaTime;
            float rate = 0;
            Vector3 changePosition;
            foreach (var item in animations.ToList())
            {
                AnimationObject animation = item.Value;
                if (currentTime < animation.startTime)
                {
                    break;
                }
                if (currentTime >= animation.startTime + animation.needTime)
                {
                    animation.gameObject.transform.localPosition = animation.toPosition;
                    animations.Remove(item.Key);
                    break;
                }
                rate = (currentTime - animation.startTime) / animation.needTime;
                changePosition = animation.toPosition - animation.fromPosition;
                animation.gameObject.transform.localPosition = animation.fromPosition + (rate * changePosition);
            }
        }
    }
}
