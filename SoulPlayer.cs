using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulAttuned
{
    /// <summary>
    /// 灵魂谐振玩家数据类
    /// 存储所有与玩家角色绑定的自定义数据
    /// </summary>
    public class SoulPlayer : ModPlayer
    {
        #region 核心数据字段

        // === 经验与等级 ===
        /// <summary>当前经验值</summary>
        public int currentXP = 0;
        
        /// <summary>当前等级</summary>
        public int currentLevel = 1;

        // === 升级奖励 ===
        /// <summary>可用天赋点（每5级获得1点）</summary>
        public int talentPoints = 0;
        
        /// <summary>可用基础点（每级获得1点）</summary>
        public int basicPoints = 0;

        // === 已解锁的天赋 ===
        /// <summary>已解锁的天赋节点ID列表</summary>
        public HashSet<string> unlockedTalents = new HashSet<string>();

        // === 基础属性等级 ===
        /// <summary>灵魂伤害等级</summary>
        public int soulDamageRank = 0;
        
        /// <summary>灵魂防御等级</summary>
        public int soulDefenseRank = 0;
        
        /// <summary>灵魂敏捷等级</summary>
        public int soulAgilityRank = 0;

        #endregion

        #region 数据持久化

        /// <summary>
        /// 保存玩家数据
        /// </summary>
        public override void SaveData(TagCompound tag)
        {
            tag["currentXP"] = currentXP;
            tag["currentLevel"] = currentLevel;
            tag["talentPoints"] = talentPoints;
            tag["basicPoints"] = basicPoints;
            tag["soulDamageRank"] = soulDamageRank;
            tag["soulDefenseRank"] = soulDefenseRank;
            tag["soulAgilityRank"] = soulAgilityRank;
            
            // 保存已解锁的天赋列表
            tag["unlockedTalents"] = new List<string>(unlockedTalents);
        }

        /// <summary>
        /// 加载玩家数据（防御性加载）
        /// </summary>
        public override void LoadData(TagCompound tag)
        {
            // 防御性检查：如果键不存在，使用默认值
            currentXP = tag.ContainsKey("currentXP") ? tag.GetInt("currentXP") : 0;
            currentLevel = tag.ContainsKey("currentLevel") ? tag.GetInt("currentLevel") : 1;
            talentPoints = tag.ContainsKey("talentPoints") ? tag.GetInt("talentPoints") : 0;
            basicPoints = tag.ContainsKey("basicPoints") ? tag.GetInt("basicPoints") : 0;
            soulDamageRank = tag.ContainsKey("soulDamageRank") ? tag.GetInt("soulDamageRank") : 0;
            soulDefenseRank = tag.ContainsKey("soulDefenseRank") ? tag.GetInt("soulDefenseRank") : 0;
            soulAgilityRank = tag.ContainsKey("soulAgilityRank") ? tag.GetInt("soulAgilityRank") : 0;

            // 加载已解锁的天赋列表
            if (tag.ContainsKey("unlockedTalents"))
            {
                unlockedTalents = new HashSet<string>(tag.GetList<string>("unlockedTalents"));
            }
            else
            {
                unlockedTalents = new HashSet<string>();
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 检查是否已解锁指定天赋
        /// </summary>
        /// <param name="talentID">天赋节点ID</param>
        /// <returns>是否已解锁</returns>
        public bool HasTalent(string talentID)
        {
            return unlockedTalents.Contains(talentID);
        }

        /// <summary>
        /// 解锁天赋节点
        /// </summary>
        /// <param name="talentID">天赋节点ID</param>
        /// <returns>是否成功解锁</returns>
        public bool UnlockTalent(string talentID)
        {
            if (talentPoints <= 0 || HasTalent(talentID))
                return false;

            unlockedTalents.Add(talentID);
            talentPoints--;
            return true;
        }

        #endregion

        #region 生命周期钩子

        /// <summary>
        /// 每帧重置效果（在属性计算前调用）
        /// </summary>
        public override void ResetEffects()
        {
            // 将在后续实现技能效果时使用
        }

        #endregion
    }
}

