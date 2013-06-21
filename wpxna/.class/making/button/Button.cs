using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace zoyobar.game
{

	internal class Button
		: Making, ILockable
	{

		internal static void Press ( Button button )
		{
			button.IsPressing = true;

			button.press ( );

			if ( null != button.Pressing )
				button.Pressing ( button, new ButtonEventArgs ( button.command ) );

		}

		internal static void Unpress ( Button button )
		{
			if ( !button.IsPressing )
				return;

			button.IsPressing = false;

			button.unpress ( );

			if ( null != button.Pressed )
				button.Pressed ( button, new ButtonEventArgs ( button.command ) );

		}

		internal static bool PressTest ( Button button, IList<Motion> motions )
		{

			if ( !button.isEnabled || !button.isVisible )
				return false;

			foreach ( Motion motion in motions )
				if ( motion.Type == MotionType.Down || motion.Type == MotionType.Press )
				{
					Point location = new Point ( ( int ) motion.Position.X, ( int ) motion.Position.Y );

					if ( button.bound.Contains ( location ) )
					{
						Press ( button );
						return true;
					}

				}

			Unpress ( button );
			return false;
		}

		internal static void Select ( Button button )
		{

			if ( !button.isEnabled )
				return;

			button.select ( );

			if ( null != button.Selected )
				button.Selected ( button, new ButtonEventArgs ( button.command ) );

		}

		internal static bool ClickTest ( Button button, IList<Motion> motions )
		{

			if ( !button.isEnabled || !button.isVisible )
				return false;


			foreach ( Motion motion in motions )
				if ( motion.Type == MotionType.Up )
				{
					Point location = new Point ( ( int ) motion.Position.X, ( int ) motion.Position.Y );

					if ( button.bound.Contains ( location ) )
					{
						Select ( button );
						return true;
					}

				}

			return false;
		}

		internal event EventHandler<ButtonEventArgs> Selected;
		internal event EventHandler<ButtonEventArgs> Pressing;
		internal event EventHandler<ButtonEventArgs> Pressed;

		protected readonly Movie backgroundMovie;

		protected string command;

		internal bool IsPressing = false;

		private bool isEnabled = true;
		internal virtual bool IsEnabled
		{
			get { return this.isEnabled; }
			set
			{
				Movie.Play ( this.backgroundMovie, value ? this.upMovieSequenceName : this.disableMovieSequenceName );

				this.isEnabled = value;
			}
		}

		internal readonly bool IsSole;

		protected Vector2 location;
		public virtual Vector2 Location
		{
			get { return this.location; }
			set {
				this.location = value;
				
				this.backgroundMovie.Location = value;
				
				this.bound = new Rectangle (
					( int ) ( value.X ),
					( int ) ( value.Y ),
					this.Width,
					this.Height
					);
			}
		}

		internal override bool IsVisible
		{
			set
			{
				base.IsVisible = value;

				this.backgroundMovie.IsVisible = value;
			}
		}

		internal readonly int Width;
		internal readonly int Height;
		
		private Rectangle bound;

		protected string upMovieSequenceName = "up";
		protected string downMovieSequenceName = "down";
		protected string disableMovieSequenceName = "disable";

		internal Button ( string name, string resourceName, string command, Vector2 location, int width, int height, Point upFrameIndex, params MovieSequence[] movieSequences )
			: this ( name, resourceName, command, location, width, height,
			true,
			upFrameIndex,
			new Point ( upFrameIndex.X + 1, upFrameIndex.Y ),
			new Point ( upFrameIndex.X + 2, upFrameIndex.Y ),
			movieSequences )
		{ }
		internal Button ( string name, string resourceName, string command, Vector2 location, int width, int height, Point upFrameIndex )
			: this ( name, resourceName, command, location, width, height,
			true,
			upFrameIndex,
			new Point ( upFrameIndex.X + 1, upFrameIndex.Y ),
			new Point ( upFrameIndex.X + 2, upFrameIndex.Y ),
			null
			)
		{ }
		internal Button ( string name, string resourceName, string command, Vector2 location, int width, int height, bool isSole, Point upFrameIndex )
			: this ( name, resourceName, command, location, width, height, isSole, upFrameIndex,
			new Point ( upFrameIndex.X + 1, upFrameIndex.Y ),
			new Point ( upFrameIndex.X + 2, upFrameIndex.Y ),
			null
			)
		{ }
		internal Button ( string name, string resourceName, string command, Vector2 location, int width, int height, bool isSole, Point upFrameIndex, Point downFrameIndex )
			: this ( name, resourceName, command, location, width, height, isSole, upFrameIndex, downFrameIndex,
			Point.Zero,
			null
			)
		{ }
		internal Button ( string name, string resourceName, string command, Vector2 location, int width, int height, bool isSole, Point upFrameIndex, Point downFrameIndex, Point disableFrameIndex, params MovieSequence[] movieSequences )
			: base ( name, resourceName )
		{
			this.command = command;
			this.IsSole = isSole;

			this.Width = width;
			this.Height = height;

			List<MovieSequence> sequences = new List<MovieSequence> ( );
			sequences.Add ( new MovieSequence ( this.upMovieSequenceName, upFrameIndex ) );
			sequences.Add ( new MovieSequence ( this.downMovieSequenceName, downFrameIndex ) );
			sequences.Add ( new MovieSequence ( this.disableMovieSequenceName, disableFrameIndex ) );

			if ( null != movieSequences && movieSequences.Length != 0 )
				sequences.AddRange ( movieSequences );

			this.backgroundMovie = new Movie ( "background", resourceName, location, width, height, 0, 1f, this.upMovieSequenceName,
				sequences.ToArray ( )
				);

			this.Location = location;
		}

		internal override void Init ( Scene scene )
		{
			base.Init ( scene );

			this.backgroundMovie.Init ( scene );
		}

		internal override void InitResource ( ResourceManager resourceManager )
		{
			base.InitResource ( resourceManager );

			this.backgroundMovie.InitResource ( resourceManager );
		}

		protected virtual void select ( )
		{ this.scene.AudioManager.PlaySound ( "click.s" ); }

		protected virtual void press ( )
		{

			if ( this.backgroundMovie.CurrentSequenceName != this.downMovieSequenceName )
				Movie.Play ( this.backgroundMovie, this.downMovieSequenceName );

		}

		protected virtual void unpress ( )
		{

			if ( this.backgroundMovie.CurrentSequenceName != this.upMovieSequenceName )
				Movie.Play ( this.backgroundMovie, this.upMovieSequenceName );

		}

		internal virtual void Draw ( SpriteBatch batch )
		{

			if ( !this.isVisible )
				return;

			Movie.Draw ( this.backgroundMovie, null, batch );
		}

		public override void Dispose ( )
		{
			this.backgroundMovie.Dispose ( );

			base.Dispose ( );
		}

	}

}
