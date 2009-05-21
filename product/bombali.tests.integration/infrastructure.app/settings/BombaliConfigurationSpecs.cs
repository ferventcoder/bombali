namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;

    public class BombaliConfigurationSpecs
    {

        [TestFixture]
        public class when_getting_the_website_Monitor_configuration
        {
            BombaliConfiguration _config;

            [SetUp]
            public void I_want_to()
            {
                _config = new BombaliConfiguration();
            }

            [Test]
            public void Verify_successful_creation()
            {
                Assert.IsNotNull(_config);
            }

            [Test]
            public void should_not_be_null()
            {
                Assert.IsNotNull(BombaliConfiguration.settings);
            }

            [Test]
            public void Verify_EmailFrom_returns_correctly()
            {
                Assert.AreEqual("bombali.service@noreply.com", BombaliConfiguration.settings.email_from);
            }

            [Test]
            public void Verify_SMTP_Host_returns_correctly()
            {
                Assert.AreEqual("somewhere.com", BombaliConfiguration.settings.smtp_host);
            }
        }
    }
}