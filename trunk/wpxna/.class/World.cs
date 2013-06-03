//#define T1
//#define T2
//#define T3
//#define T4
//#define T5
#define T6
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;

namespace zoyobar.game
{


	public partial class World
		: PhoneApplicationPage
	{

		internal static float TextureXScale = 1;
		internal static float TextureYScale = 1;
		internal static Vector2 TextureScale = new Vector2 ( TextureXScale, TextureYScale );

		internal static float XScale = 1;
		internal static float YScale = 1;
		internal static Vector2 Scale = new Vector2 ( XScale, YScale );
		internal static Vector2 FlipScale = new Vector2 ( YScale, XScale );

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

		#region " Example "
#if T1
		private SpriteFont myfont;
#endif

#if T2
		private readonly ResourceManager resourceManager;
#endif

#if T3
		private readonly ResourceManager resourceManager;
		private readonly Shape birdShape;
#endif

#if T4
		private readonly ResourceManager resourceManager;
		private readonly Movie bird2;
		private int bird2Angle = 0;
#endif

#if T5
		private readonly ResourceManager resourceManager;
		private readonly AudioManager audioManager;
		private int step = 1;
#endif

#if T6
		private readonly SingleRectangleHitArea hitArea1;
		private readonly SingleRectangleHitArea hitArea2;
		private int step = 1;
#endif
		#endregion

		public World ( )
			: this ( Color.Black )
		{ }
		public World ( Color backgroundColor )
			: base ( )
		{
			Calculator.Init ( );

			SharedGraphicsDeviceManager graph = SharedGraphicsDeviceManager.Current;
			this.GraphicsDevice = graph.GraphicsDevice;

			this.timer.UpdateInterval = TimeSpan.FromSeconds ( 1.0 / 30 );
			this.timer.Update += this.OnUpdate;
			this.timer.Draw += this.OnDraw;

			this.BackgroundColor = backgroundColor;

			PhoneApplicationService.Current.Activated += this.activate;
			PhoneApplicationService.Current.Deactivated += this.deactivate;

			#region " Example "
#if T2
			this.resourceManager = new ResourceManager ( new Resource[] {
				new Resource ( "bird", ResourceType.Image, @"image\bird" ),
				new Resource ( "click", ResourceType.Sound, @"sound\click" )
			} );
			this.resourceManager.World = this;
#endif

#if T3
			this.resourceManager = new ResourceManager ( new Resource[] {
				new Resource ( "bird", ResourceType.Image, @"image\bird" )
			} );
			this.resourceManager.World = this;

			this.birdShape = new Shape ( "shape.bird", "bird", new Vector2 ( 50, 50 ) );

			TextureXScale = 0.5f;
			TextureYScale = 0.5f;
			TextureScale = new Vector2 ( TextureXScale, TextureYScale );

			XScale = 0.5f;
			YScale = 0.5f;
			Scale = new Vector2 ( XScale, YScale );
			FlipScale = new Vector2 ( YScale, XScale );
#endif

#if T4
			this.resourceManager = new ResourceManager ( new Resource[] {
				new Resource ( "bird2.image", ResourceType.Image, @"image\bird2" )
			} );
			this.resourceManager.World = this;

			this.bird2 = new Movie ( "bird2.m", "bird2.image", new Vector2 ( 200, 200 ), 80, 80, 3, 0, "live",
				new MovieSequence ( "live", true, new Point ( 1, 1 ), new Point ( 2, 1 ) ),
				new MovieSequence ( "dead", 30, false, new Point ( 3, 1 ), new Point ( 4, 1 ) )
				);
			this.bird2.Ended += this.bird2MovieEnded;
#endif

#if T5
			this.resourceManager = new ResourceManager ( new Resource[] {
				new Resource ( "click.s", ResourceType.Sound, @"sound\click" ),
				new Resource ( "music1", ResourceType.Music, @"sound\music1" )
			} );
			this.resourceManager.World = this;

			this.audioManager = new AudioManager ( );
#endif

#if T6
			this.hitArea1 = new SingleRectangleHitArea ( new Rectangle ( 0, 0, 100, 100 ) );
			this.hitArea2 = new SingleRectangleHitArea ( new Rectangle ( 200, 200, 100, 100 ) );
#endif
			#endregion
		}

		#region " Example "
#if T4
		private void bird2MovieEnded ( object sender, MovieEventArgs e )
		{
			Debug.WriteLine ( "bird2MovieEnded: e.SequenceName=" + e.SequenceName );
		}
#endif
		#endregion

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

			#region " Example "
#if T1
			ContentManager content = new ContentManager ( this.Services, "Content" );
			this.myfont = content.Load<SpriteFont> ( @"font\myfont" );
#endif

#if T2
			this.resourceManager.LoadContent ( );
#endif

#if T3
			this.resourceManager.LoadContent ( );

			this.birdShape.InitResource ( this.resourceManager );
#endif

#if T4
			this.resourceManager.LoadContent ( );
			this.bird2.InitResource ( this.resourceManager );
#endif

#if T5
			this.resourceManager.LoadContent ( );
			this.audioManager.LoadContent ( this.resourceManager );
#endif
			#endregion

			base.OnNavigatedTo ( e );
		}

