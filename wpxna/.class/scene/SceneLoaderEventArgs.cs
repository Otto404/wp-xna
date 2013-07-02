using System;
using System.Collections.Generic;

namespace zoyobar.game
{

	internal class SceneLoaderEventArgs
		: EventArgs, IDisposable
	{

		internal readonly List<Scene> Scenes = new List<Scene> ( );
		internal readonly Type[] AfterSceneTypes;

		internal SceneLoaderEventArgs ( IList<Scene> scenes, Type[] afterSceneTypes )
		{
			this.Scenes.AddRange ( scenes );

			this.AfterSceneTypes = afterSceneTypes;
		}

		public void Dispose ( )
		{ this.Scenes.Clear ( ); }

	}

}
