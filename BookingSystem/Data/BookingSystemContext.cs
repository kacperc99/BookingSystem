using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Model;

namespace BookingSystem.Data
{
    public class BookingSystemContext : DbContext
    {
        public BookingSystemContext() { }
        public BookingSystemContext (DbContextOptions<BookingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingSystem.Model.UserModel> UserModel { get; set; } = default!;

        public virtual DbSet<BookingSystem.Model.LocationModel>? LocationModel { get; set; }

        public virtual DbSet<BookingSystem.Model.DeskModel>? DeskModel { get; set; }

        public virtual DbSet<BookingSystem.Model.ReservationModel>? ReservationModel { get; set; }
    }
}
