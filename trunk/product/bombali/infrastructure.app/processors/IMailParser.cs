namespace bombali.infrastructure.app.processors
{
    using System.Collections.Generic;
    using domain;
    
    public interface IMailParser
    {
        MailQueryType parse(Email message, IList<IMonitor> monitors, IDictionary<string, ApprovalType> authorization_dictionary);
    }
}