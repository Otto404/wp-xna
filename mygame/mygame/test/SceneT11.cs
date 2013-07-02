using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT11
		: Scene
	{
		private readonly Label l1;

		internal SceneT11 ( )
			: base ( Vector2.Zero, GestureType.None,
			new Resource[] {
				new Resource ( "peg", ResourceType.Font, @"font\myfont" ),
				new Resource ( "scene.sound", ResourceType.Music, @"sound\music1" ),
			},
			new Making[] {
				new Label ( "l1", "I'm SceneT11!!!!!!", 2f, Color.White, 0f )
			}
			)
		{
			this.l1 = this.makings[ "l1" ] as Label;
		}

		protected override void drawing ( GameTime time, SpriteBatch batch )
		{
			base.drawing ( time, batch );

			Label.Draw ( this.l1, batch );
		}

	}

}
