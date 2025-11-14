using System.Collections.Generic;

namespace SoulAttuned.Core
{
    /// <summary>
    /// 天赋节点数据类
    /// </summary>
    public class TalentNode
    {
        /// <summary>
        /// 节点唯一ID
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// 节点名称（中文）
        /// </summary>
        public string NameCN { get; set; }

        /// <summary>
        /// 节点名称（英文）
        /// </summary>
        public string NameEN { get; set; }

        /// <summary>
        /// 节点描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 所需天赋点数
        /// </summary>
        public int TalentPointCost { get; set; } = 1;

        /// <summary>
        /// 所需等级
        /// </summary>
        public int RequiredLevel { get; set; } = 1;

        /// <summary>
        /// 所需谐振阶段
        /// </summary>
        public ResonancePhase RequiredPhase { get; set; } = ResonancePhase.Phase1_Start;

        /// <summary>
        /// 前置节点ID列表
        /// </summary>
        public List<string> Prerequisites { get; set; } = new List<string>();

        /// <summary>
        /// 节点类型
        /// </summary>
        public TalentNodeType NodeType { get; set; } = TalentNodeType.Passive;

        /// <summary>
        /// 职业分支
        /// </summary>
        public ClassBranch ClassBranch { get; set; } = ClassBranch.Universal;

        /// <summary>
        /// UI位置（环数，0-7）
        /// </summary>
        public int Ring { get; set; } = 0;

        /// <summary>
        /// UI位置（角度，0-360）
        /// </summary>
        public float Angle { get; set; } = 0f;

        public TalentNode(string nodeID, string nameCN, string nameEN, string description)
        {
            NodeID = nodeID;
            NameCN = nameCN;
            NameEN = nameEN;
            Description = description;
        }
    }

    /// <summary>
    /// 天赋节点类型
    /// </summary>
    public enum TalentNodeType
    {
        /// <summary>
        /// 被动节点（永久效果）
        /// </summary>
        Passive,

        /// <summary>
        /// 主动节点（需要玩家操作触发）
        /// </summary>
        Active,

        /// <summary>
        /// 关键节点（大型被动效果）
        /// </summary>
        Keystone
    }

    /// <summary>
    /// 职业分支
    /// </summary>
    public enum ClassBranch
    {
        /// <summary>
        /// 通用（所有职业）
        /// </summary>
        Universal,

        /// <summary>
        /// 近战
        /// </summary>
        Melee,

        /// <summary>
        /// 远程
        /// </summary>
        Ranged,

        /// <summary>
        /// 法师
        /// </summary>
        Mage,

        /// <summary>
        /// 召唤师
        /// </summary>
        Summoner
    }

    /// <summary>
    /// 节点状态
    /// </summary>
    public enum NodeStatus
    {
        /// <summary>
        /// 已激活
        /// </summary>
        Active,

        /// <summary>
        /// 可解锁（满足所有条件）
        /// </summary>
        Unlockable,

        /// <summary>
        /// 已锁定 - 点数不足
        /// </summary>
        LockedPoints,

        /// <summary>
        /// 已锁定 - 谐振阶段不足
        /// </summary>
        LockedResonance,

        /// <summary>
        /// 已锁定 - 前置节点未解锁
        /// </summary>
        LockedPrerequisites
    }
}

