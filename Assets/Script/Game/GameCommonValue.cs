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
        /// 木、鸣、七的身高和宽度比例
        /// </summary>
        public static readonly float muRateHeightWidth = 1f;
        public static readonly float mingRateHeightWidth = 1.2f;
        public static readonly float qiRateHeightWidth = 0.6f;

        /// <summary>
        /// 木、鸣、七的跳跃高度比例
        /// </summary>
        public static readonly float muJumpRateHeight = 0.5f;
        public static readonly float mingJumpRateHeight = 1f;
        public static readonly float qiJumpRateHeight = 0.3f;

        /// <summary>
        /// 木、鸣、七的受伤高度比例
        /// </summary>
        public static readonly float muFallDownRateHeight = 1.5f;
        public static readonly float mingFallDownRateHeight = 2.2f;
        public static readonly float qiFallDownRateHeight = 0.9f;

        /// <summary>
        /// 木、鸣、七的血量
        /// </summary>
        public static readonly int muBloodVolume = 1;
        public static readonly int mingBloodVolume = 10;
        public static readonly int qiBloodVolume = 1;

        /// <summary>
        /// 存档点的宽度和高度
        /// </summary>
        public static readonly float savePointHeight = 0.8f;
        public static readonly float savePointWidth = 0.6f;

        /// <summary>
        /// 终点的宽度和高度
        /// </summary>
        public static readonly float endPointHeight = 0.6f;
        public static readonly float endPointWidth = 0.8f;
    }
}
