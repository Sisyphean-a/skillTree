using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SoulAttuned.Core
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

        #region 经验与升级系统

        /// <summary>
        /// 添加经验值
        /// </summary>
        /// <param name="amount">经验值数量</param>
        public void AddXP(int amount)
        {
            if (amount <= 0)
                return;

            currentXP += amount;
            Utils.SoulLogger.Debug($"玩家 {Player.name} 获得 {amount} 经验，当前经验: {currentXP}");

            // 尝试升级
            TryLevelUp();
        }

        /// <summary>
        /// 尝试升级
        /// </summary>
        private void TryLevelUp()
        {
            // 获取当前等级上限
            int levelCap = ResonanceSystem.GetCurrentLevelCap();

            // 检查是否达到等级上限
            if (currentLevel >= levelCap)
            {
                Utils.SoulLogger.Debug($"玩家 {Player.name} 已达到等级上限 {levelCap}");
                return;
            }

            // 检查是否有足够的经验升级
            int requiredXP = GetXPRequiredForLevel(currentLevel + 1);

            while (currentXP >= requiredXP && currentLevel < levelCap)
            {
                // 升级
                currentLevel++;
                currentXP -= requiredXP;

                // 发放奖励
                GiveRewards();

                // 通知玩家
                NotifyLevelUp();

                // 计算下一级所需经验
                requiredXP = GetXPRequiredForLevel(currentLevel + 1);
            }
        }

        /// <summary>
        /// 计算升级到指定等级所需的经验值
        /// 公式: BaseXP × (Level^1.5)
        /// </summary>
        /// <param name="level">目标等级</param>
        /// <returns>所需经验值</returns>
        public int GetXPRequiredForLevel(int level)
        {
            if (level <= 1)
                return 0;

            var config = ModContent.GetInstance<Systems.SoulConfig>();
            double exponent = config.LevelingExponent;
            int baseXP = config.BaseXPRequired;

            return (int)(baseXP * Math.Pow(level, exponent));
        }

        /// <summary>
        /// 发放升级奖励
        /// </summary>
        private void GiveRewards()
        {
            // 每级获得1个基础点
            basicPoints++;

            // 每5级获得1个天赋点
            if (currentLevel % 5 == 0)
            {
                talentPoints++;
                Utils.SoulLogger.Info($"玩家 {Player.name} 升到 {currentLevel} 级，获得天赋点！");
            }

            Utils.SoulLogger.Info($"玩家 {Player.name} 升到 {currentLevel} 级");
        }

        /// <summary>
        /// 通知玩家升级
        /// </summary>
        private void NotifyLevelUp()
        {
            // 只在客户端显示消息
            if (Main.netMode == Terraria.ID.NetmodeID.Server)
                return;

            string message = $"升级！当前等级: {currentLevel}";

            // 如果获得了天赋点，额外提示
            if (currentLevel % 5 == 0)
            {
                message += " [获得天赋点]";
            }

            Main.NewText(message, 255, 215, 0); // 金色文字
        }

        #endregion

        #region 天赋系统

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

        #region 基础属性效果

        /// <summary>
        /// 修改玩家伤害
        /// 应用灵魂伤害加成：每级 +0.5% 综合伤害
        /// </summary>
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            // 这里可以添加受伤相关的效果
        }

        /// <summary>
        /// 修改玩家最大生命值
        /// </summary>
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;
        }

        /// <summary>
        /// 后处理更新（每帧调用）
        /// 应用移动速度加成
        /// </summary>
        public override void PostUpdateMiscEffects()
        {
            // 应用灵魂敏捷加成：每级 +0.5% 移动速度
            if (soulAgilityRank > 0)
            {
                float speedBonus = soulAgilityRank * 0.005f; // 0.5% per rank
                Player.moveSpeed += speedBonus;
                Player.runAcceleration += speedBonus;
            }
        }

        /// <summary>
        /// 修改玩家防御力
        /// 应用灵魂防御加成：每级 +1 基础防御
        /// </summary>
        public override void PostUpdateEquips()
        {
            // 应用灵魂防御加成
            if (soulDefenseRank > 0)
            {
                Player.statDefense += soulDefenseRank;
            }
        }

        /// <summary>
        /// 修改武器伤害
        /// 应用灵魂伤害加成：每级 +0.5% 综合伤害
        /// </summary>
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (soulDamageRank > 0)
            {
                float damageBonus = soulDamageRank * 0.005f; // 0.5% per rank
                damage += damageBonus;
            }
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

        /// <summary>
        /// 处理按键触发
        /// </summary>
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            // 检测打开技能树的热键
            if (SoulAttuned.OpenSkillTreeKey.JustPressed)
            {
                UI.UIService.ToggleSkillBook();
            }
        }

        #endregion
    }
}

