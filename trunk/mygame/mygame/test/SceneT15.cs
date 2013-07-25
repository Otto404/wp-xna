using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT15
		: CommandScene, IPlayScene
	{

		#region " Bird "
		internal class Bird
			: Spirit
		{

			internal Bird ( IPlayScene scene, Vector2 location )
				: base ( scene, 0, location,
				"bird", null,
				4, 0,
				new SingleRectangleHitArea ( new Rectangle ( -40, -40, 80, 80 ) ),
				80,
				80,
				0,
				true,
				false,
				false,
				0
				)
			{ }

			protected override void updateSpeed ( )
			{
				this.xSpeed = this.speed;
				this.ySpeed = this.speed;

				base.updateSpeed ( );
			}

			protected override void move ( )
			{
				this.Location.X += this.xSpeed;
				this.Location.Y += this.ySpeed;
			}

			internal void Go ( )
			{
				this.isMoving = true;
				this.PlayMovie ( "go" );
			}

			internal void Stop ( )
			{
				this.isMoving = false;
				this.PlayMovie ( "stop" );
			}

		}
		#endregion

		internal class BirdManager
			: SpiritManager<Bird>
		{

			internal BirdManager ( )
				: base ( )
			{ }

			internal void Go ( )
			{

				foreach ( Bird bird in this.Spirits )
					bird.Go ( );

			}

			internal void Stop ( )
			{

				foreach ( Bird bird in this.Spirits )
					bird.Stop ( );

			}

		}

		private BirdManager birdManager;
		private readonly Button goButton;
		private readonly Button stopButton;

		internal SceneT15 ( )
			: base ( Vector2.Zero, GestureType.None, "background1",
			new Resource[] {
				new Resource ( "bird2.image", ResourceType.Image, @"image\bird2" ),
				new Resource ( "go.image", ResourceType.Image, @"image\button1" ),
				new Resource ( "stop.image", ResourceType.Image, @"image\button2" ),
			},
			new Making[] {
				new Movie ( "bird", "bird2.image", 80, 80, 5, "stop",
					new MovieSequence ( "go", true, new Point ( 1, 1 ), new Point ( 2, 1 ) ),
					new MovieSequence ( "stop", true, new Point ( 3, 1 ) )
					),
				new Button ( "b.go", "go.image", "GO", new Vector2 ( 100, 100 ), 100, 50, new Point ( 1, 1 ) ),
				new Button ( "b.play", "stop.image", "STOP", new Vector2 ( 100, 300 ), 100, 50, new Point ( 1, 1 ) )
			}
			)
		{
			this.birdManager = new BirdManager ( );
			this.birdManager.Scene = this;

			this.goButton = this.makings[ "b.go" ] as Button;
			this.stopButton = this.makings[ "b.play" ] as Button;

			this.goButton.Selected += this.goButtonSelected;
			this.stopButton.Selected += this.stopButtonSelected;
		}

		private void goButtonSelected ( object sender, ButtonEventArgs e )
		{ this.birdManager.Go ( ); }

		private void stopButtonSelected ( object sender, ButtonEventArgs e )
		{ this.birdManager.Stop ( ); }

		public override void LoadContent ( )
		{
			base.LoadContent ( );

			this.birdManager.Append ( new Bird ( this, new Vector2 ( 200, 100 ) ) );
			this.birdManager.Append ( new Bird ( this, new Vector2 ( 300, 200 ) ) );
		}

		public override void UnloadContent ( )
		{
			this.birdManager.RemoveAll ( );

			base.UnloadContent ( );
		}

		public override void Dispose ( )
		{
			this.birdManager.Dispose ( );

			base.Dispose ( );
		}

	}

}
