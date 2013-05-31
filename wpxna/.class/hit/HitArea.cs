using System;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

    internal abstract class HitArea
		: IDisposable
    {
		internal bool IsEnabled;

		protected HitArea ( )
			: this (
			true
			)
		{ }
		protected HitArea ( bool isEnabled )
		{ this.IsEnabled = isEnabled; }

		protected abstract bool containTesting ( int x, int y );

		internal bool ContainTest ( Vector2 location )
		{
			return this.ContainTest ( ( int ) location.X, ( int ) location.Y );
		}
		internal bool ContainTest ( float x, float y )
		{ return this.ContainTest ( ( int ) x, ( int ) y ); }
		internal bool ContainTest ( int x, int y )
		{

			if ( !this.IsEnabled )
				return false;

			return this.containTesting ( x, y );
		}

		protected abstract bool hitTesting ( Rectangle rectangle );

		internal bool HitTest ( Rectangle rectangle )
		{

			if ( !this.IsEnabled )
				return false;

			return this.hitTesting ( rectangle );
		}

		protected abstract bool hitTesting ( HitArea area );

		internal bool HitTest ( HitArea area )
		{

			if ( !this.IsEnabled )
				return false;

			return this.hitTesting ( area );
		}

		internal abstract void Locate ( Point position );

		public virtual void Dispose ( )
		{ }

	}

}
