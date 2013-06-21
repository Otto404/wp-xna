using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT9
		: Scene
	{
		private readonly Label l1;
		private readonly Movie bird2;

		internal SceneT9 ( )
			: base ( Vector2.Zero, GestureType.Tap | GestureType.DoubleTap,
			new Resource[] {
				new Resource ( "bird2.image", ResourceType.Image, @"image\bird2" ),
				new Resource ( "click.s", ResourceType.Sound, @"sound\click" ),
				new Resource ( "peg", ResourceType.Font, @"font\myfont" ),
			},
			new Making[] {
				new Movie ( "bird2.m", "bird2.image", new Vector2 ( 200, 200 ), 80, 80, 3, 0, "live",
					new MovieSequence ( "live", true, new Point ( 1, 1 ), new Point ( 2, 1 ) ),
					new MovieSequence ( "dead", 30, false, new Point ( 3, 1 ), new Point ( 4, 1 ) )
				),
				new Label ( "l1", "Hello windows phone!", 2f, Color.LightGreen, 0f )
			}
			)
		{
			this.l1 = this.makings[ "l1" ] as Label;
			this.bird2 = this.makings[ "bird2.m" ] as Movie;
		}

		protected override void inputing ( Controller controller )
		{

			if ( !controller.IsGestureEmpty && controller.Gestures[ 0 ].GestureType == GestureType.Tap )
				this.audioManager.PlaySound ( "click.s" );

		}

		protected override void updating ( GameTime time )
		{
			Movie.NextFrame ( this.bird2 );

			base.updating ( time );
		}

		protected override void drawing ( GameTime time, SpriteBatch batch )
		{
			base.drawing ( time, batch );

			Label.Draw ( this.l1, batch );
			Movie.Draw ( this.bird2, time, batch );
		}

	}

}
