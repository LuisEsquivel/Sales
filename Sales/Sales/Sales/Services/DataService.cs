

namespace Sales.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<ProductsLuis>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task<List<ProductsLuis>> GetAllProducts()
        {
            var query = await this.connection.QueryAsync<ProductsLuis>("select * from [ProductsLuis]");
            var array = query.ToArray();
            var list = array.Select(p => new ProductsLuis
            {
                NOM_PROD_VAR       = p.NOM_PROD_VAR,
                RUTA_IMAGEN_VAR    = p.RUTA_IMAGEN_VAR,
                IS_AVAILABLE_BIT   = p.IS_AVAILABLE_BIT,
                PRECIO_DEC         = p.PRECIO_DEC,
                CVE_PRODUCTO_VAR   = p.CVE_PRODUCTO_VAR,
                PUBLISH_ON_DATE    = p.PUBLISH_ON_DATE,
                REMARK_VAR         = p.REMARK_VAR,
            }).ToList();
            return list;
        }

        public async Task DeleteAllProducts()
        {
            var query = await this.connection.QueryAsync<ProductsLuis>("delete from [ProductsLuis]");
        }
    }
}