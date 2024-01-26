using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.ContactMessageRepo
{
    public interface IContactMessageRepository
    {
        public List<ContactMessage> GetAllContactMessages();
        public ContactMessage GetContactMessage(int contactMessageId);
        public void AddContactMessage(ContactMessage contactMessage);
		public void EditContactMessage(ContactMessage contactMessage);
	}
}
