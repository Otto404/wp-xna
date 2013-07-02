using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace zoyobar.game
{

	internal sealed class Anime
		: Movie
	{

		internal static void Draw ( Anime anime, GameTime time, SpriteBatch batch )
		{ Movie.Draw ( anime, time, batch ); }

		private readonly List<AnimeAction> actions = new List<AnimeAction> ( );

		internal Anime ( string name, string resourceName, Vector2 location, int width, int height, params AnimeAction[] actions )
			: this ( name, resourceName, location, width, height,
			0,
			"a",
			new MovieSequence[] { new MovieSequence ( "a", new Point ( 1, 1 ) ) },
			actions
			)
		{ }
		internal Anime ( string name, string resourceName, Vector2 location, int width, int height, int rate, string defaultSequenceName, MovieSequence[] sequences, params AnimeAction[] actions )
			: base ( name, resourceName, location, width, height, rate,
			0f,
			defaultSequenceName, sequences )
		{

			if ( null != actions )
				foreach ( AnimeAction action in actions )
					if ( null != action )
					{
						action.Anime = this;
						this.actions.Add ( action );
					}

		}

		internal void Update ( GameTime time )
		{

			foreach ( AnimeAction action in this.actions )
				action.Update ( time );

			Movie.NextFrame ( this );
		}

		public override void Dispose ( )
		{
			this.actions.Clear ( );

			base.Dispose ( );
		}

	}

}
