using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }
        public async Task<ContactVM> CreateAsync(ContactVM model)
        {
            var contact = _mapper.Map<Contact>(model);
            contact.Id = Guid.NewGuid();
            contact.CreatedAt = DateTime.UtcNow;
            contact.IsResolved = false;

            await _contactRepository.AddAsync(contact);
            return _mapper.Map<ContactVM>(contact);
        }

        public async Task<IEnumerable<ContactVM>> GetUnresolvedAsync()
        {
            var contacts = await _contactRepository.GetUnresolvedAsync();
            return _mapper.Map<IEnumerable<ContactVM>>(contacts);
        }

        public async Task ResolveAsync(Guid Id)
        {
            var contact = await _contactRepository.GetByIdAsync(Id);
            if (contact == null) throw new Exception("Không thấy liên hệ");
            contact.IsResolved = true;
            await _contactRepository.UpdateAsync(contact);

        }
    }
}
