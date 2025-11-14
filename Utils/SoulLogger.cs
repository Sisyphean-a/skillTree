using Terraria.ModLoader;

namespace SoulAttuned.Utils
{
    /// <summary>
    /// 灵魂谐振日志工具类
    /// 提供统一的日志记录接口
    /// </summary>
    public static class SoulLogger
    {
        /// <summary>
        /// 记录信息日志
        /// </summary>
        public static void Info(string message)
        {
            SoulAttuned.Instance?.Logger.Info($"[Soul] {message}");
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        public static void Warn(string message)
        {
            SoulAttuned.Instance?.Logger.Warn($"[Soul] {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        public static void Error(string message)
        {
            SoulAttuned.Instance?.Logger.Error($"[Soul] {message}");
        }

        /// <summary>
        /// 记录调试日志（仅在调试模式下）
        /// </summary>
        public static void Debug(string message)
        {
            var config = ModContent.GetInstance<Systems.SoulConfig>();
            if (config?.DebugMode == true)
            {
                SoulAttuned.Instance?.Logger.Info($"[Soul-Debug] {message}");
            }
        }

        /// <summary>
        /// 记录网络同步日志（用于调试网络问题）
        /// </summary>
        public static void NetSync(string message)
        {
            var config = ModContent.GetInstance<Systems.SoulConfig>();
            if (config?.DebugMode == true)
            {
                SoulAttuned.Instance?.Logger.Info($"[Soul-NetSync] {message}");
            }
        }
    }
}

