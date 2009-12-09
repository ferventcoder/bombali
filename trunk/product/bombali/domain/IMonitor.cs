namespace bombali.domain
{
    public interface IMonitor
    {
        double interval_in_minutes_for_check { get; }
        string name { get; }
        string what_to_check { get; }
        string who_to_notify_as_comma_separated_list { get; }
        string who_notification_comes_from { get; }
        string notification_host { get; }
        bool status_is_good { get; }

        void start_monitoring();
        void change_interval(double new_interval_in_minutes);
    }
}