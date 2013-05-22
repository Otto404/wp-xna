using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace sample
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        public GamePage()
        {
            InitializeComponent();

            // 从应用程序获取内容管理器
            contentManager = (Application.Current as App).Content;

            // 为此页面创建计时器
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 设置图形设备的共享模式以打开 XNA 呈现
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // 创建新的 SpriteBatch，可将其用于绘制纹理。
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: 在此处使用 this.content 加载游戏内容

            // 启动计时器
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // 停止计时器
            timer.Stop();

            // 设置图形设备的共享模式以关闭 XNA 呈现
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// 允许页面运行逻辑，例如更新全部内容、
        /// 检查冲突、收集输入信息以及播放音频。
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: 在此处添加更新逻辑
        }

        /// <summary>
        /// 允许页面进行自我绘制。
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: 在此处添加绘图代码
        }
    }
}
