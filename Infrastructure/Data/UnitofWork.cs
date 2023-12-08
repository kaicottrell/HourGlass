using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Data
{
	public class UnitofWork : IUnitofWork
	{
		#region UnitofWork Constructor
		private readonly ApplicationDbContext _dbContext;

		public UnitofWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		#endregion

		#region Private IGenericRepository Variables  
		private IGenericRepository<ApplicationUser> _ApplicationUser;
		private IGenericRepository<Session> _Session;
		private IGenericRepository<Template> _Template;
		private IGenericRepository<TemplateGroup> _TemplateGroup;
		//private IGenericRepository<IdentityRole> _UserRole;
		private IGenericRepository<UserTemplate> _UserTemplate;
		#endregion

		#region Public IGenericRepository Get Methods with new Instantiations

		public IGenericRepository<ApplicationUser> ApplicationUser
		{
			get
			{
				//Assigns a value only if it is null
				_ApplicationUser ??= new GenericRepository<ApplicationUser>(_dbContext);
				return _ApplicationUser;
			}
		}
		public IGenericRepository<Session> Session
		{
			get
			{
				_Session ??= new GenericRepository<Session>(_dbContext);
				return _Session;
			}
		}
		public IGenericRepository<Template> Template
		{
			get
			{
				_Template ??= new GenericRepository<Template>(_dbContext);
				return _Template;
			}
		}
		public IGenericRepository<TemplateGroup> TemplateGroup
		{
			get
			{
				_TemplateGroup ??= new GenericRepository<TemplateGroup>(_dbContext);
				return _TemplateGroup;
			}
		}

		//public IGenericRepository<IdentityRole> UserRole
		//{
		//	get
		//	{
		//		_UserRole ??= new GenericRepository<IdentityRole>(_dbContext);
		//		return _UserRole;
		//	}
		//}
		public IGenericRepository<UserTemplate> UserTemplate
		{
			get
			{
				_UserTemplate ??= new GenericRepository<UserTemplate>(_dbContext);
				return _UserTemplate;
			}
		}
		#endregion

		#region Public Commit Methods
		public int Commit()
		{
			return _dbContext.SaveChanges();
		}
		public async Task<int> CommitAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
		#endregion

		#region Public Dispose Method
		// Doesn't need to inherit off the interface, it's just the minimum
		public void Dispose()
		{
			_dbContext.Dispose();
		}
		#endregion

	}
}

