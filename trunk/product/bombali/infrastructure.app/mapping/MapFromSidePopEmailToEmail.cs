namespace bombali.infrastructure.app.mapping
{
    using System.Net.Mail;
    using System.Text;
    using domain;
    using infrastructure.mapping;
    using logging;
    using sidepop.infrastructure.extensions;
    using sidepop.Mail;

    public class MapFromSidePopEmailToEmail : IMapper<SidePOPMailMessage, Email>
    {
        public Email map_from(SidePOPMailMessage from)
        {
            Log.bound_to(this).Debug("{0} is mapping message \"{1}\" to a domain type email message for processing and archival.",ApplicationParameters.name,from.MessageId);
            Email email = new Email
                              {
                                  // message metadata
                                  message_id = from.MessageId,
                                  delivery_date = from.DeliveryDate,

                                  // main message information
                                  from_address = from.From.Address.to_lower(),
                                  to_addresses = flatten_addresses_with_pipe_separator(from.To),
                                  cc_addresses = flatten_addresses_with_pipe_separator(from.CC),
                                  bcc_addresses = flatten_addresses_with_pipe_separator(from.Bcc),
                                  priority = from.Priority.ToString(),
                                  subject = from.Subject,
                                  message_body = from.Body
                              };
            

            return email;
        }

        public string flatten_addresses_with_pipe_separator(MailAddressCollection addresses)
        {
            int counter = 0;
            StringBuilder builder = new StringBuilder();
            foreach (MailAddress address in addresses)
            {
                if (counter != 0)
                {
                    builder.Append("|");
                }
                builder.Append(address.Address.to_lower());
                counter += 1;
            }

            return builder.ToString();
        }
    }
}