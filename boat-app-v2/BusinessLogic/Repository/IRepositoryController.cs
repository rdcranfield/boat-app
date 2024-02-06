namespace boat_app_v2.BusinessLogic.Repository;

public interface IRepositoryController
{
    IBoatRepository BoatRepository { get; } 
    Task SaveAsync(); 
}