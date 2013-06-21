using System;

namespace zoyobar.game
{

	internal abstract class Making
		: IDisposable
	{
		protected readonly string resourceName;
		internal readonly string Name;
		protected Scene scene;

		protected bool isVisible = true;
		internal virtual bool IsVisible
		{
			get { return this.isVisible; }
			set { this.isVisible = value; }
		}

		protected Making ( string name, string resourceName )
		{
			this.Name = name;
			this.resourceName = resourceName;
		}

		internal virtual void Init ( Scene scene )
		{
			this.scene = scene;

			if ( this is ILockable )
				( this as ILockable ).Location += scene.Location;

		}

		internal virtual void InitResource ( ResourceManager resourceManager )
		{ }

		public virtual void Dispose ( )
		{ }

	}

}
