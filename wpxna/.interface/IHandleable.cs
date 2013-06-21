using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zoyobar.game
{

	internal interface IHandleable
	{
		void Update ( GameTime time );

		void Draw ( GameTime time );

		bool Input ( Controller controller );
	}

}
