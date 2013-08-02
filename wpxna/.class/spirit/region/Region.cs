using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal abstract class Region
		: Spirit
	{
		private readonly List<Spirit> targets = new List<Spirit> ( );

		protected Region ( IPlayScene scene, int type, Vector2 location, string movieName, float speed, int angle, HitArea hitArea, int width, int height, double destroySecond )
			: base ( scene, type, location, movieName,
			null,
			speed, angle, hitArea, width, height, destroySecond,
			true,
			false,
			false,
			0
			)
		{ }

		protected virtual bool appending ( Spirit spirit )
		{ return true; }
		protected virtual void appended ( Spirit spirit )
		{ }

		internal void Append ( IList<Spirit> spirits )
		{

			if ( null == spirits )
				return;

			foreach ( Spirit spirit in spirits )
				this.Append ( spirit );

		}
		internal void Append ( Spirit spirit )
		{

			if ( this.targets.Contains ( spirit ) || !this.appending ( spirit ) )
				return;

			this.targets.Add ( spirit );

			this.appended ( spirit );
		}

		protected override void move ( )
		{
			this.Location.X += this.xSpeed;
			this.Location.Y += this.ySpeed;
		}

		protected override Vector2 getMovieLocation ( )
		{ return this.Location - this.halfSize; }

		protected override void Dispose ( bool disposing )
		{

			try
			{

				if ( disposing )
				{
					this.targets.Clear ( );
				}

			}
			catch { }

			base.Dispose ( disposing );
		}

	}

}