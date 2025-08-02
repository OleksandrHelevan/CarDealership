using CarDealership.dto;

namespace CarDealership.service;

public interface IClientService
{
    IEnumerable<ClientDto> GetAllClients();
    ClientDto? GetClientById(int id);
    void AddClient(ClientDto clientDto);
    void UpdateClient(int id, ClientDto clientDto);
    void DeleteClient(int id);
}