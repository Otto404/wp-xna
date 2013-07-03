using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT13
		: CommandScene
	{

		internal SceneT13 ( )
			: base ( Vector2.Zero, GestureType.None, "background1",
			new Resource[] {
				new Resource ( "play.image", ResourceType.Image, @"image\button1" ),
				new Resource ( "stop.image", ResourceType.Image, @"image\button2" ),
			},
			new Making[] {
				new Button ( "b.play", "play.image", "PLAY", new Vector2 ( 100, 100 ), 100, 50, new Point ( 1, 1 ) ),
				new Button ( "s.play", "stop.image", "STOP", new Vector2 ( 100, 300 ), 100, 50, new Point ( 1, 1 ) )
			}
			)
		{ }

	}

}
