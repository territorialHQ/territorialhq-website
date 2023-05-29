using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TerritorialHQ.Data;
using System.Security.Principal;
using TerritorialHQ.Models;

namespace TerritorialHQ.Services.Base
{
    public abstract class BaseService<TEntity> : IBaseService where TEntity : class, IEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly LoggerService _logger;

        protected BaseService(ApplicationDbContext context, LoggerService logger)
        {
            _context = context;
            _logger = logger;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        public virtual IQueryable<TEntity> Query => _context.Set<TEntity>();
        public virtual IQueryable<TEntity> CustomQuery => _context.Set<TEntity>();

        public virtual IList<TEntity> GetAll()
        {
            return Query.ToList();
        }
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await Query.ToListAsync();
        }

        public virtual EntityEntry<TEntity> Update(TEntity entity)
        {
            return _context.Update(entity);
        }

        public EntityEntry<TEntity> Add(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity);
        }

        public EntityEntry<TEntity> Attach(TEntity entity) => _context.Attach(entity);

        public virtual Task<bool> ExistsAsync(string id) => Query.AnyAsync(e => e.Id == id);
        public virtual bool Exists(string id) => Query.Any(e => e.Id == id);

        public virtual Task<TEntity> FindAsync(string id) => Query.FirstOrDefaultAsync(e => e.Id == id);

        public Task<int> SaveChangesAsync(ClaimsPrincipal user = null)
        {
            var entries = _context.ChangeTracker.Entries();
            string log = "";

            if (user != null)
                log += "USER " + user.Identity.Name + ": ";

            bool anyChanges = false;
            foreach (var entry in entries)
            {
                if (entry.State != EntityState.Unchanged)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        log += "UPDATED ";
                        foreach (var v in entry.CurrentValues.Properties)
                        {
                            var cv = entry.CurrentValues[v]?.ToString();
                            var ov = entry.OriginalValues[v]?.ToString();

                            if (cv != ov)
                            {
                                anyChanges = true;
                                log += v.Name + " (OLD: " + ov + ", NEW: " + cv + ") ";
                            }
                        }

                        if (anyChanges)
                        {
                            log += " IN DATABASE " + entry.Metadata.Name?.Replace("IBB_Core.Models.", "");
                            log += " FOR ENTITY " + entry.CurrentValues["Id"]?.ToString() + ";";
                        }
                    }

                    if (entry.State == EntityState.Added)
                    {
                        log += "ADDED ENTITY TO TABLE " + entry.Metadata.Name?.Replace("IBB_Core.Models.", "");
                        log += " WITH NEW VALUES ";

                        foreach (var v in entry.CurrentValues.Properties)
                        {
                            log += v.Name + " = " + entry.CurrentValues[v]?.ToString() + ", ";
                        }
                    }

                    if (entry.State == EntityState.Deleted)
                    {
                        log += "DELETED ENTITY FROM TABLE " + entry.Metadata.Name?.Replace("IBB_Core.Models.", "");
                        log += " WITH ORIGINAL VALUES ";

                        foreach (var v in entry.OriginalValues.Properties)
                        {
                            log += v.Name + " = " + entry.OriginalValues[v]?.ToString() + ", ";
                        }
                    }

                }
            }

            if (!String.IsNullOrEmpty(log))
                _logger.Log.Information(log);

            return _context.SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(string id)
        {
            var entity = await FindAsync(id);
            Remove(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}
