using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PayCalculator.Repository;

namespace PayCalculator
{
    public class SessionManager : ISessionManager
    {
        private readonly ISalaryRepository _salaryRepository;
        private Guid? _sessionId = null;
        public SessionManager(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        public Guid GetSessionId(HttpRequestBase request, HttpResponseBase response)
        {
            if (_sessionId == null)
            {
                if (request.Cookies["PayCalculator"] != null)
                {
                    Guid parsedSessionId;
                    if (Guid.TryParse(request.Cookies["PayCalculator"].Value, out parsedSessionId))
                    {
                        return parsedSessionId;
                    }
                }

                Guid newSessionId = Guid.NewGuid();

                response.Cookies.Add(new HttpCookie("PayCalculator", newSessionId.ToString()));

                _salaryRepository.CreateUser(newSessionId, request.UserHostAddress, null, null);

                _sessionId = newSessionId;
            }

            return _sessionId.Value;
        }
    }
}