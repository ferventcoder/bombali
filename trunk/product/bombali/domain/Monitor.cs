namespace bombali.domain
{
    using System;
    using System.Threading;
    using System.Timers;
    using infrastructure;
    using infrastructure.app.monitorchecks;
    using infrastructure.logging;
    using infrastructure.notifications;
    using infrastructure.timers;

    public class Monitor : IDomainType, IMonitor
    {
        readonly ITimer the_timer;
        readonly ICheck check_utility;
        bool last_result_successful = true;

        public Monitor(string name, string what_to_check, double interval_in_minutes_for_check,
                       string emails_to_as_comma_separated_values,
                       string email_from, string notification_host, ICheck check_utility, ITimer the_timer)
        {
            this.notification_host = notification_host;
            this.the_timer = the_timer;
            this.name = name;
            this.what_to_check = what_to_check;
            this.interval_in_minutes_for_check = interval_in_minutes_for_check;
            who_to_notify_as_comma_separated_list = emails_to_as_comma_separated_values;
            who_notification_comes_from = email_from;

            this.the_timer.Elapsed += the_timer_Elapsed;
            this.check_utility = check_utility;
            status_is_good = true;
        }

        public string what_to_check { get; private set; }
        public string name { get; private set; }
        public double interval_in_minutes_for_check { get; private set; }
        public string who_to_notify_as_comma_separated_list { get; private set; }
        public string who_notification_comes_from { get; private set; }
        public string notification_host { get; private set; }
        public bool status_is_good { get; private set; }

        public void start_monitoring()
        {
            Log.bound_to(this).Info("{0} has started a monitor \"{1}\" on thread {2}.", ApplicationParameters.name, name, Thread.CurrentThread.ManagedThreadId);
            the_timer.start();
        }

        public void change_interval(double new_interval_in_minutes)
        {
            the_timer.change_interval(new_interval_in_minutes);
        }

        void the_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            the_timer.stop();
            make_decision_based_on_check(check_utility.run_check(what_to_check));
            the_timer.start();
        }

        public void make_decision_based_on_check(bool current_request_considered_success)
        {
            status_is_good = true;
            if (!current_request_considered_success) status_is_good = false;

            if (!current_request_considered_success && last_result_successful)
            {
                last_result_successful = false;
                send_notification(false, check_utility.last_response);
            }
            if (current_request_considered_success && !last_result_successful)
            {
                last_result_successful = true;
                send_notification(true, check_utility.last_response);
            }
        }

        public void send_notification(bool success, string response)
        {
            string reporting_type = "BAD";
            if (success) reporting_type = "GOOD";

            string subject = string.Format("{0} - \"{1}\" {2}", ApplicationParameters.name, name, reporting_type);
            string message = string.Format("{0} reports {1} for {2}.", ApplicationParameters.name, response, what_to_check);

            try
            {
                SendNotification.from(who_notification_comes_from).to(who_to_notify_as_comma_separated_list).with_subject(
                                subject).with_message(message).and_use_notification_host(notification_host);
            }
            catch (Exception ex)
            {
                Log.bound_to(this).Error("{0} is not able to send email for monitoring error.{1]{2}", ApplicationParameters.name, Environment.NewLine, ex.ToString());
            }
        }
    }
}