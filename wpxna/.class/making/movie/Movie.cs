using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zoyobar.game
{

	internal class Movie
		: Shape
	{

		internal static void Play ( Movie movie, string sequenceName )
		{ Play ( movie, sequenceName, false, true ); }
		internal static void Play ( Movie movie, string sequenceName, bool isRecord )
		{ Play ( movie, sequenceName, isRecord, true ); }
		internal static void Play ( Movie movie, string sequenceName, bool isRecord, bool isReplay )
		{

			if ( !isReplay && movie.CurrentSequenceName == sequenceName )
				return;

			movie.CurrentSequenceName = sequenceName;
			if ( isRecord )
				movie.sequenceNames.Add ( sequenceName );

			if ( string.IsNullOrEmpty ( sequenceName ) || !movie.sequences.ContainsKey ( sequenceName ) )
			{
				movie.isVisible = false;
				return;
			}

			movie.isVisible = true;
			movie.currentSequence = movie.sequences[sequenceName];
			movie.currentRate = movie.currentSequence.Rate == 0 ? movie.rate : movie.currentSequence.Rate;
			movie.currentFrameCount = movie.currentRate;
			MovieSequence.Reset ( movie.currentSequence );

			movie.frameRectangle = new Rectangle ( ( int ) ( ( movie.currentSequence.CurrentFrame.X - 1 ) * movie.renderWidth ), ( int ) ( ( movie.currentSequence.CurrentFrame.Y - 1 ) * movie.renderHeight ), ( int ) movie.renderWidth, ( int ) movie.renderHeight );

		}

		internal static void Back ( Movie movie, string sequenceName )
		{
			int count = movie.sequenceNames.Count;

			if ( count <= 1 )
				return;

			if ( sequenceName == movie.sequenceNames[count - 1] )
			{
				movie.sequenceNames.RemoveAt ( count - 1 );
				Play ( movie, movie.sequenceNames[count - 2] );
			}
			else
				movie.sequenceNames.Remove ( sequenceName );

		}

		internal static void NextFrame ( Movie movie )
		{ NextFrame ( movie, false ); }
		internal static void NextFrame ( Movie movie, bool isForce )
		{

			if ( !movie.isVisible || ( !isForce && --movie.currentFrameCount > 0 ) )
				return;

			if ( movie.currentSequence.FrameCount <= 1 )
			{

				if ( null != movie.Ended )
					movie.Ended ( movie, new MovieEventArgs ( movie ) );

				return;
			}

			movie.currentFrameCount = movie.currentRate;

			if ( MovieSequence.Next ( movie.currentSequence ) )
				if ( null != movie.Ended )
					movie.Ended ( movie, new MovieEventArgs ( movie ) );

			movie.frameRectangle = new Rectangle ( ( int ) ( ( movie.currentSequence.CurrentFrame.X - 1 ) * movie.renderWidth ), ( int ) ( ( movie.currentSequence.CurrentFrame.Y - 1 ) * movie.renderHeight ), ( int ) ( movie.renderWidth ), ( int ) ( movie.renderHeight ) );
		}

		internal static void Draw ( Movie movie, GameTime time, SpriteBatch batch )
		{

			if ( !movie.isVisible )
				return;

			if ( movie.rotation == 0 )
				batch.Draw ( movie.Texture, movie.location * World.Scale, movie.frameRectangle, Color.White, 0, Vector2.Zero, World.TextureScale, SpriteEffects.None, movie.order );
			else
				batch.Draw ( movie.Texture, ( movie.location + movie.rotationLocation ) * World.Scale, movie.frameRectangle, Color.White, movie.rotation, movie.rotationLocation * World.Scale, World.TextureScale, SpriteEffects.None, movie.order );

		}

		internal static Movie Clone ( Movie movie )
		{
			return new Movie ( movie );
		}

		internal event EventHandler<MovieEventArgs> Ended;

		internal readonly int Width;
		internal readonly int Height;

		private readonly float renderWidth;
		private readonly float renderHeight;

		private readonly int rate;
		private int currentRate;
		private int currentFrameCount;
		private MovieSequence currentSequence;
		internal string CurrentSequenceName;
		private float rotation = 0;
		internal int Rotation
		{
			set
			{
				this.rotation = Calculator.Radian ( value );
			}
		}
		private Vector2 rotationLocation;
		private Rectangle frameRectangle;

		private readonly Dictionary<string, MovieSequence> sequences = new Dictionary<string, MovieSequence> ( );

		private readonly List<string> sequenceNames = new List<string> ( );

		internal Movie ( Movie movie )
			: base ( movie.Name, movie.resourceName, Vector2.Zero, movie.order )
		{
			this.Width = movie.Width;
			this.Height = movie.Height;
			this.renderWidth = movie.renderWidth;
			this.renderHeight = movie.renderHeight;
			this.rate = movie.rate;
			this.currentRate = movie.currentRate;
			this.currentFrameCount = movie.currentFrameCount;
			this.CurrentSequenceName = movie.CurrentSequenceName;

			foreach ( MovieSequence sequence in movie.sequences.Values )
				this.sequences.Add ( sequence.Name, MovieSequence.Clone ( sequence ) );

			this.rotation = movie.rotation;
			this.rotationLocation = movie.rotationLocation;
			this.frameRectangle = movie.frameRectangle;
			this.sequenceNames.AddRange ( movie.sequenceNames );
			this.isVisible = movie.isVisible;

			this.location = movie.location;
			this.Texture = movie.Texture;

			Movie.Play ( this, this.CurrentSequenceName, true );
		}

		internal Movie ( string name )
			: base ( name, "empty.m" )
		{ }
		internal Movie ( string name, string resourceName, Vector2 location, int width, int height, float order, string defaultSequenceName, params MovieSequence[] sequences )
			: this ( name, resourceName, location, width, height,
			0,
			order, defaultSequenceName, sequences )
		{ }
		internal Movie ( string name, string resourceName, int width, int height, int rate, string defaultSequenceName, params MovieSequence[] sequences )
			: this ( name, resourceName,
			Vector2.Zero,
			width, height, rate,
			0,
			defaultSequenceName, sequences )
		{ }
		internal Movie ( string name, string resourceName, int width, int height, int rate, float order, params MovieSequence[] sequences )
			: this ( name, resourceName,
			Vector2.Zero,
			width, height, rate, order,
			null,
			sequences )
		{ }
		internal Movie ( string name, string resourceName, int width, int height, int rate, float order, string defaultSequenceName, params MovieSequence[] sequences )
			: this ( name, resourceName,
			Vector2.Zero,
			width, height, rate, order, defaultSequenceName, sequences )
		{ }
		internal Movie ( string name, string resourceName, Vector2 location, int width, int height, int rate, float order, params MovieSequence[] sequences )
			: this ( name, resourceName, location, width, height, rate, order,
			null,
			sequences )
		{ }
		internal Movie ( string name, string resourceName, Vector2 location, int width, int height, int rate, float order, string defaultSequenceName, params MovieSequence[] sequences )
			: base ( name, resourceName, location, order )
		{

			if ( width <= 0 || height <= 0 || rate < 0 )
				throw new ArgumentException ( "width or height can't small than 1, rate can't small than 0", "width, height, rate" );

			if ( null != sequences )
				foreach ( MovieSequence sequence in sequences )
					if ( null != sequence )
						this.sequences.Add ( sequence.Name, sequence );

			if ( this.sequences.Count == 0 )
				throw new ArgumentException ( "sequences need a valid item", "sequences" );

			this.Width = width;
			this.Height = height;
			this.rotationLocation = new Vector2 ( this.Width / 2, this.Height / 2 );

			if ( World.TextureXScale == 1 )
				this.renderWidth = width * World.XScale;
			else
				this.renderWidth = width;

			if ( World.TextureYScale == 1 )
				this.renderHeight = height * World.YScale;
			else
				this.renderHeight = height;

			this.rate = rate;

			Movie.Play ( this, defaultSequenceName, true );
		}

		public override void Dispose ( )
		{
			this.sequenceNames.Clear ( );

			this.sequences.Clear ( );

			base.Dispose ( );
		}

	}

}
