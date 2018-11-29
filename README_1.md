# Majre_ECS
ECS工程
这是我第一次使用GitHub Hello GitHub


API 解释

EntityManager

SharedComponentData
ComponentData的类型，其中每个唯一值仅存储一次。ComponentData流按所有SharedComponentData的每个值划分为子集。

EntityArchetype

ComponentData类型的特定集合和SharedComponentData值，用于定义存储在EntityManager中的ComponentData流的子集。

ComponentSystem

游戏/系统逻辑/行为发生的地方。

EntityCommandBuffer

该EntityCommandBuffer课程解决了两个重要问题：

当你在工作时，你无法访问EntityManager。
当您访问EntityManager（例如，创建一个Entity）时，您将使所有注入的数组和ComponentGroup对象无效。
该EntityCommandBuffer抽象允许你排队的变化（无论是从工作或从主线程），使他们能够在主线程上后生效。有两种方法可以使用EntityCommandBuffer：

ComponentSystem在主线程上更新的子类有一个可自动调用的子类PostUpdateCommands。要使用它，只需引用属性并排队更改即可。从系统Update功能返回后，它们将立即自动应用于世界。
以下是两个棒射手样本的示例：

PostUpdateCommands。CreateEntity（TwoStickBootstrap。BasicEnemyArchetype）;
PostUpdateCommands。SetComponent（new  Position2D { Value  =  spawnPosition }）;
PostUpdateCommands。SetComponent（new  Heading2D { Value  =  new  float2（0.0f，- 1.0f）}）;
PostUpdateCommands。SetComponent（默认（Enemy））;
PostUpdateCommands。SetComponent（新 生 { 值 =  TwoStickBootstrap。设置。enemyInitialHealth }）;
PostUpdateCommands。SetComponent（new  EnemyShootState { Cooldown  =  0.5f }）;
PostUpdateCommands。SetComponent（新 MOVESPEED { 速度 =  TwoStickBootstrap。设置。enemySpeed }）;
PostUpdateCommands。AddSharedComponent（TwoStickBootstrap。EnemyLook）;
如您所见，API与API非常相似EntityManager。在这种模式下，将自动EntityCommandBuffer视为一种便利是有帮助的，它允许您在仍然对世界进行更改的同时防止系统内的阵列失效。

对于作业，您必须EntityCommandBuffer从主线程的屏障请求，并将它们传递给作业。当BarrierSystem更新时，命令缓冲区将在顺序播放在主线程创建它们。需要这个额外的步骤，以便可以集中存储器管理，并且可以保证生成的实体和组件的确定性。

块

A Chunk包含ComponentData每个Entity。一个中的所有实体都Chunk遵循相同的内存布局。在迭代组件时，a中组件的内存访问Chunk总是完全线性的，没有浪费加载到缓存行中。这是一个很难保证。

https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/chunk_iteration.md

ComponentDataFromEntity

https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/reference/component_data_from_entity.md

ComponentGroup

该ComponentGroup是在上面的基础类的所有迭代方法是建立（注，foreach，IJobProcessComponentData，等）。本质上，a ComponentGroup由一组所需的组件和/或减法组件构成。ComponentGroup允许您根据组件提取单个实体数组。

Unity网络同步
http://gad.qq.com/article/detail/288421

AssesBunder打包工具
https://github.com/Unity-Technologies/AssetBundles-Browser/releases
使用教程
http://gad.qq.com/article/detail/287854

