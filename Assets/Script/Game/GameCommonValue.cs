using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Game
{
    class GameCommonValue
    {
        /// <summary>
        /// 角色在走动时可以显示的范围,超过范围需要移动背景
        /// </summary>
        public static readonly float leftShowLimitRate = 0.2f;
        public static readonly float rightShowLimitRate = 0.8f;

        /// <summary>
        /// 可控制角色的数量
        /// </summary>
        public static readonly int controllableRoleNumber = 3;

        /// <summary>
        /// 摄像机在改变跟随的角色时进行移动所需要的时间
        /// </summary>
        public static readonly float cameraChangeRoleTime = 0.3f;

        /// <summary>
        /// 游戏基础长度
        /// </summary>
        public static readonly float gameBaseLength = 100;

        /// <summary>
        /// 木、鸣、七的速度比例
        /// </summary>
        public static readonly float muMoveRateSpeed = 1f;
        public static readonly float mingMoveRateSpeed = 1.2f;
        public static readonly float qiMoveRateSpeed = 0.6f;

        /// <summary>
        /// 木、鸣、七的身高比例
        /// </summary>
        public static readonly float muRateHeight = 1f;
        public static readonly float mingRateHeight = 1.2f;
        public static readonly float qiRateHeight = 0.6f;

        /// <summary>
        /// 木、鸣、七的宽度比例
        /// </summary>
        public static readonly float muRateWidth = 0.5f;
        public static readonly float mingRateWidth = 0.6f;
        public static readonly float qiRateWidth = 0.3f;

        /// <summary>
        /// 木、鸣、七的跳跃高度比例
        /// </summary>
        public static readonly float muJumpRateHeight = 0.5f;
        public static readonly float mingJumpRateHeight = 1f;
        public static readonly float qiJumpRateHeight = 0.3f;

        /// <summary>
        /// 木、鸣、七的受伤高度比例
        /// </summary>
        public static readonly float muFallDownRateHeight = 3f;
        public static readonly float mingFallDownRateHeight = 5f;
        public static readonly float qiFallDownRateHeight = 1f;

        /// <summary>
        /// 木、鸣、七的体重比例
        /// </summary>
        public static readonly float muRateWeight = 1f;
        public static readonly float mingRateWeight = 2f;
        public static readonly float qiRateWeight = 0.5f;

        /// <summary>
        /// 木、鸣、七的血量
        /// </summary>
        public static readonly int muBloodVolume = 1;
        public static readonly int mingBloodVolume = 10;
        public static readonly int qiBloodVolume = 1;

        /// <summary>
        /// 存档点的宽度和高度比例
        /// </summary>
        public static readonly float savePointRateHeight = 0.8f;
        public static readonly float savePointRateWidth = 0.6f;

        /// <summary>
        /// 终点的宽度和高度比例
        /// </summary>
        public static readonly float endPointRateHeight = 0.6f;
        public static readonly float endPointRateWidth = 0.8f;

        /// <summary>
        /// 复活时角色之间的间距比例
        /// </summary>
        public static readonly float saveRoleSpaceRateLength = 0.8f;

        /// <summary>
        /// 游戏中默认加速度
        /// </summary>
        public static readonly float acceleration = 0.1f;

        /// <summary>
        /// 跳跃到至高点需要的时间
        /// </summary>
        public static readonly float jumpNeedTime = 0.3f;

        /// <summary>
        /// 与攀爬相关的时间
        /// </summary>
        public static readonly float climbStepOneNeedTime = 0.3f;
        public static readonly float climbStepTwoNeedTime = 0.5f;

        /// <summary>
        /// 与木说话相关时间
        /// </summary>
        public static readonly float muTalkShowTime = 3f;
        public static readonly float muPrepareTalkNeedTime = 5f;

        /// <summary>
        /// 与风相关的变量
        /// </summary>
        public static readonly int windMaxDurationTime = 5;
        public static readonly int windMinDurationTime = 1;
        public static readonly int windMaxSpeed = 1;
    }
}
