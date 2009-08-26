namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;

    public class EmailConfigurationCollectionSpecs
    {

        [TestFixture]
        public class When_getting_the_email_configuration_collection
        {
            EmailConfigurationCollection _config;

            [SetUp]
            public void setup_and()
            {
                _config = BombaliConfiguration.settings.items_to_monitor.Item(2).emails_to_send_to;
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