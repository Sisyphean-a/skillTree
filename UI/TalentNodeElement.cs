using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoulAttuned.Core;
using Terraria;
using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// 天赋节点UI元素
    /// </summary>
    public class TalentNodeElement : UIElement
    {
        private TalentNode node;
        private bool isHovered;

        public TalentNodeElement(TalentNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// 获取节点数据
        /// </summary>
        public TalentNode GetNode()
        {
            return node;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 检测鼠标悬停
            Rectangle hitbox = GetDimensions().ToRectangle();
            isHovered = hitbox.Contains(Main.mouseX, Main.mouseY);

            // 显示工具提示
            if (isHovered)
            {
                ShowTooltip();
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            // 尝试解锁节点
            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            NodeStatus status = GetNodeStatus(soulPlayer);

            if (status == NodeStatus.Unlockable)
            {
                // TODO: 在阶段3中实现实际的节点解锁逻辑
                // 这里只是播放音效作为反馈
                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.MenuTick);
                Main.NewText($"节点 '{node.NameCN}' 将在阶段3中实现解锁功能", Color.Yellow);
            }
            else
            {
                // 播放错误音效
                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.MenuClose);
                
                // 显示无法解锁的原因
                string reason = status switch
                {
                    NodeStatus.Active => "节点已激活",
                    NodeStatus.LockedPoints => "天赋点不足",
                    NodeStatus.LockedResonance => "谐振阶段不足",
                    NodeStatus.LockedPrerequisites => "前置节点未解锁",
                    _ => "无法解锁"
                };
                Main.NewText(reason, Color.Red);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            NodeStatus status = GetNodeStatus(soulPlayer);

            CalculatedStyle dimensions = GetDimensions();
            Vector2 center = dimensions.Center();

            // 根据状态选择颜色
            Color nodeColor = GetNodeColor(status);
            
            // 如果鼠标悬停，增加亮度
            if (isHovered)
            {
                nodeColor = Color.Lerp(nodeColor, Color.White, 0.3f);
            }

            // 绘制节点圆形
            Texture2D pixel = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale").Value;
            Rectangle rect = dimensions.ToRectangle();
            spriteBatch.Draw(pixel, rect, nodeColor);

            // 绘制边框
            Color borderColor = isHovered ? Color.White : new Color(100, 100, 100);
            DrawBorder(spriteBatch, rect, borderColor, 2);
        }

        /// <summary>
        /// 获取节点状态
        /// </summary>
        private NodeStatus GetNodeStatus(SoulPlayer soulPlayer)
        {
            // TODO: 在阶段3中实现实际的状态检查
            // 这里只是示例逻辑

            // 检查天赋点
            if (soulPlayer.talentPoints < node.TalentPointCost)
            {
                return NodeStatus.LockedPoints;
            }

            // 检查等级
            if (soulPlayer.currentLevel < node.RequiredLevel)
            {
                return NodeStatus.LockedPoints;
            }

            // 检查谐振阶段
            ResonancePhase currentPhase = ResonanceSystem.GetCurrentPhase();
            if (currentPhase < node.RequiredPhase)
            {
                return NodeStatus.LockedResonance;
            }

            // 如果所有条件满足，则可解锁
            return NodeStatus.Unlockable;
        }

        /// <summary>
        /// 根据状态获取节点颜色
        /// </summary>
        private Color GetNodeColor(NodeStatus status)
        {
            return status switch
            {
                NodeStatus.Active => new Color(255, 215, 0) * 0.9f,           // 金色 - 已激活
                NodeStatus.Unlockable => new Color(100, 200, 255) * 0.8f,     // 蓝色 - 可解锁
                NodeStatus.LockedPoints => new Color(150, 150, 150) * 0.6f,   // 灰色 - 点数不足
                NodeStatus.LockedResonance => new Color(100, 50, 150) * 0.6f, // 紫色 - 谐振不足
                NodeStatus.LockedPrerequisites => new Color(100, 100, 100) * 0.5f, // 深灰 - 前置未解锁
                _ => Color.Gray * 0.5f
            };
        }

        /// <summary>
        /// 显示工具提示
        /// </summary>
        private void ShowTooltip()
        {
            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            NodeStatus status = GetNodeStatus(soulPlayer);

            string tooltip = $"{node.NameCN} ({node.NameEN})\n{node.Description}\n\n";
            tooltip += $"消耗: {node.TalentPointCost} 天赋点\n";
            tooltip += $"需求等级: {node.RequiredLevel}\n";
            tooltip += $"需求谐振: {ResonanceSystem.GetPhaseName(node.RequiredPhase)}\n\n";
            tooltip += $"状态: {GetStatusText(status)}";

            Main.hoverItemName = tooltip;
        }

        /// <summary>
        /// 获取状态文本
        /// </summary>
        private string GetStatusText(NodeStatus status)
        {
            return status switch
            {
                NodeStatus.Active => "已激活",
                NodeStatus.Unlockable => "可解锁",
                NodeStatus.LockedPoints => "点数不足",
                NodeStatus.LockedResonance => "谐振阶段不足",
                NodeStatus.LockedPrerequisites => "前置节点未解锁",
                _ => "未知"
            };
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        private void DrawBorder(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            Texture2D pixel = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale").Value;
            
            // 上
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            // 下
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
            // 左
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            // 右
            spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}

