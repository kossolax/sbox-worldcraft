using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCraft
{
	public partial class PrimitiveCylinder : BasePrimitive
	{

		public override Model BuildModel()
		{
			var mesh = new Mesh( Material.Load( "materials/dev/reflectivity_30.vmat" ) );
			BuildMesh( mesh );

			var model = Model.Builder
				.AddMesh( mesh )
				.AddCollisionBox( Size / 2 ) // todo: AddCollisionMesh
				.Create();

			return model;
		}

		public override void BuildMesh( Mesh mesh )
		{
			var height = Size.z / 2;
			var diameter = Size.x; // todo: x, y size
			int tesselation = 32; // todo: tesselation in tool options

			var verts = new List<SimpleVertex>();
			var indices = new List<int>();
			var radius = diameter / 2;

			for ( int i = 0; i <= tesselation; i++ )
			{
				var normal = GetCircleVector( i, tesselation );
				var texCoord = new Vector2((float)i / (float)tesselation, 0.0f);

				var pos = normal * radius + Vector3.Up * height;
				GetUvs( normal, out Vector3 u, out Vector3 v );

				verts.Add( new SimpleVertex()
				{
					normal = normal,
					position = pos,
					tangent = u,
					texcoord = texCoord // todo: texcoords for cylinder sides
				} );

				pos = normal * radius + Vector3.Down * height;
				texCoord.y = 1.0f;
				verts.Add( new SimpleVertex()
				{
					normal = normal,
					position = pos,
					tangent = u,
					texcoord = texCoord // todo: texcoords for cylinder sides
				} );
			}

			for ( int i = 0; i < tesselation; i++ )
			{
				indices.Add( i * 2 );
				indices.Add( i * 2 + 1 );
				indices.Add( i * 2 + 2 );

				indices.Add( i * 2 + 1 );
				indices.Add( i * 2 + 3 );
				indices.Add( i * 2 + 2 );
			}

			CreateCap( tesselation, height, radius, Vector3.Up, indices, verts );
			CreateCap( tesselation, height, radius, Vector3.Down, indices, verts );

			mesh.CreateVertexBuffer<SimpleVertex>( verts.Count, SimpleVertex.Layout, verts.ToArray() );
			mesh.CreateIndexBuffer( indices.Count, indices.ToArray() );
		}

		protected static Vector3 GetCircleVector( int i, int tessellation )
		{
			var angle = i * (float)Math.PI * 2 / tessellation;

			var dx = (float)Math.Cos( angle );
			var dy = (float)Math.Sin( angle );

			var v = new Vector3( dx, dy, 0 );
			return v.Normal;
		}

		public void CreateCap( int tesselation, float height, float radius, Vector3 normal, List<int> indices, List<SimpleVertex> verts )
		{
			// Create cap indices.
			for ( int i = 0; i < tesselation - 2; i++ )
			{
				if ( normal.z > 0 )
				{
					indices.Add( verts.Count );
					indices.Add( verts.Count + (i + 1) % tesselation );
					indices.Add( verts.Count + (i + 2) % tesselation );
				}
				else
				{
					indices.Add( verts.Count );
					indices.Add( verts.Count + (i + 2) % tesselation );
					indices.Add( verts.Count + (i + 1) % tesselation );
				}
			}

			// Create cap vertices.
			for ( int i = 0; i < tesselation; i++ )
			{
				var pos = GetCircleVector( i, tesselation ) * radius +
								   normal * height;
				GetUvs( normal, out Vector3 u, out Vector3 v );

				verts.Add( new SimpleVertex()
				{
					normal = normal,
					position = pos,
					tangent = u,
					texcoord = Planar( (Origin + pos) / 32, u, v )
				} );
			}
		}

		private static void GetUvs( Vector3 normal, out Vector3 u, out Vector3 v )
		{
			var t1 = Vector3.Cross( normal, Vector3.Forward );
			var t2 = Vector3.Cross( normal, Vector3.Up );
			if ( t1.Length > t2.Length )
			{
				u = t1;
			}
			else
			{
				u = t2;
			}
			v = Vector3.Cross( normal, u ).Normal;
		}

	}
}
