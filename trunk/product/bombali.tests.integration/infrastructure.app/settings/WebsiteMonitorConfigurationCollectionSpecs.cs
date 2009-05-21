namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;

    public class WebsiteMonitorConfigurationCollectionSpecs
    {

        [TestFixture]
        public class When_getting_the_web_sites_to_monitor_configuration
        {
            WebsiteMonitorConfigurationCollection _config;

            [SetUp]
            public void setup_and()
            {
                _config = BombaliConfiguration.settings.sites_to_monitor;
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