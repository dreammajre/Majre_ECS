# Majre_ECS
ECS工程
这是我第一次使用GitHub Hello GitHub


来自API翻译
NativeContainer是托管值类型，为本机内存提供相对安全的C＃包装器。它包含指向非托管分配的指针。与Unity C＃作业系统一起使用时，a NativeContainer允许作业访问与主线程共享的数据，而不是使用副本。
有哪些类型的NativeContainer？

Unity附带一个NativeContainer名为NativeArray的程序。您还可以NativeArray使用NativeSlice操作a 以获取NativeArray从特定位置到特定长度的子集。

注意：实体组件系统（ECS）包扩展了Unity.Collections命名空间以包括其他类型的NativeContainer：

NativeList- 可调整大小NativeArray。
NativeHashMap - 键和值对。
NativeMultiHashMap - 每个键有多个值。
NativeQueue- 先进先出（FIFO）队列。

NativeContainer和安全系统

安全系统内置于所有NativeContainer类型。它跟踪正在阅读和写入的内容NativeContainer。

注意：所有NativeContainer类型的安全检查（例如越界检查，重新分配检查和竞争条件检查）仅在Unity 编辑器和播放模式下可用。

该安全系统的一部分是DisposeSentinel和AtomicSafetyHandle。该DisposeSentinel检测内存泄漏，给你一个错误，如果你没有正确地释放你的记忆。泄漏发生后很久就会发生内存泄漏错误。

使用AtomicSafetyHandle转移NativeContainer代码的所有权。例如，如果两个预定作业写入相同NativeArray，则安全系统会抛出一个异常，并显示一条明确的错误消息，说明解决问题的原因和方法。安排违规工作时，安全系统会抛出此异常。

在这种情况下，您可以安排具有依赖关系的作业。第一个作业可以写入NativeContainer，一旦完成执行，下一个作业就可以安全地读取和写入相同的作业NativeContainer。从主线程访问数据时，读写限制也适用。安全系统允许多个作业并行读取相同的数据。

默认情况下，当作业有权访问a时NativeContainer，它具有读写访问权限。此配置可能会降低性能。C＃作业系统不允许您安排与写入其中的NativeContainer另一个作业同时具有写入权限的作业。

如果作业不需要写入a NativeContainer，请NativeContainer使用[ReadOnly]属性标记，如下所示：

[ReadOnly]
public NativeArray<int> input;
在上面的示例中，您可以与其他对第一个也具有只读访问权限的作业同时执行作业NativeArray。

注意：无法防止从作业中访问静态数据。访问静态数据会绕过所有安全系统，并可能导致Unity崩溃。有关更多信息，请参阅C＃作业系统提示和故障排除。

NativeContainer分配器

创建a时NativeContainer，必须指定所需的内存分配类型。分配类型取决于作业运行的时间长度。通过这种方式，您可以定制分配以在每种情况下获得最佳性能。

内存分配和释放有三种分配器类型NativeContainer。在实例化你的时候需要指定合适的一个NativeContainer。

Allocator.Temp分配最快。它适用于寿命为一帧或更少的分配。您不应该将NativeContainer分配Temp用于作业。您还需要Dispose在从方法调用返回之前调用该方法（例如MonoBehaviour.Update，或从本机代码到托管代码的任何其他回调）。
Allocator.TempJob是一个比较慢的分配，Temp但速度比Persistent。它适用于四帧生命周期内的分配，并且是线程安全的。如果Dispose在四个帧内没有，则控制台会打印一个从本机代码生成的警告。大多数小型作业都使用此NativeContainer分配类型。
Allocator.Persistent是最慢的分配，但只要你需要它，并且如果有必要，可以持续整个应用程序的生命周期。它是直接调用malloc的包装器。较长的作业可以使用此NativeContainer分配类型。你不应该Persistent在性能至关重要的地方使用。
例如：

NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);
注意：上例中的数字1表示的大小NativeArray。在这种情况下，它只有一个数组元素（因为它只存储一个数据result）。

要在Unity中创建作业，您需要实现IJob接口。IJob允许您计划与正在运行的任何其他作业并行运行的单个作业。

注意：“作业”是Unity中用于实现IJob接口的任何结构的集合术语。

要创建工作，您需要：

创建一个实现的结构IJob。
添加作业使用的成员变量（blittable类型或NativeContainer类型）。
在结构中创建一个名为Execute的方法，并在其中实现作业。
执行作业时，该Execute方法在单个核心上运行一次。

注意：在设计作业时，请记住它们在数据副本上运行，除非是NativeContainer。因此，从主线程中的作业访问数据的唯一方法是写入NativeContainer。

简单作业定义的示例

// Job adding two floating point values together
public struct MyJob : IJob
{
    public float a;
    public float b;
    public NativeArray<float> result;

    public void Execute()
    {
        result[0] = a + b;
    }
}

要在主线程中安排作业，您必须：

实例化作业。
填充作业的数据。
调用Schedule方法。
调用Schedule将作业放入作业队列以便在适当的时间执行。一旦安排，你就不能打断工作。

注意：您只能Schedule从主线程调用。

安排工作的一个例子

