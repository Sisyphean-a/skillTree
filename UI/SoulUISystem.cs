using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// UI系统，管理UserInterface和界面层
    /// </summary>
    public class SoulUISystem : ModSystem
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        public static SoulUISystem Instance { get; private set; }

        /// <summary>
        /// 主UI界面
        /// </summary>
        private UserInterface soulCoreInterface;

        /// <summary>
        /// 主UI状态
        /// </summary>
        private SoulCoreUIState soulCoreUIState;

        public override void Load()
        {
            Instance = this;

            // 只在客户端创建UI
            if (!Main.dedServ)
            {
                soulCoreInterface = new UserInterface();
                soulCoreUIState = new SoulCoreUIState();
                soulCoreUIState.Activate();
            }
        }

        public override void Unload()
        {
            Instance = null;
            soulCoreInterface = null;
            soulCoreUIState = null;
        }

        /// <summary>
        /// 显示UI
        /// </summary>
        public void ShowUI()
        {
            if (soulCoreInterface != null && soulCoreUIState != null)
            {
                soulCoreInterface.SetState(soulCoreUIState);
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void HideUI()
        {
            if (soulCoreInterface != null)
            {
                soulCoreInterface.SetState(null);
            }
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        public override void UpdateUI(GameTime gameTime)
        {
            if (soulCoreInterface?.CurrentState != null)
            {
                soulCoreInterface.Update(gameTime);
            }
        }

        /// <summary>
        /// 修改界面层，将UI绘制到屏幕上
        /// </summary>
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "SoulAttuned: Soul Core UI",
                    delegate
                    {
                        if (soulCoreInterface?.CurrentState != null)
                        {
                            soulCoreInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}

