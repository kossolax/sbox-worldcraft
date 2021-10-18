using Sandbox;

namespace WorldCraft
{
	public partial class PrimitiveBox : BasePrimitive
	{
		[Net, Change( nameof( DirtyEntity ) )] public Vector3 Size { get; set; }

		public override Model BuildModel()
		{
			var mesh = new Mesh( Material.Load( "materials/dev/reflectivity_30.vmat" ) );

			var vb = new VertexBuffer();
			vb.Init( true );

			vb.AddCube( Vector3.Zero, Size, Rotation.Identity );

			mesh.CreateBuffers( vb );

			var model = Model.Builder
				.AddMesh( mesh )
				.AddCollisionBox( Size / 2 )
				.Create();

			return model;
		}
	}
}
