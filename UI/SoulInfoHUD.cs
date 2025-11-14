using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoulAttuned.Core;
using SoulAttuned.Systems;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SoulAttuned.UI
{
    /// <summary>
    /// 灵魂信息HUD显示
    /// 在屏幕上显示等级、经验、点数等信息
    /// </summary>
    public class SoulInfoHUD : ModSystem
    {
        /// <summary>
        /// HUD显示位置（屏幕右下角）
        /// </summary>
        private Vector2 hudPosition;

        /// <summary>
        /// HUD背景颜色
        /// </summary>
        private Color backgroundColor = new Color(20, 20, 40);

        /// <summary>
        /// HUD边框颜色
        /// </summary>
        private Color borderColor = new Color(100, 150, 255);

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            // 检查配置是否启用HUD
            SoulConfig config = ModContent.GetInstance<SoulConfig>();
            if (config == null || !config.ShowInfoHUD)
            {
                return;
            }

            // 检查玩家是否存在
            if (Main.LocalPlayer == null || !Main.LocalPlayer.active)
            {
                return;
            }

            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            if (soulPlayer == null)
            {
                return;
            }

            // 计算HUD位置（屏幕右下角，避开原版UI）
            float rightMargin = 20f;
            float bottomMargin = 80f; // 留出空间避开原版UI（生命值、魔力等）
            float hudWidth = 220f;
            float hudHeight = 120f;

            hudPosition = new Vector2(
                Main.screenWidth - hudWidth - rightMargin,
                Main.screenHeight - hudHeight - bottomMargin
            );

            // 应用透明度
            float opacity = config.HUDOpacity;

            // 绘制背景
            DrawBackground(spriteBatch, hudPosition, hudWidth, hudHeight, opacity);

            // 绘制信息
            DrawInfo(spriteBatch, soulPlayer, hudPosition, opacity);
        }

        /// <summary>
        /// 绘制HUD背景
        /// </summary>
        private void DrawBackground(SpriteBatch spriteBatch, Vector2 position, float width, float height, float opacity)
        {
            // 绘制背景矩形
            Rectangle bgRect = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            DrawRectangle(spriteBatch, bgRect, backgroundColor * opacity * 0.8f);

            // 绘制边框
            DrawRectangleOutline(spriteBatch, bgRect, borderColor * opacity, 2);
        }

        /// <summary>
        /// 绘制信息文本
        /// </summary>
        private void DrawInfo(SpriteBatch spriteBatch, SoulPlayer soulPlayer, Vector2 position, float opacity)
        {
            float textX = position.X + 10;
            float textY = position.Y + 10;
            float lineHeight = 22f;
            Color textColor = Color.White * opacity;

            // 标题
            string title = "灵魂谐振 Soul-Attuned";
            Terraria.Utils.DrawBorderString(spriteBatch, title, new Vector2(textX, textY), 
                new Color(150, 200, 255) * opacity, 0.85f);
            textY += lineHeight + 5;

            // 等级和经验
            int xpRequired = soulPlayer.GetXPRequiredForLevel(soulPlayer.currentLevel);
            string levelText = $"等级 Lv.{soulPlayer.currentLevel}";
            Terraria.Utils.DrawBorderString(spriteBatch, levelText, new Vector2(textX, textY), textColor, 0.8f);
            textY += lineHeight;

            // 经验条
            string xpText = $"XP: {soulPlayer.currentXP} / {xpRequired}";
            Terraria.Utils.DrawBorderString(spriteBatch, xpText, new Vector2(textX, textY), textColor, 0.75f);
            textY += lineHeight;

            // 基础点
            string basicPointsText = $"基础点: {soulPlayer.basicPoints}";
            Color basicColor = soulPlayer.basicPoints > 0 ? Color.LightGreen : Color.Gray;
            Terraria.Utils.DrawBorderString(spriteBatch, basicPointsText, new Vector2(textX, textY), 
                basicColor * opacity, 0.75f);
            textY += lineHeight;

            // 天赋点
            string talentPointsText = $"天赋点: {soulPlayer.talentPoints}";
            Color talentColor = soulPlayer.talentPoints > 0 ? Color.Gold : Color.Gray;
            Terraria.Utils.DrawBorderString(spriteBatch, talentPointsText, new Vector2(textX, textY), 
                talentColor * opacity, 0.75f);
        }

        /// <summary>
        /// 绘制填充矩形
        /// </summary>
        private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            spriteBatch.Draw(pixel, rect, color);
        }

        /// <summary>
        /// 绘制矩形边框
        /// </summary>
        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            
            // 上边
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            // 下边
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            // 左边
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            // 右边
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}

