using System;

namespace zoyobar.game
{

	internal enum ResourceType
	{
		Image,
		Font,
		Sound,
		Music,
	}

	internal struct Resource
	{

		internal readonly string Name;
		internal readonly string Path;
		internal readonly ResourceType Type;

		internal Resource ( string name, ResourceType type, string path )
		{

			if ( string.IsNullOrEmpty ( name ) || string.IsNullOrEmpty ( path ) )
				throw new ArgumentNullException ( "name, path", "name, path can't be null" );

			this.Name = name;
			this.Type = type;
			this.Path = path;
		}

	}

}
