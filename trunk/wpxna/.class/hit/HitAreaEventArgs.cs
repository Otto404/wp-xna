using System;

namespace zoyobar.game
{

	internal class HitAreaEventArgs
		: EventArgs
	{
		internal readonly HitArea HitArea;
		internal bool IsHit = false;
		internal readonly int Type;

		internal HitAreaEventArgs ( int type, HitArea areaHit )
		{
			this.Type = type;
			this.HitArea = areaHit;
		}

	}

}
