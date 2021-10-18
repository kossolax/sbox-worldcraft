using Sandbox;
using Sandbox.UI;

namespace WorldCraft
{
	[Library( "worldcraft" ), Hammer.Skip]
	public partial class Game : GameBase
	{
		public static Game Current { get; protected set; }
		public Hud Hud { get; private set; }

		[ConVar.ClientData( "grid_size" )]
		public static float GridSize { get; set; } = 16.0f;

		public Game()
		{
			Current = this;
			Transmit = TransmitType.Always;

			if ( IsClient )
			{
				Hud = new Hud();
				Local.Hud = Hud;
			}
		}

		public override void Shutdown()
		{
			if ( Current == this )
			{
				Current = null;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if ( Local.Hud == Hud )
			{
				Local.Hud = null;
			}

			Hud?.Delete();
			Hud = null;
		}

		public override void ClientJoined( Client cl )
		{
			Log.Info( $"\"{cl.Name}\" has joined the game" );
			ChatBox.AddInformation( To.Everyone, $"{cl.Name} has joined", $"avatar:{cl.SteamId}" );

			cl.Pawn = new EditorPawn();
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			Log.Info( $"\"{cl.Name}\" has left the game ({reason})" );
			ChatBox.AddInformation( To.Everyone, $"{cl.Name} has left ({reason})", $"avatar:{cl.SteamId}" );

			if ( cl.Pawn.IsValid() )
			{
				cl.Pawn.Delete();
				cl.Pawn = null;
			}
		}

		public override bool CanHearPlayerVoice( Client source, Client dest )
		{
			return true;
		}

		public override void PostLevelLoaded()
		{

		}

		FreeCamera FreeCamera { get; set; } = new();

		public override void BuildInput( InputBuilder input )
		{
			Host.AssertClient();

			Hud.ShowMouse = !input.Down( InputButton.Attack2 );

			Event.Run( "buildinput", input );

			FreeCamera?.BuildInput( input );

			Local.Pawn?.BuildInput( input );
		}

		public override CameraSetup BuildCamera( CameraSetup camSetup )
		{
			FreeCamera?.Build( ref camSetup );
			PostCameraSetup( ref camSetup );
			return camSetup;
		}

		public override void OnVoicePlayed( ulong steamId, float level )
		{
			VoiceList.Current?.OnVoicePlayed( steamId, level );
		}

		/// <summary>
		/// Called each tick.
		/// Serverside: Called for each client every tick
		/// Clientside: Called for each tick for local client. Can be called multiple times per tick.
		/// </summary>
		public override void Simulate( Client cl )
		{
			if ( !cl.Pawn.IsValid() ) return;

			// Block Simulate from running clientside
			// if we're not predictable.
			if ( !cl.Pawn.IsAuthority ) return;

			cl.Pawn.Simulate( cl );
		}

		/// <summary>
		/// Called each frame on the client only to simulate things that need to be updated every frame. An example
		/// of this would be updating their local pawn's look rotation so it updates smoothly instead of at tick rate.
		/// </summary>
		public override void FrameSimulate( Client cl )
		{
			Host.AssertClient();

			if ( !cl.Pawn.IsValid() ) return;

			// Block Simulate from running clientside
			// if we're not predictable.
			if ( !cl.Pawn.IsAuthority ) return;

			cl.Pawn?.FrameSimulate( cl );
		}
	}
}
