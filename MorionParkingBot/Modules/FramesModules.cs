using Autofac;
using MorionParkingBot.Frames;

namespace MorionParkingBot.Modules;

public class FramesModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<FrameStateLogic>()
			.As<FrameStateLogic>()
			.SingleInstance();

		builder.RegisterType<FrameStateConstructor>()
			.As<FrameStateConstructor>()
			.SingleInstance();
	}
}