using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal sealed class MovieSequence
	{

		internal static void Reset ( MovieSequence sequence )
		{
			sequence.IsLooped = false;
			sequence.currentFrameIndex = 1;

			sequence.CurrentFrame = sequence.frames[sequence.currentFrameIndex - 1];
		}

		internal static bool Next ( MovieSequence sequence )
		{
			bool isEnded = sequence.currentFrameIndex == sequence.frames.Count;

			if ( isEnded )
			{
				sequence.IsLooped = true;

				if ( !sequence.isLoop )
					return isEnded;

				sequence.currentFrameIndex = 1;
			}
			else
				sequence.currentFrameIndex++;

			sequence.CurrentFrame = sequence.frames[sequence.currentFrameIndex - 1];
			return isEnded;
		}

		internal static MovieSequence Clone (MovieSequence sequence )
		{ return new MovieSequence ( sequence ); }

		private readonly List<Point> frames = new List<Point> ( );
		internal bool IsLooped;
		private readonly bool isLoop;
		private int currentFrameIndex;
		internal Point CurrentFrame;
		internal readonly string Name;
		internal readonly int FrameCount;
		internal readonly int Rate;

		internal MovieSequence ( string name, params Point[] frames )
			: this ( name, 0, false, frames )
		{ }
		internal MovieSequence ( string name, int rate, params Point[] frames )
			: this ( name, rate, false, frames )
		{ }
		internal MovieSequence ( string name, bool isLoop, params Point[] frames )
			: this ( name, 0, isLoop, frames )
		{ }
		internal MovieSequence ( string name, int rate, bool isLoop, params Point[] frames )
		{

			if ( string.IsNullOrEmpty ( name ) )
				throw new ArgumentNullException ( "name", "name can't be null" );

			if ( null != frames )
				this.frames.AddRange ( frames );

			this.isLoop = isLoop;
			this.Name = name;
			this.FrameCount = this.frames.Count;
			this.Rate = rate < 0 ? 0 : rate;

			Reset ( this );
		}

		internal MovieSequence ( MovieSequence sequence )
		{
			this.frames = sequence.frames;
			this.isLoop = sequence.isLoop;
			this.Name = sequence.Name;
			this.FrameCount = this.frames.Count;
			this.Rate = sequence.Rate;

			Reset ( this );
		}

	}

}
