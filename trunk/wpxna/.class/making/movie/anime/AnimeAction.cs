using Microsoft.Xna.Framework;

namespace zoyobar.game
{
    
	internal abstract class AnimeAction
    {
		internal Anime Anime; 

		protected AnimeAction ( )
		{ }

		internal abstract void Update ( GameTime time );

    }

}
