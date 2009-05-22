namespace bombali.infrastructure.app.publishers
{
	public interface IPublisher
	{
		void publish(string message);
	}
}