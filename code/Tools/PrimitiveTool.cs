using Sandbox;

namespace WorldCraft
{
	[Library( "tool_primitive", Title = "primitive", Icon = "CubeTool.png" )]
	public partial class PrimitiveTool : BaseTool
	{
		public enum StateEnum
		{
			Select,
			Drag,
			Extrude
		};

		public StateEnum State = StateEnum.Select;

		public Vector3 Start;
		public Vector3 End;
		public Plane Plane;
		public Line ExtrudeLine;

		public override void FrameSimulate( Client cl )
		{
			switch ( State )
			{
				case StateEnum.Select:
					Select();
					break;
				case StateEnum.Drag:
					Drag();
					break;
				case StateEnum.Extrude:
					Extrude();
					break;
			}
		}

		void Select()
		{
			var trace = Trace.Ray( Input.Cursor, 5000.0f ).Run();
			if ( !trace.Hit ) return;

			Start = trace.EndPos.SnapToGrid( Game.GridSize );
			End = Start;

			DebugOverlay.Sphere( Start, 4, Color.Blue );

			Plane = new Plane( Start, trace.Normal );

			if ( Input.Down( InputButton.Attack1 ) )
			{
				State = StateEnum.Drag;
			}
		}

		void Drag()
		{
			var end = Plane.Trace( Input.Cursor );
			if ( end == null ) return;

			End = end.Value.SnapToGrid( Game.GridSize );

			Draw();

			// Make sure LMB is still held down
			if ( Input.Down( InputButton.Attack1 ) ) return;

			// Sanity check
			if ( Start.IsNearlyEqual( End ) )
			{
				State = StateEnum.Select;
				return;
			}

			ExtrudeLine = new Line( End - Plane.Normal * 10000, End + Plane.Normal * 10000 );

			State = StateEnum.Extrude;
		}

		void Extrude()
		{
			End = ExtrudeLine.ClosestPoint( Input.Cursor ).SnapToGrid( Game.GridSize );

			Draw();

			if ( Input.Down( InputButton.Attack1 ) )
			{
				State = StateEnum.Select;
				FinalizeBuild();
			}
		}

		void Draw()
		{
			var bounds = new BBox( Vector3.Min( Start, End ), Vector3.Max( Start, End ) );

			DebugOverlay.Box( Start, End, Color.Yellow );
			DebugOverlay.Text( bounds.Center + Vector3.OneY * bounds.Size.y / 2, $"X: { bounds.Size.x }", Color.Yellow, 0, 5000 );
			DebugOverlay.Text( bounds.Center + Vector3.OneX * bounds.Size.x / 2, $"Y: { bounds.Size.y }", Color.Yellow, 0, 5000 );

			if ( State == StateEnum.Extrude )
			{
				DebugOverlay.Line( ExtrudeLine.a, ExtrudeLine.b, Color.Red );
			}
		}

		public void FinalizeBuild()
		{
			var bounds = new BBox( Vector3.Min( Start, End ), Vector3.Max( Start, End ) );

			if ( bounds.Volume.AlmostEqual( 0 ) )
			{
				Sandbox.UI.ChatBox.AddInformation( "Invalid block twat" );
				return;
			}

			PlaceServerside( bounds.Center, bounds.Size );
		}

		[ServerCmd]
		public static void PlaceServerside( Vector3 position, Vector3 size )
		{
			var entity = new PrimitiveEntity();
			entity.Position = position;

			var primitive = new PrimitiveBox();
			primitive.Size = size;
			primitive.Entity = entity; // Circular dependency bad

			entity.Primitive = primitive;
			entity.CreateMesh();
		}
	}
}
