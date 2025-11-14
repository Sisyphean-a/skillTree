using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// UI服务层，管理UI的显示和隐藏
    /// </summary>
    public static class UIService
    {
        private static bool isSkillBookVisible = false;

        /// <summary>
        /// 获取技能书UI是否可见
        /// </summary>
        public static bool IsSkillBookVisible => isSkillBookVisible;

        /// <summary>
        /// 显示技能书UI
        /// </summary>
        public static void ShowSkillBook()
        {
            if (!isSkillBookVisible)
            {
                isSkillBookVisible = true;
                SoulUISystem.Instance?.ShowUI();
            }
        }

        /// <summary>
        /// 隐藏技能书UI
        /// </summary>
        public static void HideSkillBook()
        {
            if (isSkillBookVisible)
            {
                isSkillBookVisible = false;
                SoulUISystem.Instance?.HideUI();
            }
        }

        /// <summary>
        /// 切换技能书UI的显示状态
        /// </summary>
        public static void ToggleSkillBook()
        {
            if (isSkillBookVisible)
            {
                HideSkillBook();
            }
            else
            {
                ShowSkillBook();
            }
        }

        /// <summary>
        /// 重置UI状态（用于玩家退出世界时）
        /// </summary>
        internal static void Reset()
        {
            isSkillBookVisible = false;
        }
    }
}

