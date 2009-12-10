namespace bombali.orm
{
    using domain;
    using FluentNHibernate.Mapping;
    using FluentNHibernate.MappingModel;

    public class EmailMapping : ClassMap<Email>
    {
        public EmailMapping()
        {
            HibernateMapping.Schema("dbo");
            Table("ReceivedEmails");
            Not.LazyLoad();
            HibernateMapping.DefaultAccess.Property();
            HibernateMapping.DefaultCascade.SaveUpdate();

            //Id(x => x.id).Column("Id").GeneratedBy.Identity().UnsavedValue(0);
            Id(x => x.id).Column("Id").GeneratedBy.GuidComb();
            // message metadata
            Map(x => x.message_id);
            Map(x => x.delivery_date);
            // main message information
            Map(x => x.from_address);
            Map(x => x.to_addresses);
            Map(x => x.cc_addresses);
            Map(x => x.bcc_addresses);
            Map(x => x.priority);
            Map(x => x.subject);
            Map(x => x.message_body);
           
            //auditing fields
            Map(x => x.entered_date);
            Map(x => x.modified_date);
            Map(x => x.updating_user);
        }
    }
}