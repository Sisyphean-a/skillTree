using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoulAttuned.Core;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SoulAttuned.UI
{
    /// <summary>
    /// 基础点分配面板
    /// </summary>
    public class BasicPointsPanel : DraggablePanel
    {
        private UIText availablePointsText;
        
        // 三个属性行
        private AttributeRow damageRow;
        private AttributeRow defenseRow;
        private AttributeRow agilityRow;

        public BasicPointsPanel()
        {
            Title = "基础灵魂 (Basic Soul)";
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            BackgroundColor = new Color(30, 30, 60) * 0.85f;
            BorderColor = new Color(80, 80, 120);

            float yOffset = TitleBarHeight + 10;

            // 可用点数显示
            availablePointsText = new UIText("可用点数: 0", 0.9f);
            availablePointsText.Left.Set(10, 0f);
            availablePointsText.Top.Set(yOffset, 0f);
            Append(availablePointsText);

            yOffset += 30;

            // 灵魂伤害行
            damageRow = new AttributeRow("灵魂伤害", "Soul Damage", 
                "+0.5% 综合伤害", AttributeType.Damage);
            damageRow.Left.Set(10, 0f);
            damageRow.Top.Set(yOffset, 0f);
            damageRow.Width.Set(-20, 1f);
            damageRow.Height.Set(50, 0f);
            Append(damageRow);

            yOffset += 60;

            // 灵魂防御行
            defenseRow = new AttributeRow("灵魂防御", "Soul Defense", 
                "+1 基础防御", AttributeType.Defense);
            defenseRow.Left.Set(10, 0f);
            defenseRow.Top.Set(yOffset, 0f);
            defenseRow.Width.Set(-20, 1f);
            defenseRow.Height.Set(50, 0f);
            Append(defenseRow);

            yOffset += 60;

            // 灵魂敏捷行
            agilityRow = new AttributeRow("灵魂敏捷", "Soul Agility", 
                "+0.5% 移动速度", AttributeType.Agility);
            agilityRow.Left.Set(10, 0f);
            agilityRow.Top.Set(yOffset, 0f);
            agilityRow.Width.Set(-20, 1f);
            agilityRow.Height.Set(50, 0f);
            Append(agilityRow);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // 更新显示
            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            availablePointsText.SetText($"可用点数: {soulPlayer.basicPoints}");

            damageRow.UpdateDisplay(soulPlayer);
            defenseRow.UpdateDisplay(soulPlayer);
            agilityRow.UpdateDisplay(soulPlayer);
        }
    }

    /// <summary>
    /// 属性类型
    /// </summary>
    public enum AttributeType
    {
        Damage,
        Defense,
        Agility
    }

    /// <summary>
    /// 属性行UI元素
    /// </summary>
    public class AttributeRow : UIElement
    {
        private string nameCN;
        private string nameEN;
        private string bonus;
        private AttributeType type;

        private UIText nameText;
        private UIText rankText;
        private UIText costText;
        private UIText plusButton;

        public AttributeRow(string nameCN, string nameEN, string bonus, AttributeType type)
        {
            this.nameCN = nameCN;
            this.nameEN = nameEN;
            this.bonus = bonus;
            this.type = type;
        }

        public override void OnInitialize()
        {
            // 属性名称
            nameText = new UIText($"{nameCN} ({nameEN})", 0.85f);
            nameText.Left.Set(0, 0f);
            nameText.Top.Set(0, 0f);
            Append(nameText);

            // 等级显示
            rankText = new UIText("等级: 0", 0.8f);
            rankText.Left.Set(0, 0f);
            rankText.Top.Set(20, 0f);
            Append(rankText);

            // 成本显示
            costText = new UIText("成本: 1", 0.75f);
            costText.Left.Set(0, 0f);
            costText.Top.Set(35, 0f);
            Append(costText);

            // + 按钮（使用文本按钮代替图片按钮）
            plusButton = new UIText("[+]", 1.0f);
            plusButton.Left.Set(-50, 1f);
            plusButton.Top.Set(5, 0f);
            plusButton.TextColor = Color.LightGreen;
            plusButton.OnLeftClick += OnPlusButtonClick;
            plusButton.OnMouseOver += (evt, element) => plusButton.TextColor = Color.Yellow;
            plusButton.OnMouseOut += (evt, element) => plusButton.TextColor = Color.LightGreen;
            Append(plusButton);
        }

        /// <summary>
        /// 更新显示
        /// </summary>
        public void UpdateDisplay(SoulPlayer soulPlayer)
        {
            int currentRank = GetCurrentRank(soulPlayer);
            int cost = GetCost(currentRank);

            rankText.SetText($"等级: {currentRank} ({bonus})");
            costText.SetText($"成本: {cost} 点");

            // 如果点数不足，显示红色并禁用按钮
            if (soulPlayer.basicPoints < cost)
            {
                costText.TextColor = Color.Red;
                plusButton.TextColor = Color.Gray;
            }
            else
            {
                costText.TextColor = Color.White;
                plusButton.TextColor = Color.LightGreen;
            }
        }

        private void OnPlusButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            SoulPlayer soulPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            int currentRank = GetCurrentRank(soulPlayer);
            int cost = GetCost(currentRank);

            // 检查是否有足够的点数
            if (soulPlayer.basicPoints >= cost)
            {
                // 扣除点数
                soulPlayer.basicPoints -= cost;

                // 增加属性等级
                switch (type)
                {
                    case AttributeType.Damage:
                        soulPlayer.soulDamageRank++;
                        break;
                    case AttributeType.Defense:
                        soulPlayer.soulDefenseRank++;
                        break;
                    case AttributeType.Agility:
                        soulPlayer.soulAgilityRank++;
                        break;
                }

                // 播放音效
                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.MenuTick);
            }
            else
            {
                // 播放错误音效
                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.MenuClose);
            }
        }

        private int GetCurrentRank(SoulPlayer soulPlayer)
        {
            return type switch
            {
                AttributeType.Damage => soulPlayer.soulDamageRank,
                AttributeType.Defense => soulPlayer.soulDefenseRank,
                AttributeType.Agility => soulPlayer.soulAgilityRank,
                _ => 0
            };
        }

        /// <summary>
        /// 计算下一级的成本（边际成本递增）
        /// </summary>
        private int GetCost(int currentRank)
        {
            return type switch
            {
                AttributeType.Damage => currentRank < 10 ? 1 : (currentRank < 20 ? 2 : 3),
                AttributeType.Defense => currentRank < 5 ? 2 : (currentRank < 10 ? 3 : 4),
                AttributeType.Agility => currentRank < 10 ? 2 : 3,
                _ => 1
            };
        }
    }
}

