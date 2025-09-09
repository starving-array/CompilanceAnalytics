using ComplianceAnalytics.Domain.Entities;
using ComplianceAnalytics.Domain.Repositories;
using ComplianceAnalytics.Infrastructure.Data;

namespace ComplianceAnalytics.Infrastructure.Repositories
{
    public class LocationRepository : GenericRepository<LocationEntity>, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context) { }
    }
}
