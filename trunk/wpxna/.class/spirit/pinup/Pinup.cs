using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal abstract class Pinup
		: Spirit
	{

		protected Pinup ( IPlayScene scene, int type, Vector2 location, string movieName, float speed, int angle, int width, int height, double destroySecond )
			: base ( scene, type, location, movieName,
			null,
			speed,
			angle,
			null,
			width, height, destroySecond,
			true,
			false,
			false,
			0
			)
		{ }

		protected override Vector2 getMovieLocation ( )
		{ return this.Location - this.halfSize; }

		protected override void updateSpeed ( )
		{
			this.xSpeed = Calculator.Cos ( this.angle ) * this.speed;
			this.ySpeed = Calculator.Sin ( this.angle ) * this.speed;

			base.updateSpeed ( );
		}

		protected override void move ( )
		{
			this.Location.X += this.xSpeed;
			this.Location.Y += this.ySpeed;
		}

	}

}