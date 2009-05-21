namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;

    public class EmailConfigurationElementSpecs
    {

        [TestFixture]
        public class when_getting_an_email_configuration_element
        {
            EmailConfigurationCollection _config;

            [SetUp]
            public void setup_and()
            {
                _config = BombaliConfiguration.settings.sites_to_monitor.Item(2).emails_to_send_to;
            }

            [Test]
            public void Verify_Name_of_first_item_returns_correctly()
            {
                Assert.AreEqual("Me", _config.Item(0).name);
            }

            [Test]
            public void Verify_Description_of_first_item_returns_correctly()
            {
                Assert.AreEqual("my email", _config.Item(0).description);
            }

            [Test]
            public void Verify_Description_has_a_default_value_of_empty_string()
            {
                Assert.AreEqual("", _config.Item(1).description);
            }

            [Test]
            public void Verify_Enabled_of_first_item_is_true()
            {
                Assert.AreEqual(true, _config.Item(0).enabled);
            }

            [Test]
            public void Verify_Enabled_of_second_item_is_false()
            {
                Assert.AreEqual(false, _config.Item(1).enabled);
            }

            [Test]
            public void Verify_Enabled_has_a_default_value_of_true()
            {
                Assert.AreEqual(true, _config.Item(2).enabled);
            }

            [Test]
            public void Verify_Email_of_first_item_returns_correctly()
            {
                Assert.AreEqual("someone@overhere.com", _config.Item(0).email);
            }

        }
    }
}