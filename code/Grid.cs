using Sandbox;

namespace WorldCraft
{
	public partial class Grid : CustomSceneObject
	{
		protected Material GridMaterial = Material.Load( "materials/worldcraft.grid.vmat" );

		public Vector3 Origin;
		public Vector3 Normal;

		public Grid()
		{
			Bounds = new BBox( -10000, 10000 );
		}

		public override void RenderSceneObject()
		{
			Render.Set( "GridSize", Game.GridSize );
			Render.Set( "GridOrigin", Origin );
			Render.Set( "GridNormal", Normal );

			var vb = Render.GetDynamicVB();

			// make it fuckin massive
			vb.AddQuad( new Rect( -10000, -10000, 20000, 20000 ) );
			vb.Draw( GridMaterial );
		}
	}
}
