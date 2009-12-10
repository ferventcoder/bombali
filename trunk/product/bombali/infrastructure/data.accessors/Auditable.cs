namespace bombali.infrastructure.data.accessors
{
    using System;

    public interface Auditable
    {
        DateTime? entered_date { get; set; }
        DateTime? modified_date { get; set; }
        string updating_user { get; set; }
    }
}