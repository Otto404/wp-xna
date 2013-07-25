using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal abstract class Item
		: Spirit
	{

		private readonly bool isAutoPick;

		protected Item ( IPlayScene scene, int type, Vector2 location, string movieName, float speed, int angle, HitArea hitArea, int width, int height, double destroySecond, bool isAreaLimited, bool isAreaEntered, double areaSecond, bool isAutoPick )
			: base ( scene, type, location, movieName,
			null,
			speed, angle, hitArea, width, height, destroySecond,
			false,
			isAreaLimited, isAreaEntered, areaSecond )
		{
			this.isAutoPick = isAutoPick;

			this.isMoving = true;
		}

		protected override void move ( )
		{
			this.Location.X += this.xSpeed;
			this.Location.Y += this.ySpeed;
		}

		protected override Vector2 getMovieLocation ( )
		{ return this.Location - this.halfSize; }

		protected override void updateSpeed ( )
		{
			this.xSpeed = Calculator.Cos ( this.angle ) * this.speed;
			this.ySpeed = Calculator.Sin ( this.angle ) * this.speed;

			base.updateSpeed ( );
		}

	}

}
