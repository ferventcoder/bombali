namespace bombali.infrastructure.app.monitorchecks
{
    public interface ICheck
    {
        string last_response { get; }
        bool run_check(string what_to_check);
    }
}