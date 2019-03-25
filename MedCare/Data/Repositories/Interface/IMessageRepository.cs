using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedCare.Models.Database;

namespace MedCare.Data.Repositories.Interface
{
    public interface IMessageRepository
    {
        Task<Boolean> SendMessage(Message message);
        Task<Message> GetMessage(int messageId, string to);
        Task<IEnumerable<Message>> GetReceivedMessages(string email);
        Task<Boolean> DeleteMessage(int id); 
    }
}
