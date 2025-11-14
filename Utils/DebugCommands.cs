using Terraria;
using Terraria.ModLoader;

namespace SoulAttuned.Utils
{
    /// <summary>
    /// 调试命令类
    /// 提供测试用的作弊命令
    /// </summary>
    public class DebugCommands : ModCommand
    {
        public override string Command => "soul";

        public override CommandType Type => CommandType.Chat;

        public override string Usage => "/soul <subcommand> [args]\n" +
            "  /soul addxp <amount> - 添加经验值\n" +
            "  /soul setlevel <level> - 设置等级\n" +
            "  /soul addtalent <points> - 添加天赋点\n" +
            "  /soul addbasic <points> - 添加基础点\n" +
            "  /soul reset - 重置所有数据\n" +
            "  /soul info - 显示当前状态";

        public override string Description => "灵魂谐振系统调试命令";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // 检查是否启用调试模式
            var config = ModContent.GetInstance<Systems.SoulConfig>();
            if (!config.DebugMode && !caller.Player.name.Contains("Debug"))
            {
                caller.Reply("调试模式未启用！请在配置中启用调试模式。", Microsoft.Xna.Framework.Color.Red);
                return;
            }

            if (args.Length == 0)
            {
                caller.Reply(Usage, Microsoft.Xna.Framework.Color.Yellow);
                return;
            }

            var player = caller.Player.GetModPlayer<Core.SoulPlayer>();
            string subcommand = args[0].ToLower();

            switch (subcommand)
            {
                case "addxp":
                    if (args.Length < 2 || !int.TryParse(args[1], out int xpAmount))
                    {
                        caller.Reply("用法: /soul addxp <amount>", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                    player.currentXP += xpAmount;
                    caller.Reply($"添加了 {xpAmount} 经验值。当前经验: {player.currentXP}", Microsoft.Xna.Framework.Color.Green);
                    break;

                case "setlevel":
                    if (args.Length < 2 || !int.TryParse(args[1], out int level))
                    {
                        caller.Reply("用法: /soul setlevel <level>", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                    player.currentLevel = level;
                    caller.Reply($"等级设置为 {level}", Microsoft.Xna.Framework.Color.Green);
                    break;

                case "addtalent":
                    if (args.Length < 2 || !int.TryParse(args[1], out int talentPoints))
                    {
                        caller.Reply("用法: /soul addtalent <points>", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                    player.talentPoints += talentPoints;
                    caller.Reply($"添加了 {talentPoints} 天赋点。当前天赋点: {player.talentPoints}", Microsoft.Xna.Framework.Color.Green);
                    break;

                case "addbasic":
                    if (args.Length < 2 || !int.TryParse(args[1], out int basicPoints))
                    {
                        caller.Reply("用法: /soul addbasic <points>", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                    player.basicPoints += basicPoints;
                    caller.Reply($"添加了 {basicPoints} 基础点。当前基础点: {player.basicPoints}", Microsoft.Xna.Framework.Color.Green);
                    break;

                case "reset":
                    player.currentXP = 0;
                    player.currentLevel = 1;
                    player.talentPoints = 0;
                    player.basicPoints = 0;
                    player.soulDamageRank = 0;
                    player.soulDefenseRank = 0;
                    player.soulAgilityRank = 0;
                    player.unlockedTalents.Clear();
                    caller.Reply("所有数据已重置！", Microsoft.Xna.Framework.Color.Yellow);
                    break;

                case "info":
                    caller.Reply($"=== 灵魂谐振状态 ===", Microsoft.Xna.Framework.Color.Cyan);
                    caller.Reply($"等级: {player.currentLevel}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"经验: {player.currentXP}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"天赋点: {player.talentPoints}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"基础点: {player.basicPoints}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"灵魂伤害等级: {player.soulDamageRank}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"灵魂防御等级: {player.soulDefenseRank}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"灵魂敏捷等级: {player.soulAgilityRank}", Microsoft.Xna.Framework.Color.White);
                    caller.Reply($"已解锁天赋数: {player.unlockedTalents.Count}", Microsoft.Xna.Framework.Color.White);
                    break;

                default:
                    caller.Reply($"未知的子命令: {subcommand}", Microsoft.Xna.Framework.Color.Red);
                    caller.Reply(Usage, Microsoft.Xna.Framework.Color.Yellow);
                    break;
            }
        }
    }
}

