using Sandbox;

namespace WorldCraft
{
	public partial class BasePrimitive : BaseNetworkable
	{
		[Net] public PrimitiveEntity Entity { get; set; }

		/// <summary>
		/// Tell our entity the primitive is dirty and needs rebuilding.
		/// </summary>
		protected void DirtyEntity()
		{
			Entity?.CreateMesh();
		}

		public virtual void BuildMesh( Mesh mesh ) { }
		public virtual Model BuildModel() { return null; }
		public virtual void DrawDebug( Vector3 origin ) { }
	}
}
