using DevFreela.Core.Entities;

namespace DevFreela.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAll();
        Task<Project?> GetDetailsById(int id);
        Task<Project?> GetById(int id);
        Task<bool> Exists(int id);
        Task<int> Add(Project project);
        Task AddComment(ProjectComment comment);
        Task Update(Project project);
        //Task Delete(int id);
    }
}
