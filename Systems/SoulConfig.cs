using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SoulAttuned.Systems
{
    /// <summary>
    /// 灵魂谐振系统配置
    /// 允许玩家自定义模组参数
    /// </summary>
    public class SoulConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        #region 经验与升级配置

        [Header("ExperienceAndLeveling")]
        
        [Label("XP Multiplier | XP倍率")]
        [Tooltip("Multiplies all XP gained | 所有获得的经验值乘以此倍率")]
        [Range(0.1f, 10f)]
        [DefaultValue(1f)]
        [Increment(0.1f)]
        public float XPMultiplier { get; set; }

        [Label("Base XP for Leveling | 升级基础XP")]
        [Tooltip("Base XP required for leveling (formula: BaseXP × Level^1.5) | 升级所需的基础XP（公式：基础XP × 等级^1.5）")]
        [Range(50, 1000)]
        [DefaultValue(100)]
        [Increment(10)]
        public int BaseXPRequired { get; set; }

        [Label("Leveling Exponent | 升级指数")]
        [Tooltip("Exponent in leveling formula (higher = slower leveling) | 升级公式中的指数（越高升级越慢）")]
        [Range(1f, 2.5f)]
        [DefaultValue(1.5f)]
        [Increment(0.1f)]
        public float LevelingExponent { get; set; }

        #endregion

        #region 天赋点配置

        [Header("TalentPoints")]

        [Label("Talent Point Frequency | 天赋点获取频率")]
        [Tooltip("Gain 1 talent point every X levels | 每X级获得1个天赋点")]
        [Range(1, 10)]
        [DefaultValue(5)]
        public int TalentPointFrequency { get; set; }

        #endregion

        #region 调试选项

        [Header("Debug")]

        [Label("Enable Debug Mode | 启用调试模式")]
        [Tooltip("Shows debug information and enables cheat commands | 显示调试信息并启用作弊命令")]
        [DefaultValue(false)]
        public bool DebugMode { get; set; }

        #endregion
    }
}

