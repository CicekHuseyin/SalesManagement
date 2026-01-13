using FluentValidation;
using SalesManagement.Core.Exceptions;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Services.Concretes;

public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<Customer> _customerRepo;
    private readonly IValidator<Customer> _validator;

    public CustomerService(IGenericRepository<Customer> customerRepo, IValidator<Customer> validator)
    {
        _customerRepo = customerRepo;
        _validator = validator;
    }

    public void AddCustomer(Customer customer)
    {
        var validationResult = _validator.Validate(customer);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _customerRepo.Add(customer);
        _customerRepo.Save();
    }

    public IEnumerable<Customer> GetCustomers()
    {
        return _customerRepo.GetAll();  
    }

    public void UpdateCustomer(Customer customer)
    {
        var validationResult = _validator.Validate(customer);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (customer.Id <= 0)
            throw new BusinessException("Geçersiz müşteri bilgisi.");

        var existingCustomer = _customerRepo.Get(c => c.Id == customer.Id);
        if (existingCustomer == null)
            throw new BusinessException("Güncellenecek müşteri bulunamadı.");

        _customerRepo.Update(customer);
        _customerRepo.Save();
    }
}
