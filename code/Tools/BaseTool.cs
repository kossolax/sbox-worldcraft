using Sandbox;

namespace WorldCraft
{
	public partial class BaseTool : BaseNetworkable
	{
		public virtual void Simulate( Client cl ) { }
		public virtual void FrameSimulate( Client cl ) { }
	}
}