// Create a native array of a single float to store the result. This example waits for the job to complete for illustration purposes
NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);

// Set up the job data
MyJob jobData = new MyJob();
jobData.a = 10;
jobData.b = 10;
jobData.result = result;

// Schedule the job
JobHandle handle = jobData.Schedule();

// Wait for the job to complete
handle.Complete();

// All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
float aPlusB = result[0];

// Free the memory allocated by the result array
result.Dispose();

JobHandle和依赖项
当您调用作业的Schedule方法时，它将返回JobHandle。您可以JobHandle在代码中使用a 作为其他作业的依赖项。如果作业取决于另一个作业的结果，您可以将第一个作业JobHandle作为参数传递给第二个作业的Schedule方法，如下所示：

JobHandle firstJobHandle = firstJob.Schedule();
secondJob.Schedule(firstJobHandle);
结合依赖关系

如果作业有许多依赖项，则可以使用JobHandle.CombineDependencies方法合并它们。CombineDependencies允许您将它们传递给Schedule方法。

NativeArray<JobHandle> handles = new NativeArray<JobHandle>(numJobs, Allocator.TempJob);

// Populate `handles` with `JobHandles` from multiple scheduled jobs...

JobHandle jh = JobHandle.CombineDependencies(handles);
在主线程中等待工作

使用JobHandle强迫你的代码在主线程等待您的工作执行完毕。要做到这一点，调用该方法完成的JobHandle。此时，您知道主线程可以安全地访问作业正在使用的NativeContainer。

注意：在计划作业时，作业不会开始执行。如果您正在等待主线程中的作业，并且您需要访问作业正在使用的NativeContainer数据，则可以调用该方法JobHandle.Complete。此方法从内存高速缓存中刷新作业并启动执行过程。调用该作业的类型Complete的JobHandle返回所有权NativeContainer到主线程。您需要调用Completea JobHandle以NativeContainer再次从主线程安全地访问这些类型。也可以通过调用返回的所有权主线程Complete上JobHandle是从工作的依赖。例如，你可以叫Complete上jobA，或者也可以称之为Complete上jobB取决于jobA。两者都导致了NativeContainerjobA调用后在主线程上安全访问时使用的类型Complete。

否则，如果您不需要访问数据，则需要明确刷新批处理。为此，请调用静态方法JobHandle.ScheduleBatchedJobs。请注意，调用此方法可能会对性能产生负面影响。

多个作业和依赖项的示例

工作代码：

// Job adding two floating point values together
public struct MyJob : IJob
{
    public float a;
    public float b;
    public NativeArray<float> result;

    public void Execute()
    {
        result[0] = a + b;
    }
}

// Job adding one to a value
public struct AddOneJob : IJob
{
    public NativeArray<float> result;
    
    public void Execute()
    {
        result[0] = result[0] + 1;
    }
}
主线程代码：

// Create a native array of a single float to store the result in. This example waits for the job to complete
NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);

// Setup the data for job #1
MyJob jobData = new MyJob();
jobData.a = 10;
jobData.b = 10;
jobData.result = result;

// Schedule job #1
JobHandle firstHandle = jobData.Schedule();

// Setup the data for job #2
AddOneJob incJobData = new AddOneJob();
incJobData.result = result;

// Schedule job #2
JobHandle secondHandle = incJobData.Schedule(firstHandle);

// Wait for job #2 to complete
secondHandle.Complete();

// All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
float aPlusB = result[0];

// Free the memory allocated by the result array
result.Dispose();

并行作业
当调度工作，只能有一个工作做一个任务。在游戏中，通常希望对大量对象执行相同的操作。有一个名为IJobParallelFor的独立作业类型来处理这个问题。

注意：“ParallelFor”作业是Unity中用于实现IJobParallelFor接口的任何结构的集合术语。

ParallelFor作业使用NativeArray数据作为其数据源。ParallelFor作业跨多个核心运行。每个核心有一个作业，每个作业处理一部分工作量。IJobParallelFor表现得像IJob，但它不是单个Execute方法，而是Execute在数据源中的每个项目上调用一次方法。方法中有一个整数参数Execute。该索引用于访问和操作作业实现中的数据源的单个元素。

ParallelFor作业定义的示例：

struct IncrementByDeltaTimeJob: IJobParallelFor
{
    public NativeArray<float> values;
    public float deltaTime;

    public void Execute (int index)
    {
        float temp = values[index];
        temp += deltaTime;
        values[index] = temp;
    }
}


调度ParallelFor作业

在调度ParallelFor作业时，必须指定NativeArray要拆分的数据源的长度。NativeArray如果结构中有多个，Unity C＃作业系统无法知道您要将哪个用作数据源。长度还告诉C＃Job System Execute预计会有多少方法。

幕后花絮
，ParallelFor作业的调度更复杂。在调度ParallelFor作业时，C＃作业系统将工作分成批处理以在核心之间分配。每批包含一组Execute方法。然后，C＃作业系统在每个CPU核心的Unity本机作业系统中调度最多一个作业，并将该本机作业通过一些批次来完成。

