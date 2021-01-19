# TowerDefense-GameFramework-Demo

## 简介

这是一款基于开源框架[GameFramework][1]（以下简称GF）实现的一款塔防游戏Demo。Demo原型是Unity官方放在Assets Store上的Demo [Tower Defense Template][2]。此项目是对Demo原型使用GF进行再实现以及扩展，主要用于个人对GF的学习和实践，也给其他学习GF的同学一个参考。

## 版本信息

- Unity 2019.4.1f1
- GameFramework 2020.12.31
- Tower Defense Template 1.4

## 游戏简介

### 游戏预览

![简介1][4]  
![简介2][5]  
![简介3][13]  
![简介4][14]  

### 游戏介绍

游戏是塔防类型，总共五个关卡，每个关卡的地形环境、产生的敌人、以及可使用的塔都不一样。玩家利用获得的能量根据具体情况选择合适的塔，并建造在适当的位置来阻止敌人攻击基地。

#### 能量

玩家在关卡开始有少量初始能量，通过击杀敌人和建造能量塔均可以获得能量，能量用于建造和升级塔。

#### 塔

1. 加农炮塔：高射速、低伤害
2. 火箭炮塔：高AOE伤害（仅攻击地面敌人）
3. 激光炮塔：低射速、高伤害、远射程
4. 能量塔：每隔一段时间产生能量
5. 电子脉冲塔：对附近的敌人附加减速效果
6. 导弹阵列：对大范围敌人造成高额伤害，在场上存在10秒钟后自我销毁  
**塔可以进行升级，升级后可提升射程、伤害、减速率、能量产生效率等**

#### 敌人

1. 虫子：低血量、高移速
2. 直升机：可避免火箭炮塔的攻击，并且在道路被炮塔阻塞时可直接越过炮塔前往基地
3. 坦克：高血量、低移速
4. Boss：超高血量、超低移速
5. 超级虫子：高血量版虫子
6. 超级直升机：高血量版直升机
7. 超级坦克：高血量版坦克
8. 超级Boss：高血量版Boss  
**敌人一般不会攻击塔，但在塔完全阻挡住敌人前进的路时，就会攻击塔（直升机敌人不攻击塔，会直接越过塔），正确方式是结合地形情况建塔制造迂回路线，增加敌人达到基地需要行走的路程，但又不完全阻挡道路，避免塔被攻击**

#### 基地

基地是敌人进攻的最终目标，也是玩家需要守护的目标，当基地血量为0时游戏失败。

#### 关卡结算

若玩家在消灭关卡所有敌人且基地血量不为0时，则通关成功，若在消灭所有怪物前，基地血量被攻击至0，则游戏失败。通关成功会根据基地剩余血量进行评分。

## 相关实现

本项目用到了GF中的多个模块包括全局配置、数据表、实体、事件、文件系统、有限状态机、文件系统、本地化、对象池、引用池、流程、资源、场景、游戏配置、声音、UI等。

### 数据配置

![数据配置][6]  
游戏内所有数据均以Excel形式进行配置，导出生成二进制文件后在运行时加载读取。

### 本地化

![本地化][7]  
利用本地化模块以及资源模块中的变体实现游戏本地化。

### 引用池

![引用池][8]  
项目中大量重复使用的对象都使用了引用池进行缓存，避免频繁的内存分配。

### 资源打包配置

![资源打包配置1][9]  
![资源打包配置2][10]  
已对所有资源进行打包配置，设置了正确的分包信息、文件系统等。并根据内置分析工具做到0冗余、0循环引用。

### 热更新

![热更新][11]  
游戏启动会检测版本信息并进行基本资源（即非关卡内资源）更新。

### 分包下载

![热更新][12]  
游戏对每个关卡资源单独进行分包，进入关卡前需要下载更新相应的资源，而暂时没有玩到的关卡可以暂时不下载。

## 注意事项

游戏在Editor下默认以Editor模式启动，即读取工程内资源运行，不会读取AB包也不会进行更新。项目已正确配置打包信息，并完成了相应的热更逻辑的实现，若要测试更新模式，需要在Base组件取消Editor Resource Mode，并确保Resource组件的Resource Mode为Updatable模式。在打包资源并正确部署资源后即可正常运行更新模式（借助HFS等工具可在本地进行部署和测试）。

## 结语

感谢[GameFramework][1]作者[Ellan Jiang][3]提供的优秀框架。

  [1]: https://github.com/EllanJiang/GameFramework "GF link"
  [2]: https://assetstore.unity.com/packages/essentials/tutorial-projects/tower-defense-template-107692 "Tower Defense Template Link"
  [3]: https://github.com/EllanJiang "Ellan Jiang link"
  [4]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/1.png "简介1"
  [5]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/2.JPG "简介2"
  [6]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/3.png "数据配置"
  [7]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/4.JPG "本地化"
  [8]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/5.png "引用池"
  [9]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/6.png "资源打包配置1"
  [10]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/7.png "资源打包配置2"
  [11]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/8.png "热更新"
  [12]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/9.png "分包下载"
  [13]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/10.gif "简介3"
  [14]: https://github.com/DrFlower/TowerDefense-GameFramework-Demo/blob/master/Doc/11.gif "简介4"
