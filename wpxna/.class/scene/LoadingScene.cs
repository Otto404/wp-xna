using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace zoyobar.game
{

	internal sealed class LoadingScene
		: Scene
	{
		internal event EventHandler<SceneLoaderEventArgs> Loaded;

		private readonly SceneLoader loader;
		private bool isLoading = false;
		private readonly Label loadingLabel;

		internal LoadingScene ( SceneLoader loader )
			: this ( loader,
			null
			)
		{ }
		internal LoadingScene ( SceneLoader loader, Label loadingLabel )
			: base ( Vector2.Zero, GestureType.None,
			new Resource[] {
				new Resource ( "peg", ResourceType.Font, @"font\peg" )
			},
			new Making[] {
				loadingLabel
			}
			)
		{

			if ( null == loader )
				throw new ArgumentNullException ( "loader", "loader can't be null" );

			this.loader = loader;
			this.loadingLabel = loadingLabel;
		}

		public override void LoadContent ( )
		{
			base.LoadContent ( );

			if ( null != this.loadingLabel )
			{
				Label.InitSize ( this.loadingLabel );

				if ( this.loadingLabel.Rotation == 0 )
					this.loadingLabel.Location = new Vector2 ( 50, 50 );
				else
					this.loadingLabel.Location = new Vector2 ( 50, 430 );

			}

		}

		private void loaded ( object sender, SceneLoaderEventArgs e )
		{
			this.Close ( );

			if ( null != this.Loaded )
				this.Loaded ( this, e );

		}

		protected override void updating ( GameTime time )
		{

			if ( this.isLoading )
				return;

			this.isLoading = true;

			if ( null != this.Loaded )
				this.loader.Loaded += this.loaded;

			this.loader.LoadResource ( );
		}

		protected override void drawing ( GameTime time, SpriteBatch batch )
		{

			if ( null != this.loadingLabel )
			{
				this.world.GraphicsDevice.Clear ( this.world.BackgroundColor );

				Label.Draw ( this.loadingLabel, batch );
			}

		}

		public override void Dispose ( )
		{
			this.loader.Loaded -= this.loaded;
			this.loader.Dispose ( );

			base.Dispose ( );
		}

	}

}
