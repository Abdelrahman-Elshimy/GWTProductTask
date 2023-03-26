using Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BaseService
    {
        #region Fields

        protected readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        public BaseService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion
    }
}
