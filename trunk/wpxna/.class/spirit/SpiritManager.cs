using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal abstract class SpiritManager<T>
		: IDisposable
		where T : Spirit
	{

		internal readonly List<T> Spirits = new List<T> ( );
		private readonly int defaultOrder;

		//internal IPlayScene Scene;
		internal IScene Scene;

		protected SpiritManager ( )
			: this ( 0 )
		{ }
		protected SpiritManager ( int defaultOrder )
		{ this.defaultOrder = defaultOrder; }

		protected virtual void spiritDestroyed ( object sender, SpiritEventArgs e )
		{ this.remove ( sender as T ); }

		internal void Append ( IList<T> spirits )
		{

			if ( null != spirits )
				foreach ( T spirit in spirits )
					this.Append ( spirit );

		}

		internal void Append ( T spirit )
		{ this.Append ( spirit, this.defaultOrder ); }

		internal virtual void Append ( T spirit, int order )
		{

			spirit.Destroyed += this.spiritDestroyed;

			if ( order != 0 && spirit.DrawOrder == 0 )
				spirit.DrawOrder = order;

			this.Spirits.Add ( spirit );
			this.Scene.World.Components.Add ( spirit );
		}

		private void remove ( T spirit )
		{

			spirit.Destroyed -= this.spiritDestroyed;
			this.Scene.World.Components.Remove ( spirit );
			this.Spirits.Remove ( spirit );

			spirit.Dispose ( );
		}

		internal virtual void Update ( GameTime time )
		{ }

		internal void RemoveAll ( )
		{

			T[] spirits = this.Spirits.ToArray ( );

			foreach ( T spirit in spirits )
				this.remove ( spirit );

		}

		public virtual void Dispose ( )
		{ this.RemoveAll ( ); }

	}

}
