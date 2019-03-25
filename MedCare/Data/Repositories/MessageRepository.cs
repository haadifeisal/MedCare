using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MedCare.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {

        public readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Boolean> SendMessage(Message message)
        {
            if (message == null)
            {
                return false;
            }

            await _context.AddAsync(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Message>> GetReceivedMessages(string email)
        {
            var messages = await _context.Messages.Where(m => m.To == email).ToListAsync();

            return messages;
        }

        public async Task<Message> GetMessage(int messageId, string to)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m =>m.Id==messageId && m.To==to);
            if (message == null)
            {
                return null;
            }
            return message;
        }

        public async Task<Boolean> DeleteMessage(int id)
        {
            var message = await _context.Messages.Where(x => x.Id == id).SingleAsync();
            if (message==null)
            {
                return false;
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
