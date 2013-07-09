using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT14
		: CommandScene
	{

		internal class Bird
			: Spirit
		{

			internal Bird ( IScene scene, Vector2 location )
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

		private Bird bird;
		private readonly Button goButton;
		private readonly Button stopButton;

		internal SceneT14 ( )
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
			this.goButton = this.makings[ "b.go" ] as Button;
			this.stopButton = this.makings[ "b.play" ] as Button;

			this.goButton.Selected += this.goButtonSelected;
			this.stopButton.Selected += this.stopButtonSelected;
		}

		private void goButtonSelected ( object sender, ButtonEventArgs e )
		{ this.bird.Go ( ); }

		private void stopButtonSelected ( object sender, ButtonEventArgs e )
		{ this.bird.Stop ( ); }

		public override void LoadContent ( )
		{
			base.LoadContent ( );

			this.bird = new Bird ( this, new Vector2 ( 200, 100 ) );
			this.bird.LoadContent ( );

			this.world.Components.Add ( this.bird );
		}

		public override void UnloadContent ( )
		{
			this.world.Components.Remove ( this.bird );
			this.bird.Dispose ( );

			base.UnloadContent ( );
		}

	}

}
