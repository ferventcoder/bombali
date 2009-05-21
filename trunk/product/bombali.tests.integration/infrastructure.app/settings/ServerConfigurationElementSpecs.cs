namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;

    public class ServerConfigurationElementSpecs
    {

        [TestFixture]
        public class when_getting_a_server_configuration_element
        {
            ServerMonitorConfigurationCollection _config;

            [SetUp]
            public void set_up_and()
            {
                _config = BombaliConfiguration.settings.servers_to_monitor;
            }

            [Test]
            public void Verify_Name_of_first_item_returns_correctly()
            {
                Assert.AreEqual("Robz", _config.Item(0).name);
            }

            [Test]
            public void Verify_Name_has_a_default_value_of_empty_string()
            {
                Assert.AreEqual("", _config.Item(2).name);
            }

            [Test]
            public void Verify_Description_of_first_item_returns_correctly()
            {
                Assert.AreEqual("My home site", _config.Item(0).description);
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
            public void Verify_DayOfMonth_of_first_item_returns_correctly()
            {
                Assert.AreEqual(15, _config.Item(0).minutes_between_checks);
            }

            [Test]
            public void Verify_MinutesBetweenChecks_of_first_item_returns_correctly()
            {
                Assert.AreEqual(15d, _config.Item(0).minutes_between_checks);
            }

            [Test]
            public void Verify_MinutesBetweenChecks_can_return_a_decimal_value()
            {
                Assert.AreEqual(1.5d, _config.Item(1).minutes_between_checks);
            }

            [Test]
            public void Verify_MinutesBetweenChecks_has_a_default_value_of_30()
            {
                Assert.AreEqual(30d, _config.Item(2).minutes_between_checks);
            }

            [Test]
            public void Verify_server_address_of_first_item_returns_correctly()
            {
                Assert.AreEqual("somewhere.com", _config.Item(0).server_address);
            }

            [Test]
            public void Verify_EmailsToSendTo_have_a_default_count_of_1()
            {
                Assert.AreEqual(1, _config.Item(0).emails_to_send_to.Count);
            }

            [Test]
            public void Verify_EmailsToSendTo_of_third_item_has_a_count_of_3()
            {
                Assert.AreEqual(3, _config.Item(2).emails_to_send_to.Count);
            }
        }
    }
}