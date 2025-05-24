using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IContactService
    {
        //Tao moi mot ban ghi lien he
        Task<ContactVM> CreateAsync(ContactVM model);
        //Danh sach lien he chua duoc giai quyet
        Task<IEnumerable<ContactVM>> GetUnresolvedAsync();
        //Danh dau lien he da duoc giai quyet
        Task ResolveAsync(Guid Id);
    }
}
