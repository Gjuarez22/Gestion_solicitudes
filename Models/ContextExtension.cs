// Archivo: DbFlujosTestContext.Custom.cs
using GestionSolicitud.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GestionSolicitud.Models
{
    public partial class DbFlujosTestContext
    {
        public virtual DbSet<SpListadoSoliciturdes> ListadoSoliciturdesSp { get; set; }
        public virtual DbSet<SpBuscarAutocompletado> BuscarAutocompletadoSp { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpListadoSoliciturdes>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
            
            modelBuilder.Entity<SpBuscarAutocompletado>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
        }

        public List<SpListadoSoliciturdes> spListadoSoliciturdes(int idUsuario, int idRol)
        {
            return ListadoSoliciturdesSp
                .FromSqlRaw("EXEC sp_ListadoSoliciturdes @idUsuario, @Rol",
                    new SqlParameter("@idUsuario", idUsuario),
                    new SqlParameter("@Rol", idRol))
                .AsNoTracking()
                .ToList();
        }
        
        public List<SpBuscarAutocompletado> spBuscarAutocompletado(string buscar, int whsCode)
        {
            return BuscarAutocompletadoSp
                .FromSqlRaw("EXEC sp_BuscarItemAutocompleatado @buscar, @whsCode",
                    new SqlParameter("@buscar", buscar),
                    new SqlParameter("@whsCode", whsCode))
                .AsNoTracking()
                .ToList();
        }
    }
}