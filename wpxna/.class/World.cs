#define T1
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace zoyobar.game
{


	public partial class World
		: PhoneApplicationPage
	{
		private SpriteBatch spiritBatch;
		internal readonly Color BackgroundColor;

		#region " ServiceProvider "
		internal sealed class ServiceProvider : IServiceProvider
		{
			private readonly Dictionary<Type, object> services = new Dictionary<Type, object> ( );

			internal ServiceProvider ( Application application )
			{

				foreach ( object service in application.ApplicationLifetimeObjects )
					if ( service is IGraphicsDeviceService )
						this.AddService ( typeof ( IGraphicsDeviceService ), service );

			}

			internal void AddService ( Type serviceType, object service )
			{

				if ( null == serviceType || null == service || services.ContainsKey ( serviceType )  || !serviceType.IsAssignableFrom ( service.GetType ( ) ) )
					return;

				this.services.Add ( serviceType, service );
			}

			public object GetService ( Type serviceType )
			{

				if ( null == serviceType || !services.ContainsKey ( serviceType ) )
					return null;

				return services[ serviceType ];
			}

			internal void RemoveService ( Type serviceType )
			{

				if ( null == serviceType || !services.ContainsKey ( serviceType ) )
					return;

				this.services.Remove ( serviceType );
			}

		}
		#endregion

		internal readonly ServiceProvider Services = new ServiceProvider ( Application.Current );
		internal readonly GraphicsDevice GraphicsDevice;

		private readonly GameTimer timer = new GameTimer ( );

#if T1
		private SpriteFont myfont;
#endif

		public World ( )
			: this ( Color.Black )
		{ }
		public World ( Color backgroundColor )
			: base ( )
		{
			SharedGraphicsDeviceManager graph = SharedGraphicsDeviceManager.Current;
			this.GraphicsDevice = graph.GraphicsDevice;

			this.timer.UpdateInterval = TimeSpan.FromSeconds ( 1.0 / 30 );
			this.timer.Update += this.OnUpdate;
			this.timer.Draw += this.OnDraw;

			this.BackgroundColor = backgroundColor;

			PhoneApplicationService.Current.Activated += this.activate;
			PhoneApplicationService.Current.Deactivated += this.deactivate;
		}

		private void activate ( object sender, ActivatedEventArgs e )
		{ }

		private void deactivate ( object sender, DeactivatedEventArgs e )
		{ }

		protected override void OnNavigatedTo ( NavigationEventArgs e )
		{
			SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode ( true );
			this.spiritBatch = new SpriteBatch ( this.GraphicsDevice );
			this.Services.AddService ( typeof ( SpriteBatch ), this.spiritBatch );
			
			this.timer.Start ( );

#if T1
			ContentManager content = new ContentManager ( this.Services, "Content" );
			this.myfont = content.Load<SpriteFont> ( @"font\myfont" );
#endif

			base.OnNavigatedTo ( e );
		}

		private void OnUpdate ( object sender, GameTimerEventArgs e )
		{ }

		private void OnDraw ( object sender, GameTimerEventArgs e )
		{
			this.GraphicsDevice.Clear ( this.BackgroundColor );

#if T1
			this.spiritBatch.Begin ( );
			this.spiritBatch.DrawString ( this.myfont, "Hello!", new Vector2 ( 10, 10 ), Color.White );
			this.spiritBatch.End ( );
#endif
		}

	}

}
