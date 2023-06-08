using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Repositorios
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        // Variable de conexion
        private readonly DbContext _dbContext;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ************************************************************************************
        //                  TODOS ESTOS METODOS VIENEN DE LA INTERFAZ
        // ************************************************************************************

        public async Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro)
        {
            try
            {
                TModel model = await _dbContext.Set<TModel>().FirstOrDefaultAsync(filtro);
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TModel> Crear(TModel modelo)
        {
            try
            {
                _dbContext.Set<TModel>().Add(modelo);
                await _dbContext.SaveChangesAsync();

                // Si se inserta correctamente el modelo obtendra el id de la insercion
                return modelo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModel modelo)
        {
            try
            {
                _dbContext.Set<TModel>().Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(TModel modelo)
        {
            try
            {
                _dbContext.Set<TModel>().Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModel> queryModel = filtro == null ? _dbContext.Set<TModel>() : _dbContext.Set<TModel>().Where(filtro);
                return queryModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
