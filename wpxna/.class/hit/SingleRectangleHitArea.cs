using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal class SingleRectangleHitArea
		: HitArea
	{
		protected readonly Point location;

		protected Rectangle rectangle;

		internal Rectangle Rectangle
		{
			get { return this.rectangle; }
		}

		internal SingleRectangleHitArea ( Rectangle rectangle )
			: this ( rectangle,
			true
			)
		{ }
		internal SingleRectangleHitArea ( Rectangle rectangle, bool isEnabled )
			: base ( isEnabled )
		{
			this.rectangle = rectangle;
			this.location = rectangle.Location;
		}

		protected override bool containTesting ( int x, int y )
		{ return this.rectangle.Contains ( x, y ); }

		protected override bool hitTesting ( Rectangle rectangle )
		{ return this.rectangle.Intersects ( rectangle ); }

		protected override bool hitTesting ( HitArea area )
		{ return area.HitTest ( this.rectangle ); }

		internal override void Locate ( Point location )
		{ this.rectangle.Location = new Point ( this.location.X + location.X, this.location.Y + location.Y ); }

	}

}
