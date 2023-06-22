using HRHUBAPI.Models;

namespace HRHUBAPI.BackGroundService
{
    public class MailService : IHostedService, IDisposable
    {
        
        private Timer _timer;
        private readonly EmailHelper _emailHelper;
        public MailService(EmailHelper emailHelper)
        {

            _emailHelper = emailHelper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
          

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

            try
            {

                var List = new EmailLog().GetPendingEmail(); 
                foreach (var log in List)
                {

                    log.IsSent = Convert.ToBoolean(_emailHelper.SendEmailBackGround(log.EmailTo, log.Subject, log.Body, log.FromMail, log.Pasword,
                        log.ServerName, Convert.ToInt32(log.PortName)));
                }
                if (List.Count()>0)
                {
                new EmailLog().UpdateEmail(List);

                }

            }
            catch (Exception)
            {


            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
          

            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}

