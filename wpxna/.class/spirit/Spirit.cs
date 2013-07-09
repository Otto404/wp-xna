using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zoyobar.game
{

	internal abstract class Spirit
		: ISpirit, IDisposable
	{
		internal event EventHandler<SpiritEventArgs> Destroyed;
		internal event EventHandler<SpiritEventArgs> DrawOrderChanged;

		//protected readonly IPlayScene scene;
		protected readonly IScene scene;
		private readonly World world;
		protected readonly AudioManager audioManager;

		protected readonly Movie movie;
		private readonly string movieName;
		protected readonly Movie extendMovie;
		private readonly string extendMovieName;
		protected int type;
		internal virtual int Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		internal readonly int Width;
		internal readonly int Height;
		protected readonly Vector2 halfSize;

		internal Vector2 Location;
		internal readonly HitArea HitArea;

		protected int angle;
		internal virtual int Angle
		{
			get { return this.angle; }
			set
			{

				if ( !this.isRotable )
					return;

				value = Calculator.Degree ( value );

				this.angle = value;

				if ( this.isMovieRotable )
				{
					this.movie.Rotation = value;

					if ( null != this.extendMovie )
						this.extendMovie.Rotation = value;

				}

				this.updateSpeed ( );
			}
		}

		private float protoSpeed;
		private float protoXSpeed;
		private float protoYSpeed;
		protected float speed;
		protected float xSpeed;
		protected float ySpeed;
		public virtual float Speed
		{
			get { return this.speed; }
			set
			{
				this.speed = value;
				this.protoSpeed = this.speed;
				this.updateSpeed ( );
			}
		}

		private SpriteBatch spiritBatch;

		protected bool isMoving = false;
		protected bool isMovable = true;
		protected bool isRotable = true;
		private readonly bool isMovieRotable;

		private long destroyFrameCount;

		private readonly bool isAreaLimited;
		protected bool isAreaEntered;

		private long areaFrameCount;

		private int drawOrder = 0;
		internal int DrawOrder
		{
			get { return this.drawOrder; }
			set
			{
				this.drawOrder = value;

				if ( null != this.DrawOrderChanged )
					this.DrawOrderChanged ( this, new SpiritEventArgs ( this ) );

			}
		}

		internal bool IsVisible = true;

		protected Spirit ( IScene scene, int type, Vector2 location, string movieName, string extendMovieName, float speed, int angle, HitArea hitArea, int width, int height, double destroySecond, bool isMovieRotable, bool isAreaLimited, bool isAreaEntered, double areaSecond )
		{

			if ( null == scene || string.IsNullOrEmpty ( movieName ) )
				throw new ArgumentNullException ( "scene, movieName", "scene, movieName can't be null" );

			this.destroyFrameCount = World.ToFrameCount ( destroySecond );

			this.scene = scene;
			this.world = scene.World;
			this.audioManager = scene.AudioManager;

			this.isMovieRotable = isMovieRotable;
			this.isAreaLimited = isAreaLimited;
			this.isAreaEntered = isAreaEntered;

			this.areaFrameCount = World.ToFrameCount ( areaSecond );

			this.Location = location;

			this.movie = Movie.Clone ( this.scene.Makings[movieName] as Movie );
			this.movie.Ended += this.movieEnded;

			this.movieName = movieName;

			if ( !string.IsNullOrEmpty ( extendMovieName ) )
			{
				this.extendMovie = Movie.Clone ( this.scene.Makings[extendMovieName] as Movie );
				this.extendMovieName = extendMovieName;
			}

			this.Width = width;
			this.Height = height;
			this.halfSize = new Vector2 ( width / 2, height / 2 );

			this.Type = type;

			this.Speed = speed;
			this.Angle = angle;
			this.HitArea = hitArea;

			if ( null != this.HitArea )
				this.HitArea.Locate ( this.getHitAreaLocation ( ) );

		}

		protected virtual void movieEnded ( object sender, MovieEventArgs e )
		{ }

		internal virtual void Execute ( int action )
		{ }

		internal virtual void SetReelSpeed ( DirectionType direction, float speed )
		{

			switch ( direction )
			{
				case DirectionType.Down:
					this.ySpeed = this.protoYSpeed - speed;
					break;

				case DirectionType.Up:
					this.ySpeed = this.protoYSpeed + speed;
					break;

				case DirectionType.Left:
					this.xSpeed = this.protoXSpeed + speed;
					break;

				case DirectionType.Right:
					this.xSpeed = this.protoXSpeed - speed;
					break;
			}

		}

		protected virtual void move ( )
		{ }

		internal virtual void LoadContent ( )
		{
			this.spiritBatch = this.scene.World.Services.GetService ( typeof ( SpriteBatch ) ) as SpriteBatch;

			this.movie.Texture = ( this.scene.Makings[ this.movieName ] as Movie ).Texture;

			if ( null != this.extendMovie )
				this.extendMovie.Texture = ( this.scene.Makings[ this.extendMovieName ] as Movie ).Texture;

		}

		public void Dispose ( )
		{ this.Dispose ( true ); }

		protected virtual void Dispose ( bool disposing )
		{

			if ( disposing )
			{
				this.movie.Ended -= this.movieEnded;
				this.movie.Dispose ( );

				if ( null != this.extendMovie )
					this.extendMovie.Dispose ( );

				if ( null != this.HitArea )
					this.HitArea.Dispose ( );

			}

		}

		internal void Update ( GameTime time )
		{
			Movie.NextFrame ( this.movie );

			if ( null != this.extendMovie )
				Movie.NextFrame ( this.extendMovie );

			if ( this.scene.IsEnabled && this.world.IsEnabled )
				this.updating ( time );

		}

		internal void Draw ( GameTime time )
		{

			if ( !this.scene.IsClosed && this.IsVisible )
			{
				this.spiritBatch.Begin ( );
				this.drawing ( time, this.spiritBatch );
				this.spiritBatch.End ( );
			}

		}

		protected virtual void updating ( GameTime time )
		{

			if ( this.destroyFrameCount > 0 && --this.destroyFrameCount <= 0 )
			{
				this.Destroy ( );
				return;
			}

			/*
			if ( this.isAreaLimited )
				if ( this.HitArea.HitTest ( this.scene.BattleArea ) )
				{
					this.isAreaEntered = true;
				}
				else
					if ( this.isAreaEntered
					|| ( this.areaFrameCount > 0 && --this.areaFrameCount <= 0 )
					)
					{
						this.Destroy ( );
						return;
					}
			*/

			if ( this.isMoving && this.isMovable )
				this.move ( );

			if ( null != this.HitArea )
				this.HitArea.Locate ( this.getHitAreaLocation ( ) );

		}

		protected virtual Vector2 getMovieLocation ( )
		{ return this.Location; }

		protected virtual Point getHitAreaLocation ( )
		{ return new Point ( ( int ) this.Location.X, ( int ) this.Location.Y ); }

		protected virtual void drawing ( GameTime time, SpriteBatch batch )
		{
			this.movie.Location = this.getMovieLocation ( );

			Movie.Draw ( this.movie, time, batch );

			if ( null != this.extendMovie )
			{
				this.extendMovie.Location = this.movie.Location;
				Movie.Draw ( this.extendMovie, time, batch );
			}

		}

		public void PlayMovie ( string sequenceName )
		{ this.PlayMovie ( sequenceName, false, true ); }
		public void PlayMovie ( string sequenceName, bool isRecord )
		{ Movie.Play ( this.movie, sequenceName, isRecord, true ); }
		public void PlayMovie ( string sequenceName, bool isRecord, bool isReplay )
		{ Movie.Play ( this.movie, sequenceName, isRecord, isReplay ); }

		public void PlayExtendMovie ( string sequenceName )
		{ this.PlayExtendMovie ( sequenceName, false, true ); }
		public void PlayExtendMovie ( string sequenceName, bool isRecord )
		{ this.PlayExtendMovie ( sequenceName, isRecord, true ); }
		public void PlayExtendMovie ( string sequenceName, bool isRecord, bool isReplay )
		{

			if ( null != this.extendMovie )
				Movie.Play ( this.extendMovie, sequenceName, isRecord, isReplay );

		}

		public void BackMovie ( string sequenceName )
		{ Movie.Back ( this.movie, sequenceName ); }

		public void BackExtendMovie ( string sequenceName )
		{

			if ( null != this.extendMovie )
				Movie.Back ( this.extendMovie, sequenceName );

		}

		internal virtual void Destroy ( )
		{

			if ( null != this.Destroyed )
				this.Destroyed ( this, new SpiritEventArgs ( this ) );

		}

		protected virtual void updateSpeed ( )
		{
			this.protoXSpeed = this.xSpeed;
			this.protoYSpeed = this.ySpeed;
		}

	}

}
