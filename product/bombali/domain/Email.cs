namespace bombali.domain
{
    using System;
    using infrastructure.data.accessors;

    public class Email : Auditable
    {
        public Guid id { get; set; }

        // message metadata
        public string message_id { get; set; }
        public DateTime? delivery_date { get; set; }
        //message.Headers;
        //message.MessageNumber;
        //message.MimeVersion;
        //message.Octets;

        // main message information
        public string from_address { get; set; }
        //message.ReturnAddress;
        //message.ReplyTo;
        //message.ReplyToMessageId;
        public string to_addresses { get; set; }
        public string cc_addresses { get; set; }
        public string bcc_addresses { get; set; }
        public string priority { get; set; }
        public string subject { get; set; }
        public string message_body { get; set; }
        //message.Attachments;


        //message.BodyEncoding;
        //message.ContentDescription;
        //message.ContentDisposition;
        //message.ContentId;
        //message.ContentType;
        //message.DeliveryNotificationOptions;
        //message.Routing;
        //message.AlternateViews;

        //auditing fields
        public DateTime? entered_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string updating_user { get; set; }

    }
}