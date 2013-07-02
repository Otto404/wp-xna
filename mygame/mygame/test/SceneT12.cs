using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT12
		: Scene
	{
		private readonly Anime bird1;
		private readonly Anime bird2;

		internal SceneT12 ( )
			: base ( Vector2.Zero, GestureType.None,
			new Resource[] {
				new Resource ( "bird2.image", ResourceType.Image, @"image\bird2" ),
			},
			new Making[] {
				new Anime ( "b1", "bird2.image", new Vector2 ( 100, 100 ), 80, 80, 5, "a",
					new MovieSequence[] { new MovieSequence ( "a", true, new Point ( 1, 1 ), new Point ( 2, 1 ) ) },
					new AnimeMovementAction ( 4, 0, new Rectangle ( -80, 0, 480, 0 ) )
					),
				new Anime ( "b2", "bird2.image", new Vector2 ( 300, 300 ), 80, 80, 5, "a",
					new MovieSequence[] { new MovieSequence ( "a", true, new Point ( 2, 1 ), new Point ( 3, 1 ) ) },
					new AnimeMovementAction ( 0, 2, 0, 2 )
					),
			}
			)
		{
			this.bird1 = this.makings[ "b1" ] as Anime;
			this.bird2 = this.makings[ "b2" ] as Anime;
		}

		protected override void drawing ( GameTime time, SpriteBatch batch )
		{
			base.drawing ( time, batch );

			Anime.Draw ( this.bird1, time, batch );
			Anime.Draw ( this.bird2, time, batch );
		}

		protected override void updating ( GameTime time )
		{
			this.bird1.Update ( time );
			this.bird2.Update ( time );

			base.updating ( time );
		}

	}

}
