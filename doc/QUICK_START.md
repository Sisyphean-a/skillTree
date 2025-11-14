# 快速开始指南

## 🎯 当前状态

**阶段**: 阶段0已完成 ✅，准备开始阶段1  
**进度**: 20%  
**可以做什么**: 编译测试基础架构

---

## 🚀 立即开始测试

### 步骤1: 放置项目文件

将整个项目文件夹复制到tModLoader的模组源码目录：

**Windows路径**:
```
%USERPROFILE%\Documents\My Games\Terraria\tModLoader\ModSources\SoulAttuned\
```

**完整路径示例**:
```
C:\Users\你的用户名\Documents\My Games\Terraria\tModLoader\ModSources\SoulAttuned\
```

### 步骤2: 编译模组

1. 启动 **tModLoader**
2. 点击 **"Workshop"** 按钮
3. 点击 **"Develop Mods"**
4. 找到 **"Soul-Attuned System"**
5. 点击 **"Build + Reload"** 按钮

### 步骤3: 启用模组

1. 回到主菜单
2. 点击 **"Mods"**
3. 找到 **"Soul-Attuned System"**
4. 点击启用
5. 点击 **"Reload Mods"**

### 步骤4: 测试功能

1. 创建一个新角色或使用现有角色
2. 进入游戏世界
3. 按 **T** 键打开聊天
4. 输入命令测试：

```
/soul info          # 查看当前状态
/soul addxp 1000    # 添加经验
/soul setlevel 5    # 设置等级
```

⚠️ **注意**: 默认情况下调试命令被禁用，需要先启用调试模式！

### 步骤5: 启用调试模式

1. 在主菜单点击 **"Mod Configuration"**
2. 找到 **"Soul-Attuned System"**
3. 启用 **"Enable Debug Mode"**
4. 点击 **"Back"** 保存设置
5. 重新进入游戏

现在可以使用所有调试命令了！

---

## 🔧 调试命令完整列表

```bash
/soul info                  # 显示当前状态
/soul addxp <数量>          # 添加经验值
/soul setlevel <等级>       # 设置等级
/soul addtalent <点数>      # 添加天赋点
/soul addbasic <点数>       # 添加基础点
/soul reset                 # 重置所有数据
```

**示例**:
```bash
/soul addxp 5000           # 添加5000经验
/soul setlevel 10          # 设置为10级
/soul addtalent 3          # 添加3个天赋点
/soul addbasic 10          # 添加10个基础点
/soul info                 # 查看结果
```

---

## 📊 验证功能

### 测试数据持久化

1. 使用命令添加一些数据：
   ```
   /soul setlevel 5
   /soul addtalent 2
   ```

2. 查看状态：
   ```
   /soul info
   ```

3. **退出游戏并重新进入**

4. 再次查看状态：
   ```
   /soul info
   ```

5. ✅ 如果数据保持不变，说明持久化系统工作正常！

---

## 🐛 常见问题

### Q: 编译失败，提示找不到tModLoader.targets

**A**: 检查SoulAttuned.csproj文件中的路径：
```xml
<Import Project="..\..\references\tModLoader.targets" />
```
可能需要根据你的实际目录结构调整路径。

### Q: 模组加载失败

**A**: 检查以下几点：
1. build.txt文件是否存在且格式正确
2. 所有.cs文件是否都在同一目录
3. 查看tModLoader的日志文件（Logs文件夹）

### Q: 调试命令无效

**A**: 确保：
1. 已在配置中启用调试模式
2. 命令格式正确（注意空格）
3. 在聊天框中输入（按T键）

### Q: 数据没有保存

**A**: 
1. 确保正常退出游戏（不要强制关闭）
2. 检查SaveData/LoadData是否被正确调用
3. 查看日志是否有错误信息

---

## 📝 下一步开发

### 阶段1: 核心数据系统

现在基础架构已经完成，接下来要实现：

1. **经验获取系统**
   - 创建GlobalNPC类
   - 实现OnKill钩子
   - 计算并授予XP

2. **升级系统**
   - 实现升级检测
   - 计算所需XP
   - 发放天赋点和基础点

3. **世界谐振系统**
   - 检测Boss击败状态
   - 实现等级上限门控
   - 创建谐振阶段枚举

### 需要创建的新文件

```
SoulGlobalNPC.cs          # 处理怪物击杀和XP
ResonancePhase.cs         # 谐振阶段枚举
ExperienceSystem.cs       # 经验计算工具类
LevelingSystem.cs         # 升级逻辑
```

---

## 💡 开发提示

### 编码规范
- 所有公共方法都要有XML注释
- 使用防御性编码（检查null、边界等）
- 遵循命名约定（PascalCase/camelCase）

### 测试流程
1. 修改代码
2. 在tModLoader中点击"Build + Reload"
3. 重新进入游戏测试
4. 查看日志排查问题

### 调试技巧
- 使用SoulLogger.Debug()输出调试信息
- 启用调试模式查看详细日志
- 使用/soul命令快速测试功能

---

## 📚 参考文档

- [开发进度跟踪](DEVELOPMENT_PROGRESS.md) - 查看整体进度
- [阶段0总结](STAGE0_SUMMARY.md) - 了解已完成的工作
- [开发流程文档](开发流程拆分文档.md) - 详细任务清单
- [README](README.md) - 项目完整说明

---

## ✅ 检查清单

在开始阶段1之前，确保：

- [ ] 项目已成功编译
- [ ] 模组能在游戏中加载
- [ ] 调试命令可以正常使用
- [ ] 数据能够正确保存和加载
- [ ] 已阅读开发流程文档
- [ ] 理解下一步要实现的功能

---

**准备好了吗？让我们开始实现核心游戏逻辑吧！** 🚀

