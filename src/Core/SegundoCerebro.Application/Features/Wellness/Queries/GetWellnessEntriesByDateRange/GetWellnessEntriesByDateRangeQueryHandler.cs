using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SegundoCerebro.Application.Features.Wellness.Queries.GetWellnessEntriesByDateRange;

public class GetWellnessEntriesByDateRangeQueryHandler : IRequestHandler<GetWellnessEntriesByDateRangeQuery, IEnumerable<WellnessEntryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWellnessEntriesByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WellnessEntryDto>> Handle(GetWellnessEntriesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var entries = await _unitOfWork.WellnessEntries.FindAsync(e =>
            e.Date.Date >= request.StartDate.Date && e.Date.Date <= request.EndDate.Date);

        return _mapper.Map<IEnumerable<WellnessEntryDto>>(entries.OrderBy(e => e.Date));
    }
}