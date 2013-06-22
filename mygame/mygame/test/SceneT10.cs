using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using zoyobar.game;

namespace mygame.test
{

	internal sealed class SceneT10
		: Scene
	{
		private readonly Button buttonPlay;

		internal SceneT10 ( )
			: base ( Vector2.Zero, GestureType.None,
			new Resource[] {
				new Resource ( "play.image", ResourceType.Image, @"image\button1" ),
				new Resource ( "click.s", ResourceType.Sound, @"sound\click" ),
			},
			new Making[] {
				new Button ( "b.play", "play.image", "PLAY", new Vector2 ( 100, 100 ), 100, 50, new Point ( 1, 1 ) )
			}
			)
		{
			this.buttonPlay = this.makings[ "b.play" ] as Button;
			//this.buttonPlay.IsEnabled = false;

			this.buttonPlay.Pressing += this.buttonPlayPressing;
			this.buttonPlay.Pressed += this.buttonPlayPressed;
		}

		protected override void inputing ( Controller controller )
		{
			base.inputing ( controller );

			Button.PressTest ( this.buttonPlay, controller.Motions );
		}

		protected override void drawing ( GameTime time, SpriteBatch batch )
		{
			base.drawing ( time, batch );

			this.buttonPlay.Draw ( batch );
		}

		private void buttonPlayPressing ( object sender, ButtonEventArgs e )
		{ Debug.WriteLine ( "play button pressing" ); }
		private void buttonPlayPressed ( object sender, ButtonEventArgs e )
		{ Debug.WriteLine ( "play button pressed" ); }

		public override void Dispose ( )
		{
			this.buttonPlay.Pressing -= this.buttonPlayPressing;
			this.buttonPlay.Pressed -= this.buttonPlayPressed;

			base.Dispose ( );
		}

	}

}
