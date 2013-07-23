using System;
using System.Collections.Generic;

namespace zoyobar.game
{

	internal class BulletHitAreaEventArgs
		: HitAreaEventArgs, IDisposable
	{
		internal IList<IAssailable> Targets;

		internal BulletHitAreaEventArgs ( Bullet bullet )
			: base ( bullet.Type, bullet.HitArea )
		{ }

		public void Dispose ( )
		{

			if ( null != this.Targets && !this.Targets.IsReadOnly )
				this.Targets.Clear ( );

		}

	}

}
