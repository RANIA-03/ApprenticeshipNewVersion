using Apprenticeship.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Apprenticeship.Data.Repository.ContactMessageRepo
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        ApplicationDbContext context;
        public ContactMessageRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddContactMessage(ContactMessage contactMessage)
        {
            context.contactMessages.Add(contactMessage);
            context.SaveChanges();
        }

		public void EditContactMessage(ContactMessage contactMessage)
		{
			var contactMessageInfo = GetContactMessage(contactMessage.ContactMessageId);
            contactMessageInfo.Status = true;
			context.SaveChanges();
		}

		public List<ContactMessage> GetAllContactMessages()
        {
            return context.contactMessages.ToList();
        }

        public ContactMessage GetContactMessage(int contactMessageId)
        {
            return context.contactMessages.Where(c => c.ContactMessageId == contactMessageId).SingleOrDefault();
        }
    }
}
