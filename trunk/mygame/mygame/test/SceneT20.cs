using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Collections.Generic;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT20
		: CommandScene, IPlayScene
	{

		#region " Bird "
		internal class Bird
			: Spirit, IAssailable
		{
			internal int Life = 10;
			internal bool IsDied = false;

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

			public bool Injure ( int life, int type )
			{
				this.Life -= life;
				Debug.WriteLine ( "life={0}", this.Life );

				if ( this.Life <= 0 )
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

			internal MyBullet ( IPlayScene scene, Vector2 location, int angle )
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

		#region " MyItem "
		internal class MyItem
			: Item
		{

			internal MyItem ( IPlayScene scene, Vector2 location, int angle )
				: base ( scene, 1, location, "myitem", 5, angle,
				new SingleRectangleHitArea ( new Rectangle ( -15, -15, 30, 30 ) ),
				30, 30,
				0,
				true,
				true,
				0,
				true
				)
			{ }

		}
		#endregion

		#region " MyHit "
		internal class MyHit
			: Pinup
		{

			internal MyHit ( IPlayScene scene, Vector2 location )
				: base ( scene, 1, location, "mypinup", 0, 0,
				30, 30,
				1
				)
			{ }

		}
		#endregion

		#region " MyRegion "
		internal class MyRegion
			: Region
		{

			internal MyRegion ( IPlayScene scene, Vector2 location )
				: base ( scene, 1, location, "myr", 0, 0,
				new SingleRectangleHitArea ( new Rectangle ( -75, -75, 150, 150 ) ),
				150, 150,
				10
				)
			{ }

			protected override bool appending ( Spirit spirit )
			{
				spirit.Destroy ( );

				return false;
			}

		}
		#endregion

		internal class MyNPC
			: NPC
		{

			internal MyNPC ( IPlayScene scene, Vector2 location, int angle, IList<NPCAction> actions )
				: base ( scene, 1, location,
				"mynpc", null,
				3f, angle,
				new SingleRectangleHitArea ( new Rectangle ( -20, -20, 40, 40 ) ),
				40, 40,
				1,
				actions,
				0, true, true, true, 0 )
			{ this.isMoving = true; }

			protected override void execute ( NPCAction action )
			{
				MyNPCAction myAction = action as MyNPCAction;

				this.Angle += myAction.OffsetAngle;
			}

			protected override void updateSpeed ( )
			{
				this.xSpeed = Calculator.Cos ( this.angle ) * this.speed;
				this.ySpeed = Calculator.Sin ( this.angle ) * this.speed;

				base.updateSpeed ( );
			}

			protected override void move ( )
			{
				this.Location.X += this.xSpeed;
				this.Location.Y += this.ySpeed;
			}

		}

		internal class MyNPCAction
			: NPCAction
		{
			internal readonly int OffsetAngle;

			internal MyNPCAction ( MyNPCAction action )
				: base ( action )
			{
				this.OffsetAngle = action.OffsetAngle;
			}
			internal MyNPCAction ( int offsetAngle )
				: base ( 1, 0f, 0.5f, 10 )
			{ this.OffsetAngle = offsetAngle; }

			internal override NPCAction Clone ( )
			{ return new MyNPCAction ( this ); }

		}

		private Bird bird;
		private BirdManager birdManager;
		private BulletManager bulletManager;
		private ItemManager itemManager;
		private PinupManager pinupManager;
		private RegionManager regionManager;
		private NPCManager npcManager;
		private readonly Button goButton;

		private bool is1Hit = false;

		internal SceneT20 ( )
			: base ( Vector2.Zero, GestureType.None, "background1",
			new Resource[] {
				new Resource ( "bird2.image", ResourceType.Image, @"image\bird2" ),
				new Resource ( "bullet.image", ResourceType.Image, @"image\bullet" ),
				new Resource ( "item.image", ResourceType.Image, @"image\item" ),
				new Resource ( "pinup.image", ResourceType.Image, @"image\pinup1" ),
				new Resource ( "r.image", ResourceType.Image, @"image\r1" ),
				new Resource ( "npc.image", ResourceType.Image, @"image\npc" ),
				new Resource ( "go.image", ResourceType.Image, @"image\button1" ),
			},
			new Making[] {
				new Movie ( "bird", "bird2.image", 80, 80, 5, "live",
					new MovieSequence ( "live", true, new Point ( 1, 1 ), new Point ( 2, 1 ) )
					),
				new Movie ( "mybutton", "bullet.image", 10, 10, 0, "b",
					new MovieSequence ( "b", new Point ( 1, 1 ) )
					),
				new Movie ( "myitem", "item.image", 30, 30, 0, "i",
					new MovieSequence ( "i", new Point ( 1, 1 ) )
					),
				new Movie ( "mypinup", "pinup.image", 100, 50, 0, "p",
					new MovieSequence ( "p", new Point ( 1, 1 ) )
					),
				new Movie ( "myr", "r.image", 150, 150, 0, "r",
					new MovieSequence ( "r", new Point ( 1, 1 ) )
					),
				new Movie ( "mynpc", "npc.image", 40, 40, 0, "n",
					new MovieSequence ( "n", new Point ( 1, 1 ) )
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

			this.itemManager = new ItemManager ( );
			this.itemManager.Scene = this;
			this.itemManager.HitTesting += this.itemHitTesting;
			this.itemManager.Picked += this.itemPicked;

			this.pinupManager = new PinupManager ( );
			this.pinupManager.Scene = this;

			this.regionManager = new RegionManager ( );
			this.regionManager.Scene = this;
			this.regionManager.HitTesting += this.regionHitTesting;

			this.npcManager = new NPCManager ( );
			this.npcManager.Scene = this;

			this.goButton = this.makings[ "b.go" ] as Button;

			this.goButton.Selected += this.goButtonSelected;
		}

		private void regionHitTesting ( object sender, RegionHitAreaEventArgs e )
		{
			List<Spirit> targets = new List<Spirit> ( );

			foreach ( Bullet bullet in this.bulletManager.Spirits )
				if ( e.HitArea.HitTest ( bullet.HitArea ) )
					targets.Add ( bullet );

			e.Targets = targets;
		}

		private void itemHitTesting ( object sender, HitAreaEventArgs e )
		{

			if ( !this.bird.IsDied && e.HitArea.HitTest ( this.bird.HitArea ) )
				e.IsHit = true;

		}

		private void itemPicked ( object sender, SpiritEventArgs e )
		{
			this.bird.Life++;
			Debug.WriteLine ( "item picked, life={0}", this.bird.Life );
		}

		private void bulletHitTesting ( object sender, BulletHitAreaEventArgs e )
		{

			if ( !this.bird.IsDied && e.HitArea.HitTest ( this.bird.HitArea ) )
			{
				e.IsHit = true;
				e.Targets = new IAssailable[] { this.bird };

				if ( !this.is1Hit )
				{
					this.is1Hit = true;
					this.pinupManager.Append ( new MyHit ( this, new Vector2 ( 0, 400 ) ) );
				}

				this.regionManager.Append ( new MyRegion ( this, new Vector2 ( 100, 100 ) ) );
			}

		}

		private void goButtonSelected ( object sender, ButtonEventArgs e )
		{
			this.bulletManager.Append ( new MyBullet ( this, new Vector2 ( 10, 10 ), 45 ) );
			this.itemManager.Append ( new MyItem ( this, new Vector2 ( 420, 30 ), 135 ) );

			this.npcManager.Append ( new MyNPC ( this, new Vector2 ( 420, 420 ), 100,
				new NPCAction[] {
				new MyNPCAction ( 20 )
				}
				) );
		}

		protected override void updating ( GameTime time )
		{
			this.bulletManager.Update ( time );
			this.itemManager.Update ( time );
			this.regionManager.Update ( time );
			this.npcManager.Update ( time );

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
			this.itemManager.RemoveAll ( );
			this.pinupManager.RemoveAll ( );
			this.regionManager.RemoveAll ( );
			this.npcManager.RemoveAll ( );

			base.UnloadContent ( );
		}

		public override void Dispose ( )
		{
			this.birdManager.Dispose ( );

			this.bulletManager.HitTesting -= this.bulletHitTesting;
			this.bulletManager.Dispose ( );

			this.itemManager.HitTesting -= this.itemHitTesting;
			this.itemManager.Picked -= this.itemPicked;
			this.itemManager.Dispose ( );

			this.pinupManager.Dispose ( );
			this.npcManager.Dispose ( );

			this.goButton.Selected -= this.goButtonSelected;

			base.Dispose ( );
		}

	}

}