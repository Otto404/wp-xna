//#define T1
//#define T2
//#define T3
//#define T4
//#define T5
//#define T6
//#define T7
//#define T8
//#define T9
#define T10
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
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

		private readonly Controller controller;
		private bool isInitialized = false;
		private readonly List<Scene> scenes = new List<Scene> ( );

		private readonly GameTimer timer = new GameTimer ( );

		private bool isPreserved = false;

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

#if T7
		private readonly ResourceManager resourceManager;
		private readonly Label label1;
		private readonly Label label2;
#endif
		#endregion

		public World ( )
			: this ( Color.Black )
		{ }
		public World ( Color backgroundColor )
			: base ( )
		{
			this.controller = new Controller ( );

			Calculator.Init ( );

			SharedGraphicsDeviceManager graph = SharedGraphicsDeviceManager.Current;
			this.GraphicsDevice = graph.GraphicsDevice;

			this.timer.UpdateInterval = TimeSpan.FromSeconds ( 1.0 / 30 );
			this.timer.Update += this.OnUpdate;
			this.timer.Draw += this.OnDraw;

			this.BackgroundColor = backgroundColor;

			TouchPanel.EnabledGestures = GestureType.None;

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

#if T7
			this.resourceManager = new ResourceManager ( new Resource[] {
				new Resource ( "peg", ResourceType.Font, @"font\myfont" )
			} );
			this.resourceManager.World = this;

			this.label1 = new Label ( "l1", "Hello windows phone!", 2f, Color.LightGreen, 0f );
			this.label2 = new Label ( "l2", "peg", "Nothing!", new Vector2 ( 50, 300 ), 0, 0, 1f, Color.White, -0.01f, 1f, -90 );
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
		{ this.isPreserved = e.IsApplicationInstancePreserved; }

		private void deactivate ( object sender, DeactivatedEventArgs e )
		{ }

		protected override void OnNavigatedTo ( NavigationEventArgs e )
		{

			if ( this.isPreserved )
				return;

			SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode ( true );
			this.spiritBatch = new SpriteBatch ( this.GraphicsDevice );
			this.Services.AddService ( typeof ( SpriteBatch ), this.spiritBatch );
			this.isInitialized = true;

			foreach ( Scene scene in this.scenes )
				scene.LoadContent ( );

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

#if T7
			this.resourceManager.LoadContent ( );
			this.label1.InitResource ( this.resourceManager );
			this.label2.InitResource ( this.resourceManager );
#endif

#if T9
			this.appendScene ( new mygame.test.SceneT9 ( ), null, false );
#endif

#if T10
			this.appendScene ( new mygame.test.SceneT10 ( ), null, false );
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

			if ( !this.IsEnabled )
				return;

			this.controller.Update ( );

			Scene[] scenes = this.scenes.ToArray ( );
			GameTime time = new GameTime ( e.TotalTime, e.ElapsedTime );

			foreach ( Scene scene in scenes )
				if ( null != scene )
					scene.Update ( time );

			for ( int index = scenes.Length - 1; index >= 0; index-- )
				if ( null != scenes[ index ] && scenes[ index ].Input ( this.controller ) )
					break;

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

#if T7
			this.label2.Text = e.TotalTime.ToString ( );
#endif

#if T8

			if ( !this.controller.IsGestureEmpty )
			{
				Debug.WriteLine ( "Gestures {0}", this.controller.Gestures.Count );

				foreach ( GestureSample gesture in this.controller.Gestures )
					Debug.WriteLine ( "Gesture {0}, {1}, {2}, {3}", gesture.Position, gesture.Position2, gesture.Delta, gesture.Delta2 );

			}

			if ( !this.controller.IsMotionEmpty )
			{

				Debug.WriteLine ( "Motions {0}", this.controller.Motions.Count );

				foreach ( Motion motion in this.controller.Motions )
					Debug.WriteLine ( "Motion {0}, {1}, {2}", motion.Position, motion.Offset, motion.Type );

			}

#endif
			#endregion
		}

		private void OnDraw ( object sender, GameTimerEventArgs e )
		{
			this.GraphicsDevice.Clear ( this.BackgroundColor );

			bool isBroken = false;
			GameTime time = new GameTime ( e.TotalTime, e.ElapsedTime );

			foreach ( Scene scene in this.scenes.ToArray ( ) )
				if ( null != scene )
				{

					if ( !isBroken && scene.IsBroken )
					{
						// Draw sprites
						isBroken = true;
					}

					scene.Draw ( time );
				}

			if ( !isBroken )
				// Draw sprites.
				;

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

#if T7
			this.spiritBatch.Begin ( );
			Label.Draw ( this.label1, this.spiritBatch );
			Label.Draw ( this.label2, this.spiritBatch );
			this.spiritBatch.End ( );
#endif
			#endregion
		}

		private void appendScene ( Scene scene, Type afterSceneType, bool isInitialized )
		{

			if ( null == scene )
				return;

			//this.sceneAppending ( scene );

			if ( !isInitialized )
			{
				scene.World = this;
				scene.IsClosed = false;

				if ( this.isInitialized )
					scene.LoadContent ( );

			}

			int index = this.getSceneIndex ( afterSceneType );

			if ( index < 0 )
				this.scenes.Add ( scene );
			else
				this.scenes.Insert ( index, scene );

			TouchPanel.EnabledGestures = scene.GestureType;
		}

		internal void RemoveScene ( Scene scene )
		{

			if ( null == scene || !this.scenes.Contains ( scene ) )
				return;

			//this.sceneRemoving ( scene );

			scene.IsClosed = true;

			if ( this.isInitialized )
				try
				{

					if ( null != scene )
						scene.Dispose ( );

				}
				catch
				{ scene.Dispose ( ); }

			this.scenes.Remove ( scene );

			if ( this.scenes.Count > 0 )
				TouchPanel.EnabledGestures = this.scenes[ this.scenes.Count - 1 ].GestureType;

		}

		private int getSceneIndex ( Type sceneType )
		{

			if ( null == sceneType )
				return -1;

			string type = sceneType.ToString ( );

			for ( int index = this.scenes.Count - 1; index >= 0; index-- )
				if ( this.scenes[ index ].GetType ( ).ToString ( ) == type )
					return index;

			return -1;
		}

		private T getScene<T> ( )
			where T : Scene
		{
			int index = this.getSceneIndex ( typeof ( T ) );

			return index == -1 ? null : this.scenes[ index ] as T;
		}

	}

}
