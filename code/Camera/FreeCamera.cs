using Sandbox;

namespace WorldCraft
{
	public class FreeCamera : Camera
	{
		Angles LookAngles;
		Vector3 MoveInput;
		float MoveSpeed;

		public override void Update()
		{
			Pos += MoveInput.Normal * 300 * RealTime.Delta * Rot * MoveSpeed;
			Rot = Rotation.From( LookAngles );
			FieldOfView = 80;
			Viewer = Local.Pawn;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( !input.Down( InputButton.Attack2 ) )
			{
				MoveInput = Vector3.Zero;
				return;
			}

			MoveInput = input.AnalogMove;
			LookAngles += input.AnalogLook.WithRoll(0);

			MoveSpeed = 1.0f;
			if ( input.Down( InputButton.Run ) ) MoveSpeed = 5.0f;
			if ( input.Down( InputButton.Duck ) ) MoveSpeed = 0.2f;

			input.ViewAngles = LookAngles;
			input.Position = Pos;

			// input.ClearButtons();
			input.StopProcessing = true;
		}
	}
}
