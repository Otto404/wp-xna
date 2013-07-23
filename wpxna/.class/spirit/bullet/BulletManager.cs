using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class BulletManager
		: SpiritManager<Bullet>
	{
		internal event EventHandler<BulletHitAreaEventArgs> HitTesting;

		internal BulletManager ( )
			: base ( 2000 )
		{ }

		internal override void Update ( GameTime time )
		{

			if ( null == this.HitTesting )
				return;

			foreach ( Bullet bullet in this.Spirits.ToArray ( ) )
				if ( null != bullet.HitArea )
				{
					BulletHitAreaEventArgs hitAreaArg = new BulletHitAreaEventArgs ( bullet );
					this.HitTesting ( bullet, hitAreaArg );

					if ( hitAreaArg.IsHit )
						bullet.Attack ( hitAreaArg.Targets );

					hitAreaArg.Dispose ( );
				}

		}

		internal List<Bullet> HitTest ( HitArea area, int mode )
		{
			List<Bullet> bullets = new List<Bullet> ( );

			foreach ( Bullet bullet in this.Spirits )
				if ( mode == 1 && bullet.Type < 100 && area.HitTest ( bullet.HitArea ) )
					bullets.Add ( bullet );
				else if ( mode == 2 && bullet.Type >= 100 && area.HitTest ( bullet.HitArea ) )
					bullets.Add ( bullet );

			return bullets;
		}

	}

}
