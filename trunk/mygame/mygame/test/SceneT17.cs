using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT17
		: CommandScene
	{

		#region " Bird "
		internal class Bird
			: Spirit, IAssailable
		{
			private int life = 10;
			internal bool IsDied = false;

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

			public bool Injure ( int life, int type )
			{
				this.life -= life;
				Debug.WriteLine ( "life={0}", this.life );

				if ( this.life <= 0 )
				{
					Debug.WriteLine ( "life<0" );
					this.IsDied = true;
					this.Destroy ( );
				}

				return true;
			}

			protected override Vector2 getMovieLocation ( )
			{ return this.Location - this.halfSize; }

		}
		#endregion

		#region " BirdManager "
		internal class BirdManager
			: SpiritManager<Bird>
		{

			internal BirdManager ( )
				: base ( )
			{ }

		}
		#endregion

		#region " MyBullet "
		internal class MyBullet
			: Bullet
		{

			internal MyBullet ( IScene scene, Vector2 location, int angle )
				: base ( scene, 100, location, "mybutton", 5, angle,
				new SingleRectangleHitArea ( new Rectangle ( -5, -5, 10, 10 ) ),
				10, 10,
				2, 2,
				0,
				false,
				true,
				true,
				0
				)
			{ }

			protected override void updateSpeed ( )
			{
				this.xSpeed = Calculator.Cos ( this.angle ) * this.speed;
				this.ySpeed = Calculator.Sin ( this.angle ) * this.speed;

				base.updateSpeed ( );
			}

		}
		#endregion

		private Bird bird;
		private BirdManager birdManager;
		private BulletManager bulletManager;
		private readonly Button goButton;

		internal SceneT17 ( )
			: base ( Vector2.Zero, GestureType.None, "background1",
			new Resource[] {
				new Resource ( "bird2.image", ResourceType.Image, @"image\bird2" ),
				new Resource ( "bullet.image", ResourceType.Image, @"image\bullet" ),
				new Resource ( "go.image", ResourceType.Image, @"image\button1" ),
			},
			new Making[] {
				new Movie ( "bird", "bird2.image", 80, 80, 5, "live",
					new MovieSequence ( "live", true, new Point ( 1, 1 ), new Point ( 2, 1 ) )
					),
				new Movie ( "mybutton", "bullet.image", 10, 10, 0, "b",
					new MovieSequence ( "b", new Point ( 1, 1 ) )
					),
				new Button ( "b.go", "go.image", "GO", new Vector2 ( 10, 690 ), 100, 50, new Point ( 1, 1 ) ),
			}
			)
		{
			this.birdManager = new BirdManager ( );
			this.birdManager.Scene = this;

			this.bulletManager = new BulletManager ( );
			this.bulletManager.Scene = this;
			this.bulletManager.HitTesting += this.bulletHitTesting;

			this.goButton = this.makings[ "b.go" ] as Button;

			this.goButton.Selected += this.goButtonSelected;
		}

		private void bulletHitTesting ( object sender, BulletHitAreaEventArgs e )
		{

			if ( !this.bird.IsDied && e.HitArea.HitTest ( this.bird.HitArea ) )
			{
				e.IsHit = true;
				e.Targets = new IAssailable[] { this.bird };
			}

		}

		private void goButtonSelected ( object sender, ButtonEventArgs e )
		{ this.bulletManager.Append ( new MyBullet ( this, new Vector2 ( 10, 10 ), 45 ) ); }

		protected override void updating ( GameTime time )
		{
			this.bulletManager.Update ( time );

			base.updating ( time );
		}

		public override void LoadContent ( )
		{
			base.LoadContent ( );

			this.bird = new Bird ( this, new Vector2 ( 200, 200 ) );
			this.birdManager.Append ( this.bird );
		}

		public override void UnloadContent ( )
		{
			this.birdManager.RemoveAll ( );
			this.bulletManager.RemoveAll ( );

			base.UnloadContent ( );
		}

		public override void Dispose ( )
		{
			this.birdManager.Dispose ( );

			this.bulletManager.HitTesting -= this.bulletHitTesting;
			this.bulletManager.Dispose ( );

			this.goButton.Selected -= this.goButtonSelected;

			base.Dispose ( );
		}

	}

}