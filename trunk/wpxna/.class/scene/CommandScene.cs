using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace zoyobar.game
{

	internal class CommandScene
		: Scene
	{
		internal event EventHandler<SceneEventArgs> Executing;

		private readonly Shape backgroundShape;

		protected readonly List<Button> buttons = new List<Button> ( );
		private readonly List<Anime> animes = new List<Anime> ( );

		internal CommandScene ( Vector2 location, GestureType gestureType, string backgroundResourcePath )
			: this ( location, gestureType, backgroundResourcePath,
			null,
			null,
			false
			)
		{ }
		internal CommandScene ( Vector2 location, GestureType gestureType, string backgroundResourcePath, IList<Resource> resources, IList<Making> makings )
			: this ( location, gestureType, backgroundResourcePath, resources, makings,
			false
			)
		{ }
		internal CommandScene ( Vector2 location, GestureType gestureType, string backgroundResourcePath, IList<Resource> resources, IList<Making> makings, bool isBroken )
			: base ( location, gestureType,
			ResourceManager.Combine ( new Resource[] {
				new Resource ( "peg", ResourceType.Font, @"font\peg" ),
				new Resource ( "background", ResourceType.Image, string.Format ( @"image\{0}", backgroundResourcePath ) ),
				new Resource ( "click.s", ResourceType.Sound, @"sound\click" ),
			}, resources ),
			combine ( new Making[] {
				new Shape ( "background.s", "background" )
			}, makings ),
			isBroken
			)
		{
			this.backgroundShape = this.makings["background.s"] as Shape;

			foreach ( Making making in this.makings.Values )
				if ( making is Button )
				{
					Button button = making as Button;
					button.Selected += this.buttonSelected;
					this.buttons.Add ( button );
				}
				else if ( making is Anime )
					this.animes.Add ( making as Anime );

		}

		private void buttonSelected ( object sender, ButtonEventArgs e )
		{ this.Execute ( e.Command ); }

		protected override void drawing ( GameTime time, SpriteBatch batch )
		{
			Shape.Draw ( this.backgroundShape, time, batch );

			foreach ( Anime anime in this.animes )
				Anime.Draw ( anime, time, batch );

			foreach ( Button button in this.buttons )
				button.Draw ( batch );

		}

		protected override void updating ( GameTime time )
		{

			foreach ( Anime anime in this.animes )
				anime.Update ( time );

		}

		protected override void inputing ( Controller controller )
		{

			foreach ( Button button in this.buttons )
				Button.PressTest ( button, controller.Motions );

			foreach ( Button button in this.buttons )
				if ( Button.ClickTest ( button, controller.Motions ) && button.IsSole )
					break;

		}

		internal void Execute ( string command )
		{

			if ( null != this.Executing )
				this.Executing ( this, new SceneEventArgs ( command ) );

		}

		public override void Dispose ( )
		{

			foreach ( Button button in this.buttons )
				button.Selected -= this.buttonSelected;

			this.buttons.Clear ( );
			this.animes.Clear ( );

			base.Dispose ( );
		}

	}

}
