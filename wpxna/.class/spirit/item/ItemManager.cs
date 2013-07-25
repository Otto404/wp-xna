using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class ItemManager
		: SpiritManager<Item>
	{
		internal event EventHandler<HitAreaEventArgs> HitTesting;
		internal event EventHandler<SpiritEventArgs> Picked;

		internal ItemManager ( )
			: this ( -1000 )
		{ }
		internal ItemManager ( int defaultOrder )
			: base ( defaultOrder )
		{ }

		internal override void Update ( GameTime time )
		{

			if ( null == this.HitTesting )
				return;

			foreach ( Item item in this.Spirits.ToArray ( ) )
				if ( null != item.HitArea )
				{
					HitAreaEventArgs hitAreaArg = new HitAreaEventArgs ( item.Type, item.HitArea );
					this.HitTesting ( item, hitAreaArg );

					if ( hitAreaArg.IsHit )
					{

						if ( null != this.Picked )
							this.Picked ( item, new SpiritEventArgs ( item ) );

						item.Destroy ( );
					}

				}

		}

	}

}
