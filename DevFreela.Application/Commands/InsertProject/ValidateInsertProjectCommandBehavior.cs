﻿using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.InsertProject
{
    public class ValidateInsertProjectCommandBehavior : IPipelineBehavior<InsertProjectCommand, ResultViewModel<int>>
    {
        private readonly DevFreelaDbContext _context;

        public ValidateInsertProjectCommandBehavior(DevFreelaDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, RequestHandlerDelegate<ResultViewModel<int>> next, CancellationToken cancellationToken)
        {
            var clientExists = _context.Users.Any(u => u.Id == request.IdClient);
            var freelancerExists = _context.Users.Any(u => u.Id == request.IdFreelancer);

            if (!clientExists)
                return ResultViewModel<int>.Error("Cliente não encontrado!");

            if (!freelancerExists)
                return ResultViewModel<int>.Error("Freelancer não encontrado!");

            return await next();
        }
    }
}