
using DataAccess.Helpers;
using EcoSystemAPI.Models;
using esust.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IBaseRepository<T>
    {
        bool Insert(T entity);
        bool Delete(T entity);
        bool Update(T entity);
        T GetById(int id);
        List<T> GetAll();
       // IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
    }


    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ESutContextDB _dbContext;
        private int _s;
        public Object Error;
        public string ErrorMessage;
        public T entry;

        public BaseRepository(ESutContextDB dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Delete(T entity)
        {
            if (entity != null)
            {

                try
                {
                    _dbContext.Set<T>().Remove(entity);
                    _s = _dbContext.SaveChanges();
                    if (_s > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                    ErrorMessage = ex.InnerException.Message;
                    Error = ex.InnerException.StackTrace;
                }
               
            }

            return false;
        }

        public List<T> GetAll()
        {
            try
            {
                return _dbContext.Set<T>().Take(1000).OrderDescending().ToList();
            }
            catch (Exception ex)
            {

                ErrorLogger.Log(ex.Message);

                return null;
            }
            
        }

    

        public bool Insert(T entity)
        {
            if (entity != null)
            {
                try
                {
                   

                    _dbContext.Set<T>().Add(entity);
                    _s = _dbContext.SaveChanges();
                    if (_s > 0)
                    {
                        entry = entity;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if(ex.InnerException != null)
                    {
                        ErrorMessage = ex.InnerException.Message;
                        Error = ex.InnerException.StackTrace;
                    }
                    else
                    {
                        ErrorMessage = ex.Message;
                        Error = ex.StackTrace;
                    }

                    ErrorLogger.Log("Insert Error " + ErrorMessage);
                }

               
            }

            return false;
        }

        public bool InsertBulk(List<T> entities)
        {
            if (entities.Count > 0)
            {

                try
                {
                    _dbContext.Set<T>().AddRange(entities);
                    _s = _dbContext.SaveChanges();
                    if (_s > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ErrorMessage = ex.InnerException.Message;
                        Error = ex.InnerException.StackTrace;
                    }
                    else
                    {
                        ErrorMessage = ex.Message;
                        Error = ex.StackTrace;
                    }

                    ErrorLogger.Log("InsertBulk Error " + ErrorMessage);
                }

            }
            return false;
        }

        public bool Update(T entity)
        {
            if (entity != null)
            {
                try
                {
                    _dbContext.Set<T>().Update(entity);
                    _s = _dbContext.SaveChanges();
                    if (_s > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ErrorMessage = ex.InnerException.Message;
                        Error = ex.InnerException.StackTrace;
                    }
                    else
                    {
                        ErrorMessage = ex.Message;
                        Error = ex.StackTrace;
                    }

                    ErrorLogger.Log("Update "+ ErrorMessage);
                }
                
                
            }
            return false;
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

       
    }
}
