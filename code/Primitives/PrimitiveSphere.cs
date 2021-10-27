using Sandbox;
using System;
using System.Collections.Generic;

namespace WorldCraft
{
	public partial class PrimitiveSphere : BasePrimitive
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
			int tessellation = 16; // todo: tessellation from tool options

			if ( tessellation < 3 )
				throw new ArgumentOutOfRangeException( "tessellation" );

			var verticalSegments = tessellation;
			var horizontalSegments = tessellation * 2;
			var verts = new List<SimpleVertex>();
			var indices = new List<int>();

			// Start with a single vertex at the bottom of the sphere.
			Vector2 textureCoordinate = new Vector2( 1 );
			Vector3 tangent, binormal;

			var pos = Vector3.Down * (Size / 2);
			GetTangentBinormal( Vector3.Down, out tangent, out binormal );

			verts.Add( new SimpleVertex()
			{
				normal = Vector3.Down,
				position = pos,
				texcoord = textureCoordinate,
				tangent = tangent
			} );

			// Create rings of vertices at progressively higher latitudes.
			for ( int i = 0; i <= verticalSegments - 2; i++ )
			{
				float latitude = ((i + 1) * (float)Math.PI / verticalSegments) - (float)Math.PI / 2;

				float dz = (float)Math.Sin( latitude );
				float dxy = (float)Math.Cos( latitude );

				// Create a single ring of vertices at this latitude.
				for ( int j = 0; j <= horizontalSegments; j++ )
				{
					float longitude = j * (float)Math.PI * 2 / horizontalSegments;

					float dx = (float)Math.Cos( longitude ) * dxy;
					float dy = (float)Math.Sin( longitude ) * dxy;

					Vector3 normal = new Vector3( dx, dy, dz );
					textureCoordinate = new Vector2( (float)j / horizontalSegments, -(float)i / ((float)verticalSegments + 2) );
					pos = normal * (Size / 2);
					GetTangentBinormal( normal, out tangent, out binormal );

					verts.Add( new SimpleVertex()
					{
						position = pos,
						normal = normal,
						texcoord = textureCoordinate,
						tangent = tangent
					} );
				}
			}

			textureCoordinate = new Vector2( 0 );

			// Finish with a single vertex at the top of the sphere.
			pos = Vector3.Up * (Size / 2);
			GetTangentBinormal( Vector3.Up, out tangent, out binormal );

			verts.Add( new SimpleVertex()
			{
				position = pos,
				normal = Vector3.Up,
				texcoord = textureCoordinate,
				tangent = tangent
			} );

			// Create a fan connecting the bottom vertex to the bottom latitude ring.
			for ( int i = 0; i < horizontalSegments; i++ )
			{
				indices.Add( 1 + (i + 1) );
				indices.Add( 1 + i );
				indices.Add( 0 );
			}

			// Fill the sphere body with triangles joining each pair of latitude rings.
			for ( int i = 0; i < verticalSegments - 2; i++ )
			{
				for ( int j = 0; j < horizontalSegments; j++ )
				{
					int nextI = i + 1;
					int nextJ = (j + 1);

					indices.Add( 1 + i * (horizontalSegments + 1) + nextJ );
					indices.Add( 1 + nextI * (horizontalSegments + 1) + j );
					indices.Add( 1 + i * (horizontalSegments + 1) + j );

					indices.Add( 1 + nextI * (horizontalSegments + 1) + nextJ );
					indices.Add( 1 + nextI * (horizontalSegments + 1) + j );
					indices.Add( 1 + i * (horizontalSegments + 1) + nextJ );
				}
			}

			// Create a fan connecting the top vertex to the top latitude ring.
			for ( int i = 0; i < horizontalSegments; i++ )
			{
				indices.Add( verts.Count - 2 - (i + 1) );
				indices.Add( verts.Count - 2 - i );
				indices.Add( verts.Count - 1 );
			}

			mesh.CreateVertexBuffer<SimpleVertex>( verts.Count, SimpleVertex.Layout, verts.ToArray() );
			mesh.CreateIndexBuffer( indices.Count, indices.ToArray() );
		}

	}
}
