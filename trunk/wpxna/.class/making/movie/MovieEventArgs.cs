using System;

namespace zoyobar.game
{

	internal class MovieEventArgs
		: EventArgs
	{
		internal readonly string SequenceName;

		internal MovieEventArgs ( Movie movie )
		{
			this.SequenceName = movie.CurrentSequenceName;
		}

	}

}
