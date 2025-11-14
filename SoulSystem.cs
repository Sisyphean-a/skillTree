using Terraria;
using Terraria.ModLoader;

namespace SoulAttuned
{
    /// <summary>
    /// 灵魂谐振系统类
    /// 管理全局系统逻辑和UI
    /// </summary>
    public class SoulSystem : ModSystem
    {
        /// <summary>
        /// 系统加载时调用
        /// </summary>
        public override void Load()
        {
            // 将在后续实现UI时添加UI初始化
        }

        /// <summary>
        /// 系统卸载时调用
        /// </summary>
        public override void Unload()
        {
            // 清理UI资源
        }

        /// <summary>
        /// 后更新（每帧调用）
        /// </summary>
        public override void PostUpdateEverything()
        {
            // 将在后续实现UI更新逻辑
        }
    }
}