当本地作业在其他作业之前完成批处理时，它会从其他本机作业中窃取剩余批处理。它一次只能窃取本机作业剩余批次的一半，以确保缓存局部性。

要优化过程，您需要指定批次计数。批次计数控制您获得的作业数量，以及线程之间的工作重新分配的精细程度。批量计数较低（例如1）可以让您在线程之间进行更均匀的工作分配。它确实带来了一些开销，所以有时候增加批量计数会更好。从1开始并增加批次计数直到可忽略不计的性能增益是一种有效的策略。



调度ParallelFor作业的示例

工作代码：

// Job adding two floating point values together
public struct MyParallelJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float> a;
    [ReadOnly]
    public NativeArray<float> b;
    public NativeArray<float> result;

    public void Execute(int i)
    {
        result[i] = a[i] + b[i];
    }
}
主线程代码：

NativeArray<float> a = new NativeArray<float>(2, Allocator.TempJob);

NativeArray<float> b = new NativeArray<float>(2, Allocator.TempJob);

NativeArray<float> result = new NativeArray<float>(2, Allocator.TempJob);

a[0] = 1.1;
b[0] = 2.2;
a[1] = 3.3;
b[1] = 4.4;

MyParallelJob jobData = new MyParallelJob();
jobData.a = a;  
jobData.b = b;
jobData.result = result;

// Schedule the job with one Execute per index in the results array and only 1 item per processing batch
JobHandle handle = jobData.Schedule(result.Length, 1);

// Wait for the job to complete
handle.Complete();

// Free the memory allocated by the arrays
a.Dispose();
b.Dispose();
result.Dispose();

使用Unity C＃作业系统时，请确保遵守以下内容：

不要从作业访问静态数据

从作业访问静态数据会绕过所有安全系统。如果您访问错误的数据，可能会以意想不到的方式使Unity崩溃。例如，访问MonoBehaviour可能会导致域重新加载崩溃。

注意：由于存在这种风险，Unity的未来版本将阻止使用静态分析从作业进行全局变量访问。如果您确实访问作业中的静态数据，则应该期望您的代码在Unity的未来版本中中断。

刷新预定批次

如果希望作业开始执行，则可以使用JobHandle.ScheduleBatchedJobs刷新计划批处理。请注意，调用此方法可能会对性能产生负面影响。不刷新批处理会延迟调度，直到主线程等待结果。在所有其他情况下，使用JobHandle.Complete启动执行过程。

注意：在实体组件系统（ECS）中，将为您隐式刷新批处理，因此JobHandle.ScheduleBatchedJobs不需要调用。

不要尝试更新NativeContainer内容

由于缺少ref返回，因此无法直接更改NativeContainer的内容。例如，nativeArray[0]++;与var temp = nativeArray[0]; temp++;不更新值的写入相同nativeArray。

相反，您必须将索引中的数据复制到本地临时副本，修改该副本并将其保存回来，如下所示：

MyStruct temp = myNativeArray[i];
temp.memberVariable = 0;
myNativeArray[i] = temp;

调用JobHandle.Complete以重新获得所有权

跟踪数据所有权需要在主线程再次使用它们之前完成依赖项。仅检查JobHandle.IsCompleted是不够的。您必须调用该方法JobHandle.Complete以重新获得NativeContainer主线程的类型的所有权。呼叫Complete还可以清除安全系统中的状态。不这样做会引入内存泄漏。如果您在每个帧中安排新作业，并且依赖于前一帧的作业，则此过程也适用。

在主线程中使用Schedule和Complete

你只能调用时间表和Complete主线程。如果一个作业依赖于另一个作业，则用于JobHandle管理依赖项，而不是尝试在作业中安排作业。

在合适的时间使用计划和完成

Schedule只要您拥有所需的数据就立即打电话给工作，并且Complete在您需要结果之前不要打电话给它。优秀的做法是安排一个您不需要等待的工作，当它没有与正在运行的任何其他工作竞争时。例如，如果在一帧结束和下一帧的开始之间没有作业正在运行，并且可以接受一帧延迟，则可以将作业调度到帧的末尾并使用其结果在下一帧中。或者，如果您的游戏与其他作业的转换周期相同，并且框架中的其他位置存在大量未充分利用的时间段，则更有效地安排您的工作。

将NativeContainer类型标记为只读

请记住，作业NativeContainer默认情况下对类型具有读写访问权限。[ReadOnly]适当时使用该属性可提高性能。

检查数据依赖性

在Unity Profiler
窗口，主线程上的标记“WaitForJobGroup”表示Unity正在等待工作线程上的作业完成。此标记可能意味着您已在某处应引入数据依赖关系。寻找JobHandle.Complete跟踪数据依赖关系的位置，这些数据依赖关系迫使主线程等待。

调试作业

作业有一个Run函数，您可以使用它来代替Schedule在主线程上立即执行作业。您可以将其用于调试目的。

不要在作业中分配托管内存

在作业中分配托管内存非常慢，并且该作业无法使用Unity Burst编译器来提高性能。Burst是一种新的基于LLVM的后端编译器技术，可以让您更轻松。它需要C＃作业，并利用您平台的特定功能生成高度优化的机器代码。
