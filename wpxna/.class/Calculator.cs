using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace zoyobar.game
{

    internal sealed class Calculator
    {
		private static readonly List<float> radian = new List<float> ( 360 );
		private static readonly List<float> cos = new List<float> ( 360 );
		private static readonly List<float> sin = new List<float> ( 360 );

		internal static void Init ( )
		{
			radian.Clear ( );
			cos.Clear ( );
			sin.Clear ( );

			for ( int angle = 0; angle < 360; angle++ )
			{
				float r = MathHelper.ToRadians ( angle );
				radian.Add ( r );
				cos.Add ( ( float ) Math.Cos ( r ) );
				sin.Add ( ( float ) Math.Sin ( r ) );
			}

			cos[ 90 ] = 0;
			cos[ 270 ] = 0;
			sin[ 0 ] = 0;
			sin[ 180 ] = 0;
		}

		internal static int Degree ( int angle )
		{
		
			while ( angle >= 360 )
				angle -= 360;

			while ( angle < 0 )
				angle += 360;

			return angle;
		}

		internal static float Radian ( int angle )
		{
			return radian[ Degree ( angle ) ];
		}


		internal static float Cos ( int angle )
		{
			return cos[ Degree ( angle ) ];
		}

		internal static float Sin ( int angle )
		{
			return sin[ Degree ( angle ) ];
		}

	}

}
