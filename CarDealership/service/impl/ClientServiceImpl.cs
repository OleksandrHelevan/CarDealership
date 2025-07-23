using CarDealership.entity;
using CarDealership.mapper;
using CarDealership.model;
using CarDealership.repo;

namespace CarDealership.service.impl;

public class ClientServiceImpl : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientServiceImpl(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public IEnumerable<ClientDto> GetAllClients()
    {
        var clients = _clientRepository.GetAll();
        return clients.Select(ClientMapper.ToDto).ToList();
    }

    public ClientDto? GetClientById(int id)
    {
        var client = _clientRepository.GetById(id);
        if (client == null) return null;
        return ClientMapper.ToDto(client);
    }

    public void AddClient(ClientDto clientDto)
    {
        var client = new Client
        {
            User = new User
            {
                Login = clientDto.Login,
                Password = clientDto.Password,
                AccessRight = clientDto.AccessRight
            },
            PassportData = new PassportData
            {
                FirstName = clientDto.PassportData.FirstName,
                LastName = clientDto.PassportData.LastName,
                PassportNumber = clientDto.PassportData.PassportNumber
            }
        };

        _clientRepository.Add(client);
    }

    public void UpdateClient(int id, ClientDto clientDto)
    {
        var existingClient = _clientRepository.GetById(id);
        if (existingClient == null) return;

        existingClient.User.Login = clientDto.Login;
        existingClient.User.Password = clientDto.Password;
        existingClient.User.AccessRight = clientDto.AccessRight;

        existingClient.PassportData.FirstName = clientDto.PassportData.FirstName;
        existingClient.PassportData.LastName = clientDto.PassportData.LastName;
        existingClient.PassportData.PassportNumber = clientDto.PassportData.PassportNumber;

        _clientRepository.Update(existingClient);
    }

    public void DeleteClient(int id)
    {
        _clientRepository.Delete(id);
    }
}