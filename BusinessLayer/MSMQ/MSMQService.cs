using System;
using CommonLayer.RequestModel;
using Experimental.System.Messaging;
using FundooNotes.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BusinessLayer.MSMQ
{
    public class MSMQService
    {
        readonly EmailService emailService;
        readonly MessageQueue queue = new MessageQueue(@".\private$\FunDooNotes");

        public MSMQService(IConfiguration config)
        {
            emailService = new EmailService(config);
        }
        /// <summary>
        /// Sends the password reset link to MSMQ 
        /// </summary>
        /// <param name="resetLink">The reset link.</param>
        public void SendPasswordResetLink(ForgetPasswordModel resetLink)
        {
            try
            {
                if (!MessageQueue.Exists(queue.Path))
                {
                    MessageQueue.Create(queue.Path);
                }
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                Message msg = new Message
                {
                    Label = "password reset link",
                    Body = JsonConvert.SerializeObject(resetLink)
                };
                queue.Send(msg);
                queue.ReceiveCompleted += Queue_ReceiveCompleted;
                queue.BeginReceive(TimeSpan.FromSeconds(5));
                queue.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Handles the ReceiveCompleted event of the Queue control.
        /// sends email when message received from queue
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReceiveCompletedEventArgs"/> instance containing the event data.</param>
        void Queue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {          
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                ForgetPasswordModel model = JsonConvert.DeserializeObject<ForgetPasswordModel>(msg.Body.ToString());
                emailService.SendPasswordResetLinkEmail(model);
                queue.BeginReceive(TimeSpan.FromSeconds(5));
            }
            catch (Exception)
            {

            }
        }
    }
}
