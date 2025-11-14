using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// 可拖拽的面板基类
    /// </summary>
    public class DraggablePanel : UIPanel
    {
        private Vector2 offset;
        private bool dragging;

        /// <summary>
        /// 面板标题
        /// </summary>
        public string Title { get; set; } = "Panel";

        /// <summary>
        /// 标题栏高度
        /// </summary>
        protected virtual float TitleBarHeight => 30f;

        public override void OnInitialize()
        {
            base.OnInitialize();
            SetPadding(0);
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);

            // 检查是否点击在标题栏区域
            Vector2 mousePos = new Vector2(evt.MousePosition.X, evt.MousePosition.Y);
            Rectangle titleBarRect = GetTitleBarRectangle();

            if (titleBarRect.Contains(mousePos.ToPoint()))
            {
                offset = mousePos - new Vector2(Left.Pixels, Top.Pixels);
                dragging = true;
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            dragging = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (dragging)
            {
                // 获取鼠标位置（考虑UI缩放）
                Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
                
                // 计算新位置
                float newX = mousePos.X - offset.X;
                float newY = mousePos.Y - offset.Y;

                // 限制在屏幕范围内
                newX = MathHelper.Clamp(newX, 0, Main.screenWidth - Width.Pixels);
                newY = MathHelper.Clamp(newY, 0, Main.screenHeight - Height.Pixels);

                Left.Set(newX, 0f);
                Top.Set(newY, 0f);
                Recalculate();
            }

            // 如果鼠标不在按下状态，停止拖拽
            if (!Main.mouseLeft)
            {
                dragging = false;
            }
        }

        /// <summary>
        /// 获取标题栏矩形区域
        /// </summary>
        protected Rectangle GetTitleBarRectangle()
        {
            CalculatedStyle dimensions = GetDimensions();
            return new Rectangle(
                (int)dimensions.X,
                (int)dimensions.Y,
                (int)dimensions.Width,
                (int)TitleBarHeight
            );
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // 绘制标题栏背景
            Rectangle titleBarRect = GetTitleBarRectangle();
            Color titleBarColor = new Color(40, 40, 80) * 0.9f;
            
            // 绘制标题栏
            Texture2D pixel = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale").Value;
            spriteBatch.Draw(pixel, titleBarRect, titleBarColor);

            // 绘制标题文字
            CalculatedStyle dimensions = GetDimensions();
            Vector2 titlePos = new Vector2(
                dimensions.X + 10,
                dimensions.Y + (TitleBarHeight - 20) / 2
            );
            Terraria.Utils.DrawBorderString(spriteBatch, Title, titlePos, Color.White, 0.9f);
        }
    }
}

