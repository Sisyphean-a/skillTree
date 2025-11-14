using Terraria;
using Terraria.ModLoader;

namespace SoulAttuned.Core
{
    /// <summary>
    /// 灵魂谐振全局NPC类
    /// 负责处理怪物击杀时的经验奖励
    /// </summary>
    public class SoulGlobalNPC : GlobalNPC
    {
        /// <summary>
        /// 当NPC被击杀时调用
        /// </summary>
        public override void OnKill(NPC npc)
        {
            // 只在服务器端或单人模式下处理
            if (Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient)
                return;

            // Boss不提供经验（遵循设计文档）
            if (npc.boss)
            {
                Utils.SoulLogger.Debug($"Boss {npc.FullName} 被击败，不提供经验");
                return;
            }

            // 计算经验值
            int xpAmount = CalculateXP(npc);
            
            if (xpAmount <= 0)
                return;

            // 应用配置中的XP倍率
            var config = ModContent.GetInstance<Systems.SoulConfig>();
            xpAmount = (int)(xpAmount * config.XPMultiplier);

            // 给予所有附近的玩家经验
            DistributeXP(npc, xpAmount);
        }

        /// <summary>
        /// 计算NPC提供的经验值
        /// 公式: XP = (MaxHP/100) × (1 + Defense/10) × (1 + Damage/25)
        /// </summary>
        /// <param name="npc">被击杀的NPC</param>
        /// <returns>经验值</returns>
        private int CalculateXP(NPC npc)
        {
            // 基础经验：生命值 / 100
            float baseXP = npc.lifeMax / 100f;

            // 防御加成：1 + (防御 / 10)
            float defenseMultiplier = 1f + (npc.defense / 10f);

            // 伤害加成：1 + (伤害 / 25)
            float damageMultiplier = 1f + (npc.damage / 25f);

            // 最终经验值
            int finalXP = (int)(baseXP * defenseMultiplier * damageMultiplier);

            // 确保至少给1点经验（如果怪物有生命值）
            if (finalXP < 1 && npc.lifeMax > 0)
                finalXP = 1;

            Utils.SoulLogger.Debug($"NPC {npc.FullName} 提供 {finalXP} 经验 (HP:{npc.lifeMax}, Def:{npc.defense}, Dmg:{npc.damage})");

            return finalXP;
        }

        /// <summary>
        /// 分配经验给附近的玩家
        /// </summary>
        /// <param name="npc">被击杀的NPC</param>
        /// <param name="xpAmount">经验值</param>
        private void DistributeXP(NPC npc, int xpAmount)
        {
            // 经验分享范围（像素）
            const float XP_SHARE_RANGE = 1200f;

            // 遍历所有玩家
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                
                // 检查玩家是否有效且在范围内
                if (!player.active || player.dead)
                    continue;

                float distance = player.Distance(npc.Center);
                if (distance > XP_SHARE_RANGE)
                    continue;

                // 获取玩家的灵魂数据
                var soulPlayer = player.GetModPlayer<SoulPlayer>();
                
                // 给予经验
                soulPlayer.AddXP(xpAmount);
            }
        }
    }
}

