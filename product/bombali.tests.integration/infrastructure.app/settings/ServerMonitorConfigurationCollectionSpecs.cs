namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;

    public class ServerMonitorConfigurationCollectionSpecs
    {

        [TestFixture]
        public class When_getting_the_servers_to_monitor_configuration
        {
            ServerMonitorConfigurationCollection _config;

            [SetUp]
            public void setup_and()
            {
                _config = BombaliConfiguration.settings.servers_to_monitor;
            }

            [Test]
            public void Verify_successful_creation()
            {
                Assert.IsNotNull(_config);
            }

            [Test]
            public void should_get_back_three_items_from_the_configuration()
            {
                Assert.AreEqual(3, _config.Count);
            }

        }
    }
}