		protected override void OnNavigatedFrom ( NavigationEventArgs e )
		{

			#region " Example "
#if T4
			this.bird2.Ended -= this.bird2MovieEnded;
#endif
			#endregion

			base.OnNavigatedFrom ( e );
		}

		private void OnUpdate ( object sender, GameTimerEventArgs e )
		{

			#region " Example "
#if T2
			this.resourceManager.GetSound ( "click" ).Play ( );
#endif

#if T3
			this.birdShape.Location += new Vector2 ( 1f, 1f );
#endif

#if T4
			Movie.NextFrame ( this.bird2 );

			this.bird2.Rotation = this.bird2Angle++;

			if ( e.TotalTime.TotalSeconds > 5 && this.bird2.CurrentSequenceName == "live" )
				Movie.Play ( this.bird2, "dead" );

#endif

#if T5
			this.step++;

			if ( this.step <= 60 )
				this.audioManager.PlaySound ( "click.s" );
			else if ( this.step == 61 )
				this.audioManager.PlayMusic ( "music1" );
			else if ( this.step == 300 )
				this.audioManager.StopMusic ( );

#endif

#if T6
			this.step++;

			if ( this.step == 10 )
				Debug.WriteLine ( "Hit? {0}", this.hitArea1.HitTest ( this.hitArea2 ) );
			else if ( this.step == 20 )
			{
				this.hitArea1.Locate ( new Point ( 150, 150 ) );
				Debug.WriteLine ( "Hit? {0}", this.hitArea1.HitTest ( this.hitArea2 ) );
			}
			else if ( this.step == 30 )
				Debug.WriteLine ( "Hit? {0}", this.hitArea1.ContainTest ( 200, 200 ) );

#endif
			#endregion
		}

		private void OnDraw ( object sender, GameTimerEventArgs e )
		{
			this.GraphicsDevice.Clear ( this.BackgroundColor );

			#region " Example "
#if T1
			this.spiritBatch.Begin ( );
			this.spiritBatch.DrawString ( this.myfont, "Hello!", new Vector2 ( 10, 10 ), Color.White );
			this.spiritBatch.End ( );
#endif

#if T2
			this.spiritBatch.Begin ( );
			this.spiritBatch.Draw ( this.resourceManager.GetTexture ( "bird" ), new Vector2 ( 20, 20 ), Color.White );
			this.spiritBatch.End ( );
#endif

#if T3
			this.spiritBatch.Begin ( );
			Shape.Draw ( this.birdShape, new GameTime ( e.TotalTime, e.ElapsedTime ), this.spiritBatch );
			this.spiritBatch.End ( );
#endif

#if T4
			this.spiritBatch.Begin ( );
			Movie.Draw ( this.bird2, new GameTime ( e.TotalTime, e.ElapsedTime ), this.spiritBatch );
			this.spiritBatch.End ( );
#endif
			#endregion
		}

	}

}
