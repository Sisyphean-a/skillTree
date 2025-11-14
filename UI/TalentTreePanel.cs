using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoulAttuned.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// 天赋树面板
    /// </summary>
    public class TalentTreePanel : DraggablePanel
    {
        private UIText talentPointsText;
        private UIText levelText;
        private UIText phaseText;

        /// <summary>
        /// 天赋节点元素列表
        /// </summary>
        private List<TalentNodeElement> nodeElements = new List<TalentNodeElement>();

        /// <summary>
        /// 圆环绘制区域的顶部偏移（标题栏下方）
        /// </summary>
        private const float TreeAreaTopOffset = 100f;

        /// <summary>
        /// 基础半径（中心核心到第一环的距离）
        /// </summary>
        private const float BaseRadius = 40f;

        /// <summary>
        /// 每环之间的间距
        /// </summary>
        private const float RingSpacing = 35f;

        /// <summary>
        /// 最大圆环半径（第8环）
        /// </summary>
        private const float MaxRingRadius = BaseRadius + 8 * RingSpacing; // 40 + 8*35 = 320

        public TalentTreePanel()
        {
            Title = "灵魂核心 (Soul Core)";
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            BackgroundColor = new Color(20, 20, 40) * 0.9f;
            BorderColor = new Color(100, 100, 150);

            float yOffset = TitleBarHeight + 10;

            // 天赋点数显示
            talentPointsText = new UIText("天赋点: 0", 0.9f);
            talentPointsText.Left.Set(10, 0f);
            talentPointsText.Top.Set(yOffset, 0f);
            Append(talentPointsText);

            yOffset += 25;

            // 等级显示
            levelText = new UIText("等级: 1", 0.85f);
            levelText.Left.Set(10, 0f);
            levelText.Top.Set(yOffset, 0f);
            Append(levelText);

            yOffset += 25;

            // 谐振阶段显示
            phaseText = new UIText("谐振: 阶段1", 0.85f);
            phaseText.Left.Set(10, 0f);
            phaseText.Top.Set(yOffset, 0f);
            Append(phaseText);

            // TODO: 在阶段3中添加实际的天赋节点
            // 这里只创建一个示例节点
            CreateExampleNode();
        }

        /// <summary>
        /// 创建示例节点（用于测试UI）
        /// </summary>
        private void CreateExampleNode()
        {
            TalentNode exampleNode = new TalentNode(
                "example_node",
                "示例节点",
                "Example Node",
                "这是一个示例天赋节点\n将在阶段3中实现实际效果"
            )
            {
                Ring = 0,
                Angle = 0,
                RequiredLevel = 1,
                TalentPointCost = 1
            };

            TalentNodeElement nodeElement = new TalentNodeElement(exampleNode);

            // 计算节点位置（需要在Update中动态计算，因为需要dimensions）
            // 这里先设置一个临时位置，实际位置在Update中计算
            nodeElement.Width.Set(30, 0f);
            nodeElement.Height.Set(30, 0f);

            Append(nodeElement);
            nodeElements.Add(nodeElement);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 更新显示
            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            talentPointsText.SetText($"天赋点: {soulPlayer.talentPoints}");
            levelText.SetText($"等级: {soulPlayer.currentLevel}");

            ResonancePhase currentPhase = ResonanceSystem.GetCurrentPhase();
            string phaseName = ResonanceSystem.GetPhaseName(currentPhase);
            phaseText.SetText($"谐振: {phaseName}");

            // 更新节点位置（使其与圆环对齐）
            UpdateNodePositions();
        }

        /// <summary>
        /// 更新所有节点的位置
        /// </summary>
        private void UpdateNodePositions()
        {
            CalculatedStyle dimensions = GetDimensions();

            // 计算中心点（与DrawSelf中的计算一致）
            float centerX = dimensions.Width / 2f;
            float centerY = TitleBarHeight + TreeAreaTopOffset + MaxRingRadius;

            foreach (var nodeElement in nodeElements)
            {
                TalentNode node = nodeElement.GetNode();

                // 计算节点所在环的半径（Ring 0 = 中心核心，Ring 1-8 = 谐振环）
                float radius = BaseRadius + (node.Ring + 1) * RingSpacing;

                // 计算角度（转换为弧度）
                float angleRad = MathHelper.ToRadians(node.Angle);

                // 计算节点位置
                float x = centerX + radius * (float)System.Math.Cos(angleRad);
                float y = centerY + radius * (float)System.Math.Sin(angleRad);

                // 设置节点位置（减去节点半径以居中）
                nodeElement.Left.Set(x - 15, 0f);
                nodeElement.Top.Set(y - 15, 0f);
                nodeElement.Recalculate();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // 计算中心点位置（面板中心，标题栏下方）
            CalculatedStyle dimensions = GetDimensions();
            Vector2 center = new Vector2(
                dimensions.X + dimensions.Width / 2f,
                dimensions.Y + TitleBarHeight + TreeAreaTopOffset + MaxRingRadius
            );

            // 绘制中心圆
            DrawCircle(spriteBatch, center, 30f, new Color(100, 150, 255) * 0.8f);
            DrawCircle(spriteBatch, center, 25f, new Color(150, 200, 255) * 0.6f);

            // 绘制谐振环（8个同心圆）
            for (int i = 1; i <= 8; i++)
            {
                float radius = BaseRadius + i * RingSpacing;
                Color ringColor = new Color(80, 120, 200) * 0.3f;
                DrawCircleOutline(spriteBatch, center, radius, ringColor, 2f);
            }
        }

        /// <summary>
        /// 绘制实心圆
        /// </summary>
        private void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            Texture2D pixel = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale").Value;
            Rectangle rect = new Rectangle((int)(center.X - radius), (int)(center.Y - radius), 
                (int)(radius * 2), (int)(radius * 2));
            spriteBatch.Draw(pixel, rect, color);
        }

        /// <summary>
        /// 绘制圆形轮廓
        /// </summary>
        private void DrawCircleOutline(SpriteBatch spriteBatch, Vector2 center, float radius, Color color, float thickness)
        {
            // 简化版：绘制多个点形成圆
            int segments = 64;
            for (int i = 0; i < segments; i++)
            {
                float angle1 = MathHelper.TwoPi * i / segments;
                float angle2 = MathHelper.TwoPi * (i + 1) / segments;
                
                Vector2 p1 = center + new Vector2(
                    radius * (float)System.Math.Cos(angle1),
                    radius * (float)System.Math.Sin(angle1)
                );
                Vector2 p2 = center + new Vector2(
                    radius * (float)System.Math.Cos(angle2),
                    radius * (float)System.Math.Sin(angle2)
                );

                // 使用像素纹理绘制线段
                DrawLine(spriteBatch, p1, p2, color, thickness);
            }
        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        {
            Texture2D pixel = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale").Value;
            Vector2 edge = end - start;
            float angle = (float)System.Math.Atan2(edge.Y, edge.X);
            
            spriteBatch.Draw(pixel,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), (int)thickness),
                null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}

