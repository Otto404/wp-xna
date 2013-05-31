using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class MultiRectangleHitArea
		: SingleRectangleHitArea
	{
		private readonly List<Point> subLocations = new List<Point> ( );
		private readonly List<Rectangle> subRectangles = new List<Rectangle> ( );

		internal MultiRectangleHitArea ( Rectangle rectangle, params Rectangle[] subRectangles )
			: this ( rectangle,
			true,
			subRectangles
			)
		{ }
		internal MultiRectangleHitArea ( Rectangle rectangle, bool isEnabled, params Rectangle[] subRectangles )
			: base ( rectangle, isEnabled )
		{
			this.subRectangles.AddRange ( subRectangles );

			foreach ( Rectangle subRectangle in subRectangles )
				this.subLocations.Add ( subRectangle.Location );

		}

		protected override bool containTesting ( int x, int y )
		{

			if ( !base.containTesting ( x, y ) )
				return false;

			foreach ( Rectangle subRectangle in this.subRectangles )
				if ( subRectangle.Contains ( x, y ) )
					return true;

			return false;
		}

		protected override bool hitTesting ( Rectangle rectangle )
		{

			if ( !base.hitTesting ( rectangle ) )
				return false;

			foreach ( Rectangle subRectangle in this.subRectangles )
				if ( subRectangle.Intersects ( rectangle ) )
					return true;

			return false;
		}

		protected override bool hitTesting ( HitArea area )
		{
			
			if ( !base.hitTesting ( area ) )
				return false;

			foreach ( Rectangle subRectangle in this.subRectangles )
				if ( area.HitTest ( subRectangle ) )
					return true;

			return false;
		}

		internal override void Locate ( Point location )
		{
			base.Locate ( location );

			for ( int index = 0; index < this.subRectangles.Count; index++ )
			{
				Rectangle subRectangle = this.subRectangles[index];
				subRectangle.Location = new Point ( this.subLocations[index].X + location.X, this.subLocations[index].Y + location.Y );

				this.subRectangles[index] = subRectangle;
			}

		}

		public override void Dispose ( )
		{
			this.subLocations.Clear ( );
			this.subRectangles.Clear ( );

			base.Dispose ( );
		}

	}

}
