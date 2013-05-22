using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mygame
{
    public partial class App : Application
    {
        /// <summary>
        /// 提供对电话应用程序根帧的轻松访问。
        /// </summary>
        /// <returns>电话应用程序的根帧。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// 提供对应用程序 ContentManager 的访问。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// 提供对设置为提取 FrameworkDispatcher 的 GameTimer 的访问。
        /// </summary>
        public GameTimer FrameworkDispatcherTimer { get; private set; }

        /// <summary>
        /// 提供对应用程序 AppServiceProvider 的访问。
        /// </summary>
        public AppServiceProvider Services { get; private set; }

        /// <summary>
        /// 用于应用程序对象的构造函数。
        /// </summary>
        public App()
        {
            // 用于未捕获的异常的全局处理程序。 
            UnhandledException += Application_UnhandledException;

            // 标准 Silverlight 初始化
            InitializeComponent();

            // 电话特定的初始化
            InitializePhoneApplication();

            // XNA 初始化
            InitializeXnaApplication();

            // 调试时显示图形配置文件信息。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 显示当前帧速率计数器。
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 显示此应用程序每一帧中重新绘制的区域。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // 启用非生产分析可视化模式， 
                // 显示已传输至 GPU 的页面区域(包括彩色重叠区域)。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // 通过将应用程序 PhoneApplicationService 对象的 UserIdleDetectionMode 属性设置为禁用，
                // 可以禁用应用程序闲置检测。
                // 警告: - 仅限于在调试模式下使用该功能。禁用用户闲置检测功能的应用程序将继续运行
                // 并且在不使用电话时消耗电池。
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // 当应用程序启动时要执行的代码(例如，由 Start 开始)
        // 如果重新激活应用程序，则不执行此代码
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // 当应用程序激活时执行的代码(转到前台处理程序)
        // 如果第一次启动应用程序，则不执行此代码
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // 当应用程序停用时执行的代码(发送到后台处理程序)
        // 如果应用程序正在关闭，则不执行此代码
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 当应用程序正在关闭时执行的代码(例如，用户单击“返回”)
        // 如果应用程序已停用，则不执行此代码
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // 导航失败时要执行的代码
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 导航失败；中断至调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        // 出现未处理的异常时要执行的代码
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 发生未处理的异常；中断至调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // 避免双重初始化
        private bool phoneApplicationInitialized = false;

        // 不要在此方法中添加任何其他代码
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 创建帧，但不将其设置为 RootVisual；这可使初始屏幕
            // 在应用程序准备呈现之前一直保持活动状态。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 处理导航失败
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 确保不再次进行初始化
            phoneApplicationInitialized = true;
        }

        // 不要在此方法中添加任何其他代码
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 将根 Visual 设置为允许应用程序呈现
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 删除此处理程序，因为不再需要
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        #region XNA application initialization

        // 执行应用程序所需的 XNA 类型的初始化。
        private void InitializeXnaApplication()
        {
            // 创建服务提供程序
            Services = new AppServiceProvider();

            // 将 SharedGraphicsDeviceManager 添加到服务，作为应用程序的 IGraphicsDeviceService
            foreach (object obj in ApplicationLifetimeObjects)
            {
                if (obj is IGraphicsDeviceService)
                    Services.AddService(typeof(IGraphicsDeviceService), obj);
            }

            // 创建 ContentManager，使应用程序能够加载预编译的资产
            Content = new ContentManager(Services, "Content");

            // 创建 GameTimer 以提取 XNA FrameworkDispatcher
            FrameworkDispatcherTimer = new GameTimer();
            FrameworkDispatcherTimer.FrameAction += FrameworkDispatcherFrameAction;
            FrameworkDispatcherTimer.Start();
        }

        // 提取 FrameworkDispatcher 每一帧的事件处理程序。
        // FrameworkDispatcher 对于许多 XNA 事件
        // 和 SoundEffect 回放功能都是必需的。
        private void FrameworkDispatcherFrameAction(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        #endregion
    }
}
