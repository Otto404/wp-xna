using System;
using System.Collections.Generic;

namespace zoyobar.game
{

	internal class RegionHitAreaEventArgs
		: HitAreaEventArgs, IDisposable
	{
		internal IList<Spirit> Targets;

		internal RegionHitAreaEventArgs ( Region region )
			: base ( region.Type, region.HitArea )
		{ }

		public void Dispose ( )
		{

			if ( null != this.Targets && !this.Targets.IsReadOnly )
				this.Targets.Clear ( );

		}

	}

}
