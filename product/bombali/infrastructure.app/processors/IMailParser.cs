namespace bombali.infrastructure.app.processors
{
    using sidepop.Mail;

    public interface IMailParser
    {
        MailQueryType parse(SidePOPMailMessage message);
    }
}