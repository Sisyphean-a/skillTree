using Terraria.ModLoader;

namespace SoulAttuned
{
    /// <summary>
    /// 灵魂谐振系统 - 主模组类
    /// Soul-Attuned System - Main Mod Class
    /// </summary>
    public class SoulAttuned : Mod
    {
        /// <summary>
        /// 模组单例实例
        /// </summary>
        public static SoulAttuned Instance { get; private set; }

        /// <summary>
        /// 打开技能树的热键
        /// </summary>
        public static ModKeybind OpenSkillTreeKey { get; private set; }

        /// <summary>
        /// 模组加载时调用
        /// </summary>
        public override void Load()
        {
            Instance = this;

            // 注册热键
            OpenSkillTreeKey = KeybindLoader.RegisterKeybind(this, "OpenSkillTree", "K");

            Logger.Info("灵魂谐振系统已加载 | Soul-Attuned System Loaded");
        }

        /// <summary>
        /// 模组卸载时调用
        /// </summary>
        public override void Unload()
        {
            Instance = null;
            OpenSkillTreeKey = null;
            Logger.Info("灵魂谐振系统已卸载 | Soul-Attuned System Unloaded");
        }
    }
}

