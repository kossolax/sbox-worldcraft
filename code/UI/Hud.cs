using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace WorldCraft
{
	public partial class KeyBind : Label
	{
		public string Binding;

		public override void SetProperty( string name, string value )
		{
			base.SetProperty( name, value );

			if ( name == "bind" )
			{
				Binding = value;
			}
		}

		public override void Tick()
		{
			base.Tick();

			Text = Input.GetKeyWithBinding( Binding )?.ToUpper() ?? "UNSET";
		}
	}

	public partial class Hud : RootPanel
	{
		private bool _showMouse;
		public bool ShowMouse
		{
			set
			{
				if ( value != _showMouse )
				{
					SetClass( "accept-input", value );
					Style.PointerEvents = value ? "visible" : "none";
					Style.Dirty();
				}

				_showMouse = value;
			}
		}

		public float GridSize => Game.GridSize;

		public Hud()
		{
			SetTemplate( "/UI/Hud.html" );
		}
	}
}
