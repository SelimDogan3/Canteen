using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
namespace Cantin.Service.Services.Concrete
{
	public class StoreService : IStoreService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly IValidator<Store> validator;
		private readonly ClaimsPrincipal _user;

		private IRepository<Store> repository { get => unitOfWork.GetRepository<Store>(); }

		public StoreService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor,IValidator<Store> validator)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.validator = validator;
			_user = contextAccessor.HttpContext!.User;
		}
		public async Task<List<StoreDto>> GetAllStoreDtosNonDeleted()
		{
			List<Store> stores = await repository.GetAllAsync(x => !x.IsDeleted && x.Name != "Superadmin");
			List<StoreDto> map = mapper.Map<List<StoreDto>>(stores);
			return map;
		}
		public async Task<StoreDto> GetStoreById(Guid Id)
		{
			Store store = await repository.GetByGuidAsync(Id);
			StoreDto dto = mapper.Map<StoreDto>(store);
			return dto;
		}
		public StoreAddDto GetStoreAddDto()
		{
			StoreAddDto dto = new StoreAddDto();
			return dto;
		}
		public async Task<string> AddStoreAsync(StoreAddDto dto)
		{
			Store store = mapper.Map<Store>(dto);
			store.CreatedBy = _user.GetLoggedInUserEmail();
			await repository.AddAsync(store);
			await unitOfWork.SaveAsync();
			return dto.Name;
		}
		public async Task<StoreUpdateDto> GetStoreUpdateDtoByIdAsync(Guid Id)
		{
			Store store = await repository.GetByGuidAsync(Id);
			StoreUpdateDto dto = mapper.Map<StoreUpdateDto>(store);
			return dto;
		}
		public async Task<string> UpdateStoreAsync(StoreUpdateDto dto)
		{
			Store store = await repository.GetByGuidAsync(dto.Id);
			store = mapper.Map(dto, store);
			store.ModifiedBy = _user.GetLoggedInUserEmail();
			store.ModifiedDate = DateTime.Now;
			await repository.UpdateAsync(store);
			await unitOfWork.SaveAsync();
			return store.Name;
		}
		public async Task<string> DeleteStoreAsyncById(Guid Id)
		{
			Store store = await repository.GetByGuidAsync(Id);
			store.DeletedBy = _user.GetLoggedInUserEmail();
			store.DeletedDate = DateTime.Now;
			store.IsDeleted = true;
			await repository.UpdateAsync(store);
			await unitOfWork.SaveAsync();
			return store.Name;
		}

		public async Task ValidateStoreAsync(StoreAddDto dto, ModelStateDictionary modelState)
		{
			Store store = mapper.Map<Store>(dto);
			ValidationResult result = await validator.ValidateAsync(store);
			if (!result.IsValid) {
				result.AddErrorsToModelState(modelState);
			}
		}

		public async Task ValidateStoreAsync(StoreUpdateDto dto, ModelStateDictionary modelState)
		{
			Store store = mapper.Map<Store>(dto);
			ValidationResult result = await validator.ValidateAsync(store);
			if (!result.IsValid)
			{
				result.AddErrorsToModelState(modelState);
			}
		}
	}
}
