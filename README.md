# 灵魂谐振系统 (Soul-Attuned System)

一个为《泰拉瑞亚》tModLoader 1.4.4 开发的深度RPG成长型模组。

## 📋 项目概述

灵魂谐振系统为泰拉瑞亚添加了完整的技能树和等级系统，包括：

- **经验与升级系统** - 动态XP公式，非线性升级曲线
- **世界谐振机制** - 等级上限受Boss击败进度限制
- **双轨成长系统** - 天赋点（质变）+ 基础点（量变）
- **放射状技能树UI** - 美观的"灵魂核心"界面
- **四大职业分支** - 近战、远程、法师、召唤师
- **主动机制设计** - 奖励玩家操作，而非被动数值

## 🚀 快速开始

### 环境要求

- tModLoader 1.4.4 或更高版本
- .NET 6.0 SDK
- Visual Studio 2022 或 VS Code（推荐）

### 编译模组

1. **克隆仓库**
   ```bash
   git clone https://github.com/Sisyphean-a/skillTree.git
   cd skillTree
   ```

2. **放置到 tModLoader 模组源码目录**
   - Windows: `%USERPROFILE%\Documents\My Games\Terraria\tModLoader\ModSources\SoulAttuned`
   - 将本项目的所有文件复制到该目录

3. **在 tModLoader 中编译**
   - 启动 tModLoader
   - 点击 "Workshop" → "Develop Mods"
   - 找到 "Soul-Attuned System"
   - 点击 "Build + Reload"

### 测试模组

1. **启用调试模式**
   - 在游戏中打开 "Mod Configuration"
   - 找到 "Soul-Attuned System"
   - 启用 "Enable Debug Mode"

2. **使用调试命令**
   ```
   /soul info          - 查看当前状态
   /soul addxp 1000    - 添加1000经验
   /soul setlevel 10   - 设置等级为10
   /soul addtalent 5   - 添加5个天赋点
   /soul addbasic 10   - 添加10个基础点
   /soul reset         - 重置所有数据
   ```

## 📂 项目结构

```
SoulAttuned/
├── Core/                      # 核心数据和逻辑
│   ├── SoulPlayer.cs         # 玩家数据
│   ├── SoulGlobalNPC.cs      # NPC击杀和经验
│   └── ResonanceSystem.cs    # 世界谐振
├── Systems/                   # 系统管理
│   ├── SoulSystem.cs         # 系统管理
│   └── SoulConfig.cs         # 配置
├── Utils/                     # 工具类
│   ├── SoulLogger.cs         # 日志
│   └── DebugCommands.cs      # 调试命令
├── UI/                        # UI相关（待实现）
├── doc/                       # 文档
│   ├── DEVELOPMENT_PROGRESS.md
│   └── TESTING_GUIDE.md
└── SoulAttuned.cs            # 主模组类
```

## 📖 开发文档

- [开发进度跟踪](doc/DEVELOPMENT_PROGRESS.md) - 当前开发状态和任务清单
- [测试指南](doc/TESTING_GUIDE.md) - 功能测试指南
- [技能树设计方案](doc/技能树设计方案.md) - 核心架构和设计理念
- [节点设计](doc/节点设计.md) - 职业技能节点详细设计
- [开发流程拆分文档](doc/开发流程拆分文档.md) - 详细开发任务

## 🎯 当前开发状态

**版本**: v0.3.0 (开发中)
**当前阶段**: 阶段 3 - 技能节点实现
**总体进度**: 60%

### 已完成
- ✅ 阶段 0: 基础架构搭建
  - 项目配置和核心类框架
  - 数据持久化系统
  - 调试工具和日志系统

- ✅ 阶段 1: 核心数据系统
  - 经验获取系统（动态XP公式）
  - 世界谐振系统（8个阶段，等级上限门控）
  - 升级系统（自动升级，奖励发放）
  - 基础属性效果（伤害、防御、移动速度）

- ✅ 阶段 2: UI 界面系统
  - UI架构（UIService, SoulUISystem）
  - 可拖拽面板系统
  - 基础点分配面板（完全可用）
  - 天赋树UI框架（放射状布局，8个同心圆环）
  - 热键系统（K键打开UI）

### 进行中
- 🔄 阶段 3: 技能节点实现 (准备开始)

### 待开发
- ⬜ 阶段 4: 网络同步与优化
- ⬜ 阶段 5: 内容完善与平衡

## 🤝 贡献指南

欢迎提交Issue和Pull Request！

### 代码规范
- 类名: `PascalCase`
- 字段: `camelCase`
- 常量: `UPPER_SNAKE_CASE`
- 所有公共方法必须有XML文档注释

### 提交规范
- 格式: `[阶段] 简短描述`
- 示例: `[阶段1] 实现经验获取逻辑`

## 📄 许可证

本项目采用 MIT 许可证。详见 LICENSE 文件。

## 🙏 致谢

- tModLoader 团队 - 提供了强大的模组开发框架
- Terraria 社区 - 提供了大量的参考和灵感

---

**开发者**: Sisyphean-a  
**GitHub**: https://github.com/Sisyphean-a/skillTree  
**最后更新**: 2025-11-14

