using Microsoft.Xna.Framework;

namespace zoyobar.game
{
    
	internal sealed class AnimeMovementAction
		: AnimeAction
    {
		private readonly Rectangle area;

		private float xSpeed;
		private float ySpeed;

		private readonly long xTurnFrameCount;
		private readonly long yTurnFrameCount;

		private long xTurnFrameIndex;
		private long yTurnFrameIndex;

		internal AnimeMovementAction ( float xSpeed, float ySpeed, Rectangle area )
			: this ( xSpeed, ySpeed, 0, 0, 0, 0, area )
		{ }
		internal AnimeMovementAction ( float xSpeed, float ySpeed, float xTurnSecond, float yTurnSecond )
			: this ( xSpeed, ySpeed, xTurnSecond, yTurnSecond, 0, 0, Rectangle.Empty )
		{ }
		internal AnimeMovementAction ( float xSpeed, float ySpeed, float xTurnSecond, float yTurnSecond, float xCurrentSecond, float yCurrentSecond )
			: this ( xSpeed, ySpeed, xTurnSecond, yTurnSecond, xCurrentSecond, yCurrentSecond, Rectangle.Empty )
		{ }
		internal AnimeMovementAction ( float xSpeed, float ySpeed, float xTurnSecond, float yTurnSecond, float xCurrentSecond, float yCurrentSecond, Rectangle area )
			: base ( )
		{
			this.xTurnFrameCount = World.ToFrameCount ( xTurnSecond );
			this.yTurnFrameCount = World.ToFrameCount ( yTurnSecond );
			this.xSpeed = xSpeed;
			this.ySpeed = ySpeed;

			this.xTurnFrameIndex = World.ToFrameCount ( xCurrentSecond );
			this.yTurnFrameIndex = World.ToFrameCount ( yCurrentSecond );

			this.area = area;
		}

		internal override void Update ( GameTime time )
		{

			if ( this.xTurnFrameCount > 0 && this.xTurnFrameIndex++ > this.xTurnFrameCount )
			{
				this.xTurnFrameIndex = 0;
				this.xSpeed = -this.xSpeed;
			}

			if ( this.yTurnFrameCount > 0 && this.yTurnFrameIndex++ > this.yTurnFrameCount )
			{
				this.yTurnFrameIndex = 0;
				this.ySpeed = -this.ySpeed;
			}

			this.Anime.Location += new Vector2 ( this.xSpeed, this.ySpeed );

			if ( !this.area.IsEmpty )
			{
				Vector2 location = this.Anime.Location;

				if ( this.xSpeed > 0 )
				{

					if ( location.X - this.Anime.Width > this.area.Right )
						this.Anime.Location = new Vector2 ( this.area.Left, location.Y );

				}
				else if ( this.xSpeed < 0 )
					if ( location.X < this.area.Left )
						this.Anime.Location = new Vector2 ( this.area.Right + this.Anime.Width, location.Y );

				if ( this.ySpeed > 0 )
				{

					if ( location.Y > this.area.Bottom )
						this.Anime.Location = new Vector2 ( location.X, this.area.Top - this.Anime.Height );

				}
				else if ( this.ySpeed < 0 )
					if ( location.Y + this.Anime.Height < this.area.Top )
						this.Anime.Location = new Vector2 ( location.X, this.area.Bottom );

			}

		}

    }

}
