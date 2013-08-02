using System;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal sealed class RegionManager
		: SpiritManager<Region>
	{
		internal event EventHandler<RegionHitAreaEventArgs> HitTesting;

		internal RegionManager ( )
			: base ( 3000 )
		{ }

		internal override void Update ( GameTime time )
		{

			if ( null == this.HitTesting )
				return;

			foreach ( Region region in this.Spirits.ToArray ( ) )
			{
				RegionHitAreaEventArgs hitAreaArg = new RegionHitAreaEventArgs ( region );
				this.HitTesting ( region, hitAreaArg );

				region.Append ( hitAreaArg.Targets );

				hitAreaArg.Dispose ( );
			}

		}

	}

}
