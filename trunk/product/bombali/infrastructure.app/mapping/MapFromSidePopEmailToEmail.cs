namespace bombali.infrastructure.app.mapping
{
    using System;
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
            Log.bound_to(this).Debug("{0} is mapping message \"{1}\" to a domain type email message for processing and archival.", ApplicationParameters.name, from.MessageId);
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

            try
            {
                //this is for multimedia messages that come from some phones - I'm looking at you AT&T
                if (string.IsNullOrEmpty(email.message_body))
                {
                    Log.bound_to(this).Info("Message Body is null - looking in attachments for message.");

                    if (from.Attachments != null && from.Attachments.Count >= 1)
                    {
                        Log.bound_to(this).Debug("There is at least one attachment.");
                        foreach (Attachment attachment in from.Attachments)
                        {
                            Log.bound_to(this).Debug("Content Type MediaType|CharSet = {0}.", attachment.ContentType != null ? attachment.ContentType.MediaType + "|" + attachment.ContentType.CharSet : string.Empty);
                            if (attachment.ContentType != null && attachment.ContentType.MediaType.to_lower().Contains("text"))
                            {
                                byte[] buffer = new byte[attachment.ContentStream.Length];
                                attachment.ContentStream.Read(buffer, 0, buffer.Length);
                                email.message_body = Encoding.UTF8.GetString(buffer);
                                Log.bound_to(this).Info("After conversion - this is the message body:{0}{1}", Environment.NewLine, email.message_body);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.bound_to(this).Error("{0} service had an error getting message body from attachment on {1} (with user {2}):{3}{4}", ApplicationParameters.name,
                                    Environment.MachineName, Environment.UserName,
                                    Environment.NewLine, ex.ToString());
            }


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