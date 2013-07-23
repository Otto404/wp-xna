using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

	internal abstract class Bullet
		: Spirit
	{
		internal readonly int Power;
		private int life;

		protected Bullet ( /* IPlayScene scene */ IScene scene, int type, Vector2 location, string movieName, float speed, int angle, HitArea hitArea, int width, int height, int power, int life, double destroySecond, bool isMovieRotable, bool isAreaLimited, bool isAreaEntered, double areaSecond )
			: base ( scene, type, location, movieName,
			null,
			speed, angle, hitArea, width, height, destroySecond, isMovieRotable, isAreaLimited, isAreaEntered, areaSecond )
		{
			this.Power = power < 0 ? 0 : power;
			this.life = life;

			this.isMoving = true;
		}

		internal virtual void Attack ( IList<IAssailable> targets )
		{

			if ( null == targets )
				return;

			foreach ( IAssailable target in targets )
			{
				target.Injure ( this.Power, this.type );

				if ( this.life > 0 )
				{
					this.life -= this.Power;

					if ( this.life <= 0 )
					{
						this.Destroy ( );
						break;
					}

				}

			}

		}

		protected override void move ( )
		{
			this.Location.X += this.xSpeed;
			this.Location.Y += this.ySpeed;
		}

		protected override Vector2 getMovieLocation ( )
		{ return this.Location - this.halfSize; }

	}

}
