using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;
using OffersPlatform.Persistence.Context;

namespace OffersPlatform.Persistence.Repositories;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Company?> GetCompanyByEmailAsync(string? email, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies.
            FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        return company;
    }
    

    public async Task<Company?> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies.FindAsync(id, cancellationToken);
        return company;
    }

    public async Task<IEnumerable<Company?>> GetAllActiveCompaniesAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _dbContext.Companies.
            Where(x => x.Status == CompanyStatus.Active).
            ToListAsync(cancellationToken);
        return companies;
    }
    
    public async Task<IEnumerable<Company?>> GetAllCompaniesAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _dbContext.Companies.
            Where(x => x.Status == CompanyStatus.Active || x.Status == CompanyStatus.Inactive).
            ToListAsync(cancellationToken);
        return companies;
    }

    public async Task<string> GetPasswordHashAsync(string? email, CancellationToken cancellationToken = default)
    {
        var passwordHash = await _dbContext.Companies
            .Where(x => x.Email == email)
            .Select(x => x.PasswordHash)
            .FirstOrDefaultAsync(cancellationToken);
        
        return passwordHash;
    }
    
    public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
    {
        await _dbContext.Companies.AddAsync(company, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Company company, CancellationToken cancellationToken = default)
    {
        company.UpdatedAt = DateTime.Now;
        _dbContext.Companies.Update(company);
    }
    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies.FindAsync(new object[] { id }, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException("Company Not Found");
        }

        if (company.Status == CompanyStatus.Deleted)
        {
            throw new Exception("Company is already deleted.");
        }
        
        company.Status = CompanyStatus.Deleted;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


}