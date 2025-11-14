using Terraria;
using Terraria.ID;

namespace SoulAttuned.Core
{
    /// <summary>
    /// 世界谐振阶段枚举
    /// 定义了8个谐振阶段，每个阶段对应不同的等级上限
    /// </summary>
    public enum ResonancePhase
    {
        /// <summary>阶段1：起始 - 等级上限10</summary>
        Phase1_Start = 1,
        
        /// <summary>阶段2：肉山前-早期 - 等级上限20（击败克苏鲁之眼）</summary>
        Phase2_PreHardmodeEarly = 2,
        
        /// <summary>阶段3：肉山前-中期 - 等级上限30（击败骷髅王）</summary>
        Phase3_PreHardmodeMid = 3,
        
        /// <summary>阶段4：困难模式 - 等级上限40（击败血肉之墙）</summary>
        Phase4_Hardmode = 4,
        
        /// <summary>阶段5：机械三王 - 等级上限50（击败任意机械Boss）</summary>
        Phase5_MechBosses = 5,
        
        /// <summary>阶段6：世纪之花 - 等级上限60（击败世纪之花）</summary>
        Phase6_Plantera = 6,
        
        /// <summary>阶段7：后期 - 等级上限70（击败石巨人）</summary>
        Phase7_LateGame = 7,
        
        /// <summary>阶段8：终局 - 等级上限80（击败月亮领主）</summary>
        Phase8_Endgame = 8
    }

    /// <summary>
    /// 世界谐振系统
    /// 负责检测当前世界的谐振阶段并返回对应的等级上限
    /// </summary>
    public static class ResonanceSystem
    {
        /// <summary>
        /// 获取当前世界的谐振阶段
        /// </summary>
        /// <returns>当前谐振阶段</returns>
        public static ResonancePhase GetCurrentPhase()
        {
            // 从最高阶段开始检测，返回第一个满足条件的阶段
            if (NPC.downedMoonlord)
                return ResonancePhase.Phase8_Endgame;
            
            if (NPC.downedGolemBoss)
                return ResonancePhase.Phase7_LateGame;
            
            if (NPC.downedPlantBoss)
                return ResonancePhase.Phase6_Plantera;
            
            if (NPC.downedMechBossAny)
                return ResonancePhase.Phase5_MechBosses;
            
            if (Main.hardMode)
                return ResonancePhase.Phase4_Hardmode;
            
            if (NPC.downedBoss3)
                return ResonancePhase.Phase3_PreHardmodeMid;
            
            if (NPC.downedBoss1)
                return ResonancePhase.Phase2_PreHardmodeEarly;
            
            return ResonancePhase.Phase1_Start;
        }

        /// <summary>
        /// 获取指定谐振阶段的等级上限
        /// </summary>
        /// <param name="phase">谐振阶段</param>
        /// <returns>等级上限</returns>
        public static int GetLevelCap(ResonancePhase phase)
        {
            return phase switch
            {
                ResonancePhase.Phase1_Start => 10,
                ResonancePhase.Phase2_PreHardmodeEarly => 20,
                ResonancePhase.Phase3_PreHardmodeMid => 30,
                ResonancePhase.Phase4_Hardmode => 40,
                ResonancePhase.Phase5_MechBosses => 50,
                ResonancePhase.Phase6_Plantera => 60,
                ResonancePhase.Phase7_LateGame => 70,
                ResonancePhase.Phase8_Endgame => 80,
                _ => 10
            };
        }

        /// <summary>
        /// 获取当前世界的等级上限
        /// </summary>
        /// <returns>当前等级上限</returns>
        public static int GetCurrentLevelCap()
        {
            return GetLevelCap(GetCurrentPhase());
        }

        /// <summary>
        /// 获取谐振阶段的显示名称
        /// </summary>
        /// <param name="phase">谐振阶段</param>
        /// <returns>显示名称</returns>
        public static string GetPhaseName(ResonancePhase phase)
        {
            return phase switch
            {
                ResonancePhase.Phase1_Start => "起始",
                ResonancePhase.Phase2_PreHardmodeEarly => "肉山前-早期",
                ResonancePhase.Phase3_PreHardmodeMid => "肉山前-中期",
                ResonancePhase.Phase4_Hardmode => "困难模式",
                ResonancePhase.Phase5_MechBosses => "机械三王",
                ResonancePhase.Phase6_Plantera => "世纪之花",
                ResonancePhase.Phase7_LateGame => "后期",
                ResonancePhase.Phase8_Endgame => "终局",
                _ => "未知"
            };
        }
    }
}

