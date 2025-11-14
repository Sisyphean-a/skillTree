using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// 灵魂核心主界面状态
    /// </summary>
    public class SoulCoreUIState : UIState
    {
        /// <summary>
        /// 基础点分配面板
        /// </summary>
        private BasicPointsPanel basicPointsPanel;

        /// <summary>
        /// 天赋树面板
        /// </summary>
        private TalentTreePanel talentTreePanel;

        public override void OnInitialize()
        {
            // 创建基础点分配面板
            basicPointsPanel = new BasicPointsPanel();
            basicPointsPanel.Left.Set(50, 0f);
            basicPointsPanel.Top.Set(100, 0f);
            basicPointsPanel.Width.Set(300, 0f);
            basicPointsPanel.Height.Set(250, 0f);
            Append(basicPointsPanel);

            // 创建天赋树面板（需要足够大以容纳所有圆环）
            talentTreePanel = new TalentTreePanel();
            talentTreePanel.Left.Set(400, 0f);
            talentTreePanel.Top.Set(50, 0f);
            talentTreePanel.Width.Set(850, 0f);  // 增加宽度以容纳圆环
            talentTreePanel.Height.Set(850, 0f); // 增加高度以容纳圆环
            Append(talentTreePanel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 按ESC键关闭UI
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) &&
                !Main.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                UIService.HideSkillBook();
            }

            // 阻止鼠标点击穿透到游戏世界
            if (IsMouseHoveringOverUI())
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        /// <summary>
        /// 检查鼠标是否悬停在UI上
        /// </summary>
        private bool IsMouseHoveringOverUI()
        {
            // 检查鼠标是否在基础点面板上
            if (basicPointsPanel != null && basicPointsPanel.IsMouseHovering)
            {
                return true;
            }

            // 检查鼠标是否在天赋树面板上
            if (talentTreePanel != null && talentTreePanel.IsMouseHovering)
            {
                return true;
            }

            return false;
        }
    }
}